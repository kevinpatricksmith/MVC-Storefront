using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data
{
    public interface IShippingRepository
    {

        IQueryable<ShippingMethod> GetRates(Order order);
        IQueryable<ShippingMethod> GetShippingMethods();
    }
}
