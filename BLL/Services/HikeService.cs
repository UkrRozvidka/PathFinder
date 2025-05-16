using AutoMapper;
using BLL.DTOs.Hike;
using BLL.DTOs.Point;
using BLL.Services.Contracts;
using DAL.Entities;
using DAL.Repository.Contracts;
using IServiceProvider = BLL.Services.Contracts.IServiceProvider;


namespace BLL.Services
{
    public class HikeService : BaseService<Hike>, IHikeService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        private readonly IPointService _pointService;
        public HikeService(IMapper mapper, IGenericRepository<Hike> repository,
             IUserService userService,
             IPointService pointService,
             IServiceProvider serviceProvider) 
            : base(mapper, repository)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            _pointService = pointService;
        }

        public async Task<HikeGetDTO> GetById(long id)
        {
            var hike = await _repository.GetByIdAsync(id) ?? throw new Exception($"Hike with id = {id} not found");
            var dto = _mapper.Map<HikeGetDTO>(hike);
            return dto;
        }

        public async Task<IEnumerable<HikeGetDTO>> GetAllByAdminId(long adminId)
        {
            var hikes = await _repository.GetManyWithFilterAsync(hike => hike.AdminId == adminId);
            List<HikeGetDTO> dtos = new List<HikeGetDTO>();
            foreach (var hike in hikes)
            {
                var dto = _mapper.Map<HikeGetDTO>(hike);
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<IEnumerable<HikeGetDTO>> GetAllByUserId(long userId)
        {
            var hikeMemberService = _serviceProvider.GetService<IHikeMemberService>();
            var hikeMembers = await hikeMemberService.GetAllByUserId(userId);
           
            List<HikeGetDTO> dtos = new List<HikeGetDTO>();
            foreach (var hikeMember in hikeMembers)
            {
                var hike = await _repository.GetByIdAsync(hikeMember.HikeId);
                var dto = _mapper.Map<HikeGetDTO>(hike);
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<long> Create(HikeCreateDTO dto, long adminId)
        {
            if (dto is null) throw new ArgumentNullException("DTO cannot be null.");
            var hike = _mapper.Map<Hike>(dto);
            hike.AdminId = adminId;
            var hikeId = await _repository.AddAsync(hike);
            return hikeId;
        }

        public async Task Update(long id, HikeUpdateDTO dto)
        {
            var hike = await _repository.GetByIdAsync(id)
                ?? throw new Exception($"Hike with id = {id} not found");
            hike.Start = dto.StartPoint;
            hike.End = dto.EndPoint;
            hike.MaxDistanceKm = dto.MaxDistanceKm;
            hike.Name = dto.Name;
            await _repository.UpdateAsync(hike);
        }

        public async Task<HikeGetFullDTO> GetFullById (long id)
        {
            var hike = await _repository.GetByIdAsync(id)
               ?? throw new Exception($"Hike with id = {id} not found");
            var dto = _mapper.Map<HikeGetFullDTO>(hike);
            dto.AdminName = (await _userService.GetById(hike.AdminId)).UserName;
            var hikeMemberService = _serviceProvider.GetService<IHikeMemberService>();
            var trackService = _serviceProvider.GetService<ITrackService>();

            dto.HikeMembers = (await hikeMemberService.GetAllFullByHikeId(hike.Id)).ToList();
            dto.Tracks = (await trackService.GetAllByHikeId(hike.Id)).ToList();
            return dto;
        }

        public async Task<IEnumerable<PointGetDTO>> GetAllPoints(long id)
        {
            var hike = await _repository.GetByIdAsync(id)
               ?? throw new Exception($"Hike with id = {id} not found");
            var hikeMemberService = _serviceProvider.GetService<IHikeMemberService>();

            var hikeMembers = (await hikeMemberService.GetAllByHikeId(id)).ToList();
            var points = new List<PointGetDTO>();
            foreach (var hikeMember in hikeMembers)
            {
                var hikeMemberPoints = (await _pointService.GetAllByHikeMemberId(hikeMember.Id)).ToList();
                points.AddRange(hikeMemberPoints);
            }
            return points;
        }

        public async Task<bool> AccessAsAdmin(long hikeId, long userId)
        {
            var hike = await this.GetById(hikeId);
            return hike.AdminId == userId;
        }

        public async Task<bool> AccessAsMember(long hikeId, long userId)
        {
            var hike = await this.GetFullById(hikeId);
            foreach(var member in hike.HikeMembers)
            {
                if (member.UserId == userId) return true;
            }
            return false;
        }
    }
}
