﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class SearchCategoryConditionDTO : IConditionDTO
    {
        private String _keyWord;
        private int _minAmount;
        private int _maxAmount;
        private bool _negative;

        public String KeyWord => _keyWord;
        public int MinAmount => _minAmount;
        public int MaxAmount => _maxAmount;
        public bool Negative => _negative;
        public int ObjType { get => 5; set { return; } }
        public SearchCategoryConditionDTO(String keyWord, int minAmount, int maxAmount, bool negative)
        {
            _keyWord = keyWord;
            _minAmount = minAmount;
            _maxAmount = maxAmount;
            _negative = negative;
        }
    }
}
