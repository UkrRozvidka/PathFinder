using BLL.DTOs.User;
using DAL.Entities;

namespace BLL.Services.Contracts
{
    public interface IUserService
    {
        Task<long> Create(AuthDTO dto);
        string GenerateJwtToken(User user);
        Task<UserGetDTO> GetById(long id);
        Task<UserGetDTO> GetByUsername(string username);
        Task<UserGetDTO> GetCurrentUserAsync(string token);
        Task<User?> ValidateUserAsync(string username, string password);
    }
}