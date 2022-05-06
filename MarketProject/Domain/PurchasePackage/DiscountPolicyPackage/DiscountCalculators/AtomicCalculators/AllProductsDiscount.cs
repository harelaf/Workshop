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

        public override string GetDiscountString(int indent)
        {
            return PercentageToSubtract + "% off all products." +
                ExpirationToString(indent) +
                ConditionToString(indent);
        }
        public override string GetActualDiscountString(ISearchablePriceable searchablePriceable, int indent)
        {
            return PercentageToSubtract + "% off all products." +
                $"{newLine(indent)}{searchablePriceable.GetTotalPrice()} - {GetTotalDiscount(searchablePriceable)} = {searchablePriceable.GetTotalPrice() - GetTotalDiscount(searchablePriceable)}" +
                ExpirationToString(indent) +
                ConditionToString(indent);
        }
        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return searchablePriceable.GetTotalPrice() * PercentageToSubtract / 100;
        }
    }
}
