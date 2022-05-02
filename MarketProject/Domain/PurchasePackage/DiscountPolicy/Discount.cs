using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class Discount
    {
        protected DiscountCondition _condition;

        protected Discount(DiscountCondition condition)
        {
            _condition = condition;
        }

        internal abstract double GetTotalDiscount(ShoppingCart cart);
    }
}
