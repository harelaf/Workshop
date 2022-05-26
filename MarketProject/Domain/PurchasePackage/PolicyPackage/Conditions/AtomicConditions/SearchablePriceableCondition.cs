using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public abstract class SearchablePriceableCondition : AtomicCondition
    {
        protected String _keyWord;
        protected int _minValue, _maxValue;
        public SearchablePriceableCondition(String keyWord, int minValue, int maxValue, bool negative) : base(negative)
        {
            _keyWord = keyWord;
            _minValue = minValue;
            _maxValue = maxValue;
        }
    }
}
