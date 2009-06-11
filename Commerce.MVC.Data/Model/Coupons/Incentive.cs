using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Commerce.Data {

    public class Incentive : IIncentive {
        public Incentive() { }
        public Incentive(string code, ICoupon coupon, DateTime expiresOn) {
            this.Code = code;
            this.Coupon = coupon;
            this.ExpiresOn = expiresOn;
            this.MustHaveProducts = new string[0];
        }

        public string Code { get; set; }
        public DateTime ExpiresOn { get; set; }
        public ICoupon Coupon { get; set; }


        //qualifications
        public decimal MininumPurchase { get; set; }
        public int MinimumItems { get; set; }
        public string[] MustHaveProducts { get; set; }


        public bool IsExpired {
            get {

                return this.ExpiresOn <= DateTime.Now;
            }

        }

        /// <summary>
        /// Qualifies an order based on rules you set
        /// these are simple rules - override as needed
        /// </summary>
        public virtual void ValidateUse(Order order) {

            if (ExpiresOn < DateTime.Now)
                throw new InvalidOperationException("This coupon is Expired");

            if (order.SubTotal < MininumPurchase)
                throw new InvalidOperationException("There is a minimum of " + MininumPurchase.ToString("C") + " required");

            if (order.Items.Count < MinimumItems)
                throw new InvalidOperationException("There is a minimum of " + MinimumItems.ToString() + " items required");

            
            if (MustHaveProducts.Length > 0) {
                bool haveProduct=false;
                
                foreach (string productCode in MustHaveProducts) {
                    var item = order.Items.Where(x => x.Product.ProductCode == productCode).SingleOrDefault();
                    if (item != null) {
                        haveProduct = true;
                        break;
                    }

                }

                if (!haveProduct)
                    throw new InvalidOperationException("This coupon is not valid for the items you've selected");


            }


        }


        #region Object overrides
        public override bool Equals(object obj) {
            bool result = false;
            if (obj.GetType() == typeof(Incentive)) {
                Incentive compareTo = (Incentive)obj;
                result = compareTo.Code.Equals(this.Code, StringComparison.InvariantCultureIgnoreCase);
            }

            return result;
        }
        #endregion



    }
}
