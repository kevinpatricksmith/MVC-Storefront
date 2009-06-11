using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public class AmountOffItemCoupon:ICoupon {


        public string[] ProductCodes { get; set; }
        public decimal AmountOff { get; set; }

        public AmountOffItemCoupon( decimal amountOff, params string[] productCodes) {
            AmountOff = amountOff;
            ProductCodes = productCodes;
        }


        public void ApplyCoupon(Order order) {

            foreach (string productCode in ProductCodes) {
                OrderItem item = order.Items.Where(x => x.Product.ProductCode.Equals(productCode, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
                if (item != null) {
                    order.DiscountAmount += AmountOff * item.Quantity;
                }
            }

        }

       
    }
}
