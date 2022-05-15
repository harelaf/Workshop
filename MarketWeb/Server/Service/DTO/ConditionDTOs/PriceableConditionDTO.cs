﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class PriceableConditionDTO : IConditionDTO
    {
        private String _keyWord;
        private int _minAmount;
        private int _maxAmount;
        private bool _negative;

        public String KeyWord => _keyWord;
        public int MinAmount => _minAmount;
        public int MaxAmount => _maxAmount;
        public bool Negative => _negative;
        public PriceableConditionDTO(String keyWord, int minAmount, int maxAmount, bool negative)
        {
            _keyWord = keyWord;
            _minAmount = minAmount;
            _maxAmount = maxAmount;
            _negative = negative;
        }
    }
}