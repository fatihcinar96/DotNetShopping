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
            ViewBag.Title = page.PageTitle;
            ViewBag.Body = page.PageBody;
            ViewBag.Keywords = page.Keywords;
            ViewBag.Description = page.Description;
            return View(page);
        }
    }
}