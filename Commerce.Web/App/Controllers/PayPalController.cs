using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Net;
using System.Text;
using System.IO;
using Commerce.Services;
using Commerce.Data;
using Commerce.Pipelines;


namespace Commerce.MVC.Web.Controllers {
    public class PayPalController : Controller {
        
        IOrderService _orderService;
        ILogger _logger;
        IPipelineEngine _pipeline;
        IMailerService _mail;
        IShippingService _shippingService;

        public PayPalController(IOrderService orderService, 
            ILogger logger, IPipelineEngine pipeline, IMailerService mail, IShippingService shippingService) {
            _orderService = orderService;
            _logger = logger;
            _pipeline = pipeline;
            _mail = mail;
            _shippingService = shippingService;
        }


        /// <summary>
        /// PayPal Checkout Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Checkout() {
            Order order = _orderService.GetCurrentOrder(this.GetUserName());
            
            IList<ShippingMethod> methods=_shippingService.CalculateRates(order);


            string selectedShipping = Request.Form["shippingMethod"];

            if (!string.IsNullOrEmpty(selectedShipping)) {
                order.ShippingMethod = methods.Where(x => x.ID == int.Parse(selectedShipping)).SingleOrDefault();
                //save it
                _orderService.SaveOrder(order);
            }
            else {
                //default the shipping method to the first selected
                order.ShippingMethod = methods[0];
            }

            ViewData["ShippingMethods"] = methods;

            if (order.Items.Count == 0)
                return RedirectToAction("Index", "Catalog");
            else
                return View(order);

        }

        /// <summary>
        /// Handles the IPN Notification from PayPal
        /// </summary>
        /// <returns></returns>
        public ActionResult IPN() {
            _logger.Info("IPN Invoked");

            var formVals = new Dictionary<string, string>();
            formVals.Add("cmd", "_notify-validate");

            string response = GetPayPalResponse(formVals, true);

            if (response == "VERIFIED") {

                string transactionID = Request["txn_id"];
                string sAmountPaid = Request["mc_gross"];
                string orderID = Request["custom"];

                _logger.Info("IPN Verified for order " + orderID);

                //validate the order
                Decimal amountPaid = 0;
                Decimal.TryParse(sAmountPaid, out amountPaid);

                Order order = _orderService.GetOrder(new Guid(orderID));

                //check the amount paid

                if (AmountPaidIsValid(order, amountPaid)) {

                    Address add = new Address();
                    add.FirstName = Request["first_name"];
                    add.LastName = Request["last_name"];
                    add.Email = Request["payer_email"];
                    add.Street1 = Request["address_street"];
                    add.City = Request["address_city"];
                    add.StateOrProvince = Request["address_state"];
                    add.Country = Request["address_country"];
                    add.Zip = Request["address_zip"];
                    add.UserName = order.UserName;


                    order.ShippingAddress = add;
                    order.BillingAddress = order.ShippingAddress;
                    //process it
                    try {
                        _pipeline.AcceptPalPayment(order, transactionID, amountPaid);
                        _logger.Info("IPN Order successfully transacted: " + orderID);
                        return RedirectToAction("Receipt", "Order", new { id = order.ID });
                    }
                    catch (Exception x) {
                        HandleProcessingError(order, x);
                        return View();
                    }
                }
                else {
                    //let fail - this is the IPN so there is no viewer
                }
            }



            return View();
        }


        bool AmountPaidIsValid(Order order, decimal amountPaid) {

            //pull the order
            bool result = true;

            if (order != null) {
                if (order.Total > amountPaid) {
                    _logger.Warn("Invalid order amount to PDT/IPN: " + order.ID + "; Actual: "+amountPaid.ToString("C")+"; Should be: "+order.Total.ToString("C")+"user IP is " + Request.UserHostAddress);
                    result = false;
                }
            }
            else {
                _logger.Warn("Invalid order ID passed to PDT/IPN; user IP is " + Request.UserHostAddress);
            }
            return result;

        }


