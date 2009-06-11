using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data.SqlRepository;
using System.Data.Linq;


namespace Commerce.Data {

    public class SqlOrderRepository:IOrderRepository {

        public DB _db;
        IShippingRepository _shippingRepository;
        public SqlOrderRepository(DB db, IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
            _db = db;
        }

        IQueryable<OrderItem> GetOrderItems(Guid orderID)
        {

            return from oi in GetOrderItems()
                   where oi.OrderID == orderID
                   select oi;
        }


        ShippingMethod GetOrderShippingMethod(Commerce.Data.SqlRepository.ShippingMethod method, SqlRepository.Order order) {

            ShippingMethod result = null;
            Decimal orderWeight = order.OrderItems.Sum(x => x.Product.WeightInPounds);
            if (method != null) {
                result = new ShippingMethod();
                result.ID = method.ShippingMethodID;
                result.Carrier = method.Carrier;
                result.EstimatedDelivery = method.EstimatedDelivery;
                result.RatePerPound = method.RatePerPound;
                result.ServiceName = method.ServiceName;
                result.Cost = method.BaseRate + (method.RatePerPound * orderWeight);
            }

            return result;
        }

        

        public IQueryable<Order> GetOrders() {

            var orders = from o in _db.Orders
                         let items = GetOrderItems(o.OrderID)
                         let transactions = GetTransactions(o.OrderID)
                         select new Order
                                    {
                                        Status = (OrderStatus)o.OrderStatusID,
                                        DateCreated = o.CreatedOn,
                                        ID = o.OrderID,
                                        OrderNumber = o.OrderNumber,
                                        Items = new LazyList<OrderItem>(items),
                                        Transactions = new LazyList<Transaction>(transactions),
                                        ShippingAddress = GetAddresses().Where(x => x.ID == o.ShippingAddressID).SingleOrDefault(),
                                        BillingAddress = GetAddresses().Where(x => x.ID == o.BillingAddressID).SingleOrDefault(),
                                        ShippingMethod =GetOrderShippingMethod(o.ShippingMethod, o),
                                        UserName = o.UserName,
                                        UserLanguageCode=o.UserLanguageCode,
                                        DateShipped=o.DateShipped,
                                        EstimatedDelivery = o.EstimatedDelivery,
                                        TrackingNumber=o.TrackingNumber,
                                        TaxAmount=o.TaxAmount,
                                        DiscountReason=o.DiscountReason,
                                        DiscountAmount=o.DiscountAmount
                                        
                                    };
            return orders;

        }

        public IQueryable<OrderItem> GetOrderItems() {
           SqlCatalogRepository catalog = new SqlCatalogRepository(_db);

            return from oi in _db.OrderItems
                   select new OrderItem
                              {
                                  OrderID = oi.OrderID,
                                  Quantity = oi.Quantity,
                                  DateAdded=oi.DateAdded,
                                  LineItemPrice=oi.LineItemPrice, 
                                  Product = (from p in catalog.GetProducts()
                                             where p.ID == oi.ProductID
                                             select p
                                            ).SingleOrDefault(),
                                            

                              };
        }

        public IQueryable<Address> GetAddresses() {


            return from add in _db.Addresses
                   select new Address
                              {
                                  ID = add.AddressID,
                                  Street1 = add.Street1,
                                  Street2 = add.Street2,
                                  City = add.City,
                                  Country = add.Country,
                                  StateOrProvince = add.StateOrProvince,
                                  Email = add.Email,
                                  FirstName = add.FirstName,
                                  LastName = add.LastName,
                                  UserName = add.UserName,
                                  Longitude = add.Longitude,
                                  Latitude = add.Latitude,
                                  Zip=add.Zip,
                                  IsDefault=add.IsDefault
                              };
        }

