using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.MVC.Web.Controllers;
using System.Web.Mvc;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Tests {


    /// <summary>
    /// Summary description for CatalogControllerTests
    /// </summary>
    [TestClass]
    public class CatalogControllerTests:TestBase {


        [TestMethod]
        public void CatalogController_ShouldContain_IndexMethod_WhichAccepts_CategoryAndSubcategory() {

            CatalogController c = new CatalogController(_catalogService,_personalizationService);
            ActionResult result = c.Index(null);

            Assert.IsNotNull(result);
        }



        [TestMethod]
        public void CatalogController_IndexMethod_ShouldReturn_Products_For_Parent1() {

            CatalogController c = new CatalogController(_catalogService, _personalizationService);

            ViewResult result = (ViewResult)c.Index(null);

            Category category = (Category)result.ViewData.Model;

            Assert.IsNotNull(category.Products);
            Assert.IsTrue(category.Products.Count() > 0);

            Assert.IsNotNull(result);
        }




        [TestMethod]
        public void CatalogController_ShowMethod_ShouldReturn_ViewData_ForProduct_SKU1() {

            CatalogController c = new CatalogController(_catalogService, _personalizationService);
            ViewResult result = (ViewResult)c.Show("SKU1");
            Product p = (Product)result.ViewData.Model;

            Assert.IsNotNull(p);
            Assert.AreEqual(1, p.ID);
            Assert.AreEqual("Product1", p.Name);

            Assert.IsNotNull(result);
        }



    }
}
