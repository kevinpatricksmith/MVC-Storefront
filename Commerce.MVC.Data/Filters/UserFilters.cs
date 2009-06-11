using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.MVC.Data {
    public static class UserFilters {
        /// <summary>
        /// Filters the user query by username
        /// </summary>
        public static IQueryable<User> WithUserName(this IQueryable<User> qry, string userName) {
            return from u in qry
                   where u.UserName.ToLower()==userName.ToLower()
                   select u;
        }

        /// <summary>
        /// Filters the user query by username
        /// </summary>
        public static IQueryable<User> WithEmail(this IQueryable<User> qry, string email) {
            return from u in qry
                   where u.Email.ToLower() == email.ToLower()
                   select u;
        }

    }
}
