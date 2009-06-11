using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Tests {
    public class TestInventoryRepository:IInventoryRepository {

        List<InventoryRecord> inventoryRecords;
        public TestInventoryRepository() {
            inventoryRecords = new List<InventoryRecord>();
            
            //one for each product
            TestCatalogRepository catalog = new TestCatalogRepository();
            List<Product> prods = catalog.GetProducts().ToList();

            foreach (Product p in prods) {
                InventoryRecord rec = new InventoryRecord(p.ID,1,"test entry");
                inventoryRecords.Add(rec);
            }

        
        }

        public IQueryable<InventoryRecord> GetInventory() {


            return inventoryRecords.AsQueryable();
        }

        public void Increment(int productID, int amount, string notes) {
            InventoryRecord record = new InventoryRecord(productID, amount, notes);
            inventoryRecords.Add(record);

        }

    }
}
