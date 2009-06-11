using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Commerce.Data;
using Commerce.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commerce.Tests
{
    /// <summary>
    /// Summary description for AddressTests
    /// </summary>
    [TestClass]
    public class AddressTests : TestBase
    {


        [TestMethod]
        public void Address_ShouldHave_First_Last_Email_Street_Street2_City_State_Zip()
        {
            Address add=new Address("testuser","first","last","email",
                "street1","street2","city","stateprovince","zip","country");

            Assert.AreEqual("testuser", add.UserName);
            Assert.AreEqual("first", add.FirstName);
            Assert.AreEqual("last", add.LastName);
            Assert.AreEqual("email", add.Email);
            Assert.AreEqual("street1", add.Street1);
            Assert.AreEqual("street2", add.Street2);
            Assert.AreEqual("city", add.City);
            Assert.AreEqual("stateprovince", add.StateOrProvince);
            Assert.AreEqual("zip", add.Zip);
            Assert.AreEqual("country", add.Country);
        }

        [TestMethod]
        public void OrderRepository_Should_Return_Addresses()
        {
            Assert.IsNotNull(_orderRepository.GetAddresses());

        }
        [TestMethod]
        public void OrderRepository_Should_Return_2_TestAddresses()
        {
            Assert.AreEqual(2,_orderRepository.GetAddresses().Count());


        }
        [TestMethod]
        public void OrderService_Should_Return_2_Addresses_ForTestUser()
        {
            Assert.AreEqual(2, _orderService.GetAddresses("testuser").Count);

        }

        [TestMethod]
        public void Address_ShouldHave_Formatted_ToString()
        {

            Address add = _orderService.GetAddresses("testuser")[0];
            string expected = "first0 last0\r\nstreet10\r\nstreet20\r\ncity0, stateprovince0 zip0, country0\r\n";
            Assert.AreEqual(expected, add.ToString());

        }


        [TestMethod]
        public  void OrderRepository_CanSave_NewAddress_ForTestUser()
        {
            var add = new Address("testuser", "test", "user", "email",
            "newstreet1", "newstreet2", "newcity",
            "stateprovince", "zip", "country");

            _orderRepository.SaveAddress(add);
            IList<Address> adds = _orderService.GetAddresses("testuser");
            Assert.AreEqual(3,adds.Count);

        }

        [TestMethod]
        public void OrderRepository_CannotSave_DuplicateAddresses_ForTestUser()
        {
            var add = new Address("testuser", "test", "user", "email",
            "newstreet1", "newstreet2", "newcity",
            "stateprovince", "zip", "country");

            _orderRepository.SaveAddress(add);
            IList<Address> adds = _orderService.GetAddresses("testuser");
            Assert.AreEqual(3, adds.Count);

            _orderRepository.SaveAddress(add);
            adds = _orderService.GetAddresses("testuser");
            Assert.AreEqual(3, adds.Count);


        }

        [TestMethod]
        public void OrderService_CanReturn_Address_byID()
        {
            Address add = _orderService.GetAddress(1);
            Assert.IsNotNull(add);

        }


    }

}
