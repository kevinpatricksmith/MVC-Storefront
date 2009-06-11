using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Services;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for AuthorizationTests
    /// </summary>
    [TestClass]
    public class AuthenticationTests : TestBase
    {

        [TestMethod]
        public void AuthenticationService_Service_Can_Authenticate_Using_UserName_And_Password() {

            IAuthenticationService authService = new TestAuthenticationService();
            bool isAuthenticated = authService.IsValidLogin("test", "password");
            Assert.IsTrue(isAuthenticated);
        }

        [TestMethod]
        public void AuthenticationService_Service_Fails_Using_Invalid_UserName_And_Password() {

            IAuthenticationService authService = new TestAuthenticationService();
            bool isAuthenticated = authService.IsValidLogin("poo", "poo");
            Assert.IsFalse(isAuthenticated);
        }

        [TestMethod]
        public void AuthenticationService_Service_Can_Authenticate_Using_Url() {

            IAuthenticationService authService = new TestAuthenticationService();
            bool isAuthenticated = authService.IsValidLogin(new Uri("http://test.com/"));
            Assert.IsTrue(isAuthenticated);
        }

    }

}
