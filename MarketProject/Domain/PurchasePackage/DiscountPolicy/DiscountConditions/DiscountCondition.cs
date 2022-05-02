using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class DiscountCondition
    {
        protected bool _toNegative;

        public void ToNegative() { _toNegative = true; }
        public abstract bool Check();
    }
}
