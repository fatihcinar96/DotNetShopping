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
    public class ShippingCostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ShippingCosts
        public async Task<ActionResult> Index()
        {
            var model = db.ShippingCosts
                .Join(db.ShippingMethods, sc => sc.ShippingMethodId, sm => sm.ShippingMethodId,
                (sc, sm) => new { ShippingCost = sc, ShippingMethod = sm })
                .Join(db.Countries, scsm => scsm.ShippingCost.CountryId, c => c.CountryId, 
                (scsm, c) => new { scsm, Country = c})
                .Select(x => new ShippingListModel
            {
                ShippingMethodId = x.scsm.ShippingCost.ShippingMethodId,
                Name = x.scsm.ShippingMethod.Name,
                CountryId = x.scsm.ShippingCost.CountryId,
                CountryName = x.Country.Name,
                Domestic = x.scsm.ShippingMethod.Domestic,
                International = x.scsm.ShippingMethod.International,
                CostHalf = x.scsm.ShippingCost.CostHalf,
                CostOne = x.scsm.ShippingCost.CostOne,
                CostOneHalf = x.scsm.ShippingCost.CostOneHalf,
                CostTwo = x.scsm.ShippingCost.CostTwo,
                CostTwoHalf = x.scsm.ShippingCost.CostTwoHalf
            });
            return View(await model.ToListAsync());
        }

        // GET: ShippingCosts/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingCost shippingCost = await db.ShippingCosts.FindAsync(id);
            if (shippingCost == null)
            {
                return HttpNotFound();
            }
            return View(shippingCost);
        }

        // GET: ShippingCosts/Create
        public ActionResult Create()
        {
            var CountryId = db.Countries.OrderBy(x => x.Name).ToList();
            var ShippingMethodId = db.ShippingMethods.OrderBy(x => x.Name).ToList();
            ViewBag.CountryId = new SelectList(CountryId, "CountryId", "Name");
            ViewBag.ShippingMethodId = new SelectList(ShippingMethodId, "ShippingMethodId", "Name");
            return View();
        }

        // POST: ShippingCosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ShippingMethodId,CountryId,CostHalf,CostOne,CostOneHalf,CostTwo,CostTwoHalf")] ShippingCost shippingCost)
        {
            if (ModelState.IsValid)
            {
                db.ShippingCosts.Add(shippingCost);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(shippingCost);
        }

        // GET: ShippingCosts/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingCost shippingCost = await db.ShippingCosts.FindAsync(id);
            if (shippingCost == null)
            {
                return HttpNotFound();
            }
            return View(shippingCost);
        }

        // POST: ShippingCosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ShippingMethodId,CountryId,CostHalf,CostOne,CostOneHalf,CostTwo,CostTwoHalf")] ShippingCost shippingCost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shippingCost).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(shippingCost);
        }

        // GET: ShippingCosts/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShippingCost shippingCost = await db.ShippingCosts.FindAsync(id);
            if (shippingCost == null)
            {
                return HttpNotFound();
            }
            return View(shippingCost);
        }

        // POST: ShippingCosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            ShippingCost shippingCost = await db.ShippingCosts.FindAsync(id);
            db.ShippingCosts.Remove(shippingCost);
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
