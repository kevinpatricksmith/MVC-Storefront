using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Commerce.Data;

namespace Commerce.Tests {
    /// <summary>
    /// Summary description for InventoryTests
    /// </summary>
    [TestClass]
    public class InventoryTests:TestBase {
        [TestMethod]
        public void InventoryRecord_ShouldHave_ProductID_Increment_Notes() {
            InventoryRecord rec = new InventoryRecord(1, 1, "notes");
            Assert.AreEqual(1, rec.ProductID);
            Assert.AreEqual(1, rec.Increment);
            Assert.AreEqual("notes", rec.Notes);

        }

        [TestMethod]
        public void Inventory_Repository_Should_Return_InventoryRecords() {
            Assert.IsNotNull(_inventoryRepository);

        }

        [TestMethod]
        public void Inventory_Repository_Should_Have_OneRecord_PerProduct() {
            List<InventoryRecord> recs = _inventoryRepository.GetInventory().ToList();
            Assert.AreEqual(5, recs.Count);

        }
        [TestMethod]
        public void Inventory_Service_ShouldReturn_1_For_AmountOnHand() {
            int amount = _inventoryService.GetAmountOnHand(1);
            Assert.AreEqual(1, amount);
        }

        [TestMethod]
        public void Inventory_Filter_ShouldFilter_ByProductID1_AndReturn1() {
            List<InventoryRecord> result = _inventoryRepository.GetInventory().ForProduct(1).ToList();
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Inventory_Service_Should_Increment_Product1_By5() {
            _inventoryService.IncrementInventory(1,5,"note");
            int amount = _inventoryService.GetAmountOnHand(1);
            List<InventoryRecord> records = _inventoryService.GetInventory(1).ToList();
            Assert.AreEqual(6, amount);
        }

        [TestMethod]
        public void Inventory_Service_Should_Add_Inventory_Record_When_Incrementing() {

            IList<InventoryRecord> records = _inventoryService.GetInventory(1);
            Assert.AreEqual(1, records.Count);

            _inventoryService.IncrementInventory(1, 5, "note");
            records = _inventoryService.GetInventory(1);
            Assert.AreEqual(2, records.Count);
        }

        [TestMethod]
        public void Inventory_Service_Should_Set_Product4_To_CurrentlyUnavailable_When_Inventory_Is_0() {
            
            //there is one inventory item for every product in our test repo
            //products 4 and 5 do not allow back-order
            //so when they hit 0, the status should be updated

            _inventoryService.IncrementInventory(4, -1, "test");
            _inventoryService.SetProductInventoryStatus(_catalogService.GetProduct(4));
            Product p = _catalogService.GetProduct(4);
            Assert.AreEqual(InventoryStatus.CurrentlyUnavailable, p.Inventory);
        }


        [TestMethod]
        public void Inventory_Service_Should_Set_Product1_To_BackOrder_When_Inventory_Is_0() {

            //there is one inventory item for every product in our test repo
            //products 1-3 do allow back-order
            //so when they hit 0, the status should be updated to "Back Order"

            _inventoryService.IncrementInventory(1, -1, "test");
            _inventoryService.SetProductInventoryStatus(_catalogService.GetProduct(1));
            Product p = _catalogService.GetProduct(1);
            Assert.AreEqual(InventoryStatus.BackOrder, p.Inventory);
        }

        [TestMethod]
        public void Inventory_Service_Should_Set_Product4_To_InStock_When_Inventory_Is_Incremented_From_0_To4() {

            //there is one inventory item for every product in our test repo
            //products 4 and 5 do not allow back-order
            //so when they hit 0, the status should be updated

            _inventoryService.IncrementInventory(4, -1, "test");
            _inventoryService.SetProductInventoryStatus(_catalogService.GetProduct(4));
            Product p = _catalogService.GetProduct(4);
            Assert.AreEqual(InventoryStatus.CurrentlyUnavailable, p.Inventory);

            //add some items back
            _inventoryService.IncrementInventory(4, 4, "Adding data back in");
            _inventoryService.SetProductInventoryStatus(_catalogService.GetProduct(4));
            p = _catalogService.GetProduct(4);
            Assert.AreEqual(InventoryStatus.InStock, p.Inventory);

        }
        [TestMethod]
        public void Inventory_Service_Should_Set_Product1_To_InStock_When_Inventory_Is_IncrementedFrom_0_to4() {

            //there is one inventory item for every product in our test repo
            //products 1-3 do allow back-order
            //so when they hit 0, the status should be updated to "Back Order"

            _inventoryService.IncrementInventory(1, -1, "test");
            _inventoryService.SetProductInventoryStatus(_catalogService.GetProduct(1));
            Product p = _catalogService.GetProduct(1);
            Assert.AreEqual(InventoryStatus.BackOrder, p.Inventory);
            
            _inventoryService.IncrementInventory(1, 1, "test");
            _inventoryService.SetProductInventoryStatus(_catalogService.GetProduct(1));
            p = _catalogService.GetProduct(1);
            Assert.AreEqual(InventoryStatus.InStock, p.Inventory);
        }
    }
}
