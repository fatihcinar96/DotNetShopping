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
    public class CitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cities
        public async Task<ActionResult> Index()
        {
            var cities = db.Cities.Include(c => c.Country)
                .GroupJoin(db.States, c => c.StateId, s => s.StateId, 
                (c, s) => new {City = c, State = s})
                .SelectMany(
                        x => x.State.DefaultIfEmpty(),
                        (x, y) => new { City = x.City, State = y })
               .Select(cs => new CityListModel{
                   CityId = cs.City.CityId,
                   Name = cs.City.Name,
                   Code = cs.City.Code,
                   StateId = cs.City.StateId,
                   StateName = cs.State.Name ?? "No State",
                   CountryId = cs.City.CountryId,
                   CountryName = cs.City.Country.Name
               });
            return View(await cities.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<ActionResult> Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = await db.Cities.FindAsync(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: Cities/Create
        public ActionResult Create()
        {
            var states = db.States.OrderBy(x => x.Name).ToList();
            states.Insert(0, new State { Name = "No State" });           
            ViewBag.StateId = new SelectList(states, "StateId", "Name");
            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CityId,Name,Code,StateId,CountryId")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var states = db.States.OrderBy(x => x.Name).ToList();
            states.Insert(0, new State { Name = "No State" });
            ViewBag.StateId = new SelectList(states, "StateId", "Name", city.StateId);
            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name", city.CountryId);
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<ActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = await db.Cities.FindAsync(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            var states = db.States.OrderBy(x => x.Name).ToList();
            states.Insert(0, new State { Name = "No State" });
            ViewBag.StateId = new SelectList(states, "StateId", "Name", city.StateId);
            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name", city.CountryId);
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CityId,Name,Code,StateId,CountryId")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var states = db.States.OrderBy(x => x.Name).ToList();
            states.Insert(0, new State { Name = "No State" });
            ViewBag.StateId = new SelectList(states, "StateId", "Name", city.StateId);
            ViewBag.CountryId = new SelectList(db.Countries.OrderBy(x => x.Name), "CountryId", "Name", city.CountryId);
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<ActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = await db.Cities.FindAsync(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(short id)
        {
            City city = await db.Cities.FindAsync(id);
            db.Cities.Remove(city);
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
