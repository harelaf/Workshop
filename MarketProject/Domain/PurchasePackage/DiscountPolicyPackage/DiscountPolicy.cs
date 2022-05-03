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
    }
}
