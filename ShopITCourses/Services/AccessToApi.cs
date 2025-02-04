using Microsoft.AspNetCore.Identity;
using ShopITCourses.Data;
using ShopITCourses.Services.IServices;

namespace ShopITCourses.Services
{
    public class AccessToApi : IAccessToApi
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccessToApi(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CanAccess(string token, string role)
        {
            var user = await _userManager.FindByIdAsync(token);
            if (user != null)
            {
                return await _userManager.IsInRoleAsync(user, role);
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ValidateToken(string token)
        {
            var user = await _userManager.FindByIdAsync(token);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
