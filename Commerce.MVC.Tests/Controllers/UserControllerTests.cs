using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.MVC.Data;
using Commerce.MVC.Services;
using Commerce.MVC.Web.Controllers;
using System.Web.Mvc;

namespace Commerce.MVC.Tests {
    /// <summary>
    /// Summary description for UserControllerTests
    /// </summary>
    [TestClass]
    public class UserControllerTests {
        IUserRepository _repository;
        IUserService userService;

        [TestInitialize]
        public void Setup() {
            _repository = new TestUserRepository();
            userService = new UserService(_repository);

        }

        [TestMethod]
        public void UserController_Exists_And_Accepts_A_Repository() {

            UserController controller = new UserController(userService);
            Assert.IsNotNull(controller);

        }
        #region Login
        [TestMethod]
        public void UserController_Index_Redirects_To_Login() {

            UserController controller = new UserController(userService);
            ActionResult result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ActionRedirectResult));

            ActionRedirectResult renderResult = result as ActionRedirectResult;
            Assert.AreEqual("Login", renderResult.Values["action"]);

        }


        [TestMethod]
        public void UserController_Has_a_Login_Method_That_Renders_The_LoginView() {

            UserController controller = new UserController(userService);
            ActionResult result = controller.Login();
            Assert.IsInstanceOfType(result, typeof(RenderViewResult));

            RenderViewResult renderResult = result as RenderViewResult;
            Assert.AreEqual("Login", renderResult.ViewName);

        }

        [TestMethod]
        public void UserController_Has_a_Logout_Method_That_Renders_The_HomeIndexView() {
            //TODO: Gotta mock the FormsAuth bits

            /*
            UserController controller = new UserController(_repository);
            controller.SetFakeControllerContext();
            ActionResult result = controller.Logout();
            Assert.IsInstanceOfType(result, typeof(RenderViewResult));

            RenderViewResult renderResult = result as RenderViewResult;
            Assert.AreEqual("Index", renderResult.ViewName);
            */
        }

        [TestMethod]
        public void UserController_Authenticate_Action_Redirects_For_User1_To_TestUrl() {
            UserController controller = new UserController(userService);

            controller.SetFakeControllerContextWithLogin("user1", "first1last1", "testurl");

            ActionResult result = controller.Authenticate();

            //a redirect result is a pass - which in this case it should be
            Assert.IsInstanceOfType(result, typeof(HttpRedirectResult));
            
            HttpRedirectResult redirectResult = result as HttpRedirectResult;
            Assert.AreEqual("testurl", redirectResult.Url);
        }

        [TestMethod]
        public void UserController_Authenticate_Action_Redirects_To_Login_On_Invalid_Login() {
            //TODO: Gotta mock the TempData bits
            /*
            UserController controller = new UserController(_repository);

            controller.SetFakeControllerContextWithLogin("foo", "bar", "testurl");

            ActionResult result = controller.Authenticate();

            //a redirect result is a pass - which in this case it should be
            Assert.IsInstanceOfType(result, typeof(ActionRedirectResult));

            ActionRedirectResult redirectResult = result as ActionRedirectResult;
            Assert.AreEqual("Login", redirectResult.Values["action"]);
             * */
        }
        #endregion


        #region Registration
        [TestMethod]
        public void UserController_Has_a_Register_Method_That_Renders_The_RegisterView() {

            UserController controller = new UserController(userService);
            ActionResult result = controller.Register();
            Assert.IsInstanceOfType(result, typeof(RenderViewResult));

            RenderViewResult renderResult = result as RenderViewResult;
            Assert.AreEqual("Register", renderResult.ViewName);

        }
        [TestMethod]
        public void UserController_Has_a_Create_Method_That_Registers_The_User() {

            //Can't use this until i mock up TempData

        }



        #endregion

    }
}
