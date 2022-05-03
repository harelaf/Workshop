using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class ComposedDiscountCondition : DiscountCondition
    {
        protected List<DiscountCondition> _discountConditionList;

        protected ComposedDiscountCondition(bool negative) : base(negative)
        {
            _discountConditionList = new List<DiscountCondition>();
        }

        protected ComposedDiscountCondition(bool negative, List<DiscountCondition> conditions) : base(negative)
        {
            _discountConditionList = conditions;
        }

        public void AddConditionToComposition(DiscountCondition condition)
        {
            if (condition != null)
                _discountConditionList.Add(condition);
        }
    }
}
