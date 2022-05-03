using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class CategoryDiscount : PercentageDiscount
    {
        private String _category;
        public String Category
        {
            get { return _category; }
            private set { _category = value; }
        }
        public CategoryDiscount(double percentage_to_subtract, String category, DiscountCondition _condition, DateTime expiration) : base(percentage_to_subtract, _condition, expiration)
        {
            _category = category;
        }

        public CategoryDiscount(double percentage_to_subtract, String category, DateTime expiration) : base(percentage_to_subtract, expiration)
        {
            _category = category;
        }

        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return searchablePriceable.GetCategoryPrice(Category) * PercentageToSubtract / 100;
        }

        public override string GetDiscountString(ISearchablePriceable searchablePriceable)
        {
            return PercentageToSubtract + "% off all " + Category + " products prices." +
                "\n\n" + ExpirationToString() +
                "\n\n" + ConditionToString();
        }
    }
}
