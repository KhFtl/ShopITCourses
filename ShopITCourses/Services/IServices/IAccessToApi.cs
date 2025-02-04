namespace ShopITCourses.Services.IServices
{
    public interface IAccessToApi
    {
        Task<bool> ValidateToken(string token);
        Task<bool> CanAccess(string token, string role);
    }
}
