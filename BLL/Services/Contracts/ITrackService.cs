using BLL.DTOs.Track;

namespace BLL.Services.Contracts
{
    public interface ITrackService
    {
        Task<bool> AccessAsAdmin(long trackId, long userId);
        Task<bool> AccessAsAdminCreate(long hikeId, long userId);
        Task<bool> AccessAsMember(long trackId, long userId);
        Task<long> Create(TrackCreateDTO dto);
        Task Delete(long id);
        Task<IEnumerable<TrackGetDTO>> GetAllByHikeId(long hikeId);
        Task<TrackGetDTO> GetById(long id);
    }
}