using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class SearchCategoryCondition : SearchablePriceableCondition
    {
        public SearchCategoryCondition(string keyWord, int minValue, int maxValue, bool toNegative) : base(keyWord, minValue, maxValue, toNegative) { }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            return checkNegative(IsInRange(searchablePriceable.SearchCategoryAmount(_keyWord), _minValue, _maxValue));
        }
        public override String GetConditionString(int indent)
        {
            return $"{(ToNegative ? "(NOT) " : "")}The cost of {_keyWord} products is between {_minValue} and {_maxValue}.";
        }
    }
}
