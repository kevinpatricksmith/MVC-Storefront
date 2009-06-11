using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public class PercentOffItemCoupon:ICoupon {
        
        public string[] ProductCodes { get; set; }
        public decimal PercentOff { get; set; }

        public PercentOffItemCoupon(decimal percentOff, params string[] productCodes) {
            ProductCodes = productCodes;
            PercentOff = percentOff;
        }

        public void ApplyCoupon(Order order) {
            decimal discountRate = PercentOff / 100;
            foreach (string productCode in ProductCodes) {
                OrderItem item = order.Items.Where(x => x.Product.ProductCode.Equals(productCode, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
                if (item != null) {
                    order.DiscountAmount+=item.Product.Price * discountRate * item.Quantity;
                }
            }

        }

    }
}
