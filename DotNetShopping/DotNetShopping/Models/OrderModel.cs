using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string UserId { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string BillingEmail { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string BillingFirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string BillingLastName { get; set; }
        [Display(Name = "Company")]
        public string BillingCompany { get; set; }
        [Required]
        [Display(Name = "Street 1")]
        public string BillingStreet1 { get; set; }
        [Display(Name = "Street 2")]
        public string BillingStreet2 { get; set; }
        [Required]
        [Display(Name = "City")]
        public Int16 BillingCityId { get; set; }
        [Required]
        [Display(Name = "State")]
        public Int16 BillingStateId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Int16 BillingCountryId { get; set; }
        [Required]
        [Display(Name = "Zip")]
        public string BillingZip { get; set; }
        [Required]
        [Display(Name = "Telephone")]
        public string BillingTelephone { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string ShippingFirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string ShippingLastName { get; set; }
        [Display(Name = "Company")]
        public string ShippingCompany { get; set; }
        [Required]
        [Display(Name = "Street 1")]
        public string ShippingStreet1 { get; set; }
        [Display(Name = "Street 2")]
        public string ShippingStreet2 { get; set; }
        [Required]
        [Display(Name = "City")]
        public Int16 ShippingCityId { get; set; }
        [Required]
        [Display(Name = "State")]
        public Int16 ShippingStateId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Int16 ShippingCountryId { get; set; }
        [Required]
        [Display(Name = "Zip")]
        public string ShippingZip { get; set; }
        [Required]
        [Display(Name = "Telephone")]
        public string ShippingTelephone { get; set; }
        [Required]
        [Display(Name = "Shipping Method")]
        public Int16 ShippingMethodId { get; set; }
        public Decimal ShippingCost { get; set; }
        [Required]
        [Display(Name = "Payment Method")]
        public Int16 PaymentMethodId { get; set; }
        public string CardHolderName { get; set; }
        public string CardAccount { get; set; }
        public Decimal TotalCost { get; set; }
        public Decimal TotalPrice { get; set; }
        public Decimal TotalProfit { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public bool Paid { get; set; }
        public Decimal Discount { get; set; }
    }
    public class CheckoutModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string BillingEmail { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string BillingFirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string BillingLastName { get; set; }
        [Display(Name = "Company")]
        public string BillingCompany { get; set; }
        [Required]
        [Display(Name = "Street 1")]
        public string BillingStreet1 { get; set; }
        [Display(Name = "Street 2")]
        public string BillingStreet2 { get; set; }
        [Required]
        [Display(Name = "City")]
        public Int16 BillingCityId { get; set; }
        [Required]
        [Display(Name = "State")]
        public Int16 BillingStateId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Int16 BillingCountryId { get; set; }
        [Required]
        [Display(Name = "Zip")]
        public string BillingZip { get; set; }
        [Required]
        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string BillingTelephone { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string ShippingFirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string ShippingLastName { get; set; }
        [Display(Name = "Company")]
        public string ShippingCompany { get; set; }
        [Required]
        [Display(Name = "Street 1")]
        public string ShippingStreet1 { get; set; }
        [Display(Name = "Street 2")]
        public string ShippingStreet2 { get; set; }
        [Required]
        [Display(Name = "City")]
        public Int16 ShippingCityId { get; set; }
        [Required]
        [Display(Name = "State")]
        public Int16 ShippingStateId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Int16 ShippingCountryId { get; set; }
        [Required]
        [Display(Name = "Zip")]
        public string ShippingZip { get; set; }
        [Required]
        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ShippingTelephone { get; set; }
        [Required]
        [Display(Name = "Shipping Method")]
        public Int16 ShippingMethodId { get; set; }
        public Decimal ShippingCost { get; set; }
        [Required]
        [Display(Name = "Payment Method")]
        public Int16 PaymentMethodId { get; set; }
    }
    public class CreditCardModel
    {
        public string cardNumber { get; set; }
        public string cardHolder { get; set; }
        public int cardExpirationMonth { get; set; }
        public int cardExpirationYear { get; set; }
        public int cardCvv { get; set; }
    }

    public class OrderListModel
    {
        public Int64 OrderId { get; set; }
        public string UserId { get; set; }
        public Order.OrderStatuses OrderStatus { get; set; }
        public Decimal TotalPrice { get; set; }
        public bool Paid { get; set; }
        public DateTime OrderDate { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
    public class OrderDetailModel
    {
        public Int64 OrderId { get; set; }
        public string UserId { get; set; }
        public Order.OrderStatuses OrderStatus { get; set; }
        public Decimal TotalPrice { get; set; }
        public bool Paid { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingFirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string ShippingLastName { get; set; }
        [Display(Name = "Company")]
        public string ShippingCompany { get; set; }
        [Required]
        [Display(Name = "Street 1")]
        public string ShippingStreet1 { get; set; }
        [Display(Name = "Street 2")]
        public string ShippingStreet2 { get; set; }
        [Required]
        [Display(Name = "City")]
        public Int16 ShippingCityId { get; set; }
        [Required]
        [Display(Name = "State")]
        public Int16 ShippingStateId { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Int16 ShippingCountryId { get; set; }
        [Required]
        [Display(Name = "Zip")]
        public string ShippingZip { get; set; }
        [Required]
        [Display(Name = "Telephone")]
        public string ShippingTelephone { get; set; }
        [Required]
        [Display(Name = "Shipping Method")]
        public Int16 ShippingMethodId { get; set; }
        public string ShippingMethodName { get; set; }
        public Decimal ShippingCost { get; set; }
        [Required]
        [Display(Name = "Payment Method")]
        public Int16 PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime? ShippingDate { get; set; }
        public Decimal Discount { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public List<OrderProductListModel> OrderProducts { get; set; }
    }
}