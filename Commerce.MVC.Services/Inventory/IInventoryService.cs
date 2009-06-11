using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    public interface IInventoryService {
        void IncrementInventory(int productID, int amount, string note);
        IList<InventoryRecord> GetInventory(int productID);
        int GetAmountOnHand(int productID);
        void SetProductInventoryStatus(Product product);
        void SetProductInventoryStatus(Product product, InventoryStatus status);
    }
}
