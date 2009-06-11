using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commerce.Data {
    public class SqlIncentiveRepository:IIncentiveRepository {


        Commerce.Data.SqlRepository.DB _db;
        public SqlIncentiveRepository() {
            _db = new Commerce.Data.SqlRepository.DB();
        }

        ICoupon GetCoupon(Commerce.Data.SqlRepository.Coupon coupon) {

            ICoupon result = null;
            switch (coupon.CouponTypeID) {

                //PercentOffOrder
                case 1:
                    result = new PercentOffOrderCoupon(coupon.PercentOff);
                    break;
                //PercentOffItem
                case 2:
                    result = new PercentOffItemCoupon(coupon.PercentOff, 
                        GetSplitArray(coupon.AppliesToProductCodes));
                    break;
                //AmountOffOrder
                case 3:
                    result = new AmountOffOrderCoupon(coupon.AmountOff);
                    break;
                //AmountOffItem
                case 4:
                    result = new AmountOffItemCoupon(coupon.AmountOff, 
                        GetSplitArray(coupon.AppliesToProductCodes));
                    break;

            }

            return result;
        }

        public IQueryable<Incentive> GetIncentives() {

            var coupons = from c in _db.Coupons
                          select new Incentive
                          {
                              Code = c.Code,
                              ExpiresOn = c.ExpiresOn,
                              MinimumItems = c.MinimumItems,
                              MininumPurchase = c.MinimumPurchase,
                              MustHaveProducts = GetSplitArray(c.MustIncludeProductCodes),
                              Coupon = GetCoupon(c)

                          };

            return coupons;

        }

        string[] GetSplitArray(string input) {
            
            if (!String.IsNullOrEmpty(input)) {
                return input.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else {
                return new string[0];
            }
        }

    }
}
