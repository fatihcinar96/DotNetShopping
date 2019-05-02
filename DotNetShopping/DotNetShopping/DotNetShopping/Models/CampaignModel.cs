using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{


    public class Campaign
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 CampaignId { get; set; }
        [Display(Name = "Campaign Name")]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Promo Code")]
        public string Code { get; set; }
        public Decimal DiscountPercent { get; set; }
        public Decimal RequiredAmount { get; set; }
        public bool ProductCampaign { get; set; }
        public bool VariantCampaign { get; set; }
        public bool CodeCampaign { get; set; }
        public bool Enabled { get; set; }
        public bool OneTimeUse { get; set; }
        public bool FreeShipping { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public Int64 UseCount { get; set; }
        public Decimal TotalDiscount { get; set; }
        public Decimal TotalProfit { get; set; }
    }

    public class PromoCode
    {
        [Key]
        public string Code { get; set; }
        [ForeignKey("Campaign")]
        public Int16 CampaignId { get; set; }
        public Campaign Campaign { get; set; }
    }

    public class PromoCodeUsage
    {
        [Key]
        [Column(Order = 0)]
        public string Code { get; set; }
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        public DateTime UseDate { get; set; }
    }

    public class CampaignEditModel
    {
        [HiddenInput(DisplayValue = false)]
        public Int16 CampaignId { get; set; }
        [Display(Name = "Campaign Name")]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Promo Code")]
        public string Code { get; set; }
        public Decimal DiscountPercent { get; set; }
        public Decimal RequiredAmount { get; set; }
        public bool ProductCampaign { get; set; }
        public bool VariantCampaign { get; set; }
        public bool CodeCampaign { get; set; }
        public bool Enabled { get; set; }
        public bool OneTimeUse { get; set; }
        public bool FreeShipping { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}