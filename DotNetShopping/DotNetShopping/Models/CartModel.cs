using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DotNetShopping.Models
{

    public class Cart
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 1)]
        public Int64 VariantId { get; set; }
        public int Quantity { get; set; }

    }
    public class CartListModel
    {
        public string UserId { get; set; }
        public Int64 VariantId { get; set; }
        public Int64 ProductId { get; set; }
        public int Quantity { get; set; }
        public Decimal UnitPrice { get; set; }
        public Decimal TotalPrice { get; set; }
        public string VariantName { get; set; }
        public string ProductName { get; set; }

    }

}