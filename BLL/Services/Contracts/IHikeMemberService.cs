using BLL.DTOs.HikeMember;

namespace BLL.Services.Contracts
{
    public interface IHikeMemberService
    {
        Task<bool> AccessAsAdmin(long hikeId, long userId);
        Task<bool> AccessAsMember(long hikeId, long userId);
        Task<long> Create(HikeMemberCreateDTO dto);
        Task Delete(long id);
        Task<IEnumerable<HikeMemberGetDTO>> GetAllByHikeId(long hikeId);
        Task<IEnumerable<HikeMemberGetDTO>> GetAllByUserId(long userId);
        Task<IEnumerable<HikeMemberGetFullDTO>> GetAllFullByHikeId(long hikeId);
        Task<HikeMemberGetDTO> GetById(long id);
        Task<HikeMemberGetFullDTO> GetFullById(long id);
    }
}