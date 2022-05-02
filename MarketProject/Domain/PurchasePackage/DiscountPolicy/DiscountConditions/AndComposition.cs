using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class AndComposition : ComposedDiscountCondition
    {
        public override bool Check()
        {
            bool result = true;
            foreach (DiscountCondition discountCondition in this._discountConditionList)
            {
                if (!discountCondition.Check())
                {
                    result = false;
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
