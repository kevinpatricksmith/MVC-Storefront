using System;
using System.Collections.Generic;
using System.Linq;

namespace Commerce.Data {

    [Serializable]
    public enum OrderStatus
    {
        NotCheckoutOut=1,
        Submitted=2,
        Verified=3,
        Charged=4,
        Packaging=5,
        Shipped=6,
        Returned=7,
        Cancelled=8,
        Refunded=9
    }

    [Serializable]
    public class Order {

        public Guid ID { get; set; }
        public string OrderNumber { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public LazyList<OrderItem> Items { get; set; }
        public LazyList<Transaction> Transactions { get; set; }
        public string UserLanguageCode { get; set; }

        public OrderStatus Status { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public decimal TaxAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public DateTime? DateShipped { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public string TrackingNumber { get; set; }

        public LazyList<IIncentive> IncentivesUsed { get; set; }

        public Order():this("","") {
        }
        public Order(string userName)
            : this("",userName) {
        }
        public Order(string orderNumber, string userName) {
            this.OrderNumber = orderNumber;
            this.UserName = userName;
            this.Status = OrderStatus.NotCheckoutOut;
            this.Items=new LazyList<OrderItem>();
            this.IncentivesUsed = new LazyList<IIncentive>();
            this.ID = Guid.NewGuid();
            this.DiscountAmount = 0;
            this.DiscountReason = "--";
        }

        /// <summary>
        /// Adds a product to the cart
        /// </summary>
        public void AddItem(Product product) {
            AddItem(product, 1);
        }


        /// <summary>
        /// Removes all items from cart
        /// </summary>
        public void ClearItems() {
            this.Items.Clear();
        }

        /// <summary>
        /// Adds a product to the cart
        /// </summary>
        public void AddItem(Product product, int quantity) {

            //see if this item is in the cart already
            OrderItem item = FindItem(product);

            if (quantity != 0) {
                if (item != null) {
                    //if the passed in amount is 0, do nothing
                    //as we're assuming "add 0 of this item" means
                    //do nothing
                    if (quantity != 0)
                        AdjustQuantity(product, item.Quantity + quantity);
                } else {
                    if (quantity > 0) {
                        item = new OrderItem(this.ID,product, quantity);
                        
                        //add to list
                        this.Items.Add(item);
                    }
                }

            }

        }

        /// <summary>
        /// Adjusts the quantity of an item in the cart
        /// </summary>
        public void AdjustQuantity(Product product, int newQuantity) {
            OrderItem itemToAdjust = FindItem(product);
            if (itemToAdjust != null) {
                if (newQuantity <= 0) {
                    this.RemoveItem(product);
                } else {
                    itemToAdjust.Quantity = newQuantity;
                }

            }

        }

        /// <summary>
        /// Remmoves a product from the cart
        /// </summary>
        public void RemoveItem(Product product) {
            RemoveItem(product.ID);
        }

        /// <summary>
        /// Remmoves a product from the cart
        /// </summary>
        public void RemoveItem(int productID) {
            var itemToRemove = FindItem(productID);
            if (itemToRemove != null) {
                this.Items.Remove(itemToRemove);
            }
        }


        /// <summary>
        /// Finds an item in the cart
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public OrderItem FindItem(Product product) {
            OrderItem result = null;
            if (product != null) {
                //see if this item is in the cart already
                return FindItem(product.ID);
            }
            return result;

        }

        /// <summary>
        /// Finds an item in the cart
        /// </summary>
        /// <param name="productID">The product id to find</param>
        /// <returns></returns>
        public OrderItem FindItem(int productID) {

            this.Items = this.Items ?? new LazyList<OrderItem>();
            //see if this item is in the cart already
            return (from si in this.Items
                    where si.Product.ID == productID
                    select si).SingleOrDefault();

        }


        public void ValidateForCheckout() {

            //Must have 1 or more items
            if (this.Items.Count <= 0)
                throw new InvalidOperationException("Order must have one or more items");

            //every order must have a shipping address - used for tax calcs
            if (this.ShippingAddress == null)
                throw new InvalidOperationException("All orders must have a shipping address specified.");


            //make sure there's a payment method
            if (this.PaymentMethod == null)
                throw new InvalidOperationException("No payment method is set for this order");


            if (this.PaymentMethod is CreditCard) {
                CreditCard cc = this.PaymentMethod as CreditCard;

                if (!cc.IsValid())
                    throw new InvalidOperationException("This credit card is invalid. Please check the expiration and card number.");

            }


            if (this.HasShippableGoods) {

                //All orders with shippable goods must have shipping method 
                if (this.ShippingMethod == null)
                    throw new InvalidOperationException("Must have a shipping method selected when ordering shippable goods");

            }
        }


        public decimal TaxableGoodsSubtotal
        {
            get
            {
                //one of the many reasons to love LINQ :)
                return this.Items.Where(x => x.Product.IsTaxable)
                    .Sum(x => x.LineTotal);
            }
        }

        public decimal TotalWeightInPounds
        {
            get
            {
                return this.Items.Sum(x => x.ItemsWeight);
            }
        }

        public bool HasShippableGoods
        {
            get
            {

                return this.Items.Where(x => x.Product.Delivery == DeliveryMethod.Shipped).Count()>0;
            }
        }


        public string DiscountReason { get; set; }
        public decimal DiscountAmount { get; set; }
              
        
        public decimal SubTotal
        {
            get
            {

                return this.Items.Sum(x => x.LineTotal)-DiscountAmount;

            }
        }

        public decimal Total
        {
            get
            {
                decimal total= SubTotal + TaxAmount;
                if(this.ShippingMethod!=null)
                    total += this.ShippingMethod.Cost;

                return total;
            }
        }

        public decimal TotalPaid
        {
            get
            {
                decimal paid = 0;
                if (this.Transactions.Count > 0)
                    paid = this.Transactions.Sum(x => x.Amount);
                return paid;

            }
        }

        /// <summary>
        /// Returns the sum of the item quantity in the cart. 
        /// </summary>
        public int TotalItemQuantity
        {
            get
            {
                int numItems = 0;
                if (this.Items.Count > 0)
                {
                    numItems = (from i in this.Items
                                select i.Quantity).Sum();
                }
                return numItems; 
            }
        }

        /// <summary>
        /// Reprices all items in the cart to reflect update item prices. 
        /// </summary>
        /// <returns>
        /// True if the operation resulted in a change in the 
        /// total price of the order.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">
        /// Throws an InvalidOperationException if the cart 
        /// has been checked out. 
        /// </exception>
        public bool RepriceItems()
        {
            if (this.Status != OrderStatus.NotCheckoutOut)
            {
                throw new InvalidOperationException("Order may not be repriced once checked out."); 
            }

            decimal startingTotal = this.Total; 
            foreach (OrderItem item in this.Items)
            {
                item.LineItemPrice = item.Product.DiscountedPrice; 
            }
            return startingTotal != this.Total; 
        }


        #region Object overrides
        public override bool Equals(object obj) {
            if (obj is Order) {
                Order compareTo = (Order)obj;
                return compareTo.ID == this.ID;
            } else {
                return base.Equals(obj);
            }
        }

        public override string ToString() {
            return this.UserName + "'s Cart: " + this.Items.Count.ToString() + " items";
        }
        public override int GetHashCode() {
            return this.ID.GetHashCode();
        }
        #endregion
   }
}
