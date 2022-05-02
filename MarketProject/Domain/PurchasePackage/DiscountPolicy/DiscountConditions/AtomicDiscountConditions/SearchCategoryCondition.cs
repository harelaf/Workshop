﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class SearchCategoryCondition : SearchableCondition
    {
        public SearchCategoryCondition(string keyWord, ISearchable searchable, int minAmount, int maxAmount) : base(keyWord, searchable, minAmount, maxAmount) { }
        public override bool Check()
        {
            int amount = _searchable.SearchCategoryAmount(_keyWord);
            if (_maxAmount < 0)
            {
                if (_minAmount < 0)
                {
                    return false;
                }
                return amount >= _minAmount;
            }
            else if (_minAmount < 0)
            {
                return amount <= _maxAmount;
            }
            return amount >= _minAmount && amount <= _maxAmount;
        }
    }
}