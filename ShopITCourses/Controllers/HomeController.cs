using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ShopITCourses.Data;
using ShopITCourses.Models;
using ShopITCourses.Models.ViewModel;
using ShopITCourses.Utility;

namespace ShopITCourses.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            homeVM.Products = _db.Product;
            homeVM.Categorys = _db.Category;

            List<ShopingCart> shopingCartList = GetShopingCartSession();
            List<DetailsVM> detailsVMs = new List<DetailsVM>();
            foreach (var product in homeVM.Products)
            { 
                var exist = shopingCartList.FirstOrDefault(x => x.ProductId == product.Id);
                if (exist != null)
                {
                    detailsVMs.Add(new DetailsVM { Product = product, ExistsInCart = true });
                }
                else
                {
                    detailsVMs.Add(new DetailsVM { Product = product, ExistsInCart = false });
                }
            }
            ViewData["DetailsVM"] = detailsVMs;
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShopingCart> shopingCartList = GetShopingCartSession();
            DetailsVM detailsVM = new DetailsVM()
            {
                Product = _db.Product.Include(x => x.Category).Where(u => u.Id == id).FirstOrDefault(),
                ExistsInCart = false
            };
            foreach (var item in shopingCartList)
            {
                if (item.ProductId == id)
                {
                    detailsVM.ExistsInCart = true;
                }
            }
            return View(detailsVM);
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShopingCart> shopingCartList = GetShopingCartSession();
            shopingCartList.Add(new ShopingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFromCart(int id)
        { 
            List<ShopingCart> shopingCartList = GetShopingCartSession();
            var itemRemove = shopingCartList.SingleOrDefault(x => x.ProductId == id);
            if (itemRemove != null)
            { 
                shopingCartList.Remove(itemRemove);
            }
            HttpContext.Session.Set(WC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }
        //Метод для отримання товарів з сесії
        private List<ShopingCart> GetShopingCartSession()
        {
            List<ShopingCart> shopingCartList = new List<ShopingCart>();
            //Якщо сессія не пуста, тобто там були товари
            if (HttpContext.Session.Get<List<ShopingCart>>(WC.SessionCart) != null && HttpContext.Session.Get<List<ShopingCart>>(WC.SessionCart).Count() > 0)
            {
                shopingCartList = HttpContext.Session.Get<List<ShopingCart>>(WC.SessionCart);
            }
            return shopingCartList;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
