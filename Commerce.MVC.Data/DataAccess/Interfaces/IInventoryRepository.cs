using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public interface IInventoryRepository {

        IQueryable<InventoryRecord> GetInventory();
        void Increment(int productID,  int amount, string notes);
    }
}
