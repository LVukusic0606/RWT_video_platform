using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RwtVideos.Api.DTOs;
using RwtVideos.Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RwtVideos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        [RequestSizeLimit(550_000_000)]
        public async Task<IActionResult> Upload([FromForm] UploadVideoDto dto, CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user identity.");
            }

            try
            {
                var createdId = await _videoService.UploadAsync(dto, userId, cancellationToken);
                return CreatedAtAction(nameof(StreamVideo), new { id = createdId }, new { id = createdId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "ApprovedOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var videos = await _videoService.GetAllActiveAsync();
            return Ok(videos);
        }

        [Authorize(Policy = "ApprovedOnly")]
        [HttpGet("{id}/stream")]
        public async Task<IActionResult> StreamVideo(int id)
        {
            var videoFile = await _videoService.GetVideoFileAsync(id);
            if (videoFile == null)
            {
                return NotFound("Video not found.");
            }

            return File(videoFile.Stream, videoFile.ContentType, videoFile.DownloadFileName, enableRangeProcessing: true);
        }
    }
}
