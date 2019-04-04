using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: User
        public ActionResult Index()
        {
            var users = db.Users.Select(x => new
            {
                UserId = x.Id,
                Username = x.UserName,
                Email = x.Email,
                LastLogin = x.LastLoginTime,
                RegisterDate = x.RegistrationDate,
                RoleNames = x.Roles.Join(db.Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r }).Select(ur => ur.r.Name).ToList()
            }).ToList()
            .Select(x => new UserListModel
            {
                Email = x.Email,
                UserId = x.UserId,
                LastLoginTime = x.LastLogin,
                RegistrationDate = x.RegisterDate,
                Roles = string.Join(", ", x.RoleNames)
            }).ToList();

            return View(users);
        }
        public ActionResult Edit(string UserId)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var user = db.Users.Where(x => x.Id == UserId).Select(x => new UserRolesModel
            {
                UserId = x.Id,
                Email = x.Email,
                Roles = x.Roles.Join(db.Roles, u => u.RoleId, r => r.Id, (u, r) => new { u, r }).Select(ur => ur.r.Name).ToList()
            }).FirstOrDefault();
            ViewBag.Roles = new SelectList(roleManager.Roles, "Name", "Name");
            return View(user);
        }
        public ActionResult Roles()
        {
            var roles = db.Roles.OrderBy(x => x.Name).ToList();
            return View(roles);
        }
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateRole(string RoleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
          

            if (RoleName != "")
            {
                if (!roleManager.RoleExists(RoleName))
                {  
                    var role = new IdentityRole();
                    role.Name = RoleName;
                    roleManager.Create(role);
                }
            }
            return RedirectToAction("Roles");
        }
        public ActionResult DeleteRole(string RoleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            if (RoleName != "")
            {
                if (roleManager.RoleExists(RoleName))
                {
                    roleManager.Delete(roleManager.FindByName(RoleName));
                }
            }
            return RedirectToAction("Roles");
        }
        public ActionResult RemoveUserRole(string UserId, string RoleName)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = db.Users.Find(UserId);
            if (user != null)
            {
                UserManager.RemoveFromRole(UserId, RoleName);
            }
            return RedirectToAction("Edit", new { UserId = UserId });
        }
        [HttpPost]
        public ActionResult AddUserRole(string UserId, string Roles)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = db.Users.Find(UserId);
            if (user != null)
            {
                UserManager.AddToRole(UserId, Roles);
            }
            return RedirectToAction("Edit", new { UserId = UserId });
        }
    }
}