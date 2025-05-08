using AutoMapper;
using BLL.DTOs.HikeMember;
using BLL.Services.Contracts;
using DAL.Entities;
using DAL.Repository.Contracts;
using System.Security.AccessControl;
using IServiceProvider = BLL.Services.Contracts.IServiceProvider;

namespace BLL.Services
{
    public class HikeMemberService : BaseService<HikeMember>, IHikeMemberService
    {

        private readonly IUserService _userService;
        private readonly IPointService _pointService;
        private readonly IServiceProvider _serviceProvider;

        public HikeMemberService(IMapper mapper, IGenericRepository<HikeMember> repository,
             IUserService userService, IPointService pointService, IServiceProvider serviceProvider)
            : base(mapper, repository)
        {
            _serviceProvider = serviceProvider;
            _userService = userService;
            _pointService = pointService;
        }

        public async Task<HikeMemberGetDTO> GetById(long id)
        {
            var hikeMember = await _repository.GetByIdAsync(id)
                ?? throw new Exception($"HikeMember with id = {id} not found");
            var dto = _mapper.Map<HikeMemberGetDTO>(hikeMember);
            return dto;
        }

        public async Task<HikeMemberGetFullDTO> GetFullById(long id)
        {
            var hikeMember = await _repository.GetByIdAsync(id)
                ?? throw new Exception($"HikeMember with id = {id} not found");
            var dto = _mapper.Map<HikeMemberGetFullDTO>(hikeMember);
            dto.Points = (await _pointService.GetAllByHikeMemberId(hikeMember.Id)).ToList();
            return dto;
        }

        public async Task<long> Create(HikeMemberCreateDTO dto)
        {
            if (dto is null) throw new ArgumentNullException("DTO cannot be null.");
            var user = await _userService.GetByUsername(dto.UserName) ?? throw new Exception($"Hike with id = {dto.UserName} not found");
            var hikeService = _serviceProvider.GetService<IHikeService>();
            var hike = await hikeService.GetById(dto.HikeId)
                   ?? throw new Exception($"Hike with id = {dto.HikeId} not found");
            var hikeMember = _mapper.Map<HikeMember>(dto);
            hikeMember.UserId = user.Id;
            var hikeMemberId = await _repository.AddAsync(hikeMember);
            return hikeMemberId;
        }

        public async Task<IEnumerable<HikeMemberGetDTO>> GetAllByHikeId (long hikeId)
        {
            var hikeMembers = await _repository.GetManyWithFilterAsync(hm => hm.HikeId == hikeId);
            var dtos = new List<HikeMemberGetDTO>();
            foreach (var hikeMember in hikeMembers)
            {
                var dto = _mapper.Map<HikeMemberGetDTO>(hikeMember);
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<IEnumerable<HikeMemberGetDTO>> GetAllByUserId(long userId)
        {
            var hikeMembers = await _repository.GetManyWithFilterAsync(hm => hm.UserId == userId);
            var dtos = new List<HikeMemberGetDTO>();
            foreach (var hikeMember in hikeMembers)
            {
                var dto = _mapper.Map<HikeMemberGetDTO>(hikeMember);
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<IEnumerable<HikeMemberGetFullDTO>> GetAllFullByHikeId(long hikeId)
        {
            var hikeMembers = await _repository.GetManyWithFilterAsync(hm => hm.HikeId == hikeId);
            var dtos = new List<HikeMemberGetFullDTO>();
            var hikeService = _serviceProvider.GetService<IHikeService>();
            foreach (var hikeMember in hikeMembers)
            {
                var dto = _mapper.Map<HikeMemberGetFullDTO>(hikeMember);
                dto.UserName = (await _userService.GetById(hikeMember.UserId)).UserName;
                dto.HikeName = (await hikeService.GetById(hikeMember.HikeId)).Name; 
                dto.Points = (await _pointService.GetAllByHikeMemberId(hikeMember.Id)).ToList();
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<bool> AccessAsAdmin(long hikeId, long userId)
        {
            var hikeService = _serviceProvider.GetService<IHikeService>();
            return await hikeService.AccessAsAdmin(hikeId, userId);
        }

        public async Task<bool> AccessAsMember(long hikeId, long userId)
        {
            var hikeService = _serviceProvider.GetService<IHikeService>();
            return await hikeService.AccessAsMember(hikeId, userId);
        }

    }
}
