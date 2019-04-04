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
    public class StatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: States
        public async Task<ActionResult> Index()
        {
            var states = db.States.Include(s => s.Country);
            return View(await states.ToListAsync());
        }

        // GET: States/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // GET: States/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name");
            return View();
        }

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "StateId,Code,Name,CountryId")] State state)
        {
            if (ModelState.IsValid)
            {
                db.States.Add(state);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name", state.CountryId);
            return View(state);
        }

        // GET: States/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name", state.CountryId);
            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "StateId,Code,Name,CountryId")] State state)
        {
            if (ModelState.IsValid)
            {
                db.Entry(state).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name", state.CountryId);
            return View(state);
        }

        // GET: States/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State state = await db.States.FindAsync(id);
            if (state == null)
            {
                return HttpNotFound();
            }
            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            State state = await db.States.FindAsync(id);
            db.States.Remove(state);
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
