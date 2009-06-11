using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Commerce.Data;

namespace Commerce.MVC.Web.Views.Shared {
    public partial class AddressEntry : ViewUserControl<Address> {

        public string NamePrefix { get; set; }
    }
}
