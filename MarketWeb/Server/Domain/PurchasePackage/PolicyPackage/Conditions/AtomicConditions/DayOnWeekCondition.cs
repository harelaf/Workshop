using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class DayOnWeekCondition : AtomicCondition
    {
        protected DayOfWeek _dayOnWeek; //between 
        public String DayOnWeek => _dayOnWeek.ToString();
 
        public DayOnWeekCondition(String dayOnWeek, bool toNegative) : base(toNegative)
        {
            _dayOnWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOnWeek);
        }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return checkNegative(DateTime.Now.DayOfWeek == _dayOnWeek);
        }
        public override String GetConditionString(int indent)
        {
            return $"{(ToNegative ? "(NOT) " : "")}Today is {_dayOnWeek}.";
        }
    }
}
