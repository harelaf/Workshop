using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class SearchItemCondition : SearchablePriceableCondition
    {
        public SearchItemCondition(string keyWord, int minAmount, int maxAmount) : base(keyWord, minAmount, maxAmount){}
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return IsInRange(searchablePriceable.SearchItemAmount(_keyWord), _minValue, _maxValue);
        }
    }
}
