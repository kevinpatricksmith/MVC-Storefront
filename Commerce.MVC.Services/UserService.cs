using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.MVC.Data;
using System.Web.Security;

namespace Commerce.MVC.Services {
    public class UserService : Commerce.MVC.Services.IUserService {

        IUserRepository _repository;
        public UserService() {

        }

        public UserService(IUserRepository repository) {
            _repository = repository;
        }
        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns></returns>
        public List<User> GetUsers() {
            return _repository.GetUsers().ToList();
        }
        /// <summary>
        /// Gets a user by userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName) {
            User result= _repository.GetUsers()
                .WithUserName(userName)
                .SingleOrDefault();

            if (result == null) {
                //this user is a guest, so set Anonymous settings for them
                result = new User("Guest","",userName,"anon@nothing.com");
                result.Cart = new ShoppingCart(userName);
            }
            return result;

        }

        /// <summary>
        /// Log's the user in
        /// </summary>
        public bool Login(string userName, string password) {

            //validate
            if(String.IsNullOrEmpty(userName))
                throw new InvalidOperationException("Need a user name");

            if (String.IsNullOrEmpty(password))
                throw new InvalidOperationException("Need a password");

            return _repository.LoginUser(userName, password);
        }

        /// <summary>
        /// Deletes a user from the system
        /// </summary>
        public bool DeleteUser(string userName) {
            return _repository.DeleteUser(userName);
        }

        public bool Register(User newUser, string password, 
            string confirmPassword, string question, string answer) {


            //null/empty checks
            if (String.IsNullOrEmpty(password) || string.IsNullOrEmpty(question)
                || string.IsNullOrEmpty(answer))
                throw new InvalidOperationException("Please provide all information needed");

            //password check
            if(!password.Equals(confirmPassword,StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException("Passwords do not match");


            //see if the user is in the system already
            User userCheck = GetUser(newUser.UserName);
            
            if (userCheck != null)
                throw new InvalidOperationException("This user already exists");

            //see if the email is in the system
            userCheck = _repository.GetUsers().WithEmail(newUser.Email).SingleOrDefault();
            
            if (userCheck != null)
                throw new InvalidOperationException("This email is already in our system");


            _repository.RegisterUser(newUser.FirstName, newUser.LastName, newUser.Email,
                newUser.UserName, password, question, answer);

            return true;

        }

    }
}
