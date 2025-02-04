using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopITCourses.Data;
using ShopITCourses.Models;
using ShopITCourses.Models.ViewModel;
using ShopITCourses.Services.IServices;

namespace ShopITCourses.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IAccessToApi _accessToApi;

        public ProductController(ILogger<HomeController> logger, ApplicationDbContext db, IAccessToApi accessToApi)
        {
            _logger = logger;
            _db = db;
            _accessToApi = accessToApi;
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> GetAsync(string token)
        {
            if (!await _accessToApi.ValidateToken(token))
            {
                return Unauthorized();
            }

            HomeVM homeVM = new HomeVM();
            homeVM.Products = _db.Product;
            homeVM.Categorys = _db.Category;

            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            foreach (var prod in homeVM.Products)
            {
                if (prod.Image != null)
                {
                    prod.Image = baseUrl + WC.ImagePath + prod.Image;
                    prod.Category = homeVM.Categorys.FirstOrDefault(x => x.Id == prod.CategoryId);
                }
            }
            return Ok(homeVM);
        }

        [HttpGet("{id:int}/{token}")]
        public async Task<IActionResult> GetProductId(int id, string token)
        {
            if (!await _accessToApi.ValidateToken(token))
            {
                return Unauthorized();
            }
            Product? product = _db.Product.Include(x => x.Category).Where(u => u.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NoContent();
            }
            return Ok(product);
        }

        [HttpPut("{token}")] //Занесення даних в базу даних
        public async Task<IActionResult> PutProduct([FromBody] Product product, string token)
        {
            if (!await _accessToApi.CanAccess(token, "Admin"))
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