        /// <summary>
        /// Handles the PDT Response from PayPal
        /// </summary>
        /// <returns></returns>
        public ActionResult PDT() {
            _logger.Info("PDT Invoked");
            string transactionID = Request.QueryString["tx"];
            string sAmountPaid = Request.QueryString["amt"];
            string orderID=Request.QueryString["cm"];


            Dictionary<string, string> formVals = new Dictionary<string, string>();
            formVals.Add("cmd", "_notify-synch");
            formVals.Add("at", "JijaVlgNlwzXc5N_Zj53LS-v5EmzqsQGMa6eZcKyXad8hH7dn08ntEZlcAW");
            formVals.Add("tx", transactionID);

            string response = GetPayPalResponse(formVals, true);
            //_logger.Info("PDT Response received: " + response);
            if (response.StartsWith("SUCCESS")) {
                _logger.Info("PDT Response received for order " + orderID);

                //validate the order
                Decimal amountPaid = 0;
                Decimal.TryParse(sAmountPaid, out amountPaid);


                Order order = _orderService.GetOrder(new Guid(orderID));

                if (AmountPaidIsValid(order, amountPaid)) {


                    Address add = new Address();
                    add.FirstName = GetPDTValue(response, "first_name");
                    add.LastName = GetPDTValue(response, "last_name");
                    add.Email = GetPDTValue(response, "payer_email");
                    add.Street1 = GetPDTValue(response, "address_street");
                    add.City = GetPDTValue(response, "address_city");
                    add.StateOrProvince = GetPDTValue(response, "address_state");
                    add.Country = GetPDTValue(response, "address_country");
                    add.Zip = GetPDTValue(response, "address_zip");
                    add.UserName = order.UserName;

                    order.ShippingAddress = add;
                    order.BillingAddress = order.ShippingAddress;

                    //process it
                    try {
                        _pipeline.AcceptPalPayment(order, transactionID, amountPaid);
                        _logger.Info("PDT Order successfully transacted: " + orderID);
                        return RedirectToAction("Receipt", "Order", new { id = order.ID });
                    }
                    catch (Exception x) {
                        HandleProcessingError(order, x);
                        return View();
                    }

                }
                else {
                    //Payment amount is off
                    //this can happen if you have a Gift cert at PayPal
                    //be careful of this!
                    HandleProcessingError(order, new InvalidOperationException("Amount paid (" + amountPaid.ToString("C") + ") was below the order total"));
                    return View();
                }
            }
            else {
                ViewData["message"] = "Your payment was not successful with PayPal";
                return View();
            }
        }
        string GetPDTValue(string pdt, string key) {

            string[] keys = pdt.Split('\n');
            string thisVal = "";
            string thisKey = "";
            foreach (string s in keys) {
                string[] bits = s.Split('=');
                if (bits.Length > 1) {
                    thisVal = bits[1];
                    thisKey = bits[0];
                    if (thisKey.Equals(key,StringComparison.InvariantCultureIgnoreCase))
                        break;
                }
            }
            return thisVal;


        }
        void HandleProcessingError(Order order, Exception x) {
            //oops - this isn't good. Exceptions after payment is made is not 
            //a good thing. Log the error
            _logger.Fatal(x);

            ViewData["message"] = "We received and error during the processing if your order. We'll review the error and contact you if required";

            if (Request.Url.AbsoluteUri.Contains("localhost"))
                ViewData["message"] += x.Message;

            //let someone know this
            string message = "Error process order: " + x.Message;
            _mail.SendAdminEmail(order, "Processing Error: " + order.OrderNumber, message);
            
        }

        /// <summary>
        /// Utility method for handling PayPal Responses
        /// </summary>
        string GetPayPalResponse(Dictionary<string,string> formVals, bool useSandbox) {

            string paypalUrl = useSandbox ? "https://www.sandbox.paypal.com/cgi-bin/webscr" 
                : "https://www.paypal.com/cgi-bin/webscr";


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);

            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] param = Request.BinaryRead(Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);
            
            StringBuilder sb = new StringBuilder();
            sb.Append(strRequest);

            foreach (string key in formVals.Keys) {
                sb.AppendFormat("&{0}={1}", key, formVals[key]);
            }
            strRequest += sb.ToString();
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://urlort#");
            //req.Proxy = proxy;
            //Send the request to PayPal and get the response
            string response = "";
            using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII)) {
                
                streamOut.Write(strRequest);
                streamOut.Close();
                using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream())) {
                    response = streamIn.ReadToEnd();
                }
            }

            return response;
        }

    }
}
