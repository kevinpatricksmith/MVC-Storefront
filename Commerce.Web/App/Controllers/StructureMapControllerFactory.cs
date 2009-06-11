using System;

using System.Web.Mvc;
using StructureMap;

namespace Commerce.MVC.Web.Controllers {
    public class StructureMapControllerFactory: DefaultControllerFactory {

        protected override IController GetControllerInstance(Type controllerType) {
            IController result = null;
            if (controllerType != null) {
                try {
                    result = ObjectFactory.GetInstance(controllerType) as Controller;

                }
                catch (StructureMapException) {
                    System.Diagnostics.Debug.WriteLine(ObjectFactory.WhatDoIHave());
                    throw;
                }
            }
            return result;
        }
    }
}
