using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Data
{
    public enum UserBehavior
    {
        LoggingIn=1,
        LoggingOut = 2,
        AddItemToBasket = 3,
        RemoveItemFromBasket=4,
        CheckoutBilling=5,
        CheckoutShipping=6,
        CheckoutFinal=7,
        ViewOrder=8,
        ViewBasket=9,
        ViewCategory=10,
        ViewProduct=11
    }
    
    
    public class UserEvent
    {
        public string UserName { get; set; }
        public string IP { get; set; }
        public DateTime DateCreated { get; set; }
        public int? CategoryID { get; set; }
        public int? ProductID { get; set; }
        public Guid? OrderID { get; set; }
        public UserBehavior Behavior { get; set; }

        public UserEvent() {
            this.DateCreated = DateTime.Now;
        }

        public UserEvent(string userName, string IP, int? categoryID, int? productID, Guid orderID,
            UserBehavior behavior)
        {
            this.DateCreated = DateTime.Now;
            UserName = userName;
            this.IP = IP;
            this.CategoryID = categoryID;
            this.ProductID = productID;
            this.OrderID = orderID;
            this.Behavior = behavior;
        }


    }
}
