using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using System.Net.Mail;

namespace Commerce.Services {
    public interface IMailerService {

        void Send(Mailer mailer);
        void Send(MailMessage message);
        Mailer GetMailer(MailerType mailType, string languageCode);
        Mailer GetMailer(MailerType mailType);
        Mailer GetMailerForOrder(Order order, MailerType mailType);
        void SendOrderEmail(Order order, MailerType mailType);
        void SendAdminEmail(Order order, string subject, string message);
    }
}
