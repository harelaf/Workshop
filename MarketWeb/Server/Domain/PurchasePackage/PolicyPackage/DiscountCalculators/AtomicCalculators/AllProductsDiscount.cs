using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class AllProductsDiscount : PercentageDiscount
    {
        public AllProductsDiscount(double percentageToSubtract, DateTime expiration) : base(percentageToSubtract, expiration){}
        [JsonConstructor]
        public AllProductsDiscount(double percentageToSubtract, Condition condition, DateTime expiration) : base(percentageToSubtract, condition, expiration){}

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
        public override void applyDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return;
            searchablePriceable.SetAllProductsDiscount(this);
        }
        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return searchablePriceable.GetTotalPrice() * PercentageToSubtract / 100;
        }
    }
}
