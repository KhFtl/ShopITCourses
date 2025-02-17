using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShopITCourses.Models
{
    public class Category
    {
        [Key]
        [DisplayName("Унікальний ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Вкажіть ім'я категорії")]
        [DisplayName("Ім'я категорії")]
        public string CategoryName { get; set; } = "";
        [DisplayName("Порядок відображення")]
        [Range(1, int.MaxValue, ErrorMessage ="Значення {0} повинно бути між {1} та {2}")]
        [Required(ErrorMessage ="Вкажіть порядок")]
        public int DisplayOrder { get; set; }
    }
}
