using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy.AtomicDiscountConditions
{
    public class DayOnWeekCondition : DateTimeCondition
    {
        protected DayOfWeek _dayOnWeek; //between 
        
        public DayOnWeekCondition(String day) : base()
        {
            _dayOnWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
        }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return _now.DayOfWeek == _dayOnWeek;
        }
    }
}
