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
            Preparing = 1,
            Prepared = 2,
            Shipped = 3,
            Delivered = 4
        }
        #region properties
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
        [Required]
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
        [Required]
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
        public string ShippingCode { get; set; }
        public OrderStatuses OrderStatus { get; set; }
        public bool Paid { get; set; }
        public Decimal Discount { get; set; }
        public string ConversationId { get; set; }
        public string PaymentError { get; set; }
        #endregion
        public OrderDetailModel GetOrderDetail(Int64 OrderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var model = db.Orders.Where(x => x.OrderId == OrderId)
               .Join(db.ShippingMethods, o => o.ShippingMethodId, sm => sm.ShippingMethodId, (o, sm) => new { Order = o, ShippingMethod = sm })
               .Join(db.PaymentMethods, osm => osm.Order.PaymentMethodId, pm => pm.PaymentMethodId, (osm, pm) => new { osm, PaymentMethod = pm })
               .Select(x => new OrderDetailModel
               {
                   OrderId = x.osm.Order.OrderId,
                   OrderDate = x.osm.Order.OrderDate,
                   OrderStatus = x.osm.Order.OrderStatus,
                   Paid = x.osm.Order.Paid,
                   PaymentError = x.osm.Order.PaymentError,
                   PaymentMethodId = x.osm.Order.PaymentMethodId,
                   PaymentMethodName = x.PaymentMethod.Name,
                   ShippingCityId = x.osm.Order.ShippingCityId,
                   ShippingCompany = x.osm.Order.ShippingCompany,
                   ShippingCost = x.osm.Order.ShippingCost,
                   ShippingCountryId = x.osm.Order.ShippingCountryId,
                   ShippingDate = x.osm.Order.ShippingDate,
                   ShippingFirstName = x.osm.Order.ShippingFirstName,
                   ShippingLastName = x.osm.Order.ShippingLastName,
                   ShippingMethodId = x.osm.Order.ShippingMethodId,
                   ShippingMethodName = x.osm.ShippingMethod.Name,
                   ShippingStateId = x.osm.Order.ShippingStateId,
                   ShippingStreet1 = x.osm.Order.ShippingStreet1,
                   ShippingStreet2 = x.osm.Order.ShippingStreet2,
                   ShippingTelephone = x.osm.Order.ShippingTelephone,
                   ShippingCode = x.osm.Order.ShippingCode,
                   ShippingZip = x.osm.Order.ShippingZip,
                   TotalPrice = x.osm.Order.TotalPrice,
                   UserId = x.osm.Order.UserId,
                   Discount = x.osm.Order.Discount,
                   CityName = db.Cities.Where(c => c.CityId == x.osm.Order.ShippingCityId).FirstOrDefault().Name,
                   StateName = db.States.Where(s => s.StateId == x.osm.Order.ShippingStateId).FirstOrDefault().Name,
                   CountryName = db.Countries.Where(c => c.CountryId == x.osm.Order.ShippingCountryId).FirstOrDefault().Name,
                   OrderProducts = db.OrderProducts.Where(op => op.OrderId == OrderId)
                   .Join(db.Variants, op => op.VariantId, v => v.VariantId, (op, v) => new { OrderProduct = op, Variant = v })
                   .Join(db.Products, opv => opv.Variant.ProductId, p => p.ProductId, (opv, p) => new { opv, Product = p })
                   .Select(oplm => new OrderProductListModel
                   {
                       OrderId = oplm.opv.OrderProduct.OrderId,
                       Cost = oplm.opv.OrderProduct.Cost,
                       Quantity = oplm.opv.OrderProduct.Quantity,
                       TotalCost = oplm.opv.OrderProduct.TotalCost,
                       TotalPrice = oplm.opv.OrderProduct.TotalPrice,
                       UnitPrice = oplm.opv.OrderProduct.UnitPrice,
                       VariantId = oplm.opv.OrderProduct.VariantId,
                       ProductId = oplm.Product.ProductId,
                       ProductName = oplm.Product.Name,
                       VariantName = oplm.opv.Variant.Name,
                       FileName = db.ProductImages.Where(pi => pi.VariantId == oplm.opv.Variant.VariantId).OrderBy(pi => pi.Sequence).FirstOrDefault().FileName
                   }).ToList()
               }).FirstOrDefault();
            return model;
        }
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
        [Required]
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
        [Required]
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
        public Decimal Discount { get; set; }
        public bool Paid { get; set; }
        public string PaymentError { get; set; }
        public DateTime OrderDate { get; set; }
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
        [Required]
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
        public string ShippingMethodName { get; set; }
        public Decimal ShippingCost { get; set; }
        [Required]
        [Display(Name = "Payment Method")]
        public Int16 PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string ShippingCode { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }

        public List<OrderProductListModel> OrderProducts { get; set; }
    }
}