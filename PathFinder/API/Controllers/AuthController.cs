using BLL.DTOs.User;
using BLL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{

    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthDTO dto)
        {
            try
            {
                var id = await _userService.Create(dto);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }       
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthDTO dto)
        {
            try
            {
                var user = await _userService.ValidateUserAsync(dto.UserName, dto.Password);
                if (user == null) return Unauthorized();

                var token = _userService.GenerateJwtToken(user);

                Response.Cookies.Append("access_token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(2)
                });

                return Ok("Logged in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("access_token");
                return Ok("Logged out");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var token = Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                    return Unauthorized();

                var user = await _userService.GetCurrentUserAsync(token);
                if (user == null)
                    return Unauthorized();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
