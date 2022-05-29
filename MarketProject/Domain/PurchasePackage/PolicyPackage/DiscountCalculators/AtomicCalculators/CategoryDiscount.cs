using System;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public class CategoryDiscount : PercentageDiscount
    {
        private String _category;
        public String Category
        {
            get { return _category; }
            private set { _category = value; }
        }
        public CategoryDiscount(double percentage_to_subtract, String category, Condition _condition, DateTime expiration) : base(percentage_to_subtract, _condition, expiration)
        {
            _category = category;
        }

        public CategoryDiscount(double percentage_to_subtract, String category, DateTime expiration) : base(percentage_to_subtract, expiration)
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
