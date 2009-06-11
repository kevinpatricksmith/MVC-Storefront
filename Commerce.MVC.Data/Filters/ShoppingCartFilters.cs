using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.MVC.Data {
    public static class ShoppingCartFilters {

        /// <summary>
        /// Filter for ShoppingCart that filters by userName
        /// </summary>
        public static IQueryable<ShoppingCart> ForUser(this IQueryable<ShoppingCart> qry, string userName) {
            return from c in qry
                   where c.UserName.ToLower() == userName.ToLower()
                   select c;
        }

        /// <summary>
        /// Filters the ShoppingCartItem list by Product
        /// </summary>
        public static IQueryable<ShoppingCartItem> WithProduct(this IQueryable<ShoppingCartItem> qry, Product p) {
            return from si in qry
                   where si.Product.ID == p.ID
                   select si;
        }

        /// <summary>
        /// Filters the ShoppingCartItem list by Product
        /// </summary>
        public static IQueryable<ShoppingCartItem> ForCart(this IQueryable<ShoppingCartItem> qry, ShoppingCart cart) {
            return from si in qry
                   where si.ShoppingCartID==cart.ID
                   select si;
        }
    }
}
