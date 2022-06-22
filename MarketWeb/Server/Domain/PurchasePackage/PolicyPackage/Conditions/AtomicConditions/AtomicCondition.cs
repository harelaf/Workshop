using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public abstract class AtomicCondition : Condition
    {

        protected AtomicCondition(bool toNegative) : base(toNegative)
        {
        }

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
