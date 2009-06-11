using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.MVC.Web.App
{
    public class CommerceViewEngine:WebFormViewEngine
    {
        public CommerceViewEngine()
        {
            MasterLocationFormats = new[] { 
                "~/App/Views/{1}/{0}.master", 
                "~/App/Views/Shared/{0}.master" 
            };
            ViewLocationFormats = new[] { 
                "~/App/Views/{1}/{0}.aspx", 
                "~/App/Views/{1}/{0}.ascx", 
                "~/App/Views/Shared/{0}.aspx", 
                "~/App/Views/Shared/{0}.ascx" 
            };
            PartialViewLocationFormats = ViewLocationFormats;

        }
    }
}
