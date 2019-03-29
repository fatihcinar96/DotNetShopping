using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{

    public class ApiController : Controller
    {
        // GET: Api

        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpPost]
        public ActionResult AddToCart(Int64 VariantId, int Qty)
        {
            IEnumerable<CartListModel> model;
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var cart = db.Carts.Where(x => x.UserId == UserId && x.VariantId == VariantId).FirstOrDefault();
                if (cart != null)
                {
                    cart.Quantity += Qty;
                    db.SaveChanges();
                }
                else
                {
                    var newCart = new Cart();
                    newCart.UserId = UserId;
                    newCart.VariantId = VariantId;
                    newCart.Quantity = Qty;
                    db.Carts.Add(newCart);
                    db.SaveChanges();
                }
                CartListModel co = new CartListModel();

                model = co.GetCart(UserId);
                return Json(new { Success = true, Cart = model });

            }
            return Json(new { Success = true });

        }


        public ActionResult GetShoppingCart()
        {
            var UserId = User.Identity.GetUserId();
            CartListModel co = new CartListModel();
            var model = co.GetCart(UserId);
            return Json(new { Success = true, Cart = model }, JsonRequestBehavior.AllowGet);

        }

        

        [HttpPost]
        public ActionResult RemoveCart(Int64 variantId)
        {
            var UserId = User.Identity.GetUserId();
            var cart = db.Carts.Where(x => x.VariantId == variantId && x.UserId == UserId).FirstOrDefault();
            if (cart != null)
            {
                db.Carts.Remove(cart);
                db.SaveChanges();

            }
            CartListModel co = new CartListModel();
            var model = co.GetCart(UserId);
            return Json(new { Success = true, Cart = model });
        }

        public ActionResult GetStatesFor (Int16 CountryId)
        {
            var states = db.States.Where(x => x.CountryId == CountryId)
                .OrderBy(x => x.Name).ToList();
            return Json(new { Success = true, States = states },JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCitiesFor (Int16 CountryId)
        {
            var cities = db.Cities.Where(x => x.CountryId == CountryId)
                .OrderBy(x => x.Name).ToList();
            ViewBag.BillingCityId = new SelectList(cities);
            return Json(new { Success = true, Cities = cities }, JsonRequestBehavior.AllowGet);
        }
    }
}
