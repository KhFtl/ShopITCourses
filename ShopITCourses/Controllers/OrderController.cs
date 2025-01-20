using Microsoft.AspNetCore.Mvc;

namespace ShopITCourses.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
