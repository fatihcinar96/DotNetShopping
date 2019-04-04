using DotNetShopping.Helpers;
using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DotNetShopping.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ProductImage
        public ActionResult Index(Int64 id, Int64 ProductId)
        {
            var images = db.ProductImages.Where(x => x.VariantId == id).OrderBy(x => x.Sequence).ToList();
            if (images.Count == 0)
            {
                return RedirectToAction("PhotoAdd", new { id = id, ProductId = ProductId });
            }
            else
            {
                return View(images);
            }
            
        }
        public ActionResult PhotoAdd(Int64 id, Int64 ProductId)
        {
            ViewBag.Error = "";
            var model = new PhotoAddModel();
            model.VariantId = id;
            model.ProductId = ProductId;
            return View(model);
        }

        [HttpPost]
        public ActionResult PhotoAdd(PhotoAddModel model)
        {
            ViewBag.Error = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.UploadedFile != null)
                    {
                        var image = System.Drawing.Image.FromStream(model.UploadedFile.InputStream);
                        if (image.Width >= 1000 && image.Height >= 1000)
                        {
                            var productImage = new ProductImage();
                            string fileName = productImage.InsertProductImage(model.VariantId);
                            ImageHelper.SaveImage(fileName, image);
                        }
                        else
                        {
                            throw new Exception("Photo needs to be minimum 1000px X 1000px size!");
                        }
                    }
                }
                return RedirectToAction("Index", new { id = model.VariantId, ProductId = model.ProductId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        public ActionResult Delete(Int64 id, Int64 VariantId, Int64 ProductId)
        {
            try
            {
                var productImage = db.ProductImages.Find(id);
                if (productImage != null)
                {
                    db.ProductImages.Remove(productImage);
                    db.SaveChanges();

                    string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ProductImage"), productImage.FileName + ".jpg");
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    for (int i = 1; i <= 10; i++)
                    {
                        DeleteForSize(productImage, productImage.FileName, i);
                    }
                    return RedirectToAction("Index", new { id = VariantId, ProductId = ProductId });
                }
                throw new Exception("Product Image Not Found!");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index", new { id = VariantId, ProductId = ProductId, Error = ex.Message });
            }
        }

        private static void DeleteForSize(ProductImage productImage, string name, int i)
        {
            string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/ProductImage"), name + "-" + i + ".jpg");
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        public ActionResult Sequence(string direction, Int64 imageId)
        {
            var image = db.ProductImages.Find(imageId);
            if (image != null)
            {
                var productImages = db.ProductImages.Where(x => x.VariantId == image.VariantId).OrderBy(x => x.Sequence).ToList();
                Int16 currentIndex = 0;
                for (Int16 i = 0; i < productImages.Count; i++)
                {
                    var pi = productImages[i];
                    if (pi.Sequence != i)
                    {
                        pi.Sequence = i;
                    }
                    if (pi.ImageId == image.ImageId)
                    {
                        currentIndex = i;
                    }
                }
                
                if (direction == "up")
                {
                    if (currentIndex > 0)
                    {
                        productImages[currentIndex].Sequence--;
                        productImages[currentIndex-1].Sequence++;
                    }
                }
                else if (direction == "down")
                {
                    if (currentIndex < productImages.Count-1)
                    {
                        productImages[currentIndex].Sequence++;
                        productImages[currentIndex + 1].Sequence--;
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index", new { id = image.VariantId, ProductId = image.ProductId });
        }
    }
}