using AutoMapper;
using BLL.DTOs.Point;
using BLL.Services.Contracts;
using DAL.Entities;
using DAL.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PointService : BaseService<Point>, IPointService
    {
        private readonly Lazy<IHikeMemberService> _hikeMemberService;

        public PointService(
            IMapper mapper,
            IGenericRepository<Point> repository,
            Lazy<IHikeMemberService> hikeMemberService)
            : base(mapper, repository)
        {
            _hikeMemberService = hikeMemberService;
        }

        public async Task<PointGetDTO> GetById(long id)
        {
            var point = await _repository.GetByIdAsync(id)
                ?? throw new Exception("Point not found");
            return _mapper.Map<PointGetDTO>(point);
        }

        public async Task<long> Create(PointCreateDTO dto, long userId)
        {
            if (dto is null) throw new ArgumentNullException("Dto cannot be null");
            var hikeMember = await _hikeMemberService.Value.GetByHikeUserId(dto.HikeId, userId) 
                ?? throw new Exception("HikeMember not found.");
            if (!await IsAllowablePriority(hikeMember.Id, dto.Priority))
                throw new Exception("The priority should not be repeated and should be between 1 and 10");
            var point = new Point()
            {
                GeoPoint = dto.GeoPoint,
                Priority = dto.Priority,
                HikeMemberId = hikeMember.Id
            };
            return await _repository.AddAsync(point);
        }

        public async Task Update(long id, PointUpdateDTO dto)
        {
            if (dto is null) throw new ArgumentNullException("Dto cannot be null");

            var point = await _repository.GetByIdAsync(id)
                ?? throw new Exception($"Point with id = {id} not found");

            if (!await IsAllowablePriority(point.HikeMemberId, dto.Priority))
                throw new Exception("The priority should not be repeated and should be between 1 and 10");

            point.Priority = dto.Priority;
            point.GeoPoint = dto.GeoPoint;
            await _repository.UpdateAsync(point);
        }

        public async Task<IEnumerable<PointGetDTO>> GetAllByHikeMemberId(long hikeMemberId)
        {
            var points = await _repository.GetManyWithFilterAsync(hm => hm.HikeMemberId == hikeMemberId);
            return points.Select(p => _mapper.Map<PointGetDTO>(p)).ToList();
        }

        public async Task<bool> AccessAsAdmin(long pointId, long userId)
        {
            var point = await this.GetById(pointId);
            var hikeMember = await _hikeMemberService.Value.GetById(point.HikeMemberId);
            return await _hikeMemberService.Value.AccessAsAdmin(hikeMember.HikeId, userId);
        }

        public async Task<bool> AccessAsMember(long pointId, long userId)
        {
            var point = await this.GetById(pointId);
            var hikeMember = await _hikeMemberService.Value.GetById(point.HikeMemberId);
            return await _hikeMemberService.Value.AccessAsMember(hikeMember.HikeId, userId);
        }

        public async Task<bool> AccessAsMemberCreate(long hikeId, long userId)
        {
            return await _hikeMemberService.Value.AccessAsMemberByHikeId(hikeId, userId);
        }

        private async Task<bool> IsAllowablePriority(long hikeMemberId, int priority)
        {
            if (priority < 1 || priority > 10) return false;

            var points = await this.GetAllByHikeMemberId(hikeMemberId);
            return points.All(p => p.Priority != priority);
        }
    }
}
            