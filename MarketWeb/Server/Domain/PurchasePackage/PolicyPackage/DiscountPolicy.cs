using System.Collections.Generic;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class DiscountPolicy
    {
        private PlusDiscount _discounts;
        public PlusDiscount Discounts => _discounts;
        public DiscountPolicy()
        {
            _discounts = new PlusDiscount(new List<Discount>());
        }
        public DiscountPolicy(DiscountPolicy policy)
        {
            _discounts = policy._discounts;
        }
        public void AddDiscount(Discount discount)
        {
            _discounts.AddDiscount(discount);
        }
        public virtual double calculateDiscounts(ISearchablePriceable searchablePriceable)
        {
            return _discounts.GetTotalDiscount(searchablePriceable);
        }
        public string GetActualDiscountString(ISearchablePriceable searchablePriceable)
        {
            return _discounts.GetActualDiscountString(searchablePriceable, 0);
        }
    }
}
