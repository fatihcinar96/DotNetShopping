using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DotNetShopping.Models;

namespace DotNetShopping.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var categories = db.Categories
                .GroupJoin(db.Categories, c => c.ParentId, p => p.CategoryId, 
                (c, p) => new { Category = c, p })
                .SelectMany(x => x.p.DefaultIfEmpty(),
                (c, p) => new { c, p })
                .Select(cp => new CategoryListModel
                {
                    CategoryId = cp.c.Category.CategoryId,
                    CategoryName = cp.c.Category.Name,
                    ParentId = cp.c.Category.ParentId,
                    ParentName = cp.p.Name ?? "Parent"
                });
            return View(await categories.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            var categories = db.Categories.OrderBy(x => x.Name).ToList();
            categories.Insert(0, new Category { Name = "Parent", CategoryId = 0, ParentId = 0 });
            ViewBag.ParentId = new SelectList(categories, "CategoryId", "Name");
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryId,Name,ParentId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var categories = db.Categories.OrderBy(x => x.Name).ToList();
            categories.Insert(0, new Category { Name = "Parent", CategoryId = 0, ParentId = 0 });
            ViewBag.ParentId = new SelectList(categories, "CategoryId", "Name", category.ParentId);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            var categories = db.Categories.OrderBy(x => x.Name).ToList();
            categories.Insert(0, new Category { Name = "Parent", CategoryId = 0, ParentId = 0 });
            ViewBag.ParentId = new SelectList(categories, "CategoryId", "Name", category.ParentId);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryId,Name,ParentId")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var categories = db.Categories.OrderBy(x => x.Name).ToList();
            categories.Insert(0, new Category { Name = "Parent", CategoryId = 0, ParentId = 0 });
            ViewBag.ParentId = new SelectList(categories, "CategoryId", "Name", category.ParentId);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            Category category = await db.Categories.FindAsync(id);
            db.Categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
