using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.MVC.Data;
using Commerce.MVC.Web.Controllers;
using System.Web.Mvc;
using Commerce.MVC.Services;

namespace Commerce.MVC.Tests.Controllers {
    /// <summary>
    /// Summary description for CartControllerTests
    /// </summary>
    [TestClass]
    public class CartControllerTests {
        IShoppingCartRepository _cartRepository;
        ICatalogRepository _catalogRepository;
        IUserRepository _userRepository;

        IShoppingCartService cartService;
        ICatalogService catalogService;
        IUserService userService;

        [TestInitialize]
        public void Startup() {
            _cartRepository = new TestShoppingCartRepository();
            _catalogRepository = new TestCatalogRepository();
            _userRepository = new TestUserRepository();

            cartService = new ShoppingCartService(_cartRepository);
            catalogService = new CatalogService(_catalogRepository);
            userService = new UserService(_userRepository);

        }

        [TestMethod]
        public void ShoppingCartController_AddItem_ShouldRedirect() {

            ShoppingCartController controller = new ShoppingCartController(cartService, catalogService);
            ActionResult result=controller.AddItem(1);

            //we should get a redirect here
            Assert.IsInstanceOfType(result, typeof(ActionRedirectResult));
        }
        [TestMethod]
        public void ShoppingCartController_AddItem_ShouldRedirect_ToItemAdded_AndPassIn_ID_1() {

            ShoppingCartController controller = new ShoppingCartController(cartService, catalogService);
            ActionRedirectResult result = (ActionRedirectResult)controller.AddItem(1);

            //we should get a redirect here
            Assert.AreEqual("ItemAdded", result.Values["action"].ToString());
            Assert.AreEqual("1", result.Values["productID"].ToString());
        }
    }
}
