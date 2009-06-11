using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data.SqlRepository;
using System.Collections;

namespace Commerce.Data {
    public class SqlCatalogRepository : ICatalogRepository {

        DB _db;
        
        public SqlCatalogRepository(DB dataContext) {
            //override the current context
            //with the one passed in
            _db = dataContext;
            
        }

        private static string[] supportedLang;

        private static bool IsLanguageSupported(string languageCode)
        {
            if (supportedLang == null)
            {
                //Load the items. 
                using (DB context = new DB())
                {
                    var supportLangQuery = (from lang in context.CategoryCultureDetails
                                            select lang.Culture.LanguageCode).Distinct();
                    supportedLang = supportLangQuery.ToArray<String>();
                }
            }
            return supportedLang.Contains(languageCode); 
        }


        /// <summary>
        /// Linq To Sql implementation for Categories
        /// </summary>
        /// <returns>IQueryable of Categories</returns>
        public IQueryable<Category> GetCategories() {

            var culturedNames = from ct in _db.CategoryCultureDetails
                               where ct.Culture.LanguageCode ==             
                               System.Globalization.CultureInfo.
                               CurrentUICulture.TwoLetterISOLanguageName
                               select new
                               {
                                   ct.CategoryName,
                                   ct.CategoryID
                               };

            return from categ in _db.Categories
                   join culturedName in culturedNames 
                   on categ.CategoryID equals culturedName.CategoryID 
                   into cultureJoined
                   from cultureFinal in cultureJoined.DefaultIfEmpty() 
                   let products=GetProducts(categ.CategoryID)
                   select new Category
                   {
                       ID = categ.CategoryID,
                       Name = cultureFinal.CategoryName ?? categ.CategoryName, 
                       ParentID = categ.ParentID ?? 0,
                       IsDefault=categ.IsDefault,
                       Image = new CategoryImage(categ.ThumbUrl, categ.FullImageUrl),
                       Products = new LazyList<Product>(products)
                   };

        }

        IQueryable<Product> GetProducts(int categoryID) {
            var products = from p in GetProducts()
                           join cp in _db.Categories_Products on p.ID equals cp.ProductID
                           where cp.CategoryID == categoryID
                           select p;
            return products;
        }


        public IQueryable<ProductImage> GetImages(int productID)
        {
            return from i in GetProductImages()
                   where i.ProductID == productID
                   select i;
        }

        public IQueryable<ProductDescriptor> GetDescriptors(int productID) {
            return from pd in GetDescriptors()
                   where pd.ProductID == productID
                   select pd;
        }


        /// <summary>
        /// Linq To Sql Implementation for Products
        /// </summary>
        /// <returns></returns>
        public IQueryable<Product> GetProducts() {

            var cultureDetail = from cd in _db.ProductCultureDetails
                                where cd.Culture.LanguageCode == 
                                System.Globalization.CultureInfo.
                                CurrentUICulture.TwoLetterISOLanguageName
                                select cd;

            var result = from p in _db.Products
                         join detail in cultureDetail on p.ProductID equals detail.ProductID
                         into productCulture
                         from cultureJoined in productCulture.DefaultIfEmpty() 
                         let images = GetImages(p.ProductID)
                         let crosses = GetCrossSells(p.ProductID)
                         let related = GetRelated(p.ProductID)
                         let reviews = GetReviews(p.ProductID)
                         let descriptors=GetDescriptors(p.ProductID)
                         select new Product
                                    {
                                        ID = p.ProductID,
                                        Name = p.ProductName,
                                        Description = cultureJoined.Description ?? p.Description,
                                        ShortDescription = cultureJoined.ShortDescription ?? p.ShortDescription ,
                                        Price = cultureJoined.UnitPrice ?? p.BaseUnitPrice,
                                        Manufacturer = p.Manufacturer,
                                        ProductCode = p.ProductCode,
                                        Images = new LazyList<ProductImage>(images),
                                        CrossSells = new LazyList<Product>(crosses),
                                        RelatedProducts = new LazyList<Product>(related),
                                        Reviews = new LazyList<ProductReview>(reviews),
                                        Descriptors = new LazyList<ProductDescriptor>(descriptors),
                                        Delivery = (DeliveryMethod)p.DeliveryMethodID,
                                        Inventory=(InventoryStatus) p.InventoryStatusID,
                                        EstimatedDelivery=p.EstimatedDelivery,
                                        AllowBackOrder=p.AllowBackOrder,
                                        WeightInPounds=p.WeightInPounds,
                                        IsTaxable=p.IsTaxable
                         };


            return result;


        }

        IQueryable<ProductReview> GetReviews(int productID)
        {

            return from r in GetReviews()
                   where r.ProductID == productID
                   select r;
        }

