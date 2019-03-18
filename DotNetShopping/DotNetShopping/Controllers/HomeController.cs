using DotNetShopping.Helpers;
using DotNetShopping.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
   

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            var newProducts = db.Variants.Include("Products")
                .Where(x => x.Archived == false && x.Product.Archived == false && x.IsVisible == true && x.Stock > 0 && x.Product.OnSale == true).Join(db.Categories, v => v.Product.CategoryId,
                c => c.CategoryId, (v, c) => new { Variant = v, Category = c })
                .OrderByDescending(x => x.Variant.CreateDate).Take(4).Select(x => new ProductBoxModel
                {
                    ProductId = x.Variant.ProductId,
                    BrandName = x.Variant.Product.Brand.Name,
                    VariantName = x.Variant.Name,
                    VariantId = x.Variant.VariantId,
                    CategoryName = x.Category.Name,
                    ProductName = x.Variant.Product.Name,
                    UnitPrice = x.Variant.UnitPrice,
                    IsVisible = x.Variant.IsVisible,
                    OnSale = x.Variant.Product.OnSale,
                    PhotoName = db.ProductImages.Where(i => i.VariantId == x.Variant.VariantId).OrderBy(i => i.Sequence).FirstOrDefault().FileName,
                    RegularPrice = x.Variant.UnitPrice * 100 / 90,
                    
                    
                    
                }).ToList();
            ViewBag.newProducts = newProducts;
            return View(newProducts);
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}