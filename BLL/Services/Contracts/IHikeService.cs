using BLL.DTOs.Hike;
using BLL.DTOs.Point;

namespace BLL.Services.Contracts
{
    public interface IHikeService
    {
        Task<bool> AccessAsAdmin(long hikeId, long userId);
        Task<bool> AccessAsMember(long hikeId, long userId);
        Task<long> Create(HikeCreateDTO dto, long adminId);
        Task Delete(long id);
        Task<IEnumerable<HikeGetDTO>> GetAllByAdminId(long adminId);
        Task<IEnumerable<HikeGetDTO>> GetAllByUserId(long userId);
        Task<IEnumerable<PointGetDTO>> GetAllPoints(long id);
        Task<HikeGetDTO> GetById(long id);
        Task<HikeGetFullDTO> GetFullById(long id);
        Task Update(long id, HikeUpdateDTO dto);
    }
}