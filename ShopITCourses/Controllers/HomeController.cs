using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopITCourses.DAL.Repository.IRepository;
using ShopITCourses.Data;
using ShopITCourses.Models;
using ShopITCourses.Models.ViewModel;
using ShopITCourses.Services;
using ShopITCourses.Services.IServices;
using ShopITCourses.Utility;

namespace ShopITCourses.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        //private readonly ApplicationDbContext _db;
        private readonly ICategoryRepository _catRepo;
        private readonly IProductRepository _productRepo;
        private readonly ICurrentUser _currentUser;

        //public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, ICurrentUser currentUser)
        //{
        //    _logger = logger;
        //    _db = db;
        //    _currentUser = currentUser;
        //}

        public HomeController(ICategoryRepository catRepo, IProductRepository productRepo)
        {
            _catRepo = catRepo;
            _productRepo = productRepo;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM();
            //homeVM.Products = _db.Product;
            //homeVM.Categorys = _db.Category;
            homeVM.Products = _productRepo.GetAll(includeProperties: "Category");
            homeVM.Categorys = _catRepo.GetAll();

            List<ShopingCart> shopingCartList = ShopingCartSession.GetShopingCartSession(HttpContext);
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
            List<ShopingCart> shopingCartList = ShopingCartSession.GetShopingCartSession(HttpContext);
            DetailsVM detailsVM = new DetailsVM()
            {
                //Product = _db.Product.Include(x => x.Category).Where(u => u.Id == id).FirstOrDefault(),
                Product = _productRepo.FirstOrDefault(x => x.Id == id, "Category"),
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
            List<ShopingCart> shopingCartList = ShopingCartSession.GetShopingCartSession(HttpContext);
            shopingCartList.Add(new ShopingCart { ProductId = id });
            HttpContext.Session.Set(WC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFromCart(int id)
        { 
            List<ShopingCart> shopingCartList = ShopingCartSession.GetShopingCartSession(HttpContext);
            var itemRemove = shopingCartList.SingleOrDefault(x => x.ProductId == id);
            if (itemRemove != null)
            { 
                shopingCartList.Remove(itemRemove);
            }
            HttpContext.Session.Set(WC.SessionCart, shopingCartList);
            return RedirectToAction(nameof(Index));
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
