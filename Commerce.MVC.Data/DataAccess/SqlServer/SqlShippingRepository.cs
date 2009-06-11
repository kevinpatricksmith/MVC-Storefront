using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data.SqlRepository;

namespace Commerce.Data
{
    
    public class SqlShippingRepository:IShippingRepository
    {
        DB _db;
        public SqlShippingRepository(DB db)
        {
            _db = db;
        }
        public IQueryable<ShippingMethod> GetRates(Order order)
        {
            //pull the rates
            return from sm in _db.ShippingMethods
                          select new ShippingMethod
                          {
                              ID = sm.ShippingMethodID,
                              Carrier = sm.Carrier,
                              EstimatedDelivery = sm.EstimatedDelivery,
                              Cost = sm.BaseRate + (sm.RatePerPound * order.TotalWeightInPounds),
                              RatePerPound = sm.RatePerPound,
                              ServiceName = sm.ServiceName
                          };
        }
        public IQueryable<ShippingMethod> GetShippingMethods() {

            //pull the rates
            var methods = from sm in _db.ShippingMethods
                          select new ShippingMethod
                          {
                              ID = sm.ShippingMethodID,
                              Carrier = sm.Carrier,
                              EstimatedDelivery = sm.EstimatedDelivery,
                              RatePerPound = sm.RatePerPound,
                              ServiceName = sm.ServiceName,
                              DaysToDeliver=sm.DaysToDeliver
                          };

            return methods;


        }



    }
}