        public void SaveAddress(Address address)
        {

            if (address != null) {
                using (DB db = new DB()) {
                    //see if it's in the db
                    Commerce.Data.SqlRepository.Address add;

                    if (address.ID > 0) {
                        add = (from a in _db.Addresses
                               where a.AddressID == address.ID
                               select a).SingleOrDefault() ?? new Commerce.Data.SqlRepository.Address();

                    }
                    else {
                        add = (from a in _db.Addresses
                               where a.UserName == address.UserName && a.Street1 == address.Street1 && a.City == address.City
                               select a).SingleOrDefault() ?? new Commerce.Data.SqlRepository.Address();
                    }

                    //synch it
                    add.City = address.City;
                    add.Street2 = address.Street2;
                    add.Street1 = address.Street1;
                    add.StateOrProvince = address.StateOrProvince;
                    add.LastName = address.LastName;
                    add.FirstName = address.FirstName;
                    add.Email = address.Email;
                    add.Country = address.Country;
                    add.UserName = address.UserName;
                    add.Zip = address.Zip;
                    add.Longitude = address.Longitude;
                    add.Latitude = address.Latitude;

                    //save it
                    if (add.AddressID == 0)
                        db.Addresses.InsertOnSubmit(add);

                    db.SubmitChanges();

                    address.ID = add.AddressID;
                }
            }
        }

        public void SaveItems(Order order) {
            //TODO: I know this need to be rewritting
            //the problem I have here is that I need it ALL to be in the scope of a single DB transaction

            //Ayende <3 this method :p

            using (DB db = new DB()) {

                //see if there is an order in the DB already
                Commerce.Data.SqlRepository.Order
                    existingorder = (from o in db.Orders
                                    where o.OrderID==order.ID
                                    select o).SingleOrDefault();

                //if not, create it
                if (existingorder == null) {

                    existingorder = new Commerce.Data.SqlRepository.Order();
                    existingorder.UserName = order.UserName;
                    existingorder.CreatedOn = DateTime.Now;
                    existingorder.ModifiedOn = DateTime.Now;
                    existingorder.OrderID = order.ID;
                    existingorder.OrderStatusID = (int) order.Status;
                    existingorder.UserLanguageCode = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                    db.Orders.InsertOnSubmit(existingorder);

                } else {

                    //there's a order - pull all the ProductIDs from our Model
                    var productsInBasket = from p in order.Items
                                  
                                           select p.Product.ID;

                    //first, drop the items in the DB that aren't in the order
                    var deletedProducts = from si in db.OrderItems
                               where !productsInBasket.Contains(si.ProductID) 
                               && si.OrderID == order.ID
                               select si;

                    db.OrderItems.DeleteAllOnSubmit(deletedProducts);
                           

                    //update the ones that have changed - this applies to Quantity
                    foreach (Commerce.Data.SqlRepository.OrderItem dbItem in existingorder.OrderItems) {

                        OrderItem orderItem = order.Items.Where(
                                x => x.OrderID == dbItem.OrderID 
                                && x.Product.ID == dbItem.ProductID
                                ).SingleOrDefault();

                        //if the quantity has changed, update it
                        if (orderItem != null && dbItem.Quantity != orderItem.Quantity) {
                            dbItem.Quantity = orderItem.Quantity;
                        }

                    }

                }


                //finally, add the items that are new (ID==0)
                //setup the items to load up
                foreach (OrderItem newItem in order.Items) {

                    //see if the product is in the existing order
                    Commerce.Data.SqlRepository.OrderItem existingItem = (from items in existingorder.OrderItems
                                                                                     where items.ProductID == newItem.Product.ID
                                                                                     select items).SingleOrDefault();

                    if (existingItem == null) {
                        existingItem = new Commerce.Data.SqlRepository.OrderItem();
                        existingItem.DateAdded = DateTime.Now;
                        existingItem.OrderID = existingorder.OrderID;
                        existingItem.ProductID = newItem.Product.ID;
                        existingItem.LineItemPrice = newItem.LineItemPrice; 
                       
                    }

                    existingItem.Quantity = newItem.Quantity;
                    existingorder.OrderItems.Add(existingItem);
                }



                if (order.ShippingAddress != null)
                    existingorder.ShippingAddressID = order.ShippingAddress.ID;


                //save it in a batch - this is a transaction
                db.SubmitChanges();

            }


        }

        public bool DeleteOrder(Guid orderID) {
            bool result = false;
            

           //delete the items first
            using(DB db=new DB())
            {

                //items
                var delItems = from oi in db.OrderItems
                               where oi.OrderID == orderID
                               select oi;
                
                //order
                var delOrder = from o in db.Orders
                               where o.OrderID == orderID
                               select o;


                db.OrderItems.DeleteAllOnSubmit(delItems);
            
                //delete the order
                db.Orders.DeleteAllOnSubmit(delOrder);

                db.SubmitChanges();
                result = true;
            }

            return result;
        }

        public void DeleteAddress(int addressID)
        {
            using(DB db=new DB())
            {
                var delAdd = from a in db.Addresses
                             where a.AddressID == addressID
                             select a;
                db.Addresses.DeleteAllOnSubmit(delAdd);
                db.SubmitChanges();
            }

        }



        public IQueryable<ShippingMethod> GetShippingMethods()
        {
            return from sm in _db.ShippingMethods
                   select new ShippingMethod
                   {
                       ID = sm.ShippingMethodID,
                       Carrier = sm.Carrier,
                       EstimatedDelivery = sm.EstimatedDelivery,
                       RatePerPound = sm.RatePerPound,
                       ServiceName = sm.ServiceName
                   };

        }

        IQueryable<Transaction> GetTransactions(Guid transactionID)
        {
            return GetTransactions().Where(x => x.ID == transactionID);
        }
        public IQueryable<Transaction> GetTransactions()
        {
            return from t in _db.Transactions
                   select new Transaction
                   {
                       ID=t.TransactionID,
                       Amount=t.Amount,
                       AuthorizationCode=t.AuthorizationCode,
                       DateExecuted=t.TransactionDate,
                       Notes=t.Notes,
                       OrderID=t.OrderID
                   };
        }


        public void SaveOrder(Order order)
        {

            //save down the addresses
            if(order.ShippingAddress!=null)
                SaveAddress(order.ShippingAddress);
            
            if(order.BillingAddress!=null)
                SaveAddress(order.BillingAddress);


            using (DB db = new DB())
            {
                
                //pull the order
                Commerce.Data.SqlRepository.Order existingOrder = (from o in db.Orders
                                                               where o.OrderID == order.ID
                                                               select o).SingleOrDefault();
                if (existingOrder == null)
                   throw new InvalidOperationException("There is no order with the ID "+order.ID.ToString());

                //marry up the orders
                existingOrder.TaxAmount=order.TaxAmount;

                if (order.ShippingMethod != null) {
                    existingOrder.ShippingAmount = order.ShippingMethod.Cost;
                    existingOrder.ShippingMethodID = order.ShippingMethod.ID;
                    
                    //shipping method bits
                    existingOrder.EstimatedDelivery = DateTime.Now.AddDays(order.ShippingMethod.DaysToDeliver);
                }
                existingOrder.SubTotal=order.SubTotal;
                existingOrder.OrderStatusID=(int)order.Status;

                if(order.ShippingAddress!=null)
                    existingOrder.ShippingAddressID = order.ShippingAddress.ID;
                
                if (order.BillingAddress != null)
                    existingOrder.BillingAddressID = order.BillingAddress.ID;
                
                existingOrder.ExecutedOn = DateTime.Now;
                existingOrder.OrderNumber = order.OrderNumber;
                existingOrder.UserName = order.UserName;
                existingOrder.DiscountAmount = order.DiscountAmount;
                existingOrder.DiscountReason = order.DiscountReason;

                //save this down so we know how to correspond in the future
                existingOrder.UserLanguageCode = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

                foreach (Transaction t in order.Transactions)
                {
                    Commerce.Data.SqlRepository.Transactions newTransaction = new Commerce.Data.SqlRepository.Transactions();

                    //a little left/right action...
                    newTransaction.TransactionID = t.ID;
                    newTransaction.OrderID = t.OrderID;
                    newTransaction.Notes = t.Notes;
                    newTransaction.AuthorizationCode = t.AuthorizationCode;
                    newTransaction.Amount = t.Amount;
                    newTransaction.ProcessorID = (int)t.Processor;
                    newTransaction.TransactionDate = t.DateExecuted;

                    db.Transactions.InsertOnSubmit(newTransaction);


                }

                //cross your fingers!
                db.SubmitChanges();
            }


        }

        public void DeleteTransaction(Guid transactionID)
        {
            using (DB db = new DB())
            {
                db.Transactions.DeleteAllOnSubmit(from t in db.Transactions where t.TransactionID==transactionID select t);
                db.SubmitChanges();
            }

        }

    }
}
