using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using Commerce.Services;
using System.Reflection;

namespace Commerce.Tests {


    [TestClass]
    public class CatalogTests : TestBase
    {

        #region Category Tests

        [TestMethod]
        public void CatalogRepository_Repository_Categories_IsNotNull() {
            ICatalogRepository rep = new TestCatalogRepository();
            Assert.IsNotNull(rep.GetCategories());
        }

        [TestMethod]
        public void CatalogService_Can_Get_Categories_From_Service() {

            IList<Category> categories = _catalogService.GetCategories();
            Assert.IsTrue(categories.Count > 0);
        }

        [TestMethod]
        public void CatalogService_Can_Group_ParentCategories() {

            IList<Category> categories = _catalogService.GetCategories();
            Assert.AreEqual(10, categories.Count);

        }

        [TestMethod]
        public void CatalogService_Can_Group_SubCategories() {

            IList<Category> categories = _catalogService.GetCategories();
            Assert.AreEqual(11, categories[0].SubCategories.Count());

        }
        #endregion

        #region Product Tests
        [TestMethod]
        public void Product_ShouldHave_Name_Description__Price_Discount_WeightInPounds_Fields() {

            Product p = new Product("TestName", "TestDescription",
                100, 20,5);

            Assert.AreEqual("TestName", p.Name);
            Assert.AreEqual("TestDescription", p.Description);
            Assert.AreEqual(20, p.DiscountPercent);
            Assert.AreEqual(100, p.Price);
            Assert.AreEqual(5, p.WeightInPounds);

        }

        [TestMethod]
        public void CatalogRepository_Contains_Products() {
            ICatalogRepository rep = new TestCatalogRepository();
            Assert.IsNotNull(rep.GetProducts());
        }


        [TestMethod]
        public void Repository_Returns_Single_Product_When_Filtered_ByID_1() {
            ICatalogRepository rep = new TestCatalogRepository();

            IList<Product> products = rep.GetProducts()
                .WithProductID(1)
                .ToList();

            Assert.AreEqual(1, products.Count);
        }

        [TestMethod]
        public void CatalogService_Returns_Single_Product_With_ProductID_1() {
            Product p = _catalogService.GetProduct(1);
            Assert.IsNotNull(p);
        }

        [TestMethod]
        public void CatalogService_Returns_Single_Product_With_ProductCode_SKU1() {
            Product p = _catalogService.GetProduct("SKU1");
            Assert.IsNotNull(p);
        }
        [TestMethod]
        public void Product_Has_Three_Images() {
            ICatalogRepository rep = new TestCatalogRepository();
            Product p = rep.GetProducts().Take(1).SingleOrDefault() ;
            Assert.AreEqual(3, p.Images.Count);

        }

        [TestMethod]
        public void Product_Has_Three_Test_CrossSell_Products() {
            ICatalogRepository rep = new TestCatalogRepository();
            Product p = rep.GetProducts().Take(1).SingleOrDefault();
            Assert.AreEqual(3, p.CrossSells.Count);

        }

        [TestMethod]
        public void Product_Has_A_DeliveryMethod_Setting()
        {
            ICatalogRepository rep = new TestCatalogRepository();
            Product p = rep.GetProducts().Take(1).SingleOrDefault();
            Assert.AreEqual(p.Delivery, DeliveryMethod.Shipped);

        }

        [TestMethod]
        public void Product_Should_Have_InventoryStatus_InStock_ByDefault() {
            Product p = new Product("test", "test", 1M, 0, 1M);
            Assert.AreEqual(InventoryStatus.InStock, p.Inventory);
        }

        [TestMethod]
        public void Catalog_Repository_Should_Insert_New_Product_On_Save() {
            Product p = new Product("TestName", "TestDescription",
                100, 20, 5);

            int productCount = _catalogRepository.GetProducts().Count();

            _catalogRepository.SaveProduct(p);

            int productCount2 = _catalogRepository.GetProducts().Count();

            Assert.IsTrue(productCount2 == productCount + 1);

        }

        [TestMethod]
        public void Product_DiscountedPrice_Should_Be_Price_Minus_DiscountPercent()
        {
            Product p = _catalogService.GetProduct(1);
            p.DiscountPercent = .25M;
            Assert.AreEqual<Decimal>(p.Price * .75M, p.DiscountedPrice); 

        }

        [TestMethod]
        public void Product_DiscountedPrice_Should_Equal_Price_With_0_Discount()
        {
            Product p = _catalogService.GetProduct(1);
            p.DiscountPercent = 0M;
            Assert.AreEqual<Decimal>(p.Price, p.DiscountedPrice); 
        }

        [TestMethod]
        public void Catalog_Repository_Should_Update_Existing_Product_On_Save() {
            Product p = _catalogService.GetProduct(1);
            Assert.AreEqual(InventoryStatus.InStock, p.Inventory);

            p.Inventory = InventoryStatus.BackOrder;
            _catalogRepository.SaveProduct(p);

            p = _catalogService.GetProduct(1);
            Assert.AreEqual(InventoryStatus.BackOrder, p.Inventory);

        }
        [TestMethod]
        public void ProductDescriptor_Should_Have_Title_and_Text() {
            ProductDescriptor pd = new ProductDescriptor(1, "title", "body");
            Assert.AreEqual(1, pd.ProductID);
            Assert.AreEqual("title", pd.Title);
            Assert.AreEqual("body", pd.Body);

        }

        [TestMethod]
        public void Product_Should_Have_0_or_More_Descriptors() {
            Product p = _catalogService.GetProduct(1);
            Assert.AreEqual(1, p.Descriptors.Count);
            Assert.AreEqual("Test", p.Descriptors[0].Title);
            Assert.AreEqual("Body", p.Descriptors[0].Body);
        }

        [TestMethod]
        public void Product_Should_Have_0_or_More_Recommended() {
            Product p = _catalogService.GetProduct(1);
            Assert.IsTrue(1 <= p.Recommended.Count);
        }

        #endregion

        #region Review Tests
        [TestMethod]
        public void Review_ShouldHave_Author_Email_ProductID_Body_Date() {

            ProductReview p = new ProductReview("Rob", 
                "robcon@microsoft.com", 1, "lorem ipsum");
            Assert.AreEqual("Rob", p.Author);
            Assert.AreEqual("robcon@microsoft.com", p.Email);
            Assert.AreEqual(1, p.ProductID);
            Assert.AreEqual("lorem ipsum", p.Body);
        }

        [TestMethod]
        public void Review_Repository_Can_Return_Reviews() {

            ICatalogRepository rep=new TestCatalogRepository();
            Assert.IsNotNull(rep.GetReviews());

        }
        #endregion

        #region Image Tests

        [TestMethod]
        public void Image_ShouldHave_Thumb_and_Full_FileNames() {

            Image i = new Image("thumb.gif", "full.gif");
            Assert.AreEqual("thumb.gif", i.ThumbnailPhoto);
            Assert.AreEqual("full.gif", i.FullSizePhoto);

        }

        #endregion

        #region Related Products
        [TestMethod]
        public void CatalogService_ShouldReturn_4_Related_for_Product1() {
            IList<Product> related = _catalogService.GetRelatedProducts(1);
            Assert.AreEqual(4, related.Count);
        }

        [TestMethod] 
        public void CatalogService_Should_Load_Related_Products_For_GetProduct_1(){
            Product p = _catalogService.GetProduct(1);
            Assert.AreEqual(4, p.RelatedProducts.Count);
        }
        #endregion


    }
}
