using BLL.DTOs.Hike;
using BLL.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.API.Controllers
{
    [ApiController]
    [Route("api/hike")]
    public class HikeController : ControllerBase
    {
        private readonly IHikeService _hikeService;

        public HikeController(IHikeService hikeService)
        {
            _hikeService = hikeService;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<HikeGetFullDTO>> GetHikeFull(long id)
        {
            try
            {
                if (await _hikeService.AccessAsAdmin(id, User.GetUserId()) ||
                    await _hikeService.AccessAsMember(id, User.GetUserId()))
                {
                    var hike = await _hikeService.GetFullById(id);
                    return Ok(hike);
                }
                return Forbid();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("admin")]
        public async Task<ActionResult<IEnumerable<HikeGetDTO>>> GetAllByAdminId()
        {
            try
            {
                var hikes = await _hikeService.GetAllByAdminId(User.GetUserId());
                return Ok(hikes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("member")]
        public async Task<ActionResult<IEnumerable<HikeGetDTO>>> GetAllByUserId()
        {
            try
            {
                var hikes = await _hikeService.GetAllByUserId(User.GetUserId());
                return Ok(hikes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<long>> AddHike(HikeCreateDTO dto)
        {
            try
            {
                var id = await _hikeService.Create(dto, User.GetUserId());
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateHike(long id, HikeUpdateDTO dto)
        {
            try
            {
                if (!await _hikeService.AccessAsAdmin(id, User.GetUserId())) return Forbid();
                await _hikeService.Update(id, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteHike(long id)
        {
            try
            {
                if (!await _hikeService.AccessAsAdmin(id, User.GetUserId())) return Forbid();
                await _hikeService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
