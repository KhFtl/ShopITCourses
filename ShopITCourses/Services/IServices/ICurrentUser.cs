using Microsoft.AspNetCore.Identity;
using ShopITCourses.Models;

namespace ShopITCourses.Services.IServices
{
    public interface ICurrentUser
    {
        Task<ShopUser?> GetCurrentShopUserAsync();
        Task<IdentityUser?> GetCurrentIdentityUserAsync();
        string? GetAuthenticationMethod();
        string? GetProfilePictureUrl();
    }
}
