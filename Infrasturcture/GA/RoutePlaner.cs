using PathFinder.GA;
using System.Globalization;
using System.Text.Json;
using Infrasturcture.GA;
using Infrasturcture;

public class RoutePlanner
{
    private readonly HttpClient _httpClient = new();

    public async Task<byte[]> BuildGpxAsync(
        List<UserPoint> userPoints,
        int maxDistanceKm,
        GeoPoint startPoint,
        GeoPoint endPoint)
    {
        
        var corePoints = userPoints
            .Where(p => !p.GeoPoint.Equals(startPoint) && !p.GeoPoint.Equals(endPoint))
            .ToList();

        var ga = new GeneticAlgorithm(corePoints, maxDistanceKm, _httpClient);
        var best = await ga.RunAsync(startPoint, endPoint);

        var coordString = string.Join(";", best.Route.Select(p =>
            $"{p.GeoPoint.Lon.ToString(CultureInfo.InvariantCulture)},{p.GeoPoint.Lat.ToString(CultureInfo.InvariantCulture)}"));

        var url = $"http://localhost:5000/route/v1/foot/{coordString}?overview=full&geometries=polyline";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        var geometry = doc.RootElement.GetProperty("routes")[0].GetProperty("geometry").GetString();

        return GpxExporter.ExportPolylineToGpxBytes(geometry);
    }
}
