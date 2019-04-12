using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DotNetShopping.Controllers
{
    public class MyOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: MyOrders
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var model = db.Orders.Where(x => x.UserId == userId).OrderByDescending(x => x.OrderDate)
                .Select(x => new OrderListModel
                {
                    OrderId = x.OrderId,
                    UserId = x.UserId,
                    OrderDate = x.OrderDate,
                    OrderStatus = x.OrderStatus,
                    Paid = x.Paid,
                    TotalPrice = x.TotalPrice
                });
            return View(model.ToList());
        }
        public ActionResult Order(Int64 id)
        {
            OrderDetailModel model = db.Orders.Where(x => x.OrderId == id)
                .Join(db.ShippingMethods, o => o.ShippingMethodId, sm => sm.ShippingMethodId,
                (o, sm) => new { Order = o, ShippingMethod = sm })
                .Join(db.PaymentMethods, osm => osm.Order.PaymentMethodId, pm => pm.PaymentMethodId,
                (osm, pm) => new { osm, PaymentMethod = pm })
                .Select(x => new OrderDetailModel
                {
                    OrderId = x.osm.Order.OrderId,
                    OrderDate = x.osm.Order.OrderDate,
                    OrderStatus = x.osm.Order.OrderStatus,
                    Paid = x.osm.Order.Paid,
                    PaymentMethodId = x.osm.Order.PaymentMethodId,
                    PaymentMethodName = x.PaymentMethod.Name,
                    ShippingCityId = x.osm.Order.ShippingCityId,
                    ShippingCompany = x.osm.Order.ShippingCompany,
                    ShippingCost = x.osm.Order.ShippingCost,
                    ShippingCountryId = x.osm.Order.ShippingCountryId,
                    ShippingDate = x.osm.Order.ShippingDate,
                    ShippingFirstName = x.osm.Order.ShippingFirstName,
                    ShippingLastName = x.osm.Order.ShippingLastName,
                    ShippingMethodId = x.osm.Order.ShippingMethodId,
                    ShippingMethodName = x.osm.ShippingMethod.Name,
                    ShippingStateId = x.osm.Order.ShippingStateId,
                    ShippingStreet1 = x.osm.Order.ShippingStreet1,
                    ShippingStreet2 = x.osm.Order.ShippingStreet2,
                    ShippingTelephone = x.osm.Order.ShippingTelephone,
                    ShippingZip = x.osm.Order.ShippingZip,
                    TotalPrice = x.osm.Order.TotalPrice,
                    UserId = x.osm.Order.UserId,
                    Discount = x.osm.Order.Discount,
                    CityName = db.Cities.Where(c => c.CityId == x.osm.Order.ShippingCityId).FirstOrDefault().Name,
                    StateName = db.States.Where(s => s.StateId == x.osm.Order.ShippingStateId).FirstOrDefault().Name,
                    CountryName = db.Countries.Where(c => c.CountryId == x.osm.Order.ShippingCountryId).FirstOrDefault().Name,
                    OrderProducts = db.OrderProducts.Where(op => op.OrderId == id)
                    .Join(db.Variants, op => op.VariantId, v => v.VariantId, 
                    (op, v) => new { OrderProduct = op, Variant = v })
                    .Join(db.Products, opv => opv.Variant.ProductId, p => p.ProductId,
                    (opv, p) => new { opv, Product = p })
                    .Select(oplm => new OrderProductListModel {
                        OrderId = oplm.opv.OrderProduct.OrderId,
                        Cost = oplm.opv.OrderProduct.Cost,
                        Quantity = oplm.opv.OrderProduct.Quantity,
                        TotalCost = oplm.opv.OrderProduct.TotalCost,
                        TotalPrice = oplm.opv.OrderProduct.TotalPrice,
                        UnitPrice = oplm.opv.OrderProduct.UnitPrice,
                        VariantId = oplm.opv.OrderProduct.VariantId,
                        ProductId = oplm.Product.ProductId,
                        ProductName = oplm.Product.Name,
                        VariantName = oplm.opv.Variant.Name,
                        FileName = db.ProductImages.Where(pi => pi.VariantId == oplm.opv.Variant.VariantId)
                        .OrderBy(pi => pi.Sequence).FirstOrDefault().FileName
                    }).ToList()
                }).FirstOrDefault();
            return View(model);
        }
    }
}