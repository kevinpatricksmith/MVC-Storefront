using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Tests {
    public class TestMailerRepository:IMailerRepository {

        List<Mailer> mailers;
        public TestMailerRepository() {
            mailers = new List<Mailer>();
            
            Mailer m = new Mailer(MailerType.CustomerOrderReceived, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);

            m = new Mailer(MailerType.AdminOrderReceived, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);

            m = new Mailer(MailerType.CustomerOrderCancelled, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);

            m = new Mailer(MailerType.CustomerPaymentAuthFailed, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);

            m = new Mailer(MailerType.CustomerOrderShipped, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);

            m = new Mailer(MailerType.InventoryCheckFailed, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);
            
            m = new Mailer(MailerType.AdminProcessingError, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);

            m = new Mailer(MailerType.CustomerAddressValidationFailed, "test@test.com", "name", "subject", "body #STORENAME# #USERNAME# #ORDERLINK#", false);
            mailers.Add(m);



        }
        public IQueryable<Mailer> GetMailerTemplates(string languageCode) {


            return mailers.AsQueryable();
        }

    }
}
