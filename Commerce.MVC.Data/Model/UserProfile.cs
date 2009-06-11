using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data
{
    public class UserProfile
    {
        public string UserName { get; set; }
        public LazyList<Address> AddressBook { get; set; }
        public LazyList<Product> LastProductsViewed { get; set; }
        public LazyList<Category> LastCategoriesViewed { get; set; }
        public LazyList<Product> Recommended { get; set; }
        public Category FavoriteCategory { get; set; }
        public Order CurrentOrder { get; set; }

        public Address DefaultAddress
        {
            get
            {
                return AddressBook.Where(x => x.IsDefault)
                    .SingleOrDefault();
            }
        }

        public string FullName
        {
            get
            {
                string result = UserName;
                if (DefaultAddress!=null){
                    result=DefaultAddress.FullName;
                }
                return result;
            }
        }

        public UserProfile(string userName)
        {
            LastProductsViewed = new LazyList<Product>();
            LastCategoriesViewed = new LazyList<Category>();
            Recommended = new LazyList<Product>();
            AddressBook=new LazyList<Address>();
            this.UserName = userName;
        }

    }
}
