﻿using System;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class DayOnWeekCondition : AtomicCondition
    {
        protected DayOfWeek _dayOnWeek; //between 
        
        public DayOnWeekCondition(String day, bool negative) : base(negative)
        {
            _dayOnWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day);
        }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return checkNegative(DateTime.Now.DayOfWeek == _dayOnWeek);
        }
        public override String GetConditionString(int indent)
        {
            return $"Today is {_dayOnWeek}.";
        }
    }
}