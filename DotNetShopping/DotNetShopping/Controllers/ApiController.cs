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
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Api
        [HttpPost]
        public ActionResult AddToCart(Int64 VariantId, int Qty)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var cart = db.Carts.Where(x => x.UserId == UserId &&
                x.VariantId == VariantId).FirstOrDefault();
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
                var model = co.GetCart(UserId);
                return Json(new { Success = true, Cart = model });
            }
            return Json(new { Success = true });
        }
        [HttpPost]
        public ActionResult RemoveCart(Int64 VariantId)
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
                CartListModel co = new CartListModel();
                var model = co.GetCart(UserId);
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

        public ActionResult GetStatesFor(Int16 CountryId)
        {
            var states = db.States.Where(x => x.CountryId == CountryId)
                .OrderBy(x => x.Name).ToList();
            return Json(new { Success = true, States = states }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetCitiesFor(Int16 CountryId, Int16 StateId)
        {
            var cities = db.Cities.Where(x => x.CountryId == CountryId && x.StateId == StateId)
                .OrderBy(x => x.Name).ToList();
            return Json(new { Success = true, Cities = cities }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetShippingMethods(Int16 CountryId)
        {
            var model = db.ShippingMethods
                .Join(db.ShippingCosts, sm => sm.ShippingMethodId, sc => sc.ShippingMethodId,
                (sm, sc) => new { ShippingMethod = sm, ShippingCost = sc })
                .Where(x => x.ShippingCost.CountryId == CountryId)
                .Select( x => new ShippingListModel {
                    ShippingMethodId = x.ShippingMethod.ShippingMethodId,
                    Name = x.ShippingMethod.Name,
                    Domestic = x.ShippingMethod.Domestic,
                    International = x.ShippingMethod.International,
                    CostHalf = x.ShippingCost.CostHalf,
                    CostOne = x.ShippingCost.CostOne,
                    CostOneHalf = x.ShippingCost.CostOneHalf,
                    CostTwo = x.ShippingCost.CostTwo,
                    CostTwoHalf = x.ShippingCost.CostTwoHalf,
                    CountryId = x.ShippingCost.CountryId
                })
                .OrderBy(x => x.Name).ToList();
            return Json(new { Success = true, ShippingMethods = model }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPaymentMethods(Int16 CountryId)
        {
            var isDomestic = CountryId == 2;
            var model = db.PaymentMethods
                .Where(x => x.Domestic == isDomestic && x.International != isDomestic)
                .OrderBy(x => x.Name).ToList();
            return Json(new { Success = true, PaymentMethods = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string keyword)
        {
            var model = db.Variants.Include("Product").Include("Brand")
                .Join(db.Categories, v => v.Product.CategoryId, c => c.CategoryId, (v, c) => new { Variant = v, Category = c })
                .Where(x => (x.Variant.Archived == false && x.Variant.IsVisible == true && x.Variant.Product.Archived == false && x.Variant.Product.IsVisible == true)
                && (x.Variant.Name.Contains(keyword) || x.Variant.Product.Name.Contains(keyword) || x.Category.Name.Contains(keyword))).OrderBy(x => x.Variant.Stock).Take(10)
                .Select(x => new 
                {
                    id = x.Variant.VariantId,
                    text = x.Variant.Name + " " + x.Variant.Product.Name,
                    VariantId = x.Variant.VariantId,
                    ProductId = x.Variant.ProductId,
                    VariantName = x.Variant.Name,
                    BrandName = x.Variant.Product.Brand.Name,
                    CategoryName = x.Category.Name,
                    Stock = x.Variant.Stock,
                    ProductName = x.Variant.Product.Name,
                    UnitPrice = x.Variant.UnitPrice,
                    PhotoName = db.ProductImages.Where(pi => pi.VariantId == x.Variant.VariantId).OrderBy(pi => pi.Sequence).FirstOrDefault().FileName
                }).ToList();
            return Json(new { Success = true, results = model }, JsonRequestBehavior.AllowGet);

        }
    }
}