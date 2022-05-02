using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class SearchItemCondition : SearchableCondition
    {
        public SearchItemCondition(string keyWord, ISearchable searchable, int minAmount, int maxAmount) : base(keyWord, searchable, minAmount, maxAmount){}
        public override bool Check()
        {
            return IsInRange(_searchable.SearchItemAmount(_keyWord), _minAmount, _maxAmount);
        }
    }
}
