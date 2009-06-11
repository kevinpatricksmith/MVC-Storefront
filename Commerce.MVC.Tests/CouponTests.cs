using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for CouponTests
    /// </summary>
    [TestClass]
    public class CouponTests:TestBase {
        [TestMethod]
        public void PercentOffOrder_Coupon_Should_Have_Code_and_Apply_Method_That_Takes_Order() {
            Order order = GetTestOrder();
            PercentOffOrderCoupon coupon = new PercentOffOrderCoupon(10);

            //this shouldn't fail
            coupon.ApplyCoupon(order);
        }

        [TestMethod]
        public void PercentOffOrder_Coupon_Should_Reduce_Order_SubTotal_By_TenPercent() {
            
            Order order = GetTestOrder();
            decimal preSubTotal = order.SubTotal;
            decimal postSubTotal = order.SubTotal - order.SubTotal * 0.1M;
            PercentOffOrderCoupon coupon = new PercentOffOrderCoupon(10);

            coupon.ApplyCoupon(order);
            Assert.AreEqual(postSubTotal, order.SubTotal);
        }


        [TestMethod]
        public void Incentive_Should_Have_Code_Coupon_Expiration() {
            
            ICoupon coupon=new PercentOffOrderCoupon(10);

            Incentive i = new Incentive("XYZ", coupon, DateTime.Today.AddDays(1));
            Assert.AreEqual("XYZ", i.Code);
            Assert.AreEqual(coupon, i.Coupon);
            Assert.IsFalse(i.IsExpired);
        }

        [TestMethod]
        public void IncentiveRepository_ShouldReturn_Incentives() {
            Assert.IsNotNull(_incentiveRepository.GetIncentives());
        }


        [TestMethod]
        public void IncentiveService_Should_Return_Incentive_1() {
            IIncentive i = _incentiveService.GetIncentive("1");
            Assert.IsNotNull(i);
        }

        [TestMethod]
        public void IncentiveService_Should_Return_Incentive_2_20_Percent_Off_Coupon() {
            IIncentive i = _incentiveService.GetIncentive("2");
            Order order = GetTestOrder();
            
            decimal before=order.SubTotal;
            decimal after=before-before*.2M;

            i.Coupon.ApplyCoupon(order);
            Assert.AreEqual(order.SubTotal, after);
        }

        [TestMethod]
        public void IncentiveService_ProcessCoupon_Should_Take_20_Percent_Off_TestOrder() {
            
            IIncentive i = _incentiveService.GetIncentive("2");
            Order order = GetTestOrder();
            decimal before = order.SubTotal;
            decimal after = before - before * .2M;

            _incentiveService.ProcessCoupon("2", order);

            Assert.AreEqual(order.SubTotal, after);
        }

        [TestMethod]
        public void Incentive_Should_Have_MinimumItems_MinAmount_MustHaveProducts() {
            Incentive i = new Incentive("XYZ", new PercentOffOrderCoupon(10), DateTime.Now);
            Assert.AreEqual(0, i.MinimumItems);
            Assert.AreEqual(0, i.MininumPurchase);
            Assert.AreEqual(0, i.MustHaveProducts.Length);
        }

        [TestMethod]
        public void Incentive_Should_Throw_When_MinItems_IsMore_Than_OrderItems() {
            Order order = GetTestOrder();

            Incentive i = new Incentive("XYZ", new PercentOffOrderCoupon(10), DateTime.Now.AddDays(1));
            i.MinimumItems = 5;

            bool thrown = false;
            try {
                i.ValidateUse(order);
            }
            catch(Exception x) {
                thrown = true;
                Assert.AreEqual("There is a minimum of " + i.MinimumItems.ToString() + " items required",x.Message);
            }

            Assert.IsTrue(thrown);

        }

        [TestMethod]
        public void Incentive_Should_Throw_When_MinPurchase_IsMore_Than_OrderSubTotal() {
            Order order = GetTestOrder();

            Incentive i = new Incentive("XYZ", new PercentOffOrderCoupon(10), DateTime.Now.AddDays(1));
            
            //order subtotal is 40
            i.MininumPurchase = 100;
            
            bool thrown = false;
            try {
                i.ValidateUse(order);
            }
            catch (Exception x) {
                thrown = true;
                Assert.AreEqual("There is a minimum of " + i.MininumPurchase.ToString("C") + " required", x.Message);
            }

            Assert.IsTrue(thrown);

        }

        [TestMethod]
        public void Incentive_Should_Throw_When_Exipired() {
            Order order = GetTestOrder();

            //DateTime.Now will always be expired
            Incentive i = new Incentive("XYZ", new PercentOffOrderCoupon(10), DateTime.Now);

            bool thrown = false;
            try {
                i.ValidateUse(order);
            }
            catch (Exception x) {
                thrown = true;
                Assert.AreEqual("This coupon is Expired", x.Message);
            }

            Assert.IsTrue(thrown);

        }

        [TestMethod]
        public void Incentive_Should_NotThrow_When_MustHaveProducts_Include_SKU1() {
            Order order = GetTestOrder();

            Incentive i = new Incentive("XYZ", new PercentOffOrderCoupon(10), DateTime.Now.AddDays(1));
            i.MustHaveProducts = new string[] { "SKU1" };
            bool thrown = false;
            try {
                i.ValidateUse(order);
            }
            catch {
                thrown = true;
            }

            Assert.IsFalse(thrown);

        }

        [TestMethod]
        public void Incentive_Should_Throw_When_MustHaveProducts_Include_SKU10000() {
            Order order = GetTestOrder();

            Incentive i = new Incentive("XYZ", new PercentOffOrderCoupon(10), DateTime.Now.AddDays(1));
            i.MustHaveProducts =new string[]{"SKU10000"};
            bool thrown = false;
            try {
                i.ValidateUse(order);
            }
            catch (Exception x) {
                thrown = true;
                Assert.AreEqual("This coupon is not valid for the items you've selected", x.Message);
            }

            Assert.IsTrue(thrown);
        }


        [TestMethod]
        public void IncentiveService_Should_Throw_When_Using_EXPIRED_Code() {
            Order order = GetTestOrder();

            bool thrown = false;
            try {
                _incentiveService.ProcessCoupon("EXPIRED", order);
            }
            catch (Exception x) {
                thrown = true;
                Assert.AreEqual("This coupon is Expired", x.Message);
            }

            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void IncentiveService_Should_Throw_When_Using_INVALID_MINITEMS_Code() {
            Order order = GetTestOrder();

            bool thrown = false;
            try {
                _incentiveService.ProcessCoupon("INVALID_MINITEMS", order);
            }
            catch (Exception x) {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }


        [TestMethod]
        public void IncentiveService_Should_Throw_When_Using_INVALID_MINPURCHASE_Code() {
            Order order = GetTestOrder();

            bool thrown = false;
            try {
                _incentiveService.ProcessCoupon("INVALID_MINPURCHASE", order);
            }
            catch (Exception x) {
                thrown = true;
            }

            Assert.IsTrue(thrown);
        }

        [TestMethod]
        public void IncentiveService_Should_Not_Apply_Incentive_More_Than_Once() {
            Order order = GetTestOrder();
            decimal before = order.SubTotal;
            decimal after = before - before * .1M;

            //10% off
            _incentiveService.ProcessCoupon("1", order);
            _incentiveService.ProcessCoupon("1", order);
            _incentiveService.ProcessCoupon("1", order);
            _incentiveService.ProcessCoupon("1", order);

            Assert.AreEqual(after, order.SubTotal);

        }

        [TestMethod]
        public void Incentive_Should_Apply50PercentOff_SKU1() {
            Order order = GetTestOrder();
            decimal before = order.SubTotal;

            //SKU1 is $10, so 50% is $5 off
            decimal after = before - 5;

            _incentiveService.ProcessCoupon("50PERCENTOFFSKU1", order);

            Assert.AreEqual(after, order.SubTotal);
            Assert.AreEqual(5, order.DiscountAmount);

        }

        [TestMethod]
        public void Incentive_Should_Apply_10DollarsOff_Order() {
            Order order = GetTestOrder();
            decimal before = order.SubTotal;

            //SKU1 is $10, so 50% is $5 off
            decimal after = before - 10;

            _incentiveService.ProcessCoupon("10DOLLARSOFF", order);

            Assert.AreEqual(after, order.SubTotal);
            Assert.AreEqual(10, order.DiscountAmount);

        }

        [TestMethod]
        public void Incentive_Should_Apply_8DollarsOff_SKU1() {
            Order order = GetTestOrder();
            decimal before = order.SubTotal;

            //SKU1 is $10, so 50% is $5 off
            decimal after = before - 8;

            _incentiveService.ProcessCoupon("8DOLLARSOFFSKU1", order);

            Assert.AreEqual(after, order.SubTotal);
            Assert.AreEqual(8, order.DiscountAmount);

        }

        #region SQL Integration
       
        [TestMethod]
        public void SQL_Incentives_Should_LoadFrom_DB() {
            SqlIncentiveRepository repo = new SqlIncentiveRepository();
            List<Incentive> incentives = repo.GetIncentives().ToList();
            Assert.AreEqual(4, incentives.Count);
        }

        [TestMethod]
        public void SQL_Incentives_Should_Return_Proper_Coupon_Types() {
            SqlIncentiveRepository repo = new SqlIncentiveRepository();
            List<Incentive> incentives = repo.GetIncentives().ToList();

            Assert.IsInstanceOfType(incentives[0].Coupon, typeof(PercentOffOrderCoupon));
            Assert.IsInstanceOfType(incentives[1].Coupon, typeof(PercentOffItemCoupon));
            Assert.IsInstanceOfType(incentives[2].Coupon, typeof(AmountOffOrderCoupon));
            Assert.IsInstanceOfType(incentives[3].Coupon, typeof(AmountOffItemCoupon));

        }

        [TestMethod]
        public void SQL_PercentOffItem_ShouldReturn_BackPack1_And_10_PercentOff() {
            SqlIncentiveRepository repo = new SqlIncentiveRepository();
            List<Incentive> incentives = repo.GetIncentives().ToList();
            PercentOffItemCoupon coupon = (PercentOffItemCoupon)incentives[1].Coupon;

            Assert.AreEqual("Backpack1", coupon.ProductCodes[0]);
            Assert.AreEqual(10, coupon.PercentOff);
 
        }

        [TestMethod]
        public void SQL_PercentOffItem_ShouldReturn_10_for_AmountOff_for_Incentive_2() {
            SqlIncentiveRepository repo = new SqlIncentiveRepository();
            List<Incentive> incentives = repo.GetIncentives().ToList();
            AmountOffOrderCoupon coupon = (AmountOffOrderCoupon)incentives[2].Coupon;

            Assert.AreEqual(10, coupon.AmountOff);

        }
        [TestMethod]
        public void SQL_PercentOffItem_ShouldReturn_BackPack1_And_5_For_Amount_Off() {
            SqlIncentiveRepository repo = new SqlIncentiveRepository();
            List<Incentive> incentives = repo.GetIncentives().ToList();
            AmountOffItemCoupon coupon = (AmountOffItemCoupon)incentives[3].Coupon;

            Assert.AreEqual("Backpack1", coupon.ProductCodes[0]);
            Assert.AreEqual(5, coupon.AmountOff);

        }
        #endregion

    }
}
