using System;
using System.Collections.Generic;
using Commerce.Data;



namespace Commerce.Services {

    public interface IOrderService {
        IList<OrderItem> GetItems(Guid orderID);
        Order GetOrder(Guid orderID);
        IList<Order> GetOrders();
        IList<Address> GetAddresses(string userName);
        void SaveItems(Order order);
        Order GetCurrentOrder(string userName);
        void DeleteAddress(int addressID);
        void SaveAddress(Address add);
        Address GetAddress(int addressID);
        void SaveOrder(Order order);
        
        PagedList<Order> GetPagedOrders(int pageSize, int currentPageIndex);
        PagedList<Order> GetPagedOrders(int pageSize, int currentPageIndex, OrderStatus status);

        void ValidateOrderForCancel(Order order);
        bool CanRefundOrder(Order order);
        bool CanCancelOrder(Order order);

        LazyList<Product> GetRecommended(int productID);

        void MigrateCurrentOrder(string fromUserName, string toUserName);
    }
}
