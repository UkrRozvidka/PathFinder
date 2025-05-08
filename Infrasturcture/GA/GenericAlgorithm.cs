using Infrasturcture;
using Infrasturcture.GA;
using System.Globalization;
using System.Text.Json;

namespace PathFinder.GA;

public class GeneticAlgorithm
{
    private readonly List<UserPoint> allPoints;
    private readonly int maxDistanceKm;
    private readonly HttpClient httpClient;
    private readonly int populationSize = 50;
    private readonly int generations = 100;
    private readonly double mutationRate = 0.1;
    private readonly Random random = new();

    public GeneticAlgorithm(List<UserPoint> points, int maxDistanceKm, HttpClient httpClient)
    {
        allPoints = points;
        this.maxDistanceKm = maxDistanceKm;
        this.httpClient = httpClient;
    }

    //public async Task<Individual> RunAsync()
    //{
    //    var population = InitializePopulation();

    //    for (int gen = 0; gen < generations; gen++)
    //    {
    //        foreach (var individual in population)
    //            individual.Fitness = await EvaluateFitnessAsync(individual);

    //        population = population
    //            .OrderByDescending(i => i.Fitness)
    //            .Take(populationSize / 2)
    //            .ToList();

    //        var newGeneration = new List<Individual>(population);
    //        while (newGeneration.Count < populationSize)
    //        {
    //            var child = Crossover(Select(population), Select(population));
    //            Mutate(child);
    //            newGeneration.Add(child);
    //        }

    //        population = newGeneration;
    //    }

    //    var valid = new List<(Individual Ind, double Distance)>();
    //    foreach (var ind in population)
    //    {
    //        var dist = await GetTotalDistanceAsync(ind.Route);
    //        if (dist <= maxDistanceKm * 1000)
    //            valid.Add((ind, dist));
    //    }

    //    return valid
    //        .OrderByDescending(p => p.Ind.Fitness)
    //        .ThenBy(p => p.Distance)
    //        .Select(p => p.Ind)
    //        .FirstOrDefault()
    //        ?? throw new Exception("Не знайдено маршруту в межах обмеження.");
    //}
    public async Task<Individual> RunAsync(GeoPoint startPoint, GeoPoint endPoint)
    {
        var population = InitializePopulation(startPoint, endPoint);

        for (int gen = 0; gen < generations; gen++)
        {
            foreach (var individual in population)
                individual.Fitness = await EvaluateFitnessAsync(individual);

            population = population
                .OrderByDescending(i => i.Fitness)
                .Take(populationSize / 2)
                .ToList();

            var newGeneration = new List<Individual>(population);
            while (newGeneration.Count < populationSize)
            {
                var child = Crossover(Select(population), Select(population), startPoint, endPoint);
                Mutate(child, startPoint, endPoint);
                newGeneration.Add(child);
            }

            population = newGeneration;
        }

        var valid = new List<(Individual Ind, double Distance)>();
        foreach (var ind in population)
        {
            var dist = await GetTotalDistanceAsync(ind.Route);
            if (dist <= maxDistanceKm * 1000)
                valid.Add((ind, dist));
        }

        return valid
            .OrderByDescending(p => p.Ind.Fitness)
            .ThenBy(p => p.Distance)
            .Select(p => p.Ind)
            .FirstOrDefault()
            ?? throw new Exception("Не знайдено маршруту в межах обмеження.");
    }


    //private List<Individual> InitializePopulation()
    //{
    //    return Enumerable.Range(0, populationSize)
    //        .Select(_ =>
    //        {
    //            var subset = allPoints.OrderBy(_ => random.Next()).Take(random.Next(2, allPoints.Count + 1)).ToList();
    //            return new Individual { Route = subset };
    //        }).ToList();
    //}

    private List<Individual> InitializePopulation(GeoPoint start, GeoPoint end)
    {
        var population = new List<Individual>();
        var corePoints = allPoints
            .Where(p => !p.GeoPoint.Equals(start) && !p.GeoPoint.Equals(end))
            .ToList();

        for (int i = 0; i < populationSize; i++)
        {
            var middle = corePoints.OrderBy(_ => random.Next()).Take(random.Next(0, corePoints.Count)).ToList();

            var route = new List<UserPoint>
        {
            new UserPoint { GeoPoint = start, Priority = 0 }
        };

            route.AddRange(middle);
            route.Add(new UserPoint { GeoPoint = end, Priority = 0 });

            population.Add(new Individual { Route = route });
        }

        return population;
    }


