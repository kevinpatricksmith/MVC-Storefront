using System;
using System.Collections.Generic;
using Commerce.Data;



namespace Commerce.Services {
    public interface ICatalogService {
        IList<Commerce.Data.Category> GetCategories();
        Category GetCategory(int id);
        Category GetDefaultCategory();
        Product GetProduct(string productCode);
        Product GetProduct(int id);
        IList<Commerce.Data.Product> GetProductsByCategory(int categoryID);
        IList<Commerce.Data.Product> GetRelatedProducts(int productID);
        IList<Commerce.Data.Product> GetCrossProducts(int productID);
        IList<Product> GetProducts();
        void SaveProduct(Product p);
    }
}
