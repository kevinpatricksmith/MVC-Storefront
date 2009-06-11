using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;
using Commerce.Services;

namespace Commerce.Tests
{
    public class TestShippingRepository:IShippingRepository
    {

        public IQueryable<ShippingMethod> GetRates(Order order)
        {
            var shipping = GetShippingMethods().ToList();
            foreach (ShippingMethod m in shipping)
                m.Cost = order.TotalWeightInPounds * m.RatePerPound;

            return shipping.AsQueryable();

        }

        public IQueryable<ShippingMethod> GetShippingMethods() {
            var shipping = new List<ShippingMethod>();

            shipping.Add(new ShippingMethod("Carrier1", "Overnight", 10M, "Next Morning",1));
            shipping.Add(new ShippingMethod("Carrier1", "Next Day", 5M, "Next Day", 1));
            shipping.Add(new ShippingMethod("Carrier2", "Freight", 1M, "Whenever", 1));
            shipping.Add(new ShippingMethod("Carrier2", "Ground", 2M, "3-5 Days", 1));

            return shipping.AsQueryable();

        }
    }
}
