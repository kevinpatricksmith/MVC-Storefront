using System;
using DotNetOpenId.RelyingParty;
using Commerce.Data;
using DotNetOpenId.Extensions.AttributeExchange;
using System.Collections.Generic;

namespace Commerce.Services {
    public class OpenIDAuthenticationService:IAuthenticationService {
      

        public bool IsValidLogin(string userName, string password) {
            throw new NotImplementedException();
        }

        public bool IsValidLogin(Uri serviceUri)
        {
            bool result = false;
            var openid = new OpenIdRelyingParty();
            if (openid.Response == null)
            {
                // Stage 2: user submitting Identifier
                openid.CreateRequest(serviceUri.AbsoluteUri).RedirectToProvider();
            }
            else
            {
                result = openid.Response.Status == AuthenticationStatus.Authenticated;

                if(result)
                {
                    //synch the users

                }


            }
            return result;
        }


        public object RegisterUser(string userName, string password, string confirmPassword, string email, string reminderQuestion, string reminderAnswer) {
            throw new NotImplementedException();
        }


        bool IAuthenticationService.IsValidLogin(string logonToken)
        {
            throw new NotImplementedException();
        }

        public Address GetOpenIDAddress(Uri claimUri)
        {
            var openid = new OpenIdRelyingParty();
            Address result=new Address();
            if (openid.Response != null)
            {
                // Stage 2: user submitting Identifier
                var fetch = openid.Response.GetExtension<FetchResponse>();
                if (fetch != null)
                {
                    
                    
                    result.Email = GetFetchValue(fetch, "contact/email");
                    result.FirstName = GetFetchValue(fetch, "namePerson/first");
                    result.LastName = GetFetchValue(fetch, "namePerson/last");
                    result.Street1 = GetFetchValue(fetch, "contact/streetaddressLine1/home");
                    result.Street2 = GetFetchValue(fetch, "contact/streetaddressLine2/home");
                    result.City = GetFetchValue(fetch, "contact/city/home");
                    result.StateOrProvince = GetFetchValue(fetch, "contact/city/stateorprovince");
                    result.Country = GetFetchValue(fetch, "contact/country/home");
                    result.Zip = GetFetchValue(fetch, "contact/postalCode/home");

                    result.UserName = openid.Response.ClaimedIdentifier;

                }
            }
            else
            {
                var request=openid.CreateRequest(claimUri.AbsoluteUri);
                var fetch = new FetchRequest();
                fetch.AddAttribute(new AttributeRequest("contact/email"));
                fetch.AddAttribute(new AttributeRequest("namePerson/first"));
                fetch.AddAttribute(new AttributeRequest("namePerson/last"));
                fetch.AddAttribute(new AttributeRequest("contact/streetaddressLine1/home"));
                fetch.AddAttribute(new AttributeRequest("contact/streetaddressLine2/home"));
                fetch.AddAttribute(new AttributeRequest("contact/city/home"));
                fetch.AddAttribute(new AttributeRequest("contact/city/stateorprovince"));
                fetch.AddAttribute(new AttributeRequest("contact/country/home"));
                fetch.AddAttribute(new AttributeRequest("contact/postalCode/home"));
                request.AddExtension(fetch);
                request.RedirectToProvider();
            }
            return result;
        }

        string GetFetchValue(FetchResponse fetch, string key)
        {

            string schema = "http://schema.openid.net/";
            IList<string> results = fetch.GetAttribute(schema+key).Values;
            string result = results.Count > 0 ? results[0] : "";
            return result;
        }
    }
}
