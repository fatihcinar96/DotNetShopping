using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class Variant
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 VariantId { get; set; }
        [Required]
        [Display(Name = "Variant Name")]
        public string Name { get; set; }

        [ForeignKey("Product")]
        public Int64 ProductId { get; set; 
}
        public Product Product { get; set; }

        public Decimal UnitPrice { get; set; }
        public Decimal Cost { get; set; }
        public Int16 Stock { get; set; }

        public Int16 CampaignId { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }

        public bool IsVisible { get; set; }
        public bool Archived { get; set; }
    }
    public class VariantCreateModel
    {
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public Int64 ProductId { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Cost { get; set; }
        public Int16 Stock { get; set; }
        public bool IsVisible { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
    }
    public class VariantEditModel
    {
        public Int64 VariantId { get; set; }
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public Int64 ProductId { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Cost { get; set; }
        public Int16 Stock { get; set; }
        public bool IsVisible { get; set; }
    }
    public class VariantListModel
    {
        public Int64 VariantId { get; set; }
        [Display(Name = "Variant Name")]
        public string VariantName { get; set; }
        public Int64 ProductId { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal Cost { get; set; }
        public Int16 Stock { get; set; }
        public bool IsVisible { get; set; }
        public string PhotoName { get; set; }
    }
    public class PhotoAddModel
    {
        public Int64 VariantId { get; set; }
        public Int64 ProductId { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}