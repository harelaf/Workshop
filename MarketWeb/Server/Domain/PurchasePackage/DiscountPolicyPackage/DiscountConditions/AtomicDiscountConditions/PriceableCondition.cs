using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPackage
{
    public class PriceableCondition : SearchablePriceableCondition
    {
        public PriceableCondition(string keyWord, int minAmount, int maxAmount, bool negative) : base(keyWord, minAmount, maxAmount, negative)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return IsInRange(searchablePriceable.GetTotalPrice(), _minValue, _maxValue);
        }

        public override String GetConditionString(int indent)
        {
            return $"{(ToNegative ? "(NOT) " : "")}The total cost is between {_minValue} and {_maxValue}.";
        }
    }
}
