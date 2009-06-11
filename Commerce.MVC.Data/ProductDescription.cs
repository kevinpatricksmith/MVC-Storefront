using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.MVC.Data {
    public class ProductDescription {

        public ProductDescription() { }
        public ProductDescription(int productID, string locale, string description) {
            this.ProductID = productID;
            this.Locale = locale;
            this.Body = description;
        }

        public int ID { get; set; }
        public int ProductID { get; set; }
        public string Locale { get; set; }
        public string Body { get; set; }

    }
}
