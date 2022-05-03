using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class XorComposition : ComposedDiscountCondition
    {
        public XorComposition(bool negative) : base(negative)
        {
        }

        public XorComposition(bool negative, List<DiscountCondition> conditions) : base(negative, conditions)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result;
            bool found = _discountConditionList.Count == 0;
            foreach (DiscountCondition discountCondition in this._discountConditionList)
            {
                if (discountCondition.Check(searchablePriceable))
                {
                    if(!found)
                        found = true;
                    else
                    {
                        found = false;
                        break;
                    }
                }
            }
            result = found;
            if (ToNegative)
            {
                return !result;
            }
            return result;
        }
    }
}
