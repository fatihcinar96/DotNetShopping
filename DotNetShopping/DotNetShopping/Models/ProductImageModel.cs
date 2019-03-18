using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetShopping.Helpers;

namespace DotNetShopping.Models
{
    public class ProductImage
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Int64 ImageId { get; set; }
        public Int64 ProductId { get; set; }
        public Int64 VariantId { get; set; }
        public string FileName { get; set; }
        public Int16 Sequence { get; set; }


        public string InsertProductImage(Int64 VariantId)
        {
            string fileName = "";

            ApplicationDbContext db = new ApplicationDbContext();
            var variant = db.Variants.Include("Product").Where(x => x.VariantId == VariantId).FirstOrDefault();
            if (variant != null)
            {
                var productImage = new ProductImage();
                productImage.ProductId = variant.ProductId;
                productImage.VariantId = variant.VariantId;
                db.ProductImages.Add(productImage);
                db.SaveChanges();

                fileName = variant.Name + "_" + variant.Product.Name + "_" + productImage.ImageId;
                fileName = StringHelper.ClearFileName(fileName);
                productImage.FileName = fileName;
                Int16 sequence = db.ProductImages.Where(x => x.ProductId == variant.ProductId && x.VariantId == variant.VariantId).OrderByDescending(x => x.Sequence).FirstOrDefault().Sequence;
                productImage.Sequence = ++sequence;

                db.SaveChanges();
            }
            else
            {
                throw new Exception("Variant Not Found!");
            }
            return fileName;
        }
    }
}