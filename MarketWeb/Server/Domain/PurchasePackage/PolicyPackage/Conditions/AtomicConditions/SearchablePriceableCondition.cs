﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public abstract class SearchablePriceableCondition : AtomicCondition
    {
        protected String _keyWord;
        public String KeyWord => _keyWord;
        protected int _minValue, _maxValue;
        public int MinValue => _minValue;
        public int MaxValue => _maxValue;
        public SearchablePriceableCondition(String keyWord, int minValue, int maxValue, bool negative) : base(negative)
        {
            _keyWord = keyWord;
            _minValue = minValue;
            _maxValue = maxValue;
        }
    }
}
