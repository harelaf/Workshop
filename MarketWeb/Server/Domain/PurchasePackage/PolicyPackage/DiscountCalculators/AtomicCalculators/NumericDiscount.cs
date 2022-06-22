using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class NumericDiscount : AtomicDiscount
    {
        private double _price_to_subtract;
        public double PriceToSubtract
        {
            get { return _price_to_subtract; }
            private set
            {
                if (_price_to_subtract < 0)
                    throw new ArgumentException("the discount must be non-negative.");
                _price_to_subtract = value;
            }
        }
        [JsonConstructor]
        public NumericDiscount(double priceToSubtract, Condition condition, DateTime expiration) : base(condition, expiration)
        {
            _price_to_subtract = priceToSubtract;
        }
        public NumericDiscount(double priceToSubtract, DateTime expiration) : base(expiration)
        {
            _price_to_subtract = priceToSubtract;
        }
        public override void applyDiscount(ISearchablePriceable searchablePriceable)
        {
            if(!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return;
            searchablePriceable.SetNumericDiscount(this);
        }
        public override String GetDiscountString(int indent)
        {
            return PriceToSubtract + " off total price." +
                ExpirationToString(indent) +
                ConditionToString(indent);
        }

        public override String GetActualDiscountString(ISearchablePriceable searchablePriceable, int indent)
        {
            return GetDiscountString(indent);
        }
        public override double calcPriceFromCurrPrice(ISearchablePriceable searchablePriceable, double currPrice)
        {
            if (!CheckCondition(searchablePriceable))
                return currPrice;
            double actualPrice = currPrice - PriceToSubtract > 0 ? currPrice - PriceToSubtract : 0;
            return actualPrice;
        }
        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return PriceToSubtract;
        }
    }
}
