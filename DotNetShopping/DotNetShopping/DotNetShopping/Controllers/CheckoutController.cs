using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using Iyzipay;
using Iyzipay.Request;
using Iyzipay.Model;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using DotNetShopping.Helpers;

namespace DotNetShopping.Controllers
{
    public class CheckoutController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Checkout
        public ActionResult Cart()
        {
            CartListModel co = new CartListModel();
            var cart = co.GetCart(User.Identity.GetUserId());
            Decimal discount = co.CalculateDiscount(cart);
            ViewBag.Discount = discount;
            return View(cart);
        }
        [HttpPost]
        public ActionResult Cart(List<CartListModel> cartForm)
        {
            var userId = User.Identity.GetUserId();
            var carts = db.Carts.Where(x => x.UserId == userId).ToList();
            foreach (Cart cart in carts)
            {
                int formValue = cartForm.Where(x => x.VariantId == cart.VariantId).FirstOrDefault().Quantity;
                if (cart.Quantity != formValue)
                {
                    cart.Quantity = formValue;
                }
            }
            db.SaveChanges();
            CartListModel co = new CartListModel();
            var model = co.GetCart(User.Identity.GetUserId());
            Decimal discount = co.CalculateDiscount(model);
            ViewBag.Discount = discount;
            return View(model);
        }
        public ActionResult DeleteCart(Int64 VariantId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var cart = db.Carts.Where(x => x.UserId == UserId &&
                x.VariantId == VariantId).FirstOrDefault();
                if (cart != null)
                {
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Cart");
        }
        public ActionResult DeleteAllCart()
        {
            if (User.Identity.IsAuthenticated)
            {
                var UserId = User.Identity.GetUserId();
                var carts = db.Carts.Where(x => x.UserId == UserId).ToList();
                if (carts != null)
                {
                    db.Carts.RemoveRange(carts);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Cart");
        }
        public ActionResult Checkout()
        {
            #region fillFormData
            var countries = db.Countries.OrderBy(x => x.Name).ToList();
            ViewBag.BillingCountryId = new SelectList(countries, "CountryId", "Name");
            ViewBag.ShippingCountryId = new SelectList(countries, "CountryId", "Name");

            var selectState = new List<string>();
            ViewBag.BillingStateId = new SelectList(selectState);
            ViewBag.ShippingStateId = new SelectList(selectState);

            var selectCity = new List<string>();
            ViewBag.BillingCityId = new SelectList(selectCity);
            ViewBag.ShippingCityId = new SelectList(selectCity);

            var selectShippingMethod = new List<string>();
            ViewBag.ShippingMethodId = new SelectList(selectShippingMethod);

            var selectPaymentMethod = new List<string>();
            ViewBag.PaymentMethodId = new SelectList(selectPaymentMethod);

            CartListModel co = new CartListModel();
            var cart = co.GetCart(User.Identity.GetUserId());
            ViewBag.Cart = cart;
            Decimal discount = co.CalculateDiscount(cart);
            ViewBag.Discount = discount;
            #endregion
            return View();
        }
        [HttpPost]
        public ActionResult CheckoutPay(CheckoutModel checkout, CreditCardModel card, String paypal)
        {
            string userId = User.Identity.GetUserId();
            CartListModel co = new CartListModel();
            try
            {
                var cart = co.GetCart(User.Identity.GetUserId());
                #region createorder 
                var order = new Order();
                order.UserId = userId;
                order.BillingCityId = checkout.BillingCityId;
                order.BillingCompany = checkout.BillingCompany;
                order.BillingCountryId = checkout.BillingCountryId;
                order.BillingEmail = checkout.BillingEmail;
                order.BillingFirstName = checkout.BillingFirstName;
                order.BillingLastName = checkout.BillingLastName;
                order.BillingStateId = checkout.BillingStateId;
                order.BillingStreet1 = checkout.BillingStreet1;
                order.BillingStreet2 = checkout.BillingStreet2;
                order.BillingTelephone = checkout.BillingTelephone;
                order.BillingZip = checkout.BillingZip;
                order.ShippingCityId = checkout.ShippingCityId;
                order.ShippingCompany = checkout.ShippingCompany;
                order.ShippingCost = checkout.ShippingCost;
                order.ShippingCountryId = checkout.ShippingCountryId;
                order.ShippingFirstName = checkout.ShippingFirstName;
                order.ShippingLastName = checkout.ShippingLastName;
                order.ShippingMethodId = checkout.ShippingMethodId;
                order.ShippingStateId = checkout.ShippingStateId;
                order.ShippingStreet1 = checkout.ShippingStreet1;
                order.ShippingStreet2 = checkout.ShippingStreet2;
                order.ShippingTelephone = checkout.ShippingTelephone;
                order.ShippingZip = checkout.ShippingZip;
                order.OrderDate = DateTime.Now;
                order.OrderStatus = Order.OrderStatuses.Received;
                order.Paid = false;
                order.PaymentMethodId = checkout.PaymentMethodId;
                if (card.cardNumber != null)
                {
                    order.CardAccount = card.cardNumber.Substring(card.cardNumber.Length - 4, 4);
                    order.CardHolderName = card.cardHolder;
                }
                if (paypal != null)
                {
                    //TODO:
                    //add paypal to Order
                }

                var productTotal = cart.Sum(x => x.TotalPrice);
                var weight = cart.Sum(x => x.Quantity) * 0.25;
                var shippingCosts = db.ShippingCosts
                    .Where(x => x.ShippingMethodId == checkout.ShippingMethodId &&
                    x.CountryId == checkout.ShippingCountryId).FirstOrDefault();
                decimal shippingCost = 0;
                if (weight <= 0.5)
                {
                    shippingCost = shippingCosts.CostHalf;
                }
                else if (weight <= 1)
                {
                    shippingCost = shippingCosts.CostOne;
                }
                else if (weight <= 1.5)
                {
                    shippingCost = shippingCosts.CostOneHalf;
                }
                else if (weight <= 2)
                {
                    shippingCost = shippingCosts.CostTwo;
                }
                else
                {
                    shippingCost = shippingCosts.CostTwoHalf;
                }
                var paymentMethod = db.PaymentMethods.Find(checkout.PaymentMethodId);
                decimal paymentDiscount = paymentMethod.PaymentDiscount;
                order.Discount = paymentDiscount;
                order.TotalPrice = productTotal + shippingCost - paymentDiscount;
                order.ShippingCost = shippingCost;
                decimal productCost = cart.Sum(x => x.TotalCost);
                decimal paymentCost = (paymentMethod.PercentCost * order.TotalPrice/100) + paymentMethod.StaticCost;

                order.TotalCost = productCost + paymentCost + shippingCost;

                order.TotalProfit = order.TotalPrice - order.TotalCost;

                db.Orders.Add(order);
                db.SaveChanges();
                #endregion

                foreach (CartListModel item in cart)
                {
                    var op = new OrderProduct();
                    op.OrderId = order.OrderId;
                    op.Quantity = item.Quantity;
                    op.TotalCost = item.TotalCost;
                    op.TotalPrice = item.TotalPrice;
                    op.UnitPrice = item.UnitPrice;
                    op.VariantId = item.VariantId;
                    op.Cost = item.Cost;
                    db.OrderProducts.Add(op);
                }
                db.SaveChanges();

                //TODO: Process Payment if necessary.

                db.Carts.RemoveRange(db.Carts.Where(x => x.UserId == userId));
                db.SaveChanges();
                string errorMessage = "Success";
                string pageContent = "";
                string conversationId = "";
                if (checkout.PaymentMethodId == 1 || checkout.PaymentMethodId == 2)
                {
                    bool success = PayWithIyzipay(order, card, userId, cart, ref errorMessage, ref pageContent, ref conversationId);
                    if (success)
                    {
                        order.ConversationId = conversationId;
                        db.SaveChanges();
                        Response.Write(pageContent);
                        return View();
                    }
                    else
                    {
                        order.PaymentError = errorMessage;
                        db.SaveChanges();
                    }
                }
                var emailHelper = new EmailHelper();
                emailHelper.SendOrderReceived(order,Request,Url);
                return RedirectToAction("Order", "MyOrders", new { id = order.OrderId, message = errorMessage });
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message + "<br>";
                if (ex.InnerException != null)
                {
                    errorMessage = "Inner ex: " + ex.InnerException.Message + "<br>";
                    if (ex.InnerException != null)
                    {
                        errorMessage = "Inner ex: " + ex.InnerException.Message + "<br>";
                    }
                }
                ViewBag.Error = ex.Message;
                #region fillFormData
                var countries = db.Countries.OrderBy(x => x.Name).ToList();
                ViewBag.BillingCountryId = new SelectList(countries, "CountryId", "Name");
                ViewBag.ShippingCountryId = new SelectList(countries, "CountryId", "Name");

                var selectState = new List<string>();
                ViewBag.BillingStateId = new SelectList(selectState);
                ViewBag.ShippingStateId = new SelectList(selectState);

                var selectCity = new List<string>();
                ViewBag.BillingCityId = new SelectList(selectCity);
                ViewBag.ShippingCityId = new SelectList(selectCity);

                var selectShippingMethod = new List<string>();
                ViewBag.ShippingMethodId = new SelectList(selectShippingMethod);

                var selectPaymentMethod = new List<string>();
                ViewBag.PaymentMethodId = new SelectList(selectPaymentMethod);


                var cart = co.GetCart(User.Identity.GetUserId());
                ViewBag.Cart = cart;
                #endregion
                return View();
            }
        }

        private bool PayWithIyzipay(Order order, CreditCardModel card, String userId, IEnumerable<CartListModel> cart, ref string ErrorMessage, ref string PageContent, ref string ConversationId)
        {
            var user = db.Users.Find(userId);
            var billingCity = db.Cities.Find(order.BillingCityId).Name;
            var shippingCity = db.Cities.Find(order.ShippingCityId).Name;

            Options options = new Options();
            options.ApiKey = ConfigurationManager.AppSettings["IyziApiKey"].ToString();
            options.SecretKey = ConfigurationManager.AppSettings["IyziSecretKey"].ToString();
            options.BaseUrl = "https://sandbox-api.iyzipay.com";


            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = order.OrderId.ToString();
            request.Price = order.TotalPrice.ToString().Replace(",", ".");
            request.PaidPrice = order.TotalPrice.ToString().Replace(",", ".");
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = order.OrderId.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();
            request.CallbackUrl = "http://" + Request.Url.Authority + "/Checkout/IyziReturn";

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = card.cardHolder;
            paymentCard.CardNumber = card.cardNumber;
            paymentCard.ExpireMonth = card.cardExpirationMonth.ToString();
            paymentCard.ExpireYear = card.cardExpirationYear.ToString();
            paymentCard.Cvc = card.cardCvv.ToString();
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = userId;
            buyer.Name = order.BillingFirstName;
            buyer.Surname = order.BillingLastName;
            buyer.GsmNumber = "+90" + order.BillingTelephone;
            buyer.Email = order.BillingEmail;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", user.LastLoginTime); //"2015-10-05 12:43:35";
            buyer.RegistrationDate = String.Format("{0:yyyy-MM-dd HH:mm:ss}", user.RegistrationDate); //"2013-04-21 15:12:09";
            buyer.RegistrationAddress = order.BillingStreet1 + " " + order.BillingStreet2;
            buyer.Ip = Request.UserHostAddress;
            buyer.City = billingCity;
            buyer.Country = "Turkey";
            buyer.ZipCode = order.BillingZip;
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = order.ShippingFirstName + " " + order.ShippingLastName;
            shippingAddress.City = shippingCity;
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = order.ShippingStreet1 + " " + order.ShippingStreet2;
            shippingAddress.ZipCode = order.ShippingZip;
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = order.BillingFirstName + " " + order.BillingLastName;
            billingAddress.City = billingCity;
            billingAddress.Country = "Turkey";
            billingAddress.Description = order.BillingStreet1 + " " + order.BillingStreet2;
            billingAddress.ZipCode = order.BillingZip;
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            foreach (CartListModel item in cart)
            {
                BasketItem basketItem = new BasketItem();
                basketItem.Id = item.VariantId.ToString();
                basketItem.Name = item.VariantName + " " + item.ProductName;
                basketItem.Category1 = "Giyim";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItem.Price = item.TotalPrice.ToString().Replace(",", ".");
                basketItems.Add(basketItem);
            }
            BasketItem shippingItem = new BasketItem();
            shippingItem.Id = "K1";
            shippingItem.Name = "Kargo";
            shippingItem.Category1 = "Kargo";
            shippingItem.ItemType = BasketItemType.VIRTUAL.ToString();
            shippingItem.Price = order.ShippingCost.ToString().Replace(",", ".");
            basketItems.Add(shippingItem);

            request.BasketItems = basketItems;

            //Payment payment = Payment.Create(request, options);
            ThreedsInitialize threedsInitialize = ThreedsInitialize.Create(request, options);
            //PrintResponse<Payment>(payment);

            bool success = threedsInitialize.Status == "success";
            if (!success)
            {
                ErrorMessage = threedsInitialize.ErrorMessage;
            }
            else
            {
                ConversationId = threedsInitialize.ConversationId;
                PageContent = threedsInitialize.HtmlContent;
            }
            return success;
        }
        public ActionResult IyziReturn()
        {
            var result = Request.Form;
            var conversationId = result["conversationId"].ToString();
            var conversationData = result["conversationData"].ToString();
            var paymentId = result["paymentId"].ToString();
            var order = db.Orders.Where(x => x.ConversationId == conversationId).FirstOrDefault();

            if (order != null)
            {
                if (order.Paid == false)
                {
                    if (result["status"] == "success" && result["mdStatus"] == "1")
                    {
                        Options options = new Options();
                        options.ApiKey = ConfigurationManager.AppSettings["IyziApiKey"].ToString();
                        options.SecretKey = ConfigurationManager.AppSettings["IyziSecretKey"].ToString();
                        options.BaseUrl = "https://sandbox-api.iyzipay.com";

                        CreateThreedsPaymentRequest request = new CreateThreedsPaymentRequest();
                        request.Locale = Locale.TR.ToString();
                        request.ConversationId = conversationId;
                        request.PaymentId = paymentId;
                        request.ConversationData = conversationData;

                        ThreedsPayment threedsPayment = ThreedsPayment.Create(request, options);
                        if (threedsPayment.Status == "success")
                        {
                            order.Paid = true;
                        }
                        else
                        {
                            order.PaymentError = threedsPayment.ErrorMessage;
                        }
                    }
                    else
                    {
                        switch (result["mdStatus"])
                        {
                            case "0":
                                order.PaymentError = "3-D Secure imzası geçersiz veya doğrulama";
                                break;
                            case "2":
                                order.PaymentError = "Kart sahibi veya bankası sisteme kayıtlı değil";
                                break;
                            case "3":
                                order.PaymentError = "Kartın bankası sisteme kayıtlı değil";
                                break;
                            case "4":
                                order.PaymentError = "Doğrulama denemesi, kart sahibi sisteme daha sonra kayıt olmayı seçmiş";
                                break;
                            case "5":
                                order.PaymentError = "Doğrulama yapılamıyor";
                                break;
                            case "6":
                                order.PaymentError = "3-D Secure hatası";
                                break;
                            case "7":
                                order.PaymentError = "Sistem hatası";
                                break;
                            case "8":
                                order.PaymentError = "Bilinmeyen kart no";
                                break;
                            default:
                                break;
                        }
                    }
                    db.SaveChanges();
                }
            }


            return RedirectToAction("Order", "MyOrders", new { id = order.OrderId });
        }
        protected void PrintResponse<T>(T resource)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.WriteLine(JsonConvert.SerializeObject(resource, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }
        private void IyziTest()
        {
            Options options = new Options();
            options.ApiKey = "sandbox-sN9tZPaMwFy0uNj2E1AKJOx5tDzUIwwN";
            options.SecretKey = "sandbox-ve0yytU2wsTTuW033KEGHHoXH6R1v5Nx";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = "123456789";
            request.Price = "1";
            request.PaidPrice = "1.2";
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = "John Doe";
            paymentCard.CardNumber = "5528790000000008";
            paymentCard.ExpireMonth = "12";
            paymentCard.ExpireYear = "2030";
            paymentCard.Cvc = "123";
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = "John";
            buyer.Surname = "Doe";
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Binocular";
            firstBasketItem.Category1 = "Collectibles";
            firstBasketItem.Category2 = "Accessories";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "0.3";
            basketItems.Add(firstBasketItem);

            BasketItem secondBasketItem = new BasketItem();
            secondBasketItem.Id = "BI102";
            secondBasketItem.Name = "Game code";
            secondBasketItem.Category1 = "Game";
            secondBasketItem.Category2 = "Online Game Items";
            secondBasketItem.ItemType = BasketItemType.VIRTUAL.ToString();
            secondBasketItem.Price = "0.5";
            basketItems.Add(secondBasketItem);

            BasketItem thirdBasketItem = new BasketItem();
            thirdBasketItem.Id = "BI103";
            thirdBasketItem.Name = "Usb";
            thirdBasketItem.Category1 = "Electronics";
            thirdBasketItem.Category2 = "Usb / Cable";
            thirdBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            thirdBasketItem.Price = "0.2";
            basketItems.Add(thirdBasketItem);
            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request, options);
        }
    }
}