using AutoMapper;
using BLL.DTOs.User;
using BLL.Services.Contracts;
using DAL.Entities;
using DAL.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BLL.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IConfiguration _config;
        public UserService(IMapper mapper, IGenericRepository<User> repository,
            IConfiguration configuration) : base(mapper, repository)
        {
            _config = configuration;
        }

        public async Task<UserGetDTO> GetById(long id)
        {
            var user = await _repository.GetByIdAsync(id) ?? throw new Exception($"User with id = {id} not found");
            var dto = _mapper.Map<UserGetDTO>(user);
            return dto;
        }

        public async Task<UserGetDTO> GetByUsername(string username)
        {
            var user = await _repository.GetOneWithFilterAsync(user => username == user.UserName);
            if (user is null) throw new Exception("User not found");
            var dto = _mapper.Map<UserGetDTO>(user);
            return dto;
        }

        public async Task<long> Create(AuthDTO dto)
        {
            if (dto is null) throw new ArgumentNullException("DTO can not be null");
            var existingUser = await _repository.GetOneWithFilterAsync(u => u.UserName == dto.UserName);
            if (existingUser != null) throw new ArgumentNullException("User already exist");
            var user = _mapper.Map<User>(dto);
            user.PasswordHash = HashPassword(dto.Password);
            var userId = await _repository.AddAsync(user);
            return userId;
        }

        public async Task<UserGetDTO> GetCurrentUserAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
                return null;

            var userId = int.Parse(jsonToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
            var user = await this.GetById(userId);

            if (user == null)
                return null;

            return new UserGetDTO
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _repository.GetOneWithFilterAsync(u => u.UserName == username);
            return user is not null && VerifyPassword(password, user.PasswordHash) ? user : null;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
