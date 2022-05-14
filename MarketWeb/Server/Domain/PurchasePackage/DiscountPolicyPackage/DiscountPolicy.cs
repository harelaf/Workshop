using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class DiscountPolicy
    {
        private PlusDiscount _discounts;

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
