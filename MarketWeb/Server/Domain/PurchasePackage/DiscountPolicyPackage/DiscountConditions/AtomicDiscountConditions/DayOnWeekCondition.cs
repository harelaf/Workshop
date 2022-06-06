﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPackage
{
    public class DayOnWeekCondition : AtomicDiscountCondition
    {
        protected DayOfWeek _dayOnWeek; //between 
        
        public DayOnWeekCondition(String day, bool negative) : base(negative)
        {
            _dayOnWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
        }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return DateTime.Now.DayOfWeek == _dayOnWeek;
        }
        public override String GetConditionString(int indent)
        {
            return $"{(ToNegative ? "(NOT) " : "")}Today is {_dayOnWeek}.";
        }
    }
}
