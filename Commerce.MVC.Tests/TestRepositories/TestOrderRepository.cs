using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Tests {

    public class TestOrderRepository:IOrderRepository {
        
        private Order _order;
        private IList<Address> _addresses;
        IList<Order> _orders;
        IList<OrderItem> _orderItems;

        public TestOrderRepository() {
            _order = _order ?? new Order("TESTORDER", "testuser");
            _orders = new List<Order>();
            _orders.Add(_order);

            //create the items
            _orderItems = new List<OrderItem>();
            _addresses = new List<Address>();
            
            for (int i = 0; i < 2; i++)
            {
                var add = new Address("testuser", "first" + i, "last" + i, "email" + i,
                "street1" + i, "street2" + i, "city" + i,
                "stateprovince" + i, "zip" + i, "country" + i);
                add.IsDefault = i == 1;
                add.ID = i;
                if (!_addresses.Contains(add))
                    _addresses.Add(add);
            }

            TestCatalogRepository catalog = new TestCatalogRepository();

            for (int i = 0; i < 99; i++) {
                Order o = new Order("order" + i, "user" + 1);
                o.ID = Guid.NewGuid();


                for (int x = 1; x <= 5; x++) {
                    OrderItem item = new OrderItem(o.ID, catalog.GetProducts()
                        .Where(y=>y.ID==x).SingleOrDefault());
                    
                    item.Quantity = x == 5 ? 3 : 1;
                    
                    if(item.Quantity==1)
                        item.Quantity = x == 4 ? 2 : 1;

                    _orderItems.Add(item);
                    o.Items.Add(item);
                }

                o.ShippingAddress = GetAddresses().Take(1).Skip(1).SingleOrDefault();
                _orders.Add(o);
            }

        }

        public  void SaveAddress(Address address)
        {
            if (!_addresses.Contains(address))
                _addresses.Add(address);
        }

        public IQueryable<Address>GetAddresses()
        {
            
            return _addresses.AsQueryable();

        }

        public IQueryable<Order> GetOrders() {

            return _orders.AsQueryable();
        }

        public IQueryable<OrderItem> GetOrderItems() {
            
            return _orderItems.AsQueryable();
        }


        public void SaveItems(Order order) {
            _order = order;
        }
        public bool DeleteOrder(Guid orderID) {
            return true;
        }


        public void DeleteAddress(int addressID) {
            //nothin
        }

        public IQueryable<Transaction> GetTransactions()
        {
            var result = new List<Transaction>();
            for (int i = 1; i < 3;i++ )
            {
                Transaction p = new Transaction(Guid.NewGuid(), 10, "1234", TransactionProcessor.FakePaymentProcessor);
                result.Add(p);
            }
            return result.AsQueryable();
        }




        public void SaveOrder(Order order)
        {
            _order = order;
        }

        public void DeleteTransaction(Guid transactionID)
        {
            //nothin
        }

    }
}
