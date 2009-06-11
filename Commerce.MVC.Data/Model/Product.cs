using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    
    public enum InventoryStatus {
        InStock=1, 
        BackOrder=2,
        PreOrder=3,
        SpecialOrder=4,
        Discontinued=5,
        CurrentlyUnavailable=6
    }

    public enum DeliveryMethod
    {
        Shipped = 1,
        Download = 2
    }

    public class ProductRelatedMap {
        public int ProductID { get; set; }
        public int RelatedProductID { get; set; }
    }

    public class ProductCrossSellMap {
        public int ProductID { get; set; }
        public int CrossProductID { get; set; }
    }
    public class Product {
        

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
        public string ProductCode { get; set; }
        public string Manufacturer { get; set; }
        public DeliveryMethod Delivery { get; set; }
        public decimal WeightInPounds { get; set; }
        public bool IsTaxable { get; set; }
        public InventoryStatus Inventory { get; set; }
        public bool AllowBackOrder { get; set; }
        public string EstimatedDelivery { get; set; }
        public string DefaultImagePath { 
            get
            {
                return this.Images.Count > 0 ? this.Images[0].ThumbnailPhoto : "";

            }

        }

        public decimal DiscountedPrice
        {
            get { return Price * (1.0M - DiscountPercent); } 
        }
        public LazyList<ProductReview> Reviews { get; set; }
        public LazyList<ProductImage> Images { get; set; }
        public LazyList<Product> RelatedProducts { get; set; }
        public LazyList<Product> CrossSells { get; set; }
        public LazyList<ProductDescriptor> Descriptors { get; set; }
        public LazyList<Product> Recommended { get; set; }
        
        

        public Product() {}        
        
        public Product(string name, string description,
            decimal price, decimal discountPercent, decimal weightInPounds) {

            this.WeightInPounds = weightInPounds;
            this.Name = name;
            this.Description = description;
            this.DiscountPercent = discountPercent;
            this.Price = price;
            this.Delivery = DeliveryMethod.Shipped;
            this.Inventory = InventoryStatus.InStock;
        }

        #region Object overrides
        public override bool Equals(object obj) {
            if (obj is Product) {
                Product compareTo = (Product)obj;
                return compareTo.ID == this.ID;
            } else {
                return base.Equals(obj);
            }
        }

        public override string ToString() {
            return this.Name;
        }
        public override int GetHashCode() {
            return this.ID.GetHashCode();
        }
        #endregion
    }
}
