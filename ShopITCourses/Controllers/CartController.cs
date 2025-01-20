using Microsoft.AspNetCore.Mvc;
using ShopITCourses.Data;
using ShopITCourses.Models;
using ShopITCourses.Utility;

namespace ShopITCourses.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<ShopingCart> shopingCarts = ShopingCartSession.GetShopingCartSession(HttpContext);
            List<int> productInCart = shopingCarts.Select(x => x.ProductId).ToList();
            List<Product> prodList = _db.Product.Where(x => productInCart.Contains(x.Id)).ToList();
            return View(prodList);
        }

        public IActionResult Remove(int id)
        {
            List<ShopingCart> shopingCarts = ShopingCartSession.GetShopingCartSession(HttpContext);
            shopingCarts.Remove(shopingCarts.FirstOrDefault(x => x.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shopingCarts);
            return RedirectToAction(nameof(Index));
        }
    }
}
