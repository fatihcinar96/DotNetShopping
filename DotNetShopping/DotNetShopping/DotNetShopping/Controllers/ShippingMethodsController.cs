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
    public class ShippingMethodsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShippingMethods
        public async Task<ActionResult> Index()
        {
            return View(await db.ShippingMethods.ToListAsync());
        }

        // GET: ShippingMethods/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingMethod shippingMethod = await db.ShippingMethods.FindAsync(id);
            if (shippingMethod == null)
            {
                return HttpNotFound();
            }
            return View(shippingMethod);
        }

        // GET: ShippingMethods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShippingMethods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ShippingMethodId,Name,Domestic,International")] ShippingMethod shippingMethod)
        {
            if (ModelState.IsValid)
            {
                db.ShippingMethods.Add(shippingMethod);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(shippingMethod);
        }

        // GET: ShippingMethods/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingMethod shippingMethod = await db.ShippingMethods.FindAsync(id);
            if (shippingMethod == null)
            {
                return HttpNotFound();
            }
            return View(shippingMethod);
        }

        // POST: ShippingMethods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ShippingMethodId,Name,Domestic,International")] ShippingMethod shippingMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingMethod).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(shippingMethod);
        }

        // GET: ShippingMethods/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingMethod shippingMethod = await db.ShippingMethods.FindAsync(id);
            if (shippingMethod == null)
            {
                return HttpNotFound();
            }
            return View(shippingMethod);
        }

        // POST: ShippingMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            ShippingMethod shippingMethod = await db.ShippingMethods.FindAsync(id);
            db.ShippingMethods.Remove(shippingMethod);
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
