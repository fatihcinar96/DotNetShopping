using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class ShippingMethod
    {
        //[HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 ShippingMethodId { get; set; }
        public string Name { get; set; }
        public bool Domestic { get; set; }
        public bool International { get; set; }
    }
    public class ShippingCost
    {
        [Key]
        [Column(Order = 0)]
        public Int16 ShippingMethodId { get; set; }
        [Key]
        [Column(Order = 1)]
        public Int16 CountryId { get; set; }
        public Decimal CostHalf { get; set; }
        public Decimal CostOne { get; set; }
        public Decimal CostOneHalf { get; set; }
        public Decimal CostTwo { get; set; }
        public Decimal CostTwoHalf { get; set; }
    }
    public class ShippingListModel
    {
        public Int16 ShippingMethodId { get; set; }
        public string Name { get; set; }
        public bool Domestic { get; set; }
        public bool International { get; set; }
        public Int16 CountryId { get; set; }
        public string CountryName { get; set; }
        public Decimal CostHalf { get; set; }
        public Decimal CostOne { get; set; }
        public Decimal CostOneHalf { get; set; }
        public Decimal CostTwo { get; set; }
        public Decimal CostTwoHalf { get; set; }
    }
}