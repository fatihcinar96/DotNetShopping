using DotNetShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

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
    }

}