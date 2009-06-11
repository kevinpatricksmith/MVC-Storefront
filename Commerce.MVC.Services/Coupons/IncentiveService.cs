using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    public class IncentiveService:IIncentiveService {

        IIncentiveRepository _incentiveRepository;
        public IncentiveService(IIncentiveRepository incentiveRepository) {
            _incentiveRepository = incentiveRepository;
        }

        /// <summary>
        /// Returns an Incentive (instance of a coupon with parameters)
        /// </summary>
        public IIncentive GetIncentive(string couponCode) {
            
            return (from i in _incentiveRepository.GetIncentives()
                    where i.Code == couponCode
                    select i).SingleOrDefault();
        }

        /// <summary>
        /// Processes a coupon for an order
        /// </summary>
        public void ProcessCoupon(string couponCode, Order order) {
            
            //get the incentive
            IIncentive i = GetIncentive(couponCode);

            if (i != null) {
                IIncentive check = order.IncentivesUsed.Where(x => x.Code.Equals(couponCode, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
                if (check==null) {

                    //make sure it's valid
                    //this will throw
                    //let it
                    i.ValidateUse(order);
                    i.Coupon.ApplyCoupon(order);
                    order.IncentivesUsed.Add(i);
                    order.DiscountReason = "Coupon " + couponCode + " applied";
                }
            }
        }

        

    }
}
