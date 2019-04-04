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
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize (Roles = "Admin")]
        public ActionResult Index()
        {
            //createRolesandUsers();
            return View();
        }
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
                //Here we create a Admin super user who will maintain the website                   
                var user = UserManager.FindByEmail("huseyin@avniyalcin.com");
            if (user != null)
            {
                var result1 = UserManager.AddToRole(user.Id, "Admin");
            }


        }
    }
}