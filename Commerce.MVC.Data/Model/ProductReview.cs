using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public class ProductReview {

        public int ID { get; set; }
        public string Author { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ProductID { get; set; }
        public string Body { get; set; }

        public ProductReview() { }
        public ProductReview(string author, string email, int productID, string body) {
            this.Author = author;
            this.Email = email;
            this.ProductID = productID;
            this.Body = body;
            this.CreatedOn = DateTime.Now;
        }


    }
}
