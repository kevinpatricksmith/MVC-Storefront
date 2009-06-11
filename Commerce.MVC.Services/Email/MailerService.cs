using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using System.Net.Mail;

namespace Commerce.Services {
    public class MailerService:IMailerService {

        IMailerRepository _mailerRepository;
        string _orderAbsoluteRoot;

        public MailerService(IMailerRepository mailerRepository) {
            _mailerRepository = mailerRepository;

            
            if(System.Configuration.ConfigurationManager.AppSettings["StoreOrderRoot"]!=null)
                _orderAbsoluteRoot = System.Configuration.ConfigurationManager.AppSettings["StoreOrderRoot"].ToString();
        }

        public Mailer GetMailerForOrder(Order order, MailerType mailType) {
            //pull the template
            Mailer mailer = GetMailer(mailType, order.UserLanguageCode);


            //format it
            //this will change, for sure
            //need some better injection method
            string storeName=System.Configuration.ConfigurationManager.AppSettings["StoreName"].ToString();
            string storeReplyTo=System.Configuration.ConfigurationManager.AppSettings["StoreEmail"].ToString();
            
            mailer.Body = mailer.Body.Replace("#STORENAME#", storeName)
                .Replace("#USERNAME#",order.ShippingAddress.FullName)
                .Replace("#ORDERLINK#",System.IO.Path.Combine(_orderAbsoluteRoot,order.ID.ToString()));

            mailer.FromEmailAddress = storeReplyTo;
            mailer.ToEmailAddress = order.ShippingAddress.Email;
            mailer.UserName = order.UserName;
            mailer.Status = MailerStatus.Queued;

            return mailer;
        }
        public Mailer GetMailer(MailerType mailType, string languageCode) {
            int mailerID = (int)mailType;
            Mailer result = _mailerRepository.GetMailerTemplates(languageCode).Where(x => x.ID == mailerID).SingleOrDefault();
            if (result == null)
                throw new InvalidOperationException("There is no mailer template for this language/template selection");

            return result;


        }

        public Mailer GetMailer(MailerType mailType) {
            
            //default to English... or your favorite...
            return GetMailer(mailType, "en");
        }  
      
        public void SendOrderEmail(Order order, MailerType mailType) {

            //pull the mailer
            Mailer mailer = GetMailerForOrder(order, mailType);
            
            //no catches here - let bubble to caller
            Send(mailer);

        }

        public void Send(Mailer mailer) {


            MailMessage message = new MailMessage(mailer.FromEmailAddress, mailer.ToEmailAddress, mailer.Subject, mailer.Body);
            message.IsBodyHtml = mailer.IsHtml;
            message.ReplyTo = new MailAddress(mailer.FromEmailAddress);

            SmtpClient smtp = new SmtpClient();

            //let the exceptions bubble...
            Send(message);

            //if no problem - set to success
            mailer.SMTPResponse = "Success";

            //reset the mailer status
            mailer.Status = MailerStatus.Sent;

        }
        public void Send(MailMessage mailer) {

            SmtpClient smtp = new SmtpClient();

            //let the exceptions bubble...
            smtp.Send(mailer);

        }

        public void SendAdminEmail(Order order, string subject, string body) {

            string adminEmail = System.Configuration.ConfigurationManager.AppSettings["StoreEmail"];
            body = "Message RE Order " + order.OrderNumber + Environment.NewLine + body;
            MailMessage message = new MailMessage(adminEmail, adminEmail, subject, body);
            Send(message);

        }

    }
}
