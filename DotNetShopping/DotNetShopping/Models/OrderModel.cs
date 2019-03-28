using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DotNetShopping.Models
{
    public class Order
    {
        public enum OrderStatuses
        {
            Received = 0,
            Prepared = 1,
            Shipped = 2,
            Delivered = 3
        }

        [HiddenInput(DisplayValue = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 OrderId { get; set; }
        public string UserId { get; set; }
        public string BillingFirstName { get; set; }
        public string BillingLastName { get; set; }
        public string BillingCompany { get; set; }
        public string BillingEmail { get; set; }
        public string BillingStreet1 { get; set; }
        public string BillingStreet2 { get; set; }
        public Int16 BillingCityId { get; set; }
        public Int16 BillingStateId { get; set; }
        public Int16 BillingCountryId { get; set; }
        public string BillingZip { get; set; }
        public string BillingTelephone { get; set; }


        public string ShippingFirstName { get; set; }
        public string ShippingLastName { get; set; }
        public string ShippingCompany { get; set; }
        public string ShippingStreet1 { get; set; }
        public string ShippingStreet2 { get; set; }
        public Int16 ShippingCityId { get; set; }
        public Int16 ShippingStateId { get; set; }
        public Int16 ShippingCountryId { get; set; }
        public string ShippingZip { get; set; }
        public string ShippingTelephone { get; set; }
        public Int16 ShippingMethodId { get; set; }
        public Decimal ShippingCost { get; set; }
        public DateTime ShippingDate { get; set; }


        public Int16 PaymentMethodId { get; set; }
        public string CardHolderName { get; set; }
        public string CardAccount { get; set; }
        public Decimal TotalCost { get; set; }
        public Decimal TotalPrice { get; set; }
        public Decimal TotalProfit { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public bool Paid { get; set; }

    }
}