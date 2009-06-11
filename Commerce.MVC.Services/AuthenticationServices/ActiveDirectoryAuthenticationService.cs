using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Services {
    public class ActiveDirectoryAuthenticationService:IAuthenticationService {

        public bool IsValidLogin(string userName, string password) {
            throw new NotImplementedException();
        }

        public bool IsValidLogin(Uri serviceUri) {
            throw new NotImplementedException();
        }

        public object RegisterUser(string userName, string password, string confirmPassword, string email, string reminderQuestion, string reminderAnswer) {
            throw new NotImplementedException();
        }


        public bool IsValidLogin(string logonToken)
        {
            throw new NotImplementedException();
        }

    }
}
