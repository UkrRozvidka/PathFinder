using BLL.DTOs.Point;
using BLL.DTOs.Track;
using BLL.DTOs.User;
using BLL.Services;
using BLL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.API.Controllers
{
    [ApiController]
    [Route("api/track")]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _trackService;
        public TrackController(ITrackService trackService)
        {
            _trackService = trackService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackGetDTO>> GetTrackById(long id)
        {
            try
            {
                var track = await _trackService.GetById(id);
                return Ok(track);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> DownloadGpx(long id)
        {
            try
            {
                var track = await _trackService.GetById(id);

                if (track == null)
                    return NotFound("Маршрут відсутній");

                if (track.GpxFile == null || track.GpxFile.Length == 0)
                    return NotFound("Файл GPX небув створений");

                return File(
                    fileContents: track.GpxFile,
                    contentType: "application/gpx+xml",
                    fileDownloadName: $"{track.FileName}.gpx"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<ActionResult<long>> AddTrack(TrackCreateDTO dto)
        {
            try
            {
                var id = await _trackService.Create(dto);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrack(long id)
        {
            try
            {
                await _trackService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
