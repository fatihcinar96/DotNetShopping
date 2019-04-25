using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;

namespace DotNetShopping.Helpers
{
    public class EmailHelper
    {
        public bool SendEmail(EmailModel model,ref string Error)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.To.Add(model.MailTo);
                mailMessage.Subject = model.Title;
                mailMessage.Body = model.Body;
                mailMessage.IsBodyHtml = true;

                SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.Timeout = 20000;
                mailClient.Send(mailMessage);
                return true;
            }
            catch(SmtpException smtpex)
            {
                Error = smtpex.Message;
                return false;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return false;
            }
        }

        public void SendOrderReceived(Order order, HttpRequestBase Request,System.Web.Mvc.UrlHelper Url)
        {

                var fileContents = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/Emails/OrderReceived.html"));
                fileContents = fileContents.Replace("[#ORDERNO#]", StringHelper.GetOrderNo(order.OrderId,order.OrderDate));
                fileContents = fileContents.Replace("[#ORDERDATE#]", order.OrderDate.ToShortDateString());
                fileContents = fileContents.Replace("[#PAYMENT#]", order.Paid ? "PAID" : "NOT PAID");
                fileContents = fileContents.Replace("[#TOTALPRICE#]", order.TotalPrice.ToString());

                var orderDetails = order.GetOrderDetail(order.OrderId);
                string productsHtml = "";
                foreach(var product in orderDetails.OrderProducts)
                {
                    productsHtml += "<tr>";
                productsHtml += "<td><img src =\"" + Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content("~/ProductImage/" + product.FileName + "-1.jpg")
                + "\" /></td>";
                    productsHtml += "<td>" + product.VariantName + " " + product.ProductName + "</td>";
                    productsHtml += "<td>" + product.Quantity + "</td>";
                    productsHtml += "<td>" + product.UnitPrice + "</td>";
                    productsHtml += "<td>" + product.TotalPrice + "</td>";
                    productsHtml += "</tr>";
                }
               fileContents =  fileContents.Replace("[#PRODUCTS#]" , productsHtml);
            var model = new EmailModel();
            model.Body = fileContents;
            model.MailTo = order.BillingEmail;
            model.Title = "Order Received!";
            string error = "";
            SendEmail(model, ref error);
        }


        public void SendOrderShipped(Int64 OrderId, HttpRequestBase Request, System.Web.Mvc.UrlHelper Url,ApplicationUser user)
        {
            var order = new Order();
            var orderDetails = order.GetOrderDetail(OrderId);

            var fileContents = System.IO.File.ReadAllText(HostingEnvironment.MapPath(@"~/Content/Emails/OrderShipped.html"));
            fileContents = fileContents.Replace("[#ORDERNO#]", StringHelper.GetOrderNo(orderDetails.OrderId, orderDetails.OrderDate));
            fileContents = fileContents.Replace("[#ORDERDATE#]", orderDetails.OrderDate.ToShortDateString());
            fileContents = fileContents.Replace("[#PAYMENT#]", orderDetails.Paid ? "PAID" : "NOT PAID");
            fileContents = fileContents.Replace("[#SHIPPING#]", orderDetails.ShippingMethodName);
            fileContents = fileContents.Replace("[#SHIPPINGCODE#]", orderDetails.ShippingCode);
            fileContents = fileContents.Replace("[#SHIPPINGCOST#]","$" + orderDetails.ShippingCost.ToString());
            fileContents = fileContents.Replace("[#SHIPPINGADDRESS#]", orderDetails.CountryName + "," + orderDetails.CityName + "," + orderDetails.ShippingStreet1 + "," + orderDetails.ShippingStreet2);
            fileContents = fileContents.Replace("[#TOTALPRICE#]", orderDetails.TotalPrice.ToString());

            string productsHtml = "";
            foreach (var product in orderDetails.OrderProducts)
            {
                productsHtml += "<tr>";
                productsHtml += "<td><img src =\"" + Request.Url.Scheme + "://" + Request.Url.Authority + Url.Content("~/ProductImage/" + product.FileName + "-1.jpg")
                + "\" /></td>";
                productsHtml += "<td>" + product.VariantName + " " + product.ProductName + "</td>";
                productsHtml += "<td>" + product.Quantity + "</td>";
                productsHtml += "<td>" + product.UnitPrice + "</td>";
                productsHtml += "<td>" + product.TotalPrice + "</td>";
                productsHtml += "</tr>";
            }
            fileContents = fileContents.Replace("[#PRODUCTS#]", productsHtml);

            
            var model = new EmailModel();
            model.Body = fileContents;
            model.MailTo = user.Email;
            model.Title = "Order Shipped!";
            string error = "";
            SendEmail(model, ref error);
        }
    }

}