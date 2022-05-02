using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class DateTimeCondition : AtomicDiscountCondition
    {
        protected DateTime _now;
        
        public DateTimeCondition()
        {
            _now = DateTime.Now;
        }
    }
}
