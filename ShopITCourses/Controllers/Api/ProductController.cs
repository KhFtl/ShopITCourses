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

        public ProductController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
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
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetProductId(int id)
        { 
            Product? product = _db.Product.Include(x => x.Category).Where(u => u.Id == id).FirstOrDefault();
            if (product == null)
            {
                return NoContent();
            }
            return Ok(product);
        }

        [HttpPut] //Занесення даних в базу даних
        public IActionResult PutProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                return BadRequest();
            }
            return BadRequest();
        }
    }
}
