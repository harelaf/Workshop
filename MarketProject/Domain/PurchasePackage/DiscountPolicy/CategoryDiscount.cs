using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class CategoryDiscount : PercentageDiscount
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

        public override double GetTotalDiscount(ShoppingCart cart)
        {
            if (!CheckCondition() || GetExpirationDate(cart) < DateTime.Now)
                return 0;
            return cart.GetTotalPrice(Category) * (100 - PercentageToSubtract) / 100;
        }

        public override string GetDiscountString(ShoppingCart cart)
        {
            return PercentageToSubtract + "% off all " + Category + " products prices." +
                "\n\n" + ExpirationToString() +
                "\n\n" + ConditionToString();
        }
    }
}
