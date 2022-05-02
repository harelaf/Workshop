using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class OrComposition : ComposedDiscountCondition
    {
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result = false;
            foreach (DiscountCondition discountCondition in this._discountConditionList)
            {
                if (discountCondition.Check(searchablePriceable))
                {
                    result = true;
                    break;
                }
            }
            if (_toNegative)
            {
                return !result;
            }
            return result;
        }
    }
}
