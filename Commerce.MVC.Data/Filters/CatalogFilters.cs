using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public static class CatalogFilters {



        /// <summary>
        /// Filters The query by ProductID
        /// </summary>
        public static IQueryable<Product> WithProductID(this IQueryable<Product> qry,
            int ID) {

            return from p in qry
                   where p.ID == ID
                   select p;
        }

        /// <summary>
        /// Filters The query by ProductCode
        /// </summary>
        public static IQueryable<Product> WithProductCode(this IQueryable<Product> qry,
            string productCode) {

            return from p in qry
                   where p.ProductCode.ToLower()==productCode.ToLower()
                   select p;
        }



        /// <summary>
        /// Filters The query by Product
        /// </summary>
        public static IQueryable<ProductImage> ImagesForProduct(this IQueryable<ProductImage> qry,
            Product p) {

            return from i in qry
                   where i.ProductID == p.ID
                   select i;
        }

        /// <summary>
        /// Filters The query by ProductCode
        /// </summary>
        public static IQueryable<ProductReview> ReviewsForProduct(this IQueryable<ProductReview> qry,
            Product p) {

            return from r in qry
                   where r.ProductID == p.ID
                   select r;
        }

        /// <summary>
        /// Filters The query by CategoryID
        /// </summary>
        public static IQueryable<Category> WithCategoryID(this IQueryable<Category> qry,
            int ID) {

            return from c in qry
                   where c.ID == ID
                   select c;
        }

        /// <summary>
        /// Filters an IList of Category and returns a Category by name
        /// </summary>
        public static Category WithCategoryName(this IList<Category> list, string categoryName) {
            return (from s in list
                    where s.Name.ToLower()==categoryName.ToLower()
                    select s).SingleOrDefault();
        }

        /// <summary>
        /// Filters an IList of Category and returns a Category by name
        /// </summary>
        public static Category DefaultCategory(this IEnumerable<Category> list) {
            return (from s in list
                    where s.IsDefault
                    select s).SingleOrDefault();
        }

    }
}
