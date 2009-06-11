using System;
using Commerce.Services;
namespace Commerce.Tests {
    public class TestAuthenticationService:IAuthenticationService {

        public bool IsValidLogin(string userName, string password) {

            return userName == "test" && password == "password";

        }

        public bool IsValidLogin(Uri serviceUri) {
            return serviceUri.AbsoluteUri.Equals("http://test.com/");
        }



        public object RegisterUser(string userName, string password, string confirmPassword, string email, string reminderQuestion, string reminderAnswer) {
            return true;
        }




        public bool IsValidLogin(string logonToken)
        {
            throw new NotImplementedException();
        }

    }
}
