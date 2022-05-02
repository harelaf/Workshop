using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class SearchCategoryCondition : SearchablePriceableCondition
    {
        public SearchCategoryCondition(string keyWord, int minAmount, int maxAmount) : base(keyWord, minAmount, maxAmount) { }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return IsInRange(searchablePriceable.SearchCategoryAmount(_keyWord), _minValue, _maxValue);
        }
    }
}
