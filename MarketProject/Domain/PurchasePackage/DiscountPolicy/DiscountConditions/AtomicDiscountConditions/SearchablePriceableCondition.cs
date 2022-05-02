using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class SearchablePriceableCondition : AtomicDiscountCondition
    {
        protected String _keyWord;
        protected int _minValue, _maxValue;
        public SearchablePriceableCondition(String keyWord, int minAmount, int maxAmount)
        {
            _keyWord = keyWord;
            _minValue = minAmount;
            _maxValue = maxAmount;
        }
    }
}