    private async Task<double> EvaluateFitnessAsync(Individual individual)
    {
        double score = individual.Route.Sum(p => p.Priority);
        double distance = await GetTotalDistanceAsync(individual.Route);

        if (distance > maxDistanceKm * 1000)
        {
            double penaltyFactor = maxDistanceKm * 1000 / distance; 
            score *= penaltyFactor;
        }

        return score;
    }

    private async Task<double> GetTotalDistanceAsync(List<UserPoint> route)
    {
        if (route.Count < 2) return 0;

        string coordString = string.Join(";", route.Select(p =>
            $"{p.GeoPoint.Lon.ToString(CultureInfo.InvariantCulture)},{p.GeoPoint.Lat.ToString(CultureInfo.InvariantCulture)}"));
        string url = $"http://localhost:5000/route/v1/foot/{coordString}";

        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var dist = doc.RootElement
                .GetProperty("routes")[0]
                .GetProperty("distance")
                .GetDouble();

            return dist;
        }
        catch
        {
            return double.MaxValue;
        }
    }

    private Individual Select(List<Individual> population) =>
        population[random.Next(population.Count)];

    //private Individual Crossover(Individual p1, Individual p2)
    //{
    //    var combined = p1.Route.Concat(p2.Route).Distinct().ToList();
    //    int size = random.Next(2, combined.Count + 1);
    //    return new Individual { Route = combined.OrderBy(_ => random.Next()).Take(size).ToList() };
    //}

    private Individual Crossover(Individual p1, Individual p2, GeoPoint start, GeoPoint end)
    {
        var combined = p1.Route
            .Concat(p2.Route)
            .Where(p => !p.GeoPoint.Equals(start) && !p.GeoPoint.Equals(end))
            .Distinct()
            .ToList();

        var middle = combined.OrderBy(_ => random.Next()).Take(random.Next(0, combined.Count)).ToList();
        var route = new List<UserPoint>
        {
            new UserPoint { GeoPoint = start, Priority = 0 }
        };
        route.AddRange(middle);
        route.Add(new UserPoint { GeoPoint = end, Priority = 0 });
        return new Individual { Route = route };
    }


    //private void Mutate(Individual ind)
    //{
    //    if (random.NextDouble() >= mutationRate) return;

    //    int action = random.Next(3);
    //    switch (action)
    //    {
    //        case 0 when ind.Route.Count >= 2:
    //            (ind.Route[random.Next(ind.Route.Count)],
    //             ind.Route[random.Next(ind.Route.Count)]) =
    //                (ind.Route[random.Next(ind.Route.Count)],
    //                 ind.Route[random.Next(ind.Route.Count)]);
    //            break;
    //        case 1:
    //            var available = allPoints.Except(ind.Route).ToList();
    //            if (available.Count > 0)
    //                ind.Route.Add(available[random.Next(available.Count)]);
    //            break;
    //        case 2 when ind.Route.Count > 2:
    //            ind.Route.RemoveAt(random.Next(ind.Route.Count));
    //            break;
    //    }
    //}

    private void Mutate(Individual ind, GeoPoint start, GeoPoint end)
    {
        if (random.NextDouble() >= mutationRate)
            return;

        var core = ind.Route.Skip(1).Take(ind.Route.Count - 2).ToList();

        int action = random.Next(3);
        switch (action)
        {
            case 0 when core.Count >= 2:
                int i = random.Next(core.Count);
                int j = random.Next(core.Count);
                (core[i], core[j]) = (core[j], core[i]);
                break;
            case 1:
                var available = allPoints.Except(ind.Route).ToList();
                if (available.Count > 0)
                    core.Add(available[random.Next(available.Count)]);
                break;
            case 2 when core.Count > 2:
                core.RemoveAt(random.Next(core.Count));
                break;
        }

        ind.Route = new List<UserPoint> { new UserPoint { GeoPoint = start, Priority = 0 } }
            .Concat(core)
            .Concat(new[] { new UserPoint { GeoPoint = end, Priority = 0 } })
            .ToList();
    }

}

public class Individual
{
    public List<UserPoint> Route { get; set; } = new();
    public double Fitness { get; set; }

    public Individual Clone() => new()
    {
        Route = new List<UserPoint>(Route),
        Fitness = Fitness
    };
}
