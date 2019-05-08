using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    .OrderBy(i => i.Sequence).FirstOrDefault().FileName,
                    CampaignName = x.Variant.Product.CampaignId == 0 ? "" : db.Campaigns.Where(c=> c.CampaignId == x.Variant.Product.CampaignId).FirstOrDefault().Name

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
                    .OrderBy(i => i.Sequence).FirstOrDefault().FileName,
                    CampaignName = x.Variant.Product.CampaignId == 0 ? "" : db.Campaigns.Where(c => c.CampaignId == x.Variant.Product.CampaignId).FirstOrDefault().Name

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

        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult BestSellerProducts()
        {
            try
            {

                var day = DateTime.Now.AddDays(-30);
                var products = db.Orders
                    .Join(db.OrderProducts, o => o.OrderId, op => op.OrderId, (o, op) => new { Order = o, OrderProduct = op })
                    .Join(db.Variants, oop => oop.OrderProduct.VariantId, v => v.VariantId, (oop, v) => new { oop, Variant = v })
                    .Join(db.Products, oopv => oopv.Variant.ProductId, p => p.ProductId, (oopv, p) => new { oopv, Product = p })
                    .Join(db.Categories, oopvp => oopvp.Product.CategoryId, c => c.CategoryId, (oopvp, c) => new { oopvp, Category = c })
                    .Join(db.Brands, oopvpc => oopvpc.oopvp.Product.Brand.BrandId, b => b.BrandId, (oopvpc, b) => new { oopvpc, Brand = b })
                    .Where(x => x.oopvpc.oopvp.oopv.Variant.Archived == false &&
                    x.oopvpc.oopvp.oopv.Variant.IsVisible == true &&
                    x.oopvpc.oopvp.Product.IsVisible == true &&
                    x.oopvpc.oopvp.Product.OnSale == true &&
                    x.oopvpc.oopvp.Product.Archived == false &&
                    x.oopvpc.oopvp.oopv.Variant.Stock > 0 &&
                    x.oopvpc.oopvp.oopv.oop.Order.OrderDate > day

                    )
                    .GroupBy(x => new
                    {
                        ProductId = x.oopvpc.oopvp.Product.ProductId,
                        VariantId = x.oopvpc.oopvp.oopv.Variant.VariantId,
                        ProductName = x.oopvpc.oopvp.oopv.Variant.Product.Name,
                        VariantName = x.oopvpc.oopvp.oopv.Variant.Name,
                        BrandName = x.Brand.Name,
                        CategoryName = x.oopvpc.Category.Name,
                        UnitPrice = x.oopvpc.oopvp.oopv.Variant.UnitPrice,
                        CampaignId = x.oopvpc.oopvp.Product.CampaignId

                    })
                    .Select(group => new
                    {
                        ProductBox = group.Key,
                        Sold = group.Sum(x => x.oopvpc.oopvp.oopv.oop.OrderProduct.Quantity)
                    })
                    .Select(x => new
                    {
                        ProductId = x.ProductBox.ProductId,
                        VariantId = x.ProductBox.VariantId,
                        ProductName = x.ProductBox.ProductName,
                        VariantName = x.ProductBox.VariantName,
                        BrandName = x.ProductBox.BrandName,
                        CategoryName = x.ProductBox.CategoryName,
                        UnitPrice = x.ProductBox.UnitPrice,
                        Sold = x.Sold,
                        CampaignId = x.ProductBox.CampaignId
                    })

                    .OrderByDescending(x => x.Sold)
                    .Take(12)
                    .Select(x => new ProductBoxModel
                    {
                        ProductId = x.ProductId,
                        VariantId = x.VariantId,
                        ProductName = x.ProductName,
                        VariantName = x.VariantName,
                        BrandName = x.BrandName,
                        CategoryName = x.CategoryName,
                        UnitPrice = x.UnitPrice,
                        PhotoName = db.ProductImages.Where(i => i.VariantId == x.VariantId)
                        .OrderBy(i => i.Sequence).FirstOrDefault().FileName,
                        CampaignName = x.CampaignId == 0 ? "" : db.Campaigns.Where(c => c.CampaignId == x.CampaignId).FirstOrDefault().Name

                    }).ToList();
                ViewBag.BestSellerProducts = products;

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }


    }
}