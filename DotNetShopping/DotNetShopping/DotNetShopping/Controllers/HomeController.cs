using DotNetShopping.Models;
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
            return View();
        }
        [OutputCache(Duration = 60)]
        public ActionResult FeaturedProducts()
        {
            try
            {
                var newProducts = db.Variants.Include("Product").Include("Brand")
                .Where(x => x.Archived == false && x.Product.Archived == false
                && x.IsVisible == true && x.Stock > 0 && x.Product.OnSale == true)
                .Join(db.Categories, v => v.Product.CategoryId,
                c => c.CategoryId, (v, c) => new { Variant = v, Category = c })
                .OrderByDescending(x => x.Variant.CreateDate)
                .Take(12).Select(x => new ProductBoxModel
                {
                    ProductId = x.Variant.ProductId,
                    VariantId = x.Variant.VariantId,
                    ProductName = x.Variant.Product.Name,
                    VariantName = x.Variant.Name,
                    BrandName = x.Variant.Product.Brand.Name,
                    CategoryName = x.Category.Name,
                    UnitPrice = x.Variant.UnitPrice,
                    PhotoName = db.ProductImages
                    .Where(i => i.VariantId == x.Variant.VariantId)
                    .OrderBy(i => i.Sequence).FirstOrDefault().FileName
                })
                .ToList();
                ViewBag.NewProducts = newProducts;
            }
            catch (Exception ex)
            {
                ViewBag.NewProducts = new List<ProductBoxModel>();
                ViewBag.Error = ex.Message;
            }

            return View();
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
        public ActionResult Products(Int16? Category, Decimal? min, Decimal? max, Int16? Brand)
        {
            Category = Category ?? 0;
            if (Category > 0)
            {
                var SelectedCategory = db.Categories.Find(Category);
                ViewBag.SelectedCategory = SelectedCategory;
            }
            min = min ?? 0;
            ViewBag.Min = min;
            max = max ?? 1000;
            ViewBag.Max = max;
            Brand = Brand ?? 0;
            ViewBag.Brand = Brand;
            try
            {
                var productsQuery = db.Variants.Include("Product").Include("Brand")
                .Where(x => x.Archived == false && x.Product.Archived == false
                && x.IsVisible == true && x.Stock > 0 && x.Product.OnSale == true)
                .Join(db.Categories, v => v.Product.CategoryId,
                c => c.CategoryId, (v, c) => new { Variant = v, Category = c });
                if (Category > 0)
                {
                    productsQuery = productsQuery.Where(x => x.Category.CategoryId == Category || x.Category.ParentId == Category);
                }
                productsQuery = productsQuery.Where(x => x.Variant.UnitPrice > min && x.Variant.UnitPrice < max);

                if (Brand > 0)
                {
                    productsQuery = productsQuery.Where(x => x.Variant.Product.BrandId == Brand);
                }

                var products = productsQuery.OrderByDescending(x => x.Variant.CreateDate)
                .Take(12).Select(x => new ProductBoxModel
                {
                    ProductId = x.Variant.ProductId,
                    VariantId = x.Variant.VariantId,
                    ProductName = x.Variant.Product.Name,
                    VariantName = x.Variant.Name,
                    BrandName = x.Variant.Product.Brand.Name,
                    CategoryName = x.Category.Name,
                    UnitPrice = x.Variant.UnitPrice,
                    PhotoName = db.ProductImages
                    .Where(i => i.VariantId == x.Variant.VariantId)
                    .OrderBy(i => i.Sequence).FirstOrDefault().FileName
                })
                .ToList();
                ViewBag.Products = products;

                var categories = db.Categories.Where(x => x.ParentId == (Category ?? 0)).OrderBy(x => x.Name).ToList();
                ViewBag.Categories = categories;

                var brands = db.Brands.OrderBy(x => x.Name).ToList();
                ViewBag.Brands = brands;
            }
            catch (Exception ex)
            {
                ViewBag.Products = new List<ProductBoxModel>();
                ViewBag.Error = ex.Message;
            }
            return View();
        }
    }
}