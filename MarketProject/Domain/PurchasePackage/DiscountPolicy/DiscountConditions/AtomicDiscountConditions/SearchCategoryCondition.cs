using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class SearchCategoryCondition : SearchableCondition
    {
        public SearchCategoryCondition(string keyWord, ISearchable searchable, int minAmount, int maxAmount) : base(keyWord, searchable, minAmount, maxAmount) { }
        public override bool Check()
        {
            return IsInRange(_searchable.SearchCategoryAmount(_keyWord), _minAmount, _maxAmount);
        }
    }
}
