using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class ComposedDiscountCondition : DiscountCondition
    {
        protected List<DiscountCondition> _discountConditionList;

    }
}
