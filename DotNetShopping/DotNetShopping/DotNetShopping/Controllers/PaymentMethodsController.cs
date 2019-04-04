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
    public class PaymentMethodsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PaymentMethods
        public async Task<ActionResult> Index()
        {
            return View(await db.PaymentMethods.ToListAsync());
        }

        // GET: PaymentMethods/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = await db.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PaymentMethodId,Name,Domestic,International,StaticCost,PercentCost,MinAmount,MaxAmount,PaymentInfo,PaymentDiscount")] PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                db.PaymentMethods.Add(paymentMethod);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(paymentMethod);
        }

        // GET: PaymentMethods/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = await db.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PaymentMethodId,Name,Domestic,International,StaticCost,PercentCost,MinAmount,MaxAmount,PaymentInfo,PaymentDiscount")] PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentMethod).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = await db.PaymentMethods.FindAsync(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            PaymentMethod paymentMethod = await db.PaymentMethods.FindAsync(id);
            db.PaymentMethods.Remove(paymentMethod);
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
