using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Order(Int64 id, string message)
        {
            ViewBag.Message = message;
            var order = new Order();
            var model = order.GetOrderDetail(id);
            return View(model);
        }
    }
}