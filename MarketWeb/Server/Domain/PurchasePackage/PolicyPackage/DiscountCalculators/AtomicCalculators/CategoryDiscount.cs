using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class CategoryDiscount : PercentageDiscount
    {
        private String _category;
        public String Category
        {
            get { return _category; }
            private set { _category = value; }
        }
        [JsonConstructor]
        public CategoryDiscount(double percentageToSubtract, String category, Condition condition, DateTime expiration) : base(percentageToSubtract, condition, expiration)
        {
            _category = category;
        }

        public CategoryDiscount(double percentageToSubtract, String category, DateTime expiration) : base(percentageToSubtract, expiration)
        {
            _category = category;
        }

        public override void applyDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return;
            searchablePriceable.SetCategoryDiscount(this, Category);
        }

        public override string GetDiscountString(int indent)
        {
            return $"{PercentageToSubtract}% off all '{Category}' products prices." +
                ExpirationToString(indent) +
                ConditionToString(indent);
        }
        public override string GetActualDiscountString(ISearchablePriceable searchablePriceable, int indent)
        {
            return $"{PercentageToSubtract}% off all '{Category}' products prices." +
                $"{newLine(indent)}{searchablePriceable.GetCategoryPrice(Category)} - {GetTotalDiscount(searchablePriceable)} = {searchablePriceable.GetCategoryPrice(Category) - GetTotalDiscount(searchablePriceable)}" +
                ExpirationToString(indent) +
                ConditionToString(indent);
        }
        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return searchablePriceable.GetCategoryPrice(Category) * PercentageToSubtract / 100;
        }
    }
}
