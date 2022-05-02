using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class XorComposition : ComposedDiscountCondition
    {
        public override bool Check()
        {
            bool result = false;
            bool found = false;
            foreach (DiscountCondition discountCondition in this._discountConditionList)
            {
                if (!found && discountCondition.Check())
                {
                    found = true;
                }
                if(found && discountCondition.Check())
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
