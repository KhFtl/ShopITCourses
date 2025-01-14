namespace ShopITCourses.Models.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<Category>? Categorys { get; set; }
    }
}
