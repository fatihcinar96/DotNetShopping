using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class AdminPageController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: AdminPage

        public ActionResult Index()
        {
            var pages = db.Pages.Select(x => new PageViewModel
            {
                PageId = x.PageId,
                PageTitle = x.PageTitle,
                PageBody = x.PageBody,
                Description = x.Description,
                Keywords = x.Keywords,
                CreateDate = x.CreateDate,
                CreateUser = x.CreateUser,
                UpdateDate = x.UpdateDate,
                UpdateUser = x.UpdateUser
            });
            return View(pages.ToList());
        }

        [Authorize(Roles = "Admin")]

        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        
        [HttpPost,ValidateInput(false)]
        public ActionResult Create(PageCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var today = DateTime.Now;
                    var user = User.Identity.GetUserId();
                    var p = new Page();
                    p.PageId = model.PageId;
                    p.PageTitle = model.PageTitle;
                    p.PageBody = model.PageBody;
                    p.Keywords = model.Keywords;
                    p.Description = model.Description;
                    p.CreateUser = user.ToString();
                    p.CreateDate = today;
                    p.UpdateDate = today;
                    p.UpdateUser = user.ToString();
                    db.Pages.Add(p);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception("Something went wrong!");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }


        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(PageEditModel model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if(model != null)
                    {
                        var user = User.Identity.GetUserId().ToString();
                        var today = DateTime.Now;
                        var page = db.Pages.Find(model.PageId);
                        page.PageTitle = model.PageTitle;
                        page.PageBody = model.PageBody;
                        page.Keywords = model.Keywords;
                        page.Description = model.Description;
                        page.UpdateUser = user;
                        page.UpdateDate = today;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

       
        public ActionResult Delete(string PageId)
        {

            Page page = db.Pages.Find(PageId);
            db.Pages.Remove(page);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}