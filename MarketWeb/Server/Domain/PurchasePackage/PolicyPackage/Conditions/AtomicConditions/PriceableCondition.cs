using System;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class PriceableCondition : SearchablePriceableCondition
    {
        public PriceableCondition(string keyWord, int minAmount, int maxAmount, bool negative) : base(keyWord, minAmount, maxAmount, negative)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return checkNegative(IsInRange(searchablePriceable.GetTotalPrice(), _minValue, _maxValue));
        }

        public override String GetConditionString(int indent)
        {
            return $"The total cost is between {_minValue} and {_maxValue}.";
        }
    }
}
