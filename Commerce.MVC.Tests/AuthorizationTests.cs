using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Commerce.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for AuthorizationTests
    /// </summary>
    [TestClass]
    public class AuthorizationTests : TestBase
    {

        [TestMethod]
        public void Authorization_Service_Can_Authorize_TestUser_As_SuperAdmin() {
            IAuthorizationService authService = new TestAuthorizationService();
            bool isSuper = authService.IsSuperAdmin("testuser");
            Assert.IsTrue(isSuper);
        }

        [TestMethod]
        public void Authorization_Service_Fails_SuperAdmin_When_User_Is_Not_TestUser() {
            IAuthorizationService authService = new TestAuthorizationService();
            bool isSuper = authService.IsSuperAdmin("poo");
            Assert.IsFalse(isSuper);
        }
    }
}
