using System;
namespace Commerce.Data {
    public interface IIncentive {
        string Code { get; set; }
        Commerce.Data.ICoupon Coupon { get; set; }
        bool Equals(object obj);
        DateTime ExpiresOn { get; set; }
        bool IsExpired { get; }
        int MinimumItems { get; set; }
        decimal MininumPurchase { get; set; }
        string[] MustHaveProducts { get; set; }
        void ValidateUse(Commerce.Data.Order order);
    }
}
