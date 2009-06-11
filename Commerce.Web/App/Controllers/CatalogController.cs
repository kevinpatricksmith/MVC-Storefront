using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.MVC.Web.Controllers {
    public class CatalogController : Controller {
        
        #region .ctor
        ICatalogService _catalogService;
        IPersonalizationService _personalizationService;

        public CatalogController(ICatalogService catalogService, 
            IPersonalizationService personalizationService) {

            _catalogService = catalogService;
            _personalizationService = personalizationService;
        }

        #endregion  
        
        /// <summary>
        /// Main method for the Catalog
        /// </summary>
        public ActionResult Index(int? id) {


            int categoryID = id ?? 0;
            Category category = _catalogService.GetCategory(categoryID) ??
                _catalogService.GetDefaultCategory();
            
            //log the event
            _personalizationService.SaveCategoryView(this.GetUserName(), Request.UserHostAddress, category);

            return View(category);
        }

        public ActionResult Show(string id) {

            Product product = _catalogService.GetProduct(id);
            
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //log the event
                _personalizationService.SaveProductView(this.GetUserName(), Request.UserHostAddress, product);
                return View("Show", product);
            }

        }



        public ActionResult CategoryList()
        {
            return View("_CategoryList", _catalogService.GetCategories());
        }


        //[Authorize(Roles="Administrator")]
        public ActionResult Edit(int id)
        {
            ViewData["Products"] = _catalogService.GetProducts();
            Product p = _catalogService.GetProduct(id);
            return View(p);

        }
    }
}
