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
using Microsoft.Web.Mvc;
using Commerce.MVC.Web.Controllers;

public static class CatalogHelper {

        /// <summary>
        /// Creates an image tag for a product
        /// </summary>
        /// <param name="imageName">The name of the product image</param>
        public static string ProductImage(this HtmlHelper helper, string imageName) {
            string productImageDirectory = VirtualPathUtility.ToAbsolute("~/Content/ProductImages");
            string productImagePath = string.Format("{0}/{1}", productImageDirectory, imageName);
            return helper.Image(productImagePath);
        }

        public static void CatalogList(this HtmlHelper helper)
        {
            helper.RenderAction<CatalogController>(x => x.CategoryList());
        }

    }
