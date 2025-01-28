using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopITCourses.Data;
using ShopITCourses.Models;
using ShopITCourses.Services.IServices;
using System.Security.Claims;

namespace ShopITCourses.Services
{
    //Сервіс для отримання даних про автентифікованого користувача
    public class CurrentUser : ICurrentUser
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(UserManager<IdentityUser> userManager, ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<IdentityUser?> GetCurrentIdentityUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return null;
            }
            return _userManager.FindByIdAsync(userId);
        }

        public Task<ShopUser?> GetCurrentShopUserAsync()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return null;
            }
            return _db.ShopUsers.FirstOrDefaultAsync(x => x.Id == userId);
        }
        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
