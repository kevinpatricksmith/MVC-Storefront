using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Commerce.MVC.Web.Views.Shared {
    public partial class ProductDisplay : System.Web.Mvc.ViewUserControl<Commerce.Data.Product> {
        
        public bool DisplayAddToCartButton {
            get {
                bool _displayAddToCartButton = true;

                if (ViewData["DisplayAddToCartButton"] != null) {
                    bool.TryParse(ViewData["DisplayAddToCartButton"].ToString(), 
                        out _displayAddToCartButton);
                }

                return _displayAddToCartButton;
            }
        }

        public bool DisplayDescription {
           get {
              bool _displayDescription = true;
              if (ViewData["DisplayDescription"] != null) {
                  bool.TryParse(ViewData["DisplayDescription"].ToString(), out _displayDescription);
              }
              return _displayDescription;
           }
        }
    
    }
}
