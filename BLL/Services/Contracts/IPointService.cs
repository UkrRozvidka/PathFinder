using BLL.DTOs.Point;

namespace BLL.Services.Contracts
{
    public interface IPointService
    {
        Task<bool> AccessAsAdmin(long pointId, long userId);
        Task<bool> AccessAsMember(long pointId, long userId);
        Task<bool> AccessAsMemberCreate(long hikeMemberId, long userId);
        Task<long> Create(PointCreateDTO dto, long userId);
        Task Delete(long id);
        Task<IEnumerable<PointGetDTO>> GetAllByHikeMemberId(long hikeMemberId);
        Task<PointGetDTO> GetById(long id);
        Task Update(long id, PointUpdateDTO dto);
    }
}