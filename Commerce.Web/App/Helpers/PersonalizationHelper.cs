using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Mvc;

namespace Commerce.MVC.Web.Helpers
{
    public static class PersonalizationHelper
    {
        public static string GetFriendlyUserName(this ViewPage pg){

            string result = "Guest";
            if (HttpContext.Current.Request.Cookies["shopperName"] != null)
                result = HttpContext.Current.Request.Cookies["shopperName"].Value;
            return result;
        }
    }
}
