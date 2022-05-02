using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class AllProductsDiscount : PercentageDiscount
    {
        public AllProductsDiscount(double percentage_to_subtract, DateTime expiration) : base(percentage_to_subtract, expiration){}

        public AllProductsDiscount(double percentage_to_subtract, DiscountCondition _condition, DateTime expiration) : base(percentage_to_subtract, _condition, expiration){}

        public override string GetDiscountString(ShoppingCart cart)
        {
            return PercentageToSubtract + "% off all products." +
                "\n\n" + ExpirationToString() +
                "\n\n" + ConditionToString();
        }

        public override double GetTotalDiscount(ShoppingCart cart)
        {
            if (!CheckCondition() || GetExpirationDate(cart) < DateTime.Now)
                return 0;
            return cart.GetTotalPrice() * (100 - PercentageToSubtract) / 100;
        }
    }
}
