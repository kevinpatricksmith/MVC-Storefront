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
using Commerce.Services;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

public static class AppHelper {

    /// <summary>
    /// Builds an Image URL
    /// </summary>
    /// <param name="imageFile">The file name of the image</param>
    public static string ImageUrl(string imageFile) {
        return VirtualPathUtility.ToAbsolute("~/content/images/" + imageFile);
    }

    /// <summary>
    /// Builds a CSS URL
    /// </summary>
    /// <param name="cssFile">The name of the CSS file</param>
    public static string CssUrl(string cssFile) {
        return VirtualPathUtility.ToAbsolute("~/content/css/" + cssFile);
    }

}
