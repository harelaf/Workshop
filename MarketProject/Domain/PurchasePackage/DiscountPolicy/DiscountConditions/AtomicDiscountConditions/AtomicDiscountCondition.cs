using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class AtomicDiscountCondition : DiscountCondition
    {
        public bool IsInRange(double value, double min, double max)
        {
            if (max < 0)
            {
                if (min < 0)
                {
                    return false;
                }
                return value >= min;
            }
            else if (min < 0)
            {
                return value <= max;
            }
            return value >= min && value <= max;
        }
    }
}
