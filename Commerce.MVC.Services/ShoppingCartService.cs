using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.MVC.Data;

namespace Commerce.MVC.Services {
    public class ShoppingCartService : Commerce.MVC.Services.IShoppingCartService {

        IShoppingCartRepository _repository;
        
        public ShoppingCartService(IShoppingCartRepository repository) {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a Shopping Cart for a user from the Repository
        /// </summary>
        /// <param name="userName">The user's username</param>
        /// <returns></returns>
        public ShoppingCart GetCart(string userName) {
            ShoppingCart result = null;

            result = _repository.GetCarts().ForUser(userName).SingleOrDefault();

            if (result == null) {
                //create a new one for the user
                result = new ShoppingCart(userName);
                result.ID = Guid.NewGuid();
                result.CreatedOn = DateTime.Now;
                result.ModifiedOn = DateTime.Now;
                result.Items = new List<ShoppingCartItem>();

            } else {
                //pull the items
                result.Items = _repository.GetCartItems().ForCart(result).ToList();
            }
            return result;
        }

        /// <summary>
        /// Saves the cart to the selected Repository
        /// </summary>
        /// <param name="cart"></param>
        public void SaveCart(ShoppingCart cart) {
            _repository.SaveCart(cart);
        }


        /// <summary>
        /// Removes the cart from the Repository
        /// </summary>
        /// <param name="userName"></param>
        public void DeleteCart(string userName) {
             ShoppingCart cart = GetCart(userName);
             _repository.DeleteCart(cart);
        }


    }
}
