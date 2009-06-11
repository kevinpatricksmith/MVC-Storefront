using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public class OrderItem {

        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Guid OrderID { get; set; }

        public decimal LineTotal
        {
            get
            {
                return this.Quantity*(this.LineItemPrice);

            }
        }
        public decimal ItemsWeight
        {
            get
            {
                return this.Quantity * (this.Product.WeightInPounds);

            }
        }

        public DateTime DateAdded{ get; set; }

        public OrderItem():this(Guid.Empty,null,0)
        {
        }

        public OrderItem(Guid orderID, Product product)
            : this(orderID, product, 1) {
        }
        public OrderItem(Product product, int quantity)
            : this(Guid.Empty, product, quantity) {
        }
        public OrderItem(Guid orderID, Product product, int quantity)
            {
            this.OrderID = orderID;
            this.Product = product;
            this.Quantity = quantity;
            if (this.Product != null)
            {
                this.LineItemPrice = product.DiscountedPrice;
            }
        }

        public decimal LineItemPrice { get; set; }


        #region Object overrides
        public override bool Equals(object obj) {
            if (obj is OrderItem) {
                OrderItem compareTo = (OrderItem)obj;
                return compareTo.Product.ID == this.Product.ID
                    && compareTo.OrderID == this.OrderID;
            } else {
                return base.Equals(obj);
            }
        }

        public override string ToString() {
            return this.Product.Name;
        }
        public override int GetHashCode() {
            return this.Product.ID.GetHashCode();
        }
        #endregion
    }
}
