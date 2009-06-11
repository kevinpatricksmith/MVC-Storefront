using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Commerce.Data;
using Microsoft.Web.Mvc;
using System.Workflow.Runtime;
using Commerce.Services;
using Commerce.Pipelines;

namespace Commerce.MVC.Web {


    public class GlobalApplication : System.Web.HttpApplication {
        
        public static void RegisterRoutes(RouteCollection routes) {
            // Note: Change the URL to "{controller}.mvc/{action}/{id}" to enable
            //       automatic support on IIS6 and IIS7 classic mode

           
            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter constraints
            );


        }

        
        
        protected void Application_Start() {
            
            RegisterRoutes(RouteTable.Routes);

            
            //DI Stuff
            Bootstrapper.ConfigureStructureMap();

            ControllerBuilder.Current.SetControllerFactory(
                new Commerce.MVC.Web.Controllers.StructureMapControllerFactory()
                );

            ViewEngines.Engines.Add(new Commerce.MVC.Web.App.CommerceViewEngine());


            GetLogger().Info("App is starting");

        }

        protected void Application_End() {
            GetLogger().Info("App is shutting down");
        }

        protected void Application_Error() {
            Exception lastException = Server.GetLastError();
            GetLogger().Fatal(lastException);
        }

        ILogger GetLogger() {
            return StructureMap.ObjectFactory.GetInstance<ILogger>();
        }
    }
}