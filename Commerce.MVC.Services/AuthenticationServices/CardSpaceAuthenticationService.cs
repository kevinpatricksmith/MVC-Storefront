using System;
using Microsoft.IdentityModel.TokenProcessor;
using System.IdentityModel.Claims;

namespace Commerce.Services.AuthenticationServices
{
    public class CardSpaceAuthenticationService:IAuthenticationService
    {

        public bool IsValidLogin(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsValidLogin(Uri serviceUri)
        {
            throw new NotImplementedException();
        }

        public object RegisterUser(string userName, string password, string confirmPassword, string email, string reminderQuestion, string reminderAnswer)
        {
            throw new NotImplementedException();
        }


        public bool IsValidLogin(string xmlToken)
        {
            //synch it



            return false;
        }

    }
}
