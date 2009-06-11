using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Linq.Expressions;

namespace Commerce.Data {
    public class TestCatalogRepository : ICatalogRepository {

        IList<Product> productList;
        IList<Category> categoryList;

        public TestCatalogRepository() {
            
            //load up the repo list
            productList = new List<Product>();
            categoryList = new List<Category>();

            for (int i = 1; i <= 5; i++) {
                Product p = new Product();
                p.Name = "Product" + i.ToString();
                p.ID = i;
                p.Price = 10M;
                p.Description = "Test Description";
                p.ProductCode = "SKU" + i.ToString();
                p.WeightInPounds = 5;


                //set first three products to shipped
                p.Delivery = i <= 3 ? DeliveryMethod.Shipped : DeliveryMethod.Download;


                //set first three products to Back-orderable
                p.AllowBackOrder = i <= 3;

                //set the 2nd product to BackOrder
                p.Inventory = i == 2 ? InventoryStatus.BackOrder : InventoryStatus.InStock;

                //set all products to taxable, except the 5th
                p.IsTaxable = i != 5;


                //add three images
                p.Images = new LazyList<ProductImage>(GetProductImages().Take(3));
                //reviews
                p.Reviews = new LazyList<ProductReview>(GetReviews().Take(5));
                p.Descriptors = new LazyList<ProductDescriptor>();

                //descriptors
                p.Descriptors.Add(new ProductDescriptor(p.ID, "Test", "Body"));
                p.Recommended = new LazyList<Product>();

                //have it recommend itself, for now
                p.Recommended.Add(p);

                //related
                p.RelatedProducts = new LazyList<Product>();
                p.RelatedProducts.Add(new Product("rel1", "test", 1, 0, 5));
                p.RelatedProducts.Add(new Product("rel1", "test", 1, 0, 5));
                p.RelatedProducts.Add(new Product("rel1", "test", 1, 0, 5));
                p.RelatedProducts.Add(new Product("rel1", "test", 1, 0, 5));

                //add some Crosses
                p.CrossSells = new LazyList<Product>();
                p.CrossSells.Add(new Product("cross1", "test", 1, 0, 5));
                p.CrossSells.Add(new Product("cross2", "test", 1, 0, 5));
                p.CrossSells.Add(new Product("cross3", "test", 1, 0, 5));
                productList.Add(p);
            }

            //categories
            for (int i = 1; i <= 10; i++)
            {

                Category c = new Category();
                c.ID = i;
                c.IsDefault = i == 1;
                c.Name = "Parent" + i.ToString();
                c.ParentID = 0;
                c.Image = new CategoryImage("thumb", "full");

                int subCategoryID = 10 * i;
                for (int x = 10; x <= 20; x++)
                {
                    Category sub = new Category();
                    sub.ID = subCategoryID;
                    sub.Name = "Sub" + x.ToString();
                    sub.ParentID = i;
                    sub.Image = new CategoryImage("thumb", "full");

                    //add some products
                    sub.Products = new LazyList<Product>();
                    for (int p = 1; p <= 5; p++)
                    {
                        sub.Products.Add(productList[p-1]);
                    }


                    categoryList.Add(sub);
                    subCategoryID++;
                }
                categoryList.Add(c);
            }


        }


        public IQueryable<Category> GetCategories() {
            return categoryList.AsQueryable<Category>();
        }

        public IQueryable<Product> GetProducts() {
            return productList.AsQueryable<Product>();
        }


        public IQueryable<ProductReview> GetReviews() {
            List<ProductReview> result = new List<ProductReview>();
            for (int i = 1; i <= 30; i++) {

                ProductReview review = new ProductReview();
                review.Author = "TestAuthor";
                review.Body = "lorem ipsum";
                review.Email = "email@nowhere.com";
                review.ProductID = i;
                review.ID = i;
                result.Add(review);
            }
            return result.AsQueryable();
        }


        public IQueryable<ProductImage> GetProductImages() {
            List<ProductImage> result = new List<ProductImage>();
                
            for (int i = 1; i <= 30; i++) {
                ProductImage img = new ProductImage();
                img.FullSizePhoto = "full" + i.ToString() + ".gif";
                img.ThumbnailPhoto = "thumb" + i.ToString() + ".gif";
                img.ID = i;
                img.ProductID = i;
                result.Add(img);

            }
            

            return result.AsQueryable<ProductImage>();

        }



        public IQueryable<ProductCategoryMap> GetProductCategoryMap() {
            List<ProductCategoryMap> result = new List<ProductCategoryMap>();
            for (int i = 1; i < 101; i++) {
                
                for (int x = 10; x < 15; x++) {
                    ProductCategoryMap map = new ProductCategoryMap();
                    map.ProductID = i;
                    map.CategoryID = x;
                    result.Add(map);
                }

            }
            return result.AsQueryable();
        }



        public IQueryable<ProductRelatedMap> GetRelatedProductMap() {
            List<ProductRelatedMap> result = new List<ProductRelatedMap>();
            for (int i = 1; i <= 5; i++) {

                for (int x = 1; x <= 5; x++) {
                    ProductRelatedMap map = new ProductRelatedMap();
                    map.ProductID = i;
                    map.RelatedProductID = x;
                    if(i!=x)
                        result.Add(map);
                }

            }
            return result.AsQueryable();
        }


        #region ICatalogRepository Members


        public IQueryable<ProductCrossSellMap> GetCrossSellProductMap() {

            List<ProductCrossSellMap> result = new List<ProductCrossSellMap>();
            for (int i = 1; i <= 5; i++) {

                for (int x = 1; x <= 5; x++) {
                    ProductCrossSellMap map = new ProductCrossSellMap();
                    map.ProductID = i;
                    map.CrossProductID = x;
                    if (i != x)
                        result.Add(map);
                }

            }
            return result.AsQueryable();

        }

        #endregion

        public void SaveProduct(Product product) {
            
            //find the Product
            Product p = productList.Where(x => x.ID == product.ID).SingleOrDefault();
            if (p != null) {
                p = product;
            } else {
                productList.Add(product);
            }


        }
    }
}
