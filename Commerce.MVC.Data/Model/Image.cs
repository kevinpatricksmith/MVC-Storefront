using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    
    /// <summary>
    /// A class which represents a URL-based image
    /// </summary>
    public class Image {

        public string ThumbnailPhoto { get; set; }
        public string FullSizePhoto { get; set; }
        public int ID { get; set; }

        public Image() { }

        public Image(string thumb, string full) {
            this.ThumbnailPhoto = thumb;
            this.FullSizePhoto = full;
        }

    }

    /// <summary>
    /// An image for a Product
    /// </summary>
    public class ProductImage :Image{
        public ProductImage() : base() { }
        public int ProductID { get; set; }
        public ProductImage(string thumb, string full)
            : base(thumb, full) {
        }
    }

    /// <summary>
    /// An image for a Category
    /// </summary>
    public class CategoryImage : Image {
        public int CategoryID { get; set; }
        public CategoryImage() : base() { }
        public CategoryImage(string thumb, string full)
            : base(thumb, full) {
        }
   }

}
