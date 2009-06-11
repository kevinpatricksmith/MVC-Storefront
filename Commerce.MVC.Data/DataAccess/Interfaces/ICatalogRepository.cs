using System;
using System.Collections.Generic;
using System.Linq;
namespace Commerce.Data {
    
    public interface ICatalogRepository {
        
        IQueryable<Category> GetCategories();
        IQueryable<Product> GetProducts();
        IQueryable<ProductReview> GetReviews();
        IQueryable<ProductImage> GetProductImages();
        IQueryable<ProductCategoryMap> GetProductCategoryMap();
        IQueryable<ProductRelatedMap> GetRelatedProductMap();
        IQueryable<ProductCrossSellMap> GetCrossSellProductMap();

        void SaveProduct(Product product);

    }
}
