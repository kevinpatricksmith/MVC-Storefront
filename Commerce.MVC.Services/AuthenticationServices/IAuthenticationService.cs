using System;

namespace Commerce.Services {
    
    public interface IAuthenticationService {
        bool IsValidLogin(string userName, string password);
        bool IsValidLogin(Uri serviceUri);
        bool IsValidLogin(string logonToken);
        object RegisterUser(string userName, 
            string password, 
            string confirmPassword, 
            string email, 
            string reminderQuestion, 
            string reminderAnswer);
    }
}
