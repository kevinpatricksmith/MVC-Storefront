using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Services;
using Commerce.Data;
using System.Net.Mail;

namespace Commerce.Tests {
    public class TestMailerService:IMailerService {


        public List<Mailer> SentMail = new List<Mailer>();

        public void Send(Mailer mailer) {
            SentMail.Add(mailer);
        }
        public void Send(MailMessage message) {

            Mailer m = new Mailer();
            m.Body = message.Body;
            m.ToEmailAddress = message.To[0].Address;
            m.Subject = message.Subject;
            
            SentMail.Add(m);
        }
        public Mailer GetMailer(MailerType mailType, string languageCode) {
            return new Mailer(mailType, "test@test.com", "testuser", "subject", "testbody #STORENAME# #USERNAME# #ORDERLINK#",false);

        }
        public void SendAdminEmail(Order order, string subject, string body) {

            string adminEmail = System.Configuration.ConfigurationManager.AppSettings["StoreEmail"];
            body = "Message RE Order " + order.OrderNumber + Environment.NewLine + body;

            MailMessage message = new MailMessage(adminEmail, adminEmail, subject, body);
            Send(message);

        }
        public Mailer GetMailer(MailerType mailType) {
            return GetMailer(mailType, "");
        }

        public Mailer GetMailerForOrder(Order order, MailerType mailType) {
            //pull the template
            Mailer mailer = GetMailer(mailType, order.UserLanguageCode);


            //format it
            //this will change, for sure
            //need some better injection method
            string storeName = System.Configuration.ConfigurationManager.AppSettings["StoreName"].ToString();
            string storeReplyTo = System.Configuration.ConfigurationManager.AppSettings["StoreEmail"].ToString();

            mailer.Body = mailer.Body.Replace("#STORENAME#", storeName)
                .Replace("#USERNAME#", order.ShippingAddress.FullName)
                .Replace("#ORDERLINK#", System.IO.Path.Combine("http://store", order.ID.ToString()));

            mailer.FromEmailAddress = storeReplyTo;
            mailer.ToEmailAddress = order.ShippingAddress.Email;
            mailer.UserName = order.UserName;
            mailer.Status = MailerStatus.Queued;

            return mailer;
        }

        public void SendOrderEmail(Order order, MailerType mailType) {
            //pull the mailer
            Mailer mailer = GetMailerForOrder(order, mailType);

            //no catches here - let bubble to caller
            Send(mailer);
        }

    }
}
