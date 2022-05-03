using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class AndComposition : ComposedDiscountCondition
    {
        public AndComposition(bool negative) : base(negative)
        {
        }

        public AndComposition(bool negative, List<DiscountCondition> conditions) : base(negative, conditions)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result = true;
            foreach (DiscountCondition discountCondition in _discountConditionList)
            {
                if (!discountCondition.Check(searchablePriceable))
                {
                    result = false;
                    break;
                }
            }
            if (ToNegative)
            {
                return !result;
            }
            return result;
        }
    }
}
