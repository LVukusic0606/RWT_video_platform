using Microsoft.AspNetCore.Mvc;

namespace RwtVideos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult Hello()
        {
            return Ok("Pozdrav iz RwtVideos API-ja!");
        }
    }
}