using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.MVC.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        
        IOrderService _orderService;
        public AuthenticationController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public ActionResult Login()
        {

            string oldUserName = this.GetUserName();

            string login = Request.Form["login"];
            string password = Request.Form["password"];

            if (!String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(password))
            {
                var svc = new AspNetAuthenticationService();
                bool isValid = svc.IsValidLogin(login, password);

                //log them in 
                if (isValid)
                {
                    SetPersonalizationCookie(login, login);

                    //migrate the current order
                    _orderService.MigrateCurrentOrder(oldUserName, login);

                    return AuthAndRedirect(login);
                }
            }
            return View();
            

        }

        public ActionResult OpenIDLogin()
        {
            string claimedUrl = Request["openid.claimed_id"];
            string oldUserName = this.GetUserName();

            if (!String.IsNullOrEmpty(claimedUrl))
            {
                try {
                    Uri openIDUri = new Uri(claimedUrl);
                    var svc = new OpenIDAuthenticationService();

                    //you can just check to see if it's valid

                    //or you can use AttributeExchange...
                    Address add = svc.GetOpenIDAddress(openIDUri);

                    if (add != null) {
                        SetPersonalizationCookie(claimedUrl, "");

                        //migrate the current order
                        _orderService.MigrateCurrentOrder(oldUserName, claimedUrl);

                        return AuthAndRedirect(claimedUrl);
                    }
                }
                catch {
                    ViewData["ErrorMessage"] = "Invalid Open ID URI Entered";
                }

            }

            return View("Login");

        }

        ActionResult  AuthAndRedirect(string userName)
        {
            
            string returnUrl = Request.Form["ReturnUrl"];
            FormsAuthentication.SetAuthCookie(userName, false);

            if (!String.IsNullOrEmpty(returnUrl))
            {
                
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Checkout", "Order");
            }
        }

        void SetPersonalizationCookie(string userName, string friendlyName)
        {
            Response.Cookies["shopper"].Value = userName;
            Response.Cookies["shopperName"].Value = friendlyName;
            Response.Cookies["shopper"].Expires = DateTime.Now.AddDays(30);
            Response.Cookies["shopperName"].Expires = DateTime.Now.AddDays(30);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Index", "Catalog");

        }
    }
}
