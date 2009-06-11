using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    public class CatalogService : Commerce.Services.ICatalogService {


        ICatalogRepository _repository = null;
        IOrderService _orderService = null;
        /// <summary>
        /// Creates a CatalogService based on the passed-in repository
        /// </summary>
        /// <param name="repository">An ICatalogRepository</param>
        public CatalogService(ICatalogRepository repository, IOrderService orderService) {
            _repository = repository;
            _orderService = orderService;

            if (_repository == null)
                throw new InvalidOperationException("Repository cannot be null");
        }


        /// <summary>
        /// Get the categories and subcategories from the DB
        /// </summary>
        /// <returns>CategoryCollection</returns>
        public IList<Category> GetCategories() {
            IList<Category> rawCategories = _repository
                .GetCategories().ToList();

            var parents = (from c in rawCategories
                           where c.ParentID == 0
                           select c).ToList();

            parents.ForEach(p =>
            {
                p.SubCategories = (from subs in rawCategories
                                   where subs.ParentID == p.ID
                                   select subs).ToList();
            });

            return parents;        
        }

        /// <summary>
        /// Returns a single category by ID
        /// </summary>
        /// <param name="id">The Category ID</param>
        /// <returns>Category</returns>
        public Category GetCategory(int id) {
            
            Category result = _repository.GetCategories()
                .WithCategoryID(id)
                .SingleOrDefault();

            return result;

        }

        /// <summary>
        /// Returns a single category that is specified as the default
        /// </summary>
        public Category GetDefaultCategory()
        {
            Category result = _repository.GetCategories().DefaultCategory();
            return result;
        }

        /// <summary>
        /// Returns a single product by ID
        /// </summary>
        /// <param name="id">The Product ID</param>
        /// <returns></returns>
        public Product GetProduct(int id)
        {



            //Get the product from the repository
            Product p = _repository.GetProducts()
                .WithProductID(id).SingleOrDefault();


            p.Recommended = _orderService.GetRecommended(id);

            return p;

        }

        /// <summary>
        /// Returns Products for a given category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public IList<Product> GetProductsByCategory(int categoryID) {
            
            return (from p in _repository.GetProducts()
                   join cp in _repository.GetProductCategoryMap() on p.ID equals cp.ProductID
                   where cp.CategoryID == categoryID
                   select p).ToList();

        }

        /// <summary>
        /// Returns Related Products for a given Product
        /// </summary>
        public IList<Product> GetRelatedProducts(int productID) {
            return (from p in _repository.GetProducts()
                    join pr in _repository.GetRelatedProductMap() on p.ID equals pr.RelatedProductID
                    where pr.ProductID == productID
                    select p).ToList();
        }

        /// <summary>
        /// Returns Related Products for a given Product
        /// </summary>
        public IList<Product> GetCrossProducts(int productID) {
            return (from p in _repository.GetProducts()
                    join pc in _repository.GetCrossSellProductMap() on p.ID equals pc.CrossProductID
                    where pc.ProductID == productID
                    select p).ToList();
        }


        /// <summary>
        /// Returns a single product by ID
        /// </summary>
        /// <param name="productCode">The Product ID</param>
        /// <returns></returns>
        public Product GetProduct(string productCode) {

            //Get the product from the repository
            Product p = _repository.GetProducts().WithProductCode(productCode)
                .SingleOrDefault();

            int id = p == null ? 0 : p.ID;

            return GetProduct(id);

        }

        /// <summary>
        /// Returns all products as a List
        /// </summary>
        public IList<Product> GetProducts() {
            return _repository.GetProducts().ToList();
        }


        /// <summary>
        /// Saves the core product information to the DB
        /// </summary>
        /// <param name="p"></param>
        public void SaveProduct(Product p) {
            _repository.SaveProduct(p);
        }
     }

}
