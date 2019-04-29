using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class PageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Page
        public ActionResult Index(string PageId)
        {
            var page = db.Pages.Find(PageId);
            
            if(page!= null)
            {
                ViewBag.Title = page.PageTitle;
                ViewBag.Body = page.PageBody;
                ViewBag.Keywords = page.Keywords;
                ViewBag.Description = page.Description;
                return View(page);
            }
            else
            {
                return RedirectToAction("PageNotFound", "Home");
            }
            
        }

        
    }
}