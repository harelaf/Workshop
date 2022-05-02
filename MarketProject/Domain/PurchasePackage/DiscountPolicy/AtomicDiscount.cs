using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal abstract class AtomicDiscount : Discount
    {
        protected AtomicDiscount(DiscountCondition condition) : base(condition)
        {
        }
    }
}
