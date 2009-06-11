using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commerce.Data;
using Commerce.Services;
using System.Workflow.Runtime;
using System.Workflow.Runtime.Hosting;
using Commerce.Pipelines;

namespace Commerce.MVC.Web.Controllers
{
    public class OrderController : Controller
    {
        ICatalogService _catalogService;
        IOrderService _orderService;
        IAddressValidationService _addressValidator;
        IShippingService _shippingService;
        IPaymentService _paymentService;
        IPipelineEngine _pipeline;
        ISalesTaxService _taxService;
        IIncentiveService _incentiveService;

        public class OrderData {
            public Product ItemAdded { get; set; }
            public IList<Category> Categories { get; set; }
            public Order CurrentOrder { get; set; }
            public IList<Address> AddressBook { get; set; }
            public IList<ShippingMethod> ShippingMethods { get; set; }
        }

        /// <summary>
        /// Overloaded constructor for testing
        /// </summary>
        public OrderController(IOrderService cartService,
            ICatalogService catalogService) {

            _orderService = cartService;
            _catalogService = catalogService;
        }
        /// <summary>
        /// Overloaded constructor for testing
        /// </summary>
        public OrderController (IOrderService cartService,
            ICatalogService catalogService,IAddressValidationService addressValidator)
        {
            
            _orderService = cartService;
            _catalogService = catalogService;
            _addressValidator = addressValidator;
        }

        /// <summary>
        /// Overloaded constructor for testing
        /// </summary>
        public OrderController(IOrderService cartService,
            ICatalogService catalogService, 
            IAddressValidationService addressValidator, 
            IShippingService shippingService,
            IPaymentService paymentService,
            IPipelineEngine pipeline,
            ISalesTaxService taxService,
            IIncentiveService incentiveService
            ) {

            _orderService = cartService;
            _catalogService = catalogService;
            _addressValidator = addressValidator;
            _shippingService = shippingService;
            _paymentService = paymentService;
            _pipeline = pipeline;
            _taxService = taxService;
            _incentiveService = incentiveService;
        }

        #region Partials
        public ActionResult ShippingMethods() {
            Order order=GetTempOrder();
            IList<ShippingMethod> methods=new List<ShippingMethod>();
            if(order!=null){
                methods = _shippingService.CalculateRates(order);
            }
            return View(methods);
        }

        #endregion


        #region cart methods
        /// <summary>
        /// Adds an item to a user's cart
        /// </summary>
        public ActionResult AddItem(int id)
        {
            string userName = this.GetUserName();
            Product productToAdd = _catalogService.GetProduct(id);
            Order order = _orderService.GetCurrentOrder(userName);

            order.AddItem(productToAdd);
            _orderService.SaveItems(order);

            //bug workaround - removing with Prev 4
            return RedirectToAction("ItemAdded", new { id = id });

        }




        public ActionResult RemoveItem()
        {
            string sID=Request.Form["productid"];
            int productID = 0;
            int.TryParse(sID, out productID);

            if (productID != 0) {
                Order cart = _orderService.GetCurrentOrder(this.GetUserName());
                cart.RemoveItem(productID);
                _orderService.SaveItems(cart);
            }
            return RedirectToAction("Show");
        }


        public ActionResult UpdateItem() {
            string sID = Request.Form["productid"];
            string sQty = Request.Form["quantity"];
            int newQuantity = 0;
            int productID = 0;
            int.TryParse(sID, out productID);
            int.TryParse(sQty, out newQuantity);

            if (productID != 0) {
                Order cart = _orderService.GetCurrentOrder(this.GetUserName());
                cart.AdjustQuantity(cart.FindItem(productID).Product, newQuantity);
                _orderService.SaveItems(cart);
            }
            return RedirectToAction("Show");
        }



        /// <summary>
        /// Shows a page with confirmation/cross-sells/upsells
        /// </summary>
        public ActionResult ItemAdded(int id) {

            string userName = this.GetUserName();

            Product productAdded = _catalogService.GetProduct(id);
            OrderData data = new OrderData();

            //get the user
            data.ItemAdded = productAdded;
            ViewData["ShowAddItemButton"] = false;


            return View("ItemAdded", data);
        }

        public ActionResult Show() {
            string userName = this.GetUserName();
            //pull the cart
            Order cart = _orderService.GetCurrentOrder(userName);
            return View("Show", cart);
        }
        public ActionResult Index() {
            return Show();
        }

