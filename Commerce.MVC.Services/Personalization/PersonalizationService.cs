using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    public class PersonalizationService:IPersonalizationService {

        IPersonalizationRepository _pRepo;
        IOrderRepository _orderRepository;
        IOrderService _orderService;
        ICatalogRepository _catalogRepository;

        public PersonalizationService(
            IPersonalizationRepository pRepo,
            IOrderRepository orderRepository,
            IOrderService orderService, 
            ICatalogRepository catalogRepository
            ) {

            _pRepo = pRepo;
            _orderRepository = orderRepository;
            _orderService = orderService;
            _catalogRepository = catalogRepository;
        }

        /// <summary>
        /// Loads the user profile
        /// </summary>
        public UserProfile GetProfile(string userName) {
            
            UserProfile profile = new UserProfile(userName);
            
            //see if they have a default address
            profile.CurrentOrder = _orderService.GetCurrentOrder(userName);

            //load the addresses
            profile.AddressBook = new LazyList<Address>(
                _orderRepository.GetAddresses().Where(x=>x.UserName==userName));

            LoadLastViewedCategories(profile);
            LoadFavoriteCategory(profile);
            LoadLastViewedProducts(profile);
            LoadRecommended(profile);

            return profile;
        }

        /// <summary>
        /// Loads the top products for this user
        /// from their favorite category
        /// that they haven't bought
        /// </summary>
        /// <param name="profile"></param>
        void LoadRecommended(UserProfile profile) {

            if (profile.FavoriteCategory != null) {
                int favCategoryID = profile.FavoriteCategory.ID;

                //define all products for a category
                var catProducts = from p in _catalogRepository.GetProducts()
                                  join pc in _catalogRepository.GetProductCategoryMap() on p.ID equals pc.ProductID
                                  where pc.CategoryID == favCategoryID
                                  select p;

                ////specify the products the user's bought
                //var boughtProducts=from o in _orderRepository.GetOrders()
                //                   join oi in _orderRepository.GetOrderItems() on o.ID equals oi.OrderID
                //                   where o.UserName==profile.UserName
                //                   select oi.Product;

                ////remove the ones that this user bought
                //var cleanedProducts = from p in catProducts
                //                      where !boughtProducts.Contains<int>(p.ID)
                //                      select p;

                var groupedProductEvents = from pe in _pRepo.GetEvents()
                                           where pe.ProductID != null
                                           group pe by pe.ProductID into grouped
                                           select new {
                                               ID = grouped.Key,
                                               Count = grouped.Count()
                                           };

                //order them by what's in the event repo
                var result = from p in catProducts
                             join pe in groupedProductEvents on p.ID equals pe.ID
                             orderby pe.Count descending
                             select p;

                //set the result
                profile.Recommended = new LazyList<Product>(result.Take(5));
            }

        }
        /// <summary>
        /// Loads the favorite categories for the user
        /// </summary>
        void LoadFavoriteCategory(UserProfile profile)
        {
            //take the top viewed
            var topCategories = from e in _pRepo.GetEvents()
                                where e.UserName == profile.UserName &&
                                (e.Behavior == UserBehavior.ViewCategory || 
                                e.Behavior == UserBehavior.ViewProduct)
                                group e by e.CategoryID into grouped
                                orderby grouped.Count() descending
                                select new
                                {
                                    ID = grouped.Key,
                                    Count = grouped.Count()
                                };

            try
            {
                profile.FavoriteCategory = (from c in _catalogRepository.GetCategories()
                                            where c.ID == topCategories.Take(1).SingleOrDefault().ID
                                            select c).SingleOrDefault();
            }
            catch
            {
            }

        }

        /// <summary>
        /// Loads the LastViewedCategories list
        /// </summary>
        /// <param name="profile"></param>
        void LoadLastViewedCategories(UserProfile profile)
        {
            //Get last viewed categories
            var viewedCategories = from e in _pRepo.GetEvents()
                                 where e.UserName == profile.UserName &&
                                 (e.Behavior == UserBehavior.ViewCategory || e.Behavior==UserBehavior.ViewProduct)
                                 orderby e.DateCreated descending
                                 select e.CategoryID;

            var lastCategories = (from c in _catalogRepository.GetCategories()
                                  where viewedCategories.Contains(c.ID)
                                  select c).Distinct().Take(5);

            profile.LastCategoriesViewed = new LazyList<Category>(lastCategories);

        }
        /// <summary>
        /// Loads the last 5 viewed products
        /// </summary>
        /// <param name="profile"></param>
        void LoadLastViewedProducts(UserProfile profile)
        {

            var lastViewed = from p in _catalogRepository.GetProducts()
                             join pe in _pRepo.GetEvents() on p.ID equals pe.ProductID
                             where pe.Behavior==UserBehavior.ViewProduct && pe.UserName==profile.UserName
                             orderby pe.DateCreated descending
                             select p;


            profile.LastProductsViewed = new LazyList<Product>(lastViewed);
        }

        /// <summary>
        /// Save's the user event to the repository
        /// </summary>
        /// <param name="userEvent"></param>
        public void SaveUserEvent(UserEvent userEvent)
        {
            //make sure there's a username
            if (String.IsNullOrEmpty(userEvent.UserName))
                throw new InvalidOperationException("Need a username to track this event");

            //and an IP
            if(String.IsNullOrEmpty(userEvent.IP))
                throw new InvalidOperationException("IP Address must be set");

            _pRepo.Save(userEvent);

        }

        public void SaveCategoryView(string userName, string IP, Category category)
        {
            //track this request
            UserEvent ue = new UserEvent(userName, IP,
                category.ID, null, System.Guid.Empty, UserBehavior.ViewCategory);
            _pRepo.Save(ue);

        }

        public void SaveProductView(string userName, string IP, Product product) {
            //track this request
            UserEvent ue = new UserEvent(userName, IP,
                null,product.ID, System.Guid.Empty, UserBehavior.ViewProduct);
            _pRepo.Save(ue);
        }
    }
}
