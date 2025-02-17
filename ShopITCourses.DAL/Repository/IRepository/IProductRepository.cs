using ShopITCourses.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ShopITCourses.DAL.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
        IEnumerable<SelectListItem> GetAllDropDownList(string obj);
    }
}
