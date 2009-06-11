using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Commerce.Data
{
    public static class AddressFilters
    {
        public static IQueryable<Address> ForUser(this IQueryable<Address> qry, string userName)
        {
            return from a in qry
                   where
                       a.UserName.ToLower() ==
                       userName.ToLower()
                   select a;

        }
        public static IQueryable<Address> ByID(this IQueryable<Address> qry, int addressID) {
            return from a in qry
                   where
                       a.ID==addressID
                   select a;

        }
    }
}
