using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {

   
    public class Category {

        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }

        public IList<Category> SubCategories { get; set; }
        public LazyList<Product> Products { get; set; }
        
        public CategoryImage Image { get; set; }
        public bool IsDefault { get; set; }

        #region object overrides
        public override bool Equals(object obj) {
            if (obj is Category) {
                Category compareTo = (Category)obj;
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
