using Microsoft.AspNetCore.Mvc;
using RwtVideos.Api.Models;

namespace RwtVideos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult GetTestUser()
        {
            var user = new User
            {
                Id = 1,
                Name = "Luka",
                Email = "lvukusic@uniri.hr",
                IsApproved = true,
                Age = 21
            };
            return Ok(user);
        }
    }
}