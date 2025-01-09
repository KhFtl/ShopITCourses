using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopITCourses.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Вкажіть ім'я товару")]
        [DisplayName("Ім'я товару")]
        public string Name { get; set; } = "";

        [DisplayName("Опис товару")]
        public string? Description { get; set; }

        [Required(ErrorMessage ="Вкажіть ціну товару")]
        [Range(1, double.MaxValue)]
        [DisplayName("Ціна товару")]
        public double Price { get; set; }

        [DisplayName("Зображення товару")]
        public string? Image { get; set; }

        [Required(ErrorMessage = "Оберіть категорію")]
        [DisplayName("Категорія товару")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
