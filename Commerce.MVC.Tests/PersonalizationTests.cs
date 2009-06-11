
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using System.Linq;
using Commerce.Services;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for PersonalizationTests
    /// </summary>
    [TestClass]
    public class PersonalizationTests : TestBase
    {

        [TestMethod]
        public void Personalization_UserEvent_Should_Track_UserName_IP_Category_ProductID_OrderID_Date_Behavior()
        {
            System.Guid orderID = System.Guid.NewGuid();
            UserEvent ue = new UserEvent("testuser", 
                "127.0.0.1", 1, 1, orderID, UserBehavior.LoggingIn);

            Assert.AreEqual(ue.Behavior, UserBehavior.LoggingIn);
            Assert.AreEqual(ue.IP, "127.0.0.1");
            Assert.AreEqual(ue.CategoryID, 1);
            Assert.AreEqual(ue.OrderID, orderID);
            Assert.AreEqual(ue.ProductID, 1);
        
        }

        [TestMethod]
        public void Personalization_Repository_Should_Return_User_Events()
        {
            Assert.IsNotNull(_personalizationRepository.GetEvents());
        }

        [TestMethod]
        public void Personalization_Repository_Should_Save_User_Events()
        {
            int repoCount = _personalizationRepository.GetEvents().Count();
            UserEvent ue = new UserEvent("testuser", "127.0.0.1", 1, 1, System.Guid.NewGuid(), UserBehavior.LoggingIn);
            _personalizationRepository.Save(ue);
            Assert.AreEqual(repoCount + 1, _personalizationRepository.GetEvents().Count());
        }

        [TestMethod]
        public void Personalization_User_Profile_Should_Have_DefaultAddress_LastFiveProducts_LastFiveCategories_FavoriteCategory()
        {
            UserProfile prof = new UserProfile("testuser");
            prof.AddressBook = new LazyList<Address>(_orderService.GetAddresses("testuser").AsQueryable());
            //Address ID 1 is the default
            Address defaultAddress = _orderRepository.GetAddresses().Where(x => x.ID == 1).SingleOrDefault();
            Assert.AreEqual(prof.UserName, "testuser");
            Assert.AreEqual(defaultAddress, prof.DefaultAddress);

        }

        [TestMethod]
        public void Personalization_Service_Should_Return_UserProfile_For_TestUser()
        {
            Assert.IsNotNull(_personalizationService.GetProfile("testuser"));
        }

        [TestMethod]
        public void Personalization_Service_Should_Return_UserProfile_With_5_LastCategoriesViewed_For_TestUser()
        {
            UserProfile prof=_personalizationService.GetProfile("testuser");
            Assert.AreEqual(5, prof.LastCategoriesViewed.Count);

        }
        [TestMethod]
        public void Personalization_Service_Should_Return_UserProfile_With_5_Distinct_LastCategoriesViewed_For_TestUser()
        {
            UserProfile prof = _personalizationService.GetProfile("testuser");
            Assert.AreEqual(5, prof.LastCategoriesViewed.Distinct().Count());

        }

        [TestMethod]
        public void Personalization_Service_Should_Return_UserProfile_With_5_Sorted_LastCategoriesViewed_For_TestUser()
        {
            UserProfile prof = _personalizationService.GetProfile("testuser");

            Assert.AreEqual(10, prof.LastCategoriesViewed[0].ID);
            Assert.AreEqual(11, prof.LastCategoriesViewed[1].ID);
            Assert.AreEqual(12, prof.LastCategoriesViewed[2].ID);
            Assert.AreEqual(13, prof.LastCategoriesViewed[3].ID);
            Assert.AreEqual(14, prof.LastCategoriesViewed[4].ID);

        } 
        [TestMethod]
        public void Personalization_Service_Should_Return_FavoriteCategory_of_12_For_TestUser()
        {
            UserProfile prof = _personalizationService.GetProfile("testuser");
            Assert.AreEqual(12, prof.FavoriteCategory.ID);
        }

        [TestMethod]
        public void Personalization_Service_Should_Return_UserProfile_With_5_Distinct_LastProductsViewed_For_TestUser()
        {
            UserProfile prof = _personalizationService.GetProfile("testuser");

            Assert.AreEqual(5, prof.LastProductsViewed.Count);
            Assert.AreEqual(1, prof.LastProductsViewed[0].ID);
            Assert.AreEqual(2, prof.LastProductsViewed[1].ID);
            Assert.AreEqual(3, prof.LastProductsViewed[2].ID);
            Assert.AreEqual(4, prof.LastProductsViewed[3].ID);
            Assert.AreEqual(5, prof.LastProductsViewed[4].ID);

        }

        [TestMethod]
        public void Personalization_Service_Should_Throw_With_No_UserName_On_Save()
        {
            UserEvent ue = new UserEvent("", "127.0.0.1", 1, 1, System.Guid.NewGuid(), UserBehavior.LoggingIn);
            bool threw = false;
            try
            {
                _personalizationService.SaveUserEvent(ue);
            }
            catch (System.InvalidOperationException x)
            {
                threw = true;
            }
            Assert.IsTrue(threw);
        }

        [TestMethod]
        public void Personalization_Service_Should_Throw_With_No_IP_On_Save()
        {
            UserEvent ue = new UserEvent("testuser", "", 1, 1, System.Guid.NewGuid(), UserBehavior.LoggingIn);
            bool threw = false;
            try
            {
                _personalizationService.SaveUserEvent(ue);
            }
            catch (System.InvalidOperationException x)
            {
                threw = true;
            }
            Assert.IsTrue(threw);
        }


        [TestMethod]
        public void Personalization_Service_Should_Save_Valid_UserEvent()
        {
            UserEvent ue = new UserEvent("testuser", "127.0.0.1", 1, 1, System.Guid.NewGuid(), UserBehavior.LoggingIn);
            int eventCount = _personalizationRepository.GetEvents().Count();
            _personalizationService.SaveUserEvent(ue);
            Assert.AreEqual(eventCount + 1, _personalizationRepository.GetEvents().Count());

        }

    }
}
