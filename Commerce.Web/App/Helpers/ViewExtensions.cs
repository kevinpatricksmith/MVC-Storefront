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
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Collections;
using System.Web.Mvc.Html;

    public static class ViewExtensions {


        /// <summary>
        /// Renders a Commerce.MVC-specific user control
        /// </summary>
        /// <param name="controlName">Name of the control</param>
        /// <param name="data">ViewData to pass in</param>
        public static void RenderCommerceControl(this System.Web.Mvc.HtmlHelper helper,
            CommerceControls control, object data) {

            data = data ?? new object();

            string controlName = Enum.GetName(typeof(CommerceControls), control);
            helper.RenderPartial(controlName, data, helper.ViewContext.ViewData);
        }


        /// <summary>
        /// A helper for registering script tags on an MVC View page.
        /// </summary>
        public static string RegisterJS(this System.Web.Mvc.HtmlHelper helper, ScriptLibrary scriptLib) {
            //get the directory where the scripts are
            string scriptRoot = VirtualPathUtility.ToAbsolute("~/Content/Scripts");
            string scriptFormat="<script src=\"{0}/{1}\" type=\"text/javascript\"></script>\r\n";
            
            string scriptLibFile="";
            string result = "";
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            
            //all of the script tags
            if (scriptLib == ScriptLibrary.Ajax) {
                sb.AppendLine(RegisterScript(helper, "MicrosoftAjax.js"));
                sb.AppendLine(RegisterScript(helper, "MicrosoftAjaxMVC.js"));

            } else {
                sb.AppendLine(RegisterScript(helper, Enum.GetName(typeof(ScriptLibrary),scriptLib)+".js"));

            }
            result = sb.ToString();
            return result;
        }


        /// <summary>
        /// A helper for registering script tags on an MVC View page.
        /// </summary>
        public static string RegisterScript(this System.Web.Mvc.HtmlHelper helper, string scriptFileName) {
            //get the directory where the scripts are
            string scriptRoot = VirtualPathUtility.ToAbsolute("~/Content/Scripts");
            string scriptFormat = "<script src=\"{0}/{1}\" type=\"text/javascript\"></script>\r\n";

            string scriptLibFile = "";
            string result = "";

            return string.Format(scriptFormat, scriptRoot, scriptFileName);

        }

        public static string GetNumericPager(this HtmlHelper helper, string urlFormat, int totalRecords, int pageSize, int currentPage) {

            string linkFormat = "<a href=\"{0}\">{1}</a>";

            int totalPages = totalRecords / pageSize;
            if (totalRecords % pageSize > 0) {
                totalPages++;
            }

            bool isFirst = true;
            StringBuilder sb = new StringBuilder();

            for (int i = 1; i < totalPages + 1; i++) {
                if (!isFirst)
                    sb.Append(" | ");

                string pageLink = i.ToString();

                if (currentPage != i) {

                    sb.AppendFormat(linkFormat, string.Format(urlFormat, i), pageLink);

                } else {
                    sb.AppendFormat("<b>{0}</b>", pageLink);
                }


                isFirst = false;
            }

            return sb.ToString();
        }


        /// <summary>
        /// Creates an Option list from an IEnumerable
        /// </summary>
        public static string ToOptionList(this IEnumerable enumerable) {
            return ToOptionList(enumerable, "", "", "");
        }


        /// <summary>
        /// Creates an Option list from an IEnumerable
        /// </summary>
        public static string ToOptionList(this IEnumerable enumerable, object selectedValue) {
            return ToOptionList(enumerable, selectedValue, "", "");
        }

        /// <summary>
        /// Creates an Option list from an IEnumerable
        /// </summary>
        public static string ToOptionList(this IEnumerable enumerable, object selectedValue, string valueField, string textField) {
            SelectList list = new SelectList(enumerable, valueField, textField, selectedValue);
            string listFormat = "<option value=\"{0}\" {1}>{2}</option>\r\n";
            string checkedFlag="selected=\"selected\"";

            return ToHtmlList(listFormat, checkedFlag, enumerable, selectedValue, valueField, textField);
        }

        /// <summary>
        /// Creates an HTML name/value list for use with dropdowns, etc
        /// </summary>
        /// <returns></returns>
        static string ToHtmlList(string listFormat, string checkedFlag, IEnumerable enumerable, object selectedValue, string valueField, string textField) {
            SelectList list = new SelectList(enumerable, valueField, textField, selectedValue);
            StringBuilder sb = new StringBuilder();

            string selected = "";
            List<System.Web.Mvc.ListItem> items = list.GetListItems().ToList();
            foreach (System.Web.Mvc.ListItem item in items) {
                selected = item.Selected ? checkedFlag : "";
                sb.AppendFormat(listFormat, item.Value, selected, item.Text);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Creates a formatted list of items based on the passed in format
        /// </summary>
        /// <param name="list">The item list</param>
        /// <param name="format">The single-place format string to use</param>
        public static string ToFormattedList(this IEnumerable list, ListType listType) {
            StringBuilder sb = new StringBuilder();
            IEnumerator en = list.GetEnumerator();

            string outerListFormat = "";
            string listFormat = "";

            switch (listType) {
                case ListType.Ordered:
                    outerListFormat = "<ol>{0}</ol>";
                    listFormat = "<li>{0}</li>";
                    break;
                case ListType.Unordered:
                    outerListFormat = "<ul>{0}</ul>";
                    listFormat = "<li>{0}</li>";
                    break;
                case ListType.TableCell:
                    outerListFormat = "{0}";
                    listFormat = "<td>{0}</td>";
                    break;
                default:
                    break;
            }


            return string.Format(outerListFormat, ToFormattedList(list, listFormat));
        }

        /// <summary>
        /// Creates a formatted list of items based on the passed in format
        /// </summary>
        /// <param name="list">The item list</param>
        /// <param name="format">The single-place format string to use</param>
        public static string ToFormattedList(IEnumerable list, string format) {

            StringBuilder sb = new StringBuilder();
            foreach (object item in list) {
                sb.AppendFormat(format, item.ToString());
            }

            return sb.ToString();

        }

        public static string ToLocalCurrency(this Decimal input) {
            return Math.Round(input, System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
        }

        public static string GetSiteUrl(this ViewPage pg) {
            string Port = pg.ViewContext.HttpContext.Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = pg.ViewContext.HttpContext.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string appPath = pg.ViewContext.HttpContext.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";

            string sOut = Protocol + pg.ViewContext.HttpContext.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return sOut;
        }

        /// <summary>
        /// Creates a ReturnUrl for use with the Login page
        /// </summary>
        public static string GetReturnUrl(this ViewPage pg)
        {
            string returnUrl = "";
            
            if (pg.Html.ViewContext.HttpContext.Request.QueryString["ReturnUrl"] != null)
            {
                returnUrl = pg.Html.ViewContext.HttpContext.Request.QueryString["ReturnUrl"];
            }
            else
            {
                returnUrl = pg.Html.ViewContext.HttpContext.Request.Url.AbsoluteUri;
            }
            return returnUrl;
        }

        public static bool UserIsAdmin(this ViewPage pg)
        {
            return pg.User.IsInRole("Administrator");
        }

        public static bool UserIsAdmin(this ViewUserControl ctrl)
        {
            return ctrl.Page.User.IsInRole("Administrator");
        }

        public static string IsChecked(this ViewPage pg, bool value) {
            return value ? " checked=\"checked\" " : "";
        }

        public static IEnumerable ToEnumerable(this ViewPage pg, object enumerable) {
            return enumerable as IEnumerable;
        }

    }
