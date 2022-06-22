using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class PriceableCondition : SearchablePriceableCondition
    {
        public PriceableCondition(string keyWord, int minValue, int maxValue, bool toNegative) : base(keyWord, minValue, maxValue, toNegative)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return checkNegative(IsInRange(searchablePriceable.GetTotalPrice(), _minValue, _maxValue));
        }

        public override String GetConditionString(int indent)
        {
            return $"{(ToNegative ? "(NOT) " : "")}The total cost is between {_minValue} and {_maxValue}.";
        }
    }
}
