using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RwtVideos.Api.DTOs;
using RwtVideos.Api.Services;

namespace RwtVideos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            await _userService.RegisterAsync(dto.Name, dto.Email, dto.Password);

            return Ok(new
            {
                message = "Korisnik registriran. Čeka odobrenje admina."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var auth = await _userService.LoginAsync(dto.Email, dto.Password);
            if (auth == null) return Unauthorized("Neispravan email ili lozinka");

            return Ok(auth);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingUsers()
        {
            var pendingUsers = await _userService.GetPendingAsync();
            return Ok(pendingUsers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var approved = await _userService.ApproveAsync(id);

            if (!approved)
            {
                return NotFound("Korisnik nije pronađen");
            }

            return Ok(new
            {
                message = $"Korisnik je odobren"
            });
        }
    }
}