        #endregion


        #region Checkout Actions

        public ActionResult PayPalCheckout() {
            Order order = GetTempOrder();
            if (order != null) {
                //save it and redirect
                _orderService.SaveOrder(order);
                return RedirectToAction("Checkout", "PayPal");
            }
            else {
                return RedirectToAction("Show");
            }
        }

        IList<Address> GetUserAddresses()
        {
            string userName = this.GetUserName();
            return _orderService.GetAddresses(userName);
        }

        public ActionResult Checkout()
        {

            Order order = _orderService.GetCurrentOrder(this.GetUserName());
            if(order.Items.Count==0)
                return RedirectToAction("Show");
            else if (User.Identity.IsAuthenticated)
                return RedirectToAction("Shipping");
            else
                return View();
        }

        public ActionResult Shipping() {

            OrderData data = new OrderData();
            data.CurrentOrder = GetTempOrder();
            
            //default the shipping method
            data.ShippingMethods = _shippingService.CalculateRates(data.CurrentOrder);
            data.CurrentOrder.ShippingMethod = data.ShippingMethods[0];

            //this is for test only
            Address defaultAdd = new Address("testuser", "Jack", "Johnson", "jack@johnson.com", "1525 Bernice Street", "", "Honolulu", "HI", "96817", "US");

            string sAddressID = Request.Form["addressid"];
            int addressID = 0;
            if (!String.IsNullOrEmpty(sAddressID)) {
                int.TryParse(sAddressID, out addressID);
                defaultAdd = _orderService.GetAddress(addressID);
                
                //save it
                _orderService.SaveOrder(data.CurrentOrder);

            }


            //new up the shipping address
            data.CurrentOrder.ShippingAddress = defaultAdd;

            //send them somewhere else if there are no items
            if (data.CurrentOrder.Items.Count == 0) {
                return RedirectToAction("Show");
            } else {
                return View(data);
            }

        }

        public ActionResult ApplyCoupon() {
            string couponCode = Request.Form["couponCode"];
            if (!string.IsNullOrEmpty(couponCode)) {
                
               Order order = _orderService.GetCurrentOrder(this.GetUserName());
               try{
                    _incentiveService.ProcessCoupon(couponCode, order);
                    _orderService.SaveOrder(order);
                    TempData["Message"]="Coupon successfully applied";

               }catch(Exception x){
                   //report back the problem
                   //the coupon will throw if now valid
                   //capture the message
                   TempData["ErrorMessage"] = x.Message;
               }
               
            }
            return RedirectToAction("Show");
       }


        ActionResult ValidateShippingAndRedirect(Order order) {
            
            bool isValid = false;
            
            try {
                _addressValidator.VerifyAddress(order.ShippingAddress);
                isValid = true;

            } catch {
                this.SetErrorMessage("Please check the address you've entered; it appears invalid");
            }

            if (isValid) {
                
                //put it in TempData
                PutTempOrder(order);
                //send them off...
                return RedirectToAction("CreditCard");
            
            } else {
                OrderData data = new OrderData();
                data.CurrentOrder = order;
                return View("Shipping", order);
            }

        }

        public ActionResult ShippingFromAddressBook() {

            //this method is called when an address is sent in from the
           //address book
            string addressID = Request.Form["addressid"];
            OrderData data = new OrderData();

            if (!String.IsNullOrEmpty(addressID)) {
                data.CurrentOrder = GetTempOrder();
                //selected from address book
                data.CurrentOrder.ShippingAddress = _orderService.GetAddress(Convert.ToInt32(addressID));
                
                return ValidateShippingAndRedirect(data.CurrentOrder);

            } else {
                return View("Shipping", data);
            }

        }

        public ActionResult ShippingAddressFromCardSpace() {

            string xmlToken = Request.Form["xmlToken"];
            OrderData data = new OrderData();

            if (!String.IsNullOrEmpty(xmlToken)) {
                data.CurrentOrder = GetTempOrder();
                //see if we can read an address here
                data.CurrentOrder.ShippingAddress = new Address(xmlToken);
                data.CurrentOrder.ShippingAddress.UserName = this.GetUserName();
                return ValidateShippingAndRedirect(data.CurrentOrder);

            } else {
                return View("Shipping", data);
            }
            
        }
        
        public ActionResult BillingAddressFromCardSpace() {
            string xmlToken = Request.Form["xmlToken"];
            OrderData data = new OrderData();

            if (!String.IsNullOrEmpty(xmlToken)) {
                data.CurrentOrder = GetTempOrder();
                //see if we can read an address here
                data.CurrentOrder.BillingAddress = new Address(xmlToken);
                data.CurrentOrder.BillingAddress.UserName = this.GetUserName();
                PutTempOrder(data.CurrentOrder);
            }
            return View("CreditCard", data);
        }
     
        public ActionResult ShippingAddressFromForm() {

            //this method is called when an address is manually entered
            //manual entry
            OrderData data = new OrderData();
            data.CurrentOrder = GetTempOrder();

            //new up the shipping address
            data.CurrentOrder.ShippingAddress = new Address();
            data.CurrentOrder.ShippingAddress.UserName = this.GetUserName();
            //set if from Form POST

            UpdateModel(data.CurrentOrder.ShippingAddress,new[]{
                "FirstName",
                "LastName",
                "Email",
                "Street1",
                "Street2",
                "City",
                "StateOrProvince",
                "Zip",
                "Country"});

            return ValidateShippingAndRedirect(data.CurrentOrder);

        }

        public ActionResult CreditCard()
        {
            
            //there should be an order in TempData...
            OrderData data = new OrderData();
            data.CurrentOrder = GetTempOrder();
            
            //we should have a shipping address by now
            //calc the tax
            data.CurrentOrder.TaxAmount = _taxService.CalculateTaxAmount(data.CurrentOrder);

            string sAddressID = Request.Form["addressid"];
            int addressID = 0;

            if (!String.IsNullOrEmpty(sAddressID)) {
                int.TryParse(sAddressID, out addressID);
                data.CurrentOrder.BillingAddress = _orderService.GetAddress(addressID);
                //save it
                _orderService.SaveOrder(data.CurrentOrder);
                return View(data);
            }


            //if this is a post from the CreditCard action
            //check the number
            string ccNumber = Request.Form["accountnumber"];
            if (!string.IsNullOrEmpty(ccNumber)) {

                //set the Billing and CC
                if (data.CurrentOrder.BillingAddress == null) {
                    data.CurrentOrder.BillingAddress = new Address();
                    data.CurrentOrder.BillingAddress.UserName = this.GetUserName();
                }

                UpdateModel(data.CurrentOrder.BillingAddress, new[]{
                "FirstName",
                "LastName",
                "Email",
                "Street1",
                "Street2",
                "City",
                "StateOrProvince",
                "Zip",
                "Country"});



                CreditCard cc = new CreditCard();
              
                UpdateModel(cc, new[]{
                "AccountNumber",
                "CardType",
                "ExpirationMonth",
                "ExpirationYear",
                "VerificationCode"});
                
                data.CurrentOrder.PaymentMethod = cc;
                
                PutTempOrder(data.CurrentOrder);

                //make sure the card is valid
                if (!cc.IsValid()) {
                    this.SetErrorMessage("This credit card is not valid. Please check the number and expiration date");
                    return View(data);
                } else {
                    return RedirectToAction("Finalize");
                }
            } else {


                if (data.CurrentOrder.ShippingAddress == null) {
                    return RedirectToAction("Shipping");
                } else {
                    if (data.CurrentOrder.BillingAddress == null)
                        data.CurrentOrder.BillingAddress = data.CurrentOrder.ShippingAddress;
                    PutTempOrder(data.CurrentOrder);
                    return View(data);
                }
            }
        }

        public ActionResult SetCreditCardAndBilling() {
            //there should be an order in TempData...
            OrderData data = new OrderData();
            data.CurrentOrder = GetTempOrder();

            if (data.CurrentOrder.ShippingAddress == null) {
                return RedirectToAction("Shipping");
            } else {
                CreditCard card = new CreditCard();
                data.CurrentOrder.BillingAddress = new Address();

                UpdateModel(data.CurrentOrder.BillingAddress, new[]{
                "FirstName",
                "LastName",
                "Email",
                "Street1",
                "Street2",
                "City",
                "StateOrProvince",
                "Zip",
                "Country"});

                UpdateModel(card, new[]{
                "AccountNumber",
                "CardType",
                "ExpirationMonth",
                "ExpirationYear",
                "VerificationCode"});

                data.CurrentOrder.PaymentMethod = card;

                PutTempOrder(data.CurrentOrder);
                return RedirectToAction("Finalize");
            }
        }

