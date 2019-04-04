using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DotNetShopping.Controllers
{
    public class CheckoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Checkout
        public ActionResult Cart()
        {
            CartListModel co = new CartListModel();
            var cart = co.GetCart(User.Identity.GetUserId());
            return View(cart);
        }
        [HttpPost]
        public ActionResult Cart(List<CartListModel> cartForm)
        {
            var userId = User.Identity.GetUserId();
            var carts = db.Carts.Where(x => x.UserId == userId).ToList();
            foreach(Cart cart in carts)
            {
                int formValue = cartForm.Where(x => x.VariantId == cart.VariantId).FirstOrDefault().Quantity;
                if (cart.Quantity != formValue)
                {
                    cart.Quantity = formValue;
                }
            }
            db.SaveChanges();
            CartListModel co = new CartListModel();
            var model = co.GetCart(User.Identity.GetUserId());
            return View(model);
        }
        public ActionResult DeleteCart(Int64 VariantId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var cart = db.Carts.Where(x => x.UserId == UserId &&
                x.VariantId == VariantId).FirstOrDefault();
                if (cart != null)
                {
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Cart");
        }
        public ActionResult DeleteAllCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var carts = db.Carts.Where(x => x.UserId == UserId).ToList();
                if (carts != null)
                {
                    db.Carts.RemoveRange(carts);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Cart");
        }
        public ActionResult Checkout()
        {
            var countries = db.Countries.OrderBy(x => x.Name).ToList();
            var selectCountry = new Country();
            selectCountry.Name = "Please Select";
            countries.Insert(0, selectCountry);
            ViewBag.BillingCountryId = new SelectList(countries, "CountryId", "Name");
            ViewBag.ShippingCountryId = new SelectList(countries, "CountryId", "Name");

            var selectState = new List<string>();
            selectState.Add("Select Country");
            ViewBag.BillingStateId = new SelectList(selectState);
            ViewBag.ShippingStateId = new SelectList(selectState);

            var selectCity = new List<string>();
            selectCity.Add("Select Country/State");
            ViewBag.BillingCityId = new SelectList(selectCity);
            ViewBag.ShippingCityId = new SelectList(selectCity);

            CartListModel co = new CartListModel();
            var cart = co.GetCart(User.Identity.GetUserId());
            ViewBag.Cart = cart;

            return View();
        }
        [HttpPost]
        public ActionResult Checkout(CheckoutModel checkout , CreditCardModel card,string paypal)
        {

            return RedirectToAction("Index","Home");
        }
    }
}