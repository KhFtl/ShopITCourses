using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ShopITCourses.Controllers.Admin
{
    public class ManagerUsers : Controller
    {
        private readonly UserManager<IdentityUser> _usersManager;
        private readonly RoleManager<IdentityRole> _roleManager; 
        
        public ManagerUsers(UserManager<IdentityUser> usersManager, RoleManager<IdentityRole> roleManager)
        {
            _usersManager = usersManager;
            _roleManager = roleManager;
        }

       //GET: Users
       public async Task<IActionResult> Index(string searchBy=null, string searchValue=null)
       {
           var users = _usersManager.Users.ToList();
           if (!string.IsNullOrEmpty(searchValue))
           {
               if (searchBy == "Username")
               {
                   users = users.Where(x => x.UserName.Contains(searchBy)).ToList();
               }
               else if (searchBy == "Role")
               {
                   var usersInRole = new List<IdentityUser>();
                   foreach (var user in users)
                   {
                       var roles = await _usersManager.GetRolesAsync(user);
                       if (roles.Contains(searchValue))
                       {
                           usersInRole.Add(user);
                       }
                   }
                   users = usersInRole;
               }
           }
           var userRoles = new Dictionary<string, IList<string> > ();
           foreach (var user in users)
           {
               userRoles[user.Id] = await _usersManager.GetRolesAsync(user);
           }
           ViewBag.UserRoles = userRoles;
           return View(users);
       }
    }
}
