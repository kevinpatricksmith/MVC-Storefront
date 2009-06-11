using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public class ProductDescriptor {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int ID { get; set; }


        public ProductDescriptor() { }
        public ProductDescriptor(int productID, string title, string body) {
            this.Title = title;
            this.ProductID = productID;
            this.Body = body;
        }

    }
}
