using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    [Serializable]
    public class OrderService : Commerce.Services.IOrderService {

        IOrderRepository _orderRepository;
        ICatalogRepository _catalogRepository;
        IShippingRepository _shippingRepository;
        IShippingService _shippingService;

        public OrderService() { }
        public OrderService(IOrderRepository rep, ICatalogRepository catalog,
            IShippingRepository shippingRepository, IShippingService shippingService) 
        {
            _orderRepository = rep;
            _catalogRepository = catalog;
            _shippingRepository = shippingRepository;
            _shippingService = shippingService;
        }

        /// <summary>
        /// Gets all orders in the system
        /// </summary>
        /// <returns></returns>
        public IList<Order> GetOrders() {
            return _orderRepository.GetOrders().ToList();
        }

        /// <summary>
        /// Gets orders by page
        /// </summary>
        public PagedList<Order> GetPagedOrders(int pageSize, int currentPageIndex) {
            
            return new PagedList<Order>(_orderRepository.GetOrders(), 
                currentPageIndex, pageSize);

        }

        /// <summary>
        /// Gets orders by page, filtered by status
        /// </summary>
        public PagedList<Order> GetPagedOrders(int pageSize, int currentPageIndex, 
            OrderStatus status) {

            return new PagedList<Order>(_orderRepository.GetOrders()
                .Where(x=>x.Status==status),
                currentPageIndex, pageSize);

        }


        /// <summary>
        /// Gets a single order by ID
        /// </summary>
        public Order GetOrder(Guid orderID) {
            
            Order result= _orderRepository.GetOrders().WithOrderID(orderID).SingleOrDefault();
            if (result != null) {
                if (result.ShippingMethod != null) {
                    _shippingService.CalculateRates(result);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns all items for a given order
        /// </summary>
        public IList<OrderItem> GetItems(Guid orderID) {
            return _orderRepository.GetOrderItems().ForOrderID(orderID).ToList();
        }

        /// <summary>
        /// Returns all addresses
        /// </summary>
        /// <returns></returns>
        public IList<Address> GetAddresses(string userName)
        {
            return _orderRepository.GetAddresses().ForUser(userName).ToList();
        }

        /// <summary>
        /// Returns an address by ID
        /// </summary>
        public Address GetAddress(int addressID)
        {
            return _orderRepository.GetAddresses().ByID(addressID).SingleOrDefault();

        }

        /// <summary>
        /// Saves an address
        /// </summary>
        public void  SaveAddress(Address add)
        {
            SaveAddress(add);
        }


        /// <summary>
        /// Saves the order to the Repository
        /// </summary>
        public void SaveItems(Order order)
        {
            
            //Validations
            if (order != null) {
                _orderRepository.SaveItems(order);
            
            }
        }
        /// <summary>
        /// Returns a current, unchecked-out order for the current user
        /// </summary>
        public Order GetCurrentOrder(string userName)
        {
            Order result= _orderRepository.GetOrders().CurrentOrderForUser(userName).SingleOrDefault();
        
            if(result==null)
            {
                //create a new one
                result=new Order(userName);
                
            }
            return result;
            
        }

        public  void DeleteAddress(int addressID)
        {
            _orderRepository.DeleteAddress(addressID);
        }
        
        public void DeleteOrder(Guid orderID)
        {
            //TODO: Implement rules here - can't delete orders
            //with transactions
            _orderRepository.DeleteOrder(orderID);

        }


        /// <summary>
        /// Transacts payment on an order
        /// </summary>
        /// <param name="order">The order to transact payment</param>
        /// <returns>Commerce.Data.Transaction</returns>
        public void SubmitOrder(Order order)
        {

            //validate the order
            order.ValidateForCheckout();

            //update the status to Submitted
            order.Status = OrderStatus.Submitted;

            //save th order
            SaveOrder(order);


        }

        /// <summary>
        /// Saves the order without items, to the DB
        /// </summary>
        public void SaveOrder(Order order)
        {
            if (order != null) {

                //create an order number
                //obviously, this is arbitrary
                //create what you will, here
                order.OrderNumber = "MVC-" + order.ID.ToString().Substring(0, 6);

                //if the shipping method is null
                //default it to the first item
                order.ShippingMethod = order.ShippingMethod ?? _shippingRepository.GetShippingMethods().Take(1).SingleOrDefault();

                //save it
                _orderRepository.SaveOrder(order);
            }
          
        }

        /// <summary>
        /// Validates whether the order can be cancelled
        /// </summary>
        /// <param name="order"></param>
        public void ValidateOrderForCancel(Order order)
        {
            bool result = CanCancelOrder(order);
            if (!result)
                throw new InvalidOperationException("Cannot cancel this order at this time");


        }

        public bool CanCancelOrder(Order order)
        {
            if (order != null) {
                //can't be cancelled if it's already shipped
                //which is any status over 5
                //need to Return it instead
                if ((int)order.Status > 5) {
                    return false;
                }
                //have to make sure that it's at least checked out too
                else if ((int)order.Status < 2) {
                    return false;
                }
                else {
                    return true; ;

                }
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Check to see if a refund is possible
        /// </summary>
        public bool CanRefundOrder(Order order)
        {
            //refunds are only possible if an order has been paid already
            //or has been returned
            bool result = false;
            if (order.Status == OrderStatus.Charged || order.Status == OrderStatus.Returned)
                result = true;

            return result;
        }

        /// <summary>
        /// Returns a list of recommended products based on the passed-in product id
        /// </summary>
        public LazyList<Product> GetRecommended(int productID)
        {

            //first, pull all of the orders with the passed-in product ID
            var ordersWithProduct = from o in _orderRepository.GetOrderItems()
                         where o.Product.ID == productID
                         select o;



            //next, pull all of the other products from these orders
            //not including the productID passed in
            var productsInOrders = from items in _orderRepository.GetOrderItems()
                                   join o in ordersWithProduct on items.OrderID equals o.OrderID
                                   where items.Product.ID != productID
                                   select items;

            ////this should return a massive product list - now we need to 
            ////rollup that list, grouped by the SUM of the quantity
            var productSales = from p in productsInOrders
                               group p by p.Product.ID into g
                               orderby g.Sum(x => x.Quantity) descending
                               select new {
                                   ID = g.Key,
                                   OrderSum = g.Sum(x => x.Quantity)
                               };
            
            
            //finally, use the above query and join it to Products
            //to get a list of the products associated
            var topProducts = from p in _catalogRepository.GetProducts()
                              join ps in productSales on p.ID equals ps.ID
                              orderby ps.OrderSum descending
                              select p;

            return new LazyList<Product>(topProducts.Take(5));
            return null;

        }

        /// <summary>
        /// Migrates the user's cart on login
        /// </summary>
        public void MigrateCurrentOrder(string fromUserName, string toUserName) {

            //don't synch if they're the same person :)
            if (!fromUserName.Equals(toUserName, StringComparison.InvariantCultureIgnoreCase)) {
                
                //pull the cart for the current user, if it exists
                Order fromOrder = _orderRepository.GetOrders().CurrentOrderForUser(fromUserName).SingleOrDefault();

                //pull the existing cart, if it exists
                Order toOrder = _orderRepository.GetOrders().CurrentOrderForUser(toUserName).SingleOrDefault();

                //3 scenarios here
                //1: the user has an existing cart in the db, and a cart they've queued up this session
                //2: the user has no existing cart, and a cart they've queued up this session
                //3) the user has an existing cart, and no cart queued up this session

                if (toOrder==null) {
                    //just change the username on the fromOrder so it moves with them
                    fromOrder.UserName = toUserName;
                    SaveOrder(toOrder);

                    //remove the fromOrder
                    _orderRepository.DeleteOrder(fromOrder.ID);
                }
                else if (fromOrder != null && toOrder != null) {
                    //most common scenario - an existing cart that needs to synch
                    //with this session's cart

                    //use the "toOrder" as the valid current order
                    //synch all items in the fromOrder
                    foreach (OrderItem item in fromOrder.Items)
                        //use "AddItem" - this will append the product/quantity
                        toOrder.AddItem(item.Product, item.Quantity);


                    //save the items
                    SaveItems(toOrder);

                    //delete the fromOrder - don't need two of em
                    _orderRepository.DeleteOrder(fromOrder.ID);

                }
            }

        }


    }
}
