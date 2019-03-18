using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class Product
    {
        public enum ProductUnit
        {
            Piece = 0,
            Kilo = 1,
            Liter = 2,
            Meter = 3
        }



        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [ForeignKey("Supplier")]
        public Int16 SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        [ForeignKey("Brand")]
        public Int16 BrandId { get; set; }
        public Brand Brand { get; set; }

        public Int16 CategoryId { get; set; }
        public string Description { get; set; }
        public ProductUnit Unit { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public bool OnSale { get; set; }
        public bool IsVisible { get; set; }
        public bool Archived { get; set; }


    }

    public class ProductCreateModel
    {
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public Int16 SupplierId { get; set; }
        public Int16 BrandId { get; set; }
        public Int16 CategoryId { get; set; }
        public string Description { get; set; }
        public Product.ProductUnit Unit { get; set; }
        public bool OnSale { get; set; }
        public bool IsVisible { get; set; }
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Cost { get; set; }
        public Int16 Stock { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
    }
    public class ProductEditModel
    {
        public Int64 ProductId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public Int16 SupplierId { get; set; }
        public Int16 BrandId { get; set; }
        public Int16 CategoryId { get; set; }
        public string Description { get; set; }
        public Product.ProductUnit Unit { get; set; }
        public bool OnSale { get; set; }
        public bool IsVisible { get; set; }
    }
    public class ProductListModel
    {
        public Int64 ProductId { get; set; }
        public Int64 VariantId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public string SupplierName { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public bool OnSale { get; set; }
        public bool IsVisible { get; set; }
        public Decimal Cost { get; set; }
        public Decimal UnitPrice { get; set; }
        public Int16 Stock { get; set; }
        public string PhotoName { get; set; }

    }

    public class ProductBoxModel
    {
        public Int64 ProductId { get; set; }
        public Int64 VariantId { get; set; }
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public string CategoryName { get; set; }
        public bool OnSale { get; set; }
        public bool IsVisible { get; set; }
        public Decimal UnitPrice { get; set; }
        public string PhotoName { get; set; }
        public string BrandName { get; set; }
        public Decimal RegularPrice { get; set; }
    }
}