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
            #region fillFormData
            var countries = db.Countries.OrderBy(x => x.Name).ToList();
            ViewBag.BillingCountryId = new SelectList(countries, "CountryId", "Name");
            ViewBag.ShippingCountryId = new SelectList(countries, "CountryId", "Name");

            var selectState = new List<string>();
            ViewBag.BillingStateId = new SelectList(selectState);
            ViewBag.ShippingStateId = new SelectList(selectState);

            var selectCity = new List<string>();
            ViewBag.BillingCityId = new SelectList(selectCity);
            ViewBag.ShippingCityId = new SelectList(selectCity);

            var selectShippingMethod = new List<string>();
            ViewBag.ShippingMethodId = new SelectList(selectShippingMethod);

            var selectPaymentMethod = new List<string>();
            ViewBag.PaymentMethodId = new SelectList(selectPaymentMethod);

            CartListModel co = new CartListModel();
            var cart = co.GetCart(User.Identity.GetUserId());
            ViewBag.Cart = cart;
            #endregion
            return View();
        }
        [HttpPost]
        public ActionResult Checkout(CheckoutModel checkout, CreditCardModel card, String paypal)
        {
            string userId = User.Identity.GetUserId();
            CartListModel co = new CartListModel();
            try
            {
                var cart = co.GetCart(User.Identity.GetUserId());
                #region createorder 
                var order = new Order();
                order.UserId = userId;
                order.BillingCityId = checkout.BillingCityId;
                order.BillingCompany = checkout.BillingCompany;
                order.BillingCountryId = checkout.BillingCountryId;
                order.BillingEmail = checkout.BillingEmail;
                order.BillingFirstName = checkout.BillingFirstName;
                order.BillingLastName = checkout.BillingLastName;
                order.BillingStateId = checkout.BillingStateId;
                order.BillingStreet1 = checkout.BillingStreet1;
                order.BillingStreet2 = checkout.BillingStreet2;
                order.BillingTelephone = checkout.BillingTelephone;
                order.BillingZip = checkout.BillingZip;
                order.ShippingCityId = checkout.ShippingCityId;
                order.ShippingCompany = checkout.ShippingCompany;
                order.ShippingCost = checkout.ShippingCost;
                order.ShippingCountryId = checkout.ShippingCountryId;
                order.ShippingFirstName = checkout.ShippingFirstName;
                order.ShippingLastName = checkout.ShippingLastName;
                order.ShippingMethodId = checkout.ShippingMethodId;
                order.ShippingStateId = checkout.ShippingStateId;
                order.ShippingStreet1 = checkout.ShippingStreet1;
                order.ShippingStreet2 = checkout.ShippingStreet2;
                order.ShippingTelephone = checkout.ShippingTelephone;
                order.ShippingZip = checkout.ShippingZip;
                order.OrderDate = DateTime.Now;
                order.OrderStatus = Order.OrderStatuses.Received;
                order.Paid = false;
                order.PaymentMethodId = checkout.PaymentMethodId;
                if (card.cardNumber != null)
                {
                    order.CardAccount = card.cardNumber.Substring(card.cardNumber.Length - 4, 4);
                    order.CardHolderName = card.cardHolder;
                }
                if(paypal != null)
                {
                    //TODO:
                    //add paypal to Order
                }

                var productTotal = cart.Sum(x => x.TotalPrice);
                var weight = cart.Sum(x => x.Quantity) * 0.25;
                var shippingCosts = db.ShippingCosts
                    .Where(x => x.ShippingMethodId == checkout.ShippingMethodId &&
                    x.CountryId == checkout.ShippingCountryId).FirstOrDefault();
                decimal shippingCost = 0;
                if (weight <= 0.5)
                {
                    shippingCost = shippingCosts.CostHalf;
                }
                else if (weight <= 1)
                {
                    shippingCost = shippingCosts.CostOne;
                }
                else if (weight <= 1.5)
                {
                    shippingCost = shippingCosts.CostOneHalf;
                }
                else if (weight <= 2)
                {
                    shippingCost = shippingCosts.CostTwo;
                }
                else
                {
                    shippingCost = shippingCosts.CostTwoHalf;
                }
                var paymentMethod = db.PaymentMethods.Find(checkout.PaymentMethodId);
                decimal paymentDiscount = paymentMethod.PaymentDiscount;
                order.Discount = paymentDiscount;
                order.TotalPrice = productTotal + shippingCost - paymentDiscount;
                order.ShippingCost = shippingCost;
                decimal productCost = cart.Sum(x => x.TotalCost);
                decimal paymentCost = (paymentMethod.PercentCost * order.TotalPrice) + paymentMethod.StaticCost;

                order.TotalCost = productCost + paymentCost + shippingCost;
                
                order.TotalProfit = order.TotalPrice - order.TotalCost;

                db.Orders.Add(order);
                db.SaveChanges();
                #endregion

                foreach (CartListModel item in cart)
                {
                    var op = new OrderProduct();
                    op.OrderId = order.OrderId;
                    op.Quantity = item.Quantity;
                    op.TotalCost = item.TotalCost;
                    op.TotalPrice = item.TotalPrice;
                    op.UnitPrice = item.UnitPrice;
                    op.VariantId = item.VariantId;
                    op.Cost = item.Cost;
                    db.OrderProducts.Add(op);
                }
                db.SaveChanges();

                //TODO: Process Payment if necessary.

                db.Carts.RemoveRange(db.Carts.Where(x => x.UserId == userId));
                db.SaveChanges();

                return RedirectToAction("Order", "MyOrders", new { id = order.OrderId });
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                #region fillFormData
                var countries = db.Countries.OrderBy(x => x.Name).ToList();
                ViewBag.BillingCountryId = new SelectList(countries, "CountryId", "Name");
                ViewBag.ShippingCountryId = new SelectList(countries, "CountryId", "Name");

                var selectState = new List<string>();
                ViewBag.BillingStateId = new SelectList(selectState);
                ViewBag.ShippingStateId = new SelectList(selectState);

                var selectCity = new List<string>();
                ViewBag.BillingCityId = new SelectList(selectCity);
                ViewBag.ShippingCityId = new SelectList(selectCity);

                var selectShippingMethod = new List<string>();
                ViewBag.ShippingMethodId = new SelectList(selectShippingMethod);

                var selectPaymentMethod = new List<string>();
                ViewBag.PaymentMethodId = new SelectList(selectPaymentMethod);

                
                var cart = co.GetCart(User.Identity.GetUserId());
                ViewBag.Cart = cart;
                #endregion
                return View();
            }
        }
    }
}