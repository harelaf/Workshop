using System;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class SearchItemCondition : SearchablePriceableCondition
    {
        public SearchItemCondition(string keyWord, int minAmount, int maxAmount, bool negative) : base(keyWord, minAmount, maxAmount, negative){}
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return checkNegative(IsInRange(searchablePriceable.SearchItemAmount(_keyWord), _minValue, _maxValue));
        }
        public override String GetConditionString(int indent)
        {
            return $"The cost of {_keyWord} is between {_minValue} and {_maxValue}.";
        }
    }
}
