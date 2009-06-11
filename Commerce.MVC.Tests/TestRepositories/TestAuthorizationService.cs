using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Services;

namespace Commerce.Tests {
    public class TestAuthorizationService:IAuthorizationService {


        #region IAuthorizationService Members

        public bool IsSuperAdmin(string userName) {
            return userName.Equals("testuser");
        }

        #endregion
    }
}
