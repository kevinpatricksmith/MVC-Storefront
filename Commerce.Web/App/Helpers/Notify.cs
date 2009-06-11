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

namespace System.Web.Mvc{
    public static class UIMessaging {

        public static string DisplayError(this HtmlHelper helper) {
            string result = "";

            if (helper.ViewDataContainer.ViewData["ErrorMessage"] != null)
            {
                result = string.Format("<div class=\"notify-error\">{0}</div>", helper.ViewDataContainer.ViewData["ErrorMessage"]);
            }

            return result;
        }
        public static string Notify(this HtmlHelper helper)
        {
            string result = "";
            if (helper.ViewContext.TempData["ErrorMessage"] != null)
            {
                result = string.Format("<div class=\"notify-error\">{0}</div>", helper.ViewContext.TempData["ErrorMessage"]);
            }
            else if (helper.ViewContext.TempData["Message"] != null)
            {
                result = string.Format("<div class=\"notify-message\">{0}</div>", helper.ViewContext.TempData["Message"]);

            }

            return result;
        }

    }
}
