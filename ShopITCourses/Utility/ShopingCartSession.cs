using ShopITCourses.Models;

namespace ShopITCourses.Utility
{
    public static class ShopingCartSession
    {
        public static List<ShopingCart> GetShopingCartSession(HttpContext httpContext)
        {
            List<ShopingCart> shopingCartList = new List<ShopingCart>();
            //Якщо сессія не пуста, тобто там були товари
            var sessionCart = httpContext.Session.Get<List<ShopingCart>>(WC.SessionCart);
            if (sessionCart != null && sessionCart.Count > 0)
            {
                shopingCartList = sessionCart;
            }
            return shopingCartList;
        }
    }
}
