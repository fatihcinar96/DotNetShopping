using DotNetShopping.Helpers;
using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class EmailController : Controller
    {
        // GET: Email
        public ActionResult Index()
        {
            return View(new EmailModel());
        }
        [HttpPost ,ValidateInput(false)]
        public ActionResult Index(EmailModel model)
        {
            var emailHelper = new EmailHelper();
            string error = "";
            bool sent = emailHelper.SendEmail(model,ref error);
            ViewBag.Sent = sent;
            ViewBag.Error = error;
            return View(new EmailModel());
        }
    }
}