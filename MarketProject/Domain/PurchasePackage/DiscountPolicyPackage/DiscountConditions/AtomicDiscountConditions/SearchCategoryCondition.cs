using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class SearchCategoryCondition : SearchablePriceableCondition
    {
        public SearchCategoryCondition(string keyWord, int minAmount, int maxAmount, bool negative) : base(keyWord, minAmount, maxAmount, negative) { }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return IsInRange(searchablePriceable.SearchCategoryAmount(_keyWord), _minValue, _maxValue);
        }
    }
}
