using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShopITCourses.Models
{
    public class ShopUser : IdentityUser
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Вкажіть ваше ПІБ")]
        [DisplayName("Ваше ПІБ")]
        public string FullName { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Вкажіть адресу доставки")]
        [DisplayName("Адреса доставки")]
        public string AdressDevivery { get; set; }
    }
}
