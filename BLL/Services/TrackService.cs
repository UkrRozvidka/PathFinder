using AutoMapper;
using BLL.DTOs.Track;
using BLL.Services.Contracts;
using DAL.Entities;
using DAL.Repository.Contracts;
using Infrasturcture;
using Infrasturcture.GA;
using IServiceProvider = BLL.Services.Contracts.IServiceProvider;

namespace BLL.Services
{
    public class TrackService : BaseService<Track>, ITrackService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IPointService _pointService;
        public TrackService(IMapper mapper, IGenericRepository<Track> repository,
            IPointService pointService, IServiceProvider serviceProvider)
            : base(mapper, repository)
        {
            _serviceProvider = serviceProvider;
            _pointService = pointService;
        }

        public async Task<TrackGetDTO> GetById(long id)
        {
            var track = await _repository.GetByIdAsync(id) ?? throw new Exception("Track not found");
            var dto = _mapper.Map<TrackGetDTO>(track);
            return dto;
        }

        public async Task<IEnumerable<TrackGetDTO>> GetAllByHikeId(long hikeId)
        {
            var tracks = await _repository.GetManyWithFilterAsync(tr => tr.HikeId == hikeId);
            var dtos = new List<TrackGetDTO>();
            foreach (var track in tracks)
            {
                var dto = _mapper.Map<TrackGetDTO>(track);
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<long> Create(TrackCreateDTO dto)
        {
            if (dto is null) throw new ArgumentNullException("DTO can not be null");
            var hikeService = _serviceProvider.GetService<IHikeService>();
            var hike = await hikeService.GetById(dto.HikeId) ?? throw new Exception("Incorrect hikeId");
            var maxDistance = hike.MaxDistanceKm;
            var userPoints = new List<UserPoint>();
            var points = (await hikeService.GetAllPoints(hike.Id)).ToList();

            foreach (var point in points)
            {
                var userPoint = new UserPoint();
                userPoint.Priority = point.Priority;
                userPoint.GeoPoint = point.GeoPoint;
                userPoints.Add(userPoint);
            }

            var track = _mapper.Map<Track>(dto);
            var routePlanner = new RoutePlanner();

            var startPoint = hike.StartPoint;
            var endPoint = hike.EndPoint;

            track.GpxFile = await routePlanner.BuildGpxAsync(userPoints, maxDistance, startPoint, endPoint);
            var id = await _repository.AddAsync(track);
            return id;
        }

        public async Task<bool> AccessAsAdmin(long trackId, long userId)
        {
            var track = await this.GetById(trackId);
            var hikeService = _serviceProvider.GetService<IHikeService>();
            return await hikeService.AccessAsAdmin(track.HikeId, userId);
        }

        public async Task<bool> AccessAsMember(long trackId, long userId)
        {
            var track = await this.GetById(trackId);
            var hikeService = _serviceProvider.GetService<IHikeService>();
            return await hikeService.AccessAsMember(track.HikeId, userId);
        }

        public async Task<bool> AccessAsAdminCreate(long hikeId, long userId)
        {
            var hikeService = _serviceProvider.GetService<IHikeService>();
            return await hikeService.AccessAsMember(hikeId, userId);
        }
    }
}
