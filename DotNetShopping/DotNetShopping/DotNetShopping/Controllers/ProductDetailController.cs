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
                    .OrderBy(i => i.Sequence).ToList(),
                    CampaignName = x.Variant.Product.CampaignId == 0 ? "" : db.Campaigns.Where(c => c.CampaignId == x.Variant.Product.CampaignId).FirstOrDefault().Name

                })
                .FirstOrDefault();
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var otherVariants = db.Variants.Where(x => x.Archived == false && x.Product.Archived == false
                && x.IsVisible == true && x.Stock > 0 && x.Product.OnSale == true && x.ProductId == model.ProductId && x.VariantId != id)
                .Select(x => new ProductBoxModel
                {
                    ProductId = x.ProductId,
                    VariantId = x.VariantId,
                    VariantName = x.Name,
                    UnitPrice = x.UnitPrice,
                    PhotoName = db.ProductImages
                    .Where(i => i.VariantId == x.VariantId)
                    .OrderBy(i => i.Sequence).FirstOrDefault().FileName
                })
                .ToList();
            ViewBag.OtherVariants = otherVariants;
            return View(model);
        }
    }
}