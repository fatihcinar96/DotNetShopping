using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class ProductDetailController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductDetail
        public ActionResult Product(Int64 id, string name)
        {
            var model = db.Variants.Include("Product").Include("Brand")
                .Where(x => x.VariantId == id && x.Archived == false && x.Product.Archived == false
                && x.IsVisible == true)
                .Join(db.Categories, v => v.Product.CategoryId,
                c => c.CategoryId, (v, c) => new { Variant = v, Category = c })
                .Select(x => new ProductDetailModel
                {
                    ProductId = x.Variant.ProductId,
                    VariantId = x.Variant.VariantId,
                    ProductName = x.Variant.Product.Name,
                    VariantName = x.Variant.Name,
                    BrandName = x.Variant.Product.Brand.Name,
                    CategoryName = x.Category.Name,
                    UnitPrice = x.Variant.UnitPrice,
                    BrandId = x.Variant.Product.BrandId,
                    CategoryId = x.Variant.Product.CategoryId,
                    Description = x.Variant.Product.Description,
                    OnSale = x.Variant.Product.OnSale,
                    Stock = x.Variant.Stock,
                    Unit = x.Variant.Product.Unit,
                    Images = db.ProductImages
                    .Where(i => i.VariantId == x.Variant.VariantId)
                    .OrderBy(i => i.Sequence).ToList()
                })
                .FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}