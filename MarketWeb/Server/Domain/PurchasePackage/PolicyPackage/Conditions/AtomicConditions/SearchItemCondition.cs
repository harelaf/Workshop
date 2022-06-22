using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class SearchItemCondition : SearchablePriceableCondition
    {
        public SearchItemCondition(string keyWord, int minValue, int maxValue, bool toNegative) : base(keyWord, minValue, maxValue, toNegative) {}
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return checkNegative(IsInRange(searchablePriceable.SearchItemAmount(_keyWord), _minValue, _maxValue));
        }
        public override String GetConditionString(int indent)
        {
            return $"{(ToNegative ? "(NOT) " : "")}The cost of {_keyWord} is between {_minValue} and {_maxValue}.";
        }
    }
}