        public ActionResult Finalize() {
            OrderData data = new OrderData();
            data.CurrentOrder = GetTempOrder();

            if (data.CurrentOrder.ShippingAddress == null || data.CurrentOrder.BillingAddress == null || data.CurrentOrder.PaymentMethod == null) {
                return RedirectToAction("Checkout");
            } else {
                data.ShippingMethods = _shippingService.CalculateRates(data.CurrentOrder);
                string selectedShipping = Request.Form["shippingMethod"];

                if (!string.IsNullOrEmpty(selectedShipping)) {
                    data.CurrentOrder.ShippingMethod = data.ShippingMethods.Where(x => x.ID == int.Parse(selectedShipping)).SingleOrDefault();
                } else {
                    //default the shipping method to the first selected
                    data.CurrentOrder.ShippingMethod = data.ShippingMethods[0];

                }


                //put the order back to TempData
                PutTempOrder(data.CurrentOrder);

                return View(data);
            }
        }

        public ActionResult PlaceOrder() {
            
            Order order = GetTempOrder();
            
            if (order == null) {
                return RedirectToAction("Checkout");
            } else {

                //save the order to the db
                //this queues it for the submission process
                _orderService.SaveOrder(order);

                                
                //invoke order process
                //this is optional - you can control this
                //by using an external service if you like
                _pipeline.VerifyOrder(order);
                

                return RedirectToAction("Receipt", new { id = order.ID.ToString() });
            }

        }

        public ActionResult Receipt(string id) {

            if (!String.IsNullOrEmpty(id)) {
                Guid orderID = new Guid(id);
                OrderData data = new OrderData();
                data.CurrentOrder = _orderService.GetOrder(orderID);
                return View(data);

            } else {
                return RedirectToAction("Show");
            }
        }

        void PutTempOrder(Order order) {
            TempData["order"] = order;
        }
        Order GetTempOrder() {
            Order result = (Order)TempData["order"] ?? _orderService.GetCurrentOrder(this.GetUserName());
            return result;
        }
        #endregion

        
        #region Fulfillment Actions
        public ActionResult List(int? pageNumber) {
            

            int pageIndex = pageNumber == null ? 0 : (int)pageNumber - 1;

            string selectedStatus = Request.Form["statusFilter"];
            OrderStatus statusFilter = OrderStatus.Verified;

            if (!String.IsNullOrEmpty(selectedStatus)) {
                statusFilter = (OrderStatus)int.Parse(selectedStatus);

            }

            var orders = _orderService.GetPagedOrders(20, pageIndex,statusFilter)
                .OrderByDescending(x=>x.DateCreated)
                .OrderBy(x=>x.Status).ToList();

            return View(orders);

        }

        Order GetRequestedOrder(string id)
        {
            Order order = null;


            if (!string.IsNullOrEmpty(id))
            {
                Guid orderID = new Guid(id);
                order = _orderService.GetOrder(orderID);
            }

            return order;
            
        }
        public ActionResult Manage(string id) {
            
            Order order = GetRequestedOrder(id);
            ViewData["CanCancel"] = _orderService.CanCancelOrder(order);


            if (order == null)
                return RedirectToAction("Show");
            else
                return View(order);

        }

        public ActionResult Charge(string id)
        {
            Order order = GetRequestedOrder(id);

            if (order == null)
            {
                return RedirectToAction("Show");
            }
            else
            {
                _pipeline.ChargeOrder(order);
                TempData["Message"] = "Order Charged";
                return RedirectToAction("Manage", new { id = order.ID.ToString() });
            }
        }

        public ActionResult Cancel(string id)
        {
            Order order = GetRequestedOrder(id);

            if (order == null)
            {
                return RedirectToAction("Show");
            }
            else
            {
                _pipeline.CancelOrder(order);
                TempData["Message"] = "Order Cancelled";
                return RedirectToAction("Manage", new { id = order.ID.ToString() });
            }

        }

        public ActionResult Ship(string id)
        {
            Order order = GetRequestedOrder(id);

            if (order == null)
            {
                return RedirectToAction("Show");
            }
            else
            {

                string trackingNumber = Request.Form["trackingNumber"];

                _pipeline.ShipOrder(order,trackingNumber,DateTime.Now.AddDays(order.ShippingMethod.DaysToDeliver));
                TempData["Message"] = "Order Set as Shipped";
                return RedirectToAction("Manage", new { id = order.ID.ToString() });
            }

        }


        #endregion


    }
}
