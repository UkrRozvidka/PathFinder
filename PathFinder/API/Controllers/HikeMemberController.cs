using BLL.DTOs.Hike;
using BLL.DTOs.HikeMember;
using BLL.DTOs.Point;
using BLL.Services;
using BLL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{
    [ApiController]
    [Route("api/hikeMember")]
    public class HikeMemberController : ControllerBase
    {
        private readonly IHikeMemberService _hikeMemberService;

        public HikeMemberController(IHikeMemberService hikeMemberService)
        {
            _hikeMemberService = hikeMemberService;
        }   

        [HttpPost]
        public async Task<ActionResult> AddHikeMember(HikeMemberCreateDTO dto)
        {
            try
            {
                if(!await _hikeMemberService.AccessAsAdminByHikeId(dto.HikeId, User.GetUserId())) 
                    return Forbid();
                var id = await _hikeMemberService.Create(dto);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHikeMember(long id)
        {
            try
            {
                if (await _hikeMemberService.AccessAsAdmin(id, User.GetUserId()) ||
                   await _hikeMemberService.AccessAsMember(id, User.GetUserId()))
                    {
                    await _hikeMemberService.Delete(id);
                    return Ok();
                }
                return Forbid();               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
