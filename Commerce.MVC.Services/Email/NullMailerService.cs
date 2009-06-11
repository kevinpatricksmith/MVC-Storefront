using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using System.Net.Mail;

namespace Commerce.Services
{
    public class NullMailerService:IMailerService
    {

        IMailerRepository _mailerRepository;
        ILogger _logger;

        string _orderAbsoluteRoot;

        public NullMailerService(IMailerRepository mailerRepository, ILogger logger)
        {
            _mailerRepository = mailerRepository;
            _logger = logger;
            
            if(System.Configuration.ConfigurationManager.AppSettings["StoreOrderRoot"]!=null)
                _orderAbsoluteRoot = System.Configuration.ConfigurationManager.AppSettings["StoreOrderRoot"].ToString();
        }


        public void Send(Commerce.Data.Mailer mailer)
        {
            _logger.Info("Didn't send an email to "+mailer.ToEmailAddress);
        }

        public void Send(MailMessage message) {

            _logger.Info("Didn't send an email to " + message.To);

        }

        public void SendAdminEmail(Order order, string subject, string body) {

            string adminEmail = System.Configuration.ConfigurationManager.AppSettings["StoreEmail"];
            body = "Message RE Order " + order.OrderNumber + Environment.NewLine + body;
            MailMessage message = new MailMessage(adminEmail, adminEmail, subject, body);
            Send(message);

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

        public void SendOrderEmail(Commerce.Data.Order order, Commerce.Data.MailerType mailType)
        {
            //pull the mailer
            Mailer mailer = GetMailerForOrder(order, mailType);

            //no catches here - let bubble to caller
            Send(mailer);
        }

    }
}
