using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Commerce.Data {
    public enum MailerStatus {
        Queued,
        Sent,
        Retrying,
        Failed
    }

    public enum MailerType {
        CustomerOrderReceived=1,
        CustomerPaymentAuthFailed=2,
        CustomerOrderShipped=3,
        CustomerOrderCancelled=4,
        AdminOrderReceived=5,
        CustomerAddressValidationFailed=6,
        InventoryCheckFailed=7,
        AdminProcessingError=8
    }


    public class Mailer {

        public int ID { get; set; }
        public string ToEmailAddress { get; set; }
        public string FromEmailAddress { get; set; }
        public string UserName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastTried { get; set; }
        public string SMTPResponse { get; set; }
        public MailerStatus Status { get; set; }
        public MailerType MailType { get; set; }
        public bool IsHtml { get; set; }
        public Mailer() { }

        public Mailer(MailerType mailType, string email, string userName, 
            string subject, string body, bool isHtml) {
            MailType = mailType;
            ToEmailAddress = email;
            UserName = userName;
            Subject = subject;
            Body = body;
            IsHtml = isHtml;
        }

    }
}
