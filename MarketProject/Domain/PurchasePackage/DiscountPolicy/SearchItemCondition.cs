using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class SearchItemCondition : SearchableCondition
    {
        public SearchItemCondition(string keyWord, ISearchable searchable, int minAmount, int maxAmount) : base(keyWord, searchable, minAmount, maxAmount){}  
    }
}
