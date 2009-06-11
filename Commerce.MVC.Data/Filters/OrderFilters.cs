using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public static class OrderFilters {


        public static IQueryable<Order> WithOrderID(this IQueryable<Order> qry, Guid orderID) {
            return from o in qry
                   where o.ID == orderID
                   select o;
        }

        public static IQueryable<OrderItem> ForOrderID(this IQueryable<OrderItem> qry, Guid orderID) {
            return from o in qry
                   where o.OrderID == orderID
                   select o;
        }

        public  static IQueryable<Order> CurrentOrderForUser(this IQueryable<Order> qry, string userName)
        {
            return from o in qry
                   where o.UserName == userName && o.Status==OrderStatus.NotCheckoutOut
                   select o;
          
        }
    }
}
