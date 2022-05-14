﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public abstract class SearchablePriceableCondition : AtomicDiscountCondition
    {
        protected String _keyWord;
        protected int _minValue, _maxValue;
        public SearchablePriceableCondition(String keyWord, int minAmount, int maxAmount, bool negative) : base(negative)
        {
            _keyWord = keyWord;
            _minValue = minAmount;
            _maxValue = maxAmount;
        }
    }
}