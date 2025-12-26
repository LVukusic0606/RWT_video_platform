using Microsoft.AspNetCore.Mvc;
using RwtVideos.Api.Models;
using RwtVideos.Api.DTOs;
using System.Collections.Generic;

namespace RwtVideos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>();
        private static int nextId = 1;

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterUserDto dto)
        {
            var newUser = new User
            {
                Id = nextId++,
                Name = dto.Name,
                Email = dto.Email,
                IsApproved = false
            };

            users.Add(newUser);

            return Ok(new
            {
                message = "Korisnik registriran. Čeka odobrenje admina."
            });
        }

        [HttpGet("pending")]
        public IActionResult GetPendingUsers()
        {
            var pendingUsers = users.Where(u => !u.IsApproved).ToList();
            return Ok(pendingUsers);
        }

        [HttpPost("approve/{id}")]
        public IActionResult ApproveUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);

            if (user == null) 
            {
                return NotFound("Korisnik nije pronađen.");
            }

            user.IsApproved = true;

            return Ok(new
            {
                message = $"Korisnik {user.Name} je odobren"
            });
        }
    }
}