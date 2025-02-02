using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopITCourses.Data;
using ShopITCourses.Services.IServices;

namespace ShopITCourses.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public ProductController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("All dsfsdfds OK");
        }
    }
}