        IQueryable<Product> GetCrossSells(int productID)
        {

            var crossedProducts = from cp in _db.Products_CrossSells
                                  where cp.ProductID == productID
                                  select cp.CrossProductID;

            return from c in GetProducts()
                   where crossedProducts.Contains(c.ID)
                   select c;

        }

        IQueryable<Product> GetRelated(int productID) {

            var related = from cp in _db.Products_Relateds
                                  where cp.ProductID == productID
                                  select cp.RelatedProductID;

            return from c in GetProducts()
                   where related.Contains(c.ID)
                   select c;

        }

        public IQueryable<ProductDescriptor> GetDescriptors() {
            return from pd in _db.ProductDescriptors
                   select new ProductDescriptor
                   {
                       ID = pd.DescriptorID,
                       Body = pd.Body,
                       Title = pd.Title,
                       ProductID = pd.ProductID
                   };
        }
        /// <summary>
        /// LInq To Sql implementation for Product Reviews
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductReview> GetReviews() {


            return from rv in _db.ProductReviews
                   select new ProductReview
                   {
                       ID = rv.ProductReviewID,
                       Author = rv.Author,
                       Body = rv.Body,
                       CreatedOn = rv.ReviewDate,
                       Email = rv.Email,
                       ProductID = rv.ProductID
                   };


        }

        /// <summary>
        /// Linq To Sql implementation for Images
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductImage> GetProductImages() {

            return from i in _db.ProductImages
                   select new ProductImage
                   {
                       ID = i.ProductImageID,
                       ProductID = i.ProductID,
                       ThumbnailPhoto = i.ThumbUrl,
                       FullSizePhoto = i.FullImageUrl
                   };
        }

        /// <summary>
        /// Gets the relationships from the DB for Products/Categories
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductCategoryMap> GetProductCategoryMap() {
            return from pc in _db.Categories_Products
                   select new ProductCategoryMap
                   {
                       ProductID = pc.ProductID,
                       CategoryID = pc.CategoryID
                   };
        }



        /// <summary>
        /// Gets the relationships from the DB for Products/Related Products
        /// </summary>
        public IQueryable<ProductRelatedMap> GetRelatedProductMap() {

            return from pr in _db.Products_Relateds
                   select new ProductRelatedMap
                   {
                       ProductID = pr.ProductID,
                       RelatedProductID = pr.RelatedProductID
                   };

        }




        /// <summary>
        /// Gets the relationships from the DB for Products/Cross-Sell Products
        /// </summary>
        public IQueryable<ProductCrossSellMap> GetCrossSellProductMap() {

            return from pc in _db.Products_CrossSells
                   select new ProductCrossSellMap
                   {
                       ProductID = pc.ProductID,
                       CrossProductID = pc.CrossProductID
                   };

        }


        public void SaveProduct(Product product) {
            
            using(DB db=new DB()){
                
                //see if the product is in the system
                Commerce.Data.SqlRepository.Product dbProduct = 
                    db.Products.Where(x => x.ProductID == product.ID).SingleOrDefault();
                bool isNew = false;
                if (dbProduct == null) {
                    dbProduct = new Commerce.Data.SqlRepository.Product();
                    isNew = true;
                }
                else {
                    //remove them for refresh
                    //wish there was a better way to do this but...
                    db.ProductDescriptors.DeleteAllOnSubmit(from pd in db.ProductDescriptors where pd.ProductID == product.ID select pd);
                }

                //add the descriptors
                foreach (ProductDescriptor pd in product.Descriptors) {
                    Commerce.Data.SqlRepository.ProductDescriptor dbPd = new Commerce.Data.SqlRepository.ProductDescriptor();
                    dbPd.ProductID = product.ID;
                    dbPd.Title = pd.Title;
                    dbPd.Body = pd.Body;
                    dbProduct.ProductDescriptors.Add(dbPd);
                }

                //some left/right
                dbProduct.AllowBackOrder = product.AllowBackOrder;
                dbProduct.BaseUnitPrice = product.Price;
                dbProduct.DeliveryMethodID = (int)product.Delivery;
                dbProduct.DiscountPercent = product.DiscountPercent;
                dbProduct.EstimatedDelivery = product.EstimatedDelivery;
                dbProduct.InventoryStatusID = (int)product.Inventory;
                dbProduct.Manufacturer = product.Manufacturer;
                dbProduct.ProductCode = product.ProductCode;
                dbProduct.ProductName = product.Name;
                dbProduct.WeightInPounds = product.WeightInPounds;
                


                if (isNew)
                    db.Products.InsertOnSubmit(dbProduct);


                db.SubmitChanges();
            
            }
        }

    }

}
