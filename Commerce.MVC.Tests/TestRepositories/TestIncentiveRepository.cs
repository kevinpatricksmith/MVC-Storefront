using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Tests {

    public class TestIncentiveRepository:IIncentiveRepository {

        List<Incentive> incentives = new List<Incentive>();
        
        public TestIncentiveRepository() {

            //percent off coupons
            Incentive i = new Incentive("1", new PercentOffOrderCoupon(10), DateTime.Now.AddDays(1));
            incentives.Add(i);

            i = new Incentive("2", new PercentOffOrderCoupon(20), DateTime.Now.AddDays(1));
            incentives.Add(i);

            i = new Incentive("3", new PercentOffOrderCoupon(30), DateTime.Now.AddDays(1));
            incentives.Add(i);
            
            //this will always be expired
            i = new Incentive("EXPIRED", new PercentOffOrderCoupon(30), DateTime.Now);
            incentives.Add(i);


            i = new Incentive("INVALID_MINITEMS", new PercentOffOrderCoupon(10), DateTime.Now.AddDays(1));
            i.MinimumItems = 1000;
            incentives.Add(i);

            i = new Incentive("INVALID_MINPURCHASE", new PercentOffOrderCoupon(10), DateTime.Now.AddDays(1));
            i.MininumPurchase = 10000000;
            incentives.Add(i);

            i = new Incentive("10DOLLARSOFF", new AmountOffOrderCoupon(10), DateTime.Now.AddDays(1));
            incentives.Add(i);

            i = new Incentive("8DOLLARSOFFSKU1", new AmountOffItemCoupon(8, "SKU1"), DateTime.Now.AddDays(1));
            incentives.Add(i);

            i = new Incentive("50PERCENTOFFSKU1", new PercentOffItemCoupon(50, "SKU1"), DateTime.Now.AddDays(1));
            incentives.Add(i);

        }


        public IQueryable<Incentive> GetIncentives() {
            return incentives.AsQueryable();
        }

    }
}
