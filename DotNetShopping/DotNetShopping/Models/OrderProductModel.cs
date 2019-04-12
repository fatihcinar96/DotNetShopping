using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class OrderProduct
    {
        [Key]
        [Column(Order = 0)]
        public Int64 OrderId { get; set; }
        [Key]
        [Column(Order = 1)]
        public Int64 VariantId { get; set; }
        public int Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal TotalCost { get; set; }
        public Decimal TotalPrice { get; set; }
    }
    public class OrderProductListModel
    {
        public Int64 OrderId { get; set; }
        public Int64 VariantId { get; set; }
        public int Quantity { get; set; }
        public Decimal Cost { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal TotalCost { get; set; }
        public Decimal TotalPrice { get; set; }
        public string VariantName { get; set; }
        public string ProductName { get; set; }
        public Int64 ProductId { get; set; }
        public string FileName { get; set; }
    }
}