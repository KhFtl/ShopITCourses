using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopITCourses.Data;
using ShopITCourses.Models;

namespace ShopITCourses.Controllers.Admin
{
    [Authorize(Policy = "Admin")]
    public class ManagerUsers : Controller
    {
        private readonly UserManager<IdentityUser> _usersManager;
        private readonly ApplicationDbContext _db;

        public ManagerUsers(UserManager<IdentityUser> usersManager, ApplicationDbContext context)
        {
            _usersManager = usersManager;
            _db = context;
        }

        //GET: Users
        [HttpGet]
        public async Task<IActionResult> Index(string searchBy, string searchValue)
       {
           var users = _usersManager.Users.ToList();
           if (!string.IsNullOrEmpty(searchValue))
           {
               if (searchBy == "Username")
               {
                   users = users.Where(x => x.UserName.Contains(searchValue)).ToList();
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

        //GET: Users/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest("Не вказано ID користувача");
            }
            var user = await _usersManager.FindByIdAsync(id);
            if (user == null)
            { 
                return NotFound("Користувача не знайдено");
            }
            var roles = await _usersManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            ShopUser? shopUser = await _db.ShopUsers.FindAsync(user.Id);
            ViewBag.ShopUser = shopUser;
            return View(user);
        }
    }
}
