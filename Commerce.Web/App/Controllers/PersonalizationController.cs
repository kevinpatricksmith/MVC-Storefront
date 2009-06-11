using System.Web.Mvc;
using Commerce.Data;
using Commerce.Services;
using Commerce.MVC.Web.Controllers;
using System.Collections.Generic;

namespace Commerce.MVC.Web.Controllers
{
    public class PersonalizationController:Controller {
        private IPersonalizationService _pService;

        public PersonalizationController(IPersonalizationService pService) {
            _pService = pService;
        }


        public ActionResult Summary() {

            UserProfile user = _pService.GetProfile(this.GetUserName());
            return View(user);
        }

        public ActionResult AddressBook() {
            UserProfile user = _pService.GetProfile(this.GetUserName());
            return View(user.AddressBook);
        }

        public ActionResult Favorites() {
            UserProfile user = _pService.GetProfile(this.GetUserName());
            return View(user);
        }

    }
}