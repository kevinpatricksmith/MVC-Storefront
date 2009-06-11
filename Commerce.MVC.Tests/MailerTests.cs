using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for MailerTests
    /// </summary>
    [TestClass]
    public class MailerTests:TestBase {

        [TestMethod]
        public void Mailer_ShouldHave_UserName_Email_Subject_Body_HtmlFlag_Status_SendDate_SMTPResponse_Counts() {
            Mailer m = new Mailer(MailerType.CustomerOrderReceived,
                "test@test.com", "testuser", "test subject", "test body", false);
            Assert.AreEqual("test@test.com", m.ToEmailAddress);
            Assert.AreEqual("testuser", m.UserName);
            Assert.AreEqual("test subject", m.Subject);
            Assert.AreEqual("test body", m.Body);
        }

        [TestMethod]
        public void Mailer_Repository_Should_Return_5_Mailers_For_English() {
            Assert.AreEqual(8, _mailerRepository.GetMailerTemplates("en").Count());
        }

        [TestMethod]
        public void Mailer_Service_Should_Return_Mailer_For_OrderReceived() {
            Assert.IsNotNull(_mailerService.GetMailer(MailerType.CustomerOrderReceived,"en"));
        }

        Order GetTestOrder() {
            Order o = _orderService.GetCurrentOrder("testuser");
            o.AddItem(_catalogService.GetProduct(1), 1);
            o.AddItem(_catalogService.GetProduct(2), 1);
            o.AddItem(_catalogService.GetProduct(3), 1);
            o.AddItem(_catalogService.GetProduct(4), 1);

            o.ShippingMethod = _shippingService.CalculateRates(o)[0];
            o.PaymentMethod = new CreditCard("Visa", "testuser", "4586 9748 7358 4049", 10, 2010, "123");

            //add some addresses
            Address add = new Address("Joe", "Joe", "Tonks", "joe@joe.com",
                "1099 Alakea St", "", "Honolulu", "HI", "96813", "US");

            o.ShippingAddress = add;
            o.BillingAddress = add;


            return o;
        }

    }
}
