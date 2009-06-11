using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public static class InventoryFilters {
        public static IQueryable<InventoryRecord> ForProduct(this IQueryable<InventoryRecord> qry, int productID) {
            return from i in qry
                   where i.ProductID == productID
                   select i;
        }
    }
}
