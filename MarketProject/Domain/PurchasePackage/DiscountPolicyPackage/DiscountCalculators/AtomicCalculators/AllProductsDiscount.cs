using MarketProject.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class AllProductsDiscount : PercentageDiscount
    {
        public AllProductsDiscount(double percentage_to_subtract, DateTime expiration) : base(percentage_to_subtract, expiration){}

        public AllProductsDiscount(double percentage_to_subtract, DiscountCondition _condition, DateTime expiration) : base(percentage_to_subtract, _condition, expiration){}

        public override string GetDiscountString(ISearchablePriceable searchablePriceable)
        {
            return PercentageToSubtract + "% off all products." +
                "\n\n" + ExpirationToString() +
                "\n\n" + ConditionToString();
        }

        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return searchablePriceable.GetTotalPrice() * PercentageToSubtract / 100;
        }
    }
}
