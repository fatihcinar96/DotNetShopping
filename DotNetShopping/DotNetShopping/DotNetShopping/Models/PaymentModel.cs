using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class PaymentMethod
    {
        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int16 PaymentMethodId { get; set; }
        public string Name { get; set; }
        public bool Domestic { get; set; }
        public bool International { get; set; }
        public Decimal StaticCost { get; set; }
        public Decimal PercentCost { get; set; }
        public Decimal MinAmount { get; set; }
        public Decimal MaxAmount { get; set; }
        public string PaymentInfo { get; set; }
        public Decimal PaymentDiscount { get; set; }
    }
}