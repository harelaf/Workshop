using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class XorComposition : ComposedDiscountCondition
    {
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result = false;
            bool found = false;
            foreach (DiscountCondition discountCondition in this._discountConditionList)
            {
                if (!found && discountCondition.Check(searchablePriceable))
                {
                    found = true;
                }
                if(found && discountCondition.Check(searchablePriceable))
                {
                    result = false;
                    break;
                }
            }
            result = found;
            if (_toNegative)
            {
                return !result;
            }
            return result;
        }
    }
}
