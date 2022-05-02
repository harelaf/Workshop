using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class PriceableCondition : SearchablePriceableCondition
    {
        public PriceableCondition(string keyWord, int minAmount, int maxAmount) : base(keyWord, minAmount, maxAmount)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return IsInRange(searchablePriceable.GetTotalPrice(), _minValue, _maxValue);
        }
    }
}
