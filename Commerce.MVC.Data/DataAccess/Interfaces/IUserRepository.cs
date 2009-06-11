using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.MVC.Data {
    
    public interface IUserRepository {
        
        IQueryable<User> GetUsers();
        IQueryable<Role> GetRoles();
        User GetUser(string userName);

        IList<Role> GetRolesForUser(string userName);
        bool LoginUser(string userName, string password);
        bool RegisterUser(string first, string last, 
        string email, string userName, string password, string passwordQuestion, string passwordAnswer);
        bool DeleteUser(string userName);
    

    }



}
