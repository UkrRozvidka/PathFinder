using BLL.DTOs.Hike;
using BLL.DTOs.HikeMember;
using BLL.DTOs.Point;
using BLL.Services;
using BLL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{
    [ApiController]
    [Route("api/point")]
    public class PointController : ControllerBase
    {
        private IPointService _pointService;

        public PointController(IPointService pointService)
        {
            _pointService = pointService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PointGetDTO>> GetPoint(long id)
        {
            try
            {
                if (await _pointService.AccessAsAdmin(id, User.GetUserId()) ||
                    await _pointService.AccessAsMember(id, User.GetUserId()))
                {
                    var point = await _pointService.GetById(id);
                    return Ok(point);
                }
                return Forbid();            
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePoint(long id, PointUpdateDTO dto)
        {
            try
            {
                if (!await _pointService.AccessAsMember(id, User.GetUserId())) return Forbid();
                await _pointService.Update(id, dto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddPoint(PointCreateDTO dto)
        {
            try
            {
                if (!await _pointService.AccessAsMemberCreate(dto.HikeId, User.GetUserId())) return Forbid();
                var id = await _pointService.Create(dto, User.GetUserId());
                return Ok(id); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePoint(long id)
        {
            try
            {
                if (await _pointService.AccessAsAdmin(id, User.GetUserId()) ||
                    await _pointService.AccessAsMember(id, User.GetUserId()))
                {
                    await _pointService.Delete(id);
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
