using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class SearchableCondition : AtomicDiscountCondition
    {
        protected String _keyWord;
        protected ISearchable _searchable;
        protected int _minAmount, _maxAmount;
        public SearchableCondition(String keyWord, ISearchable searchable, int minAmount, int maxAmount)
        {
            _keyWord = keyWord;
            _searchable = searchable;
            _minAmount = minAmount;
            _maxAmount = maxAmount;
        }
    }
}
