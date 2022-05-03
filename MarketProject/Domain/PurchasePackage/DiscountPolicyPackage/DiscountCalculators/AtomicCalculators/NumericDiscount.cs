using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
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
        public NumericDiscount(double priceToSubtract, DiscountCondition _condition, DateTime expiration) : base(_condition, expiration)
        {
            _price_to_subtract = priceToSubtract;
        }
        public NumericDiscount(double priceToSubtract, DateTime expiration) : base(null, expiration)
        {
            _price_to_subtract = priceToSubtract;
        }
        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if(!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            return PriceToSubtract;
        }
        public override string GetDiscountString(ISearchablePriceable searchablePriceable)
        {
            return PriceToSubtract + " off total price." +
                "\n\n" + ExpirationToString() +
                "\n\n" + ConditionToString();
        }
    }
}
