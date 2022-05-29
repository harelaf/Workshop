using System;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public abstract class PercentageDiscount : AtomicDiscount
    {
        private double _percentage_to_subtract;
        public double PercentageToSubtract
        {
            get { return _percentage_to_subtract; }
            private set
            {
                if (_percentage_to_subtract < 0 || _percentage_to_subtract > 100)
                    throw new ArgumentException("the percentage must be non-negative and at most 100.");
                _percentage_to_subtract = value;
            }
        }

        public PercentageDiscount(double percentage_to_subtract, Condition _condition, DateTime expiration) : base(_condition, expiration)
        {
            _percentage_to_subtract = percentage_to_subtract;
        }

        public PercentageDiscount(double percentage_to_subtract, DateTime expiration) : base(expiration)
        {
            _percentage_to_subtract = percentage_to_subtract;
        }
        public override double calcPriceFromCurrPrice(double currPrice)
        {
            return currPrice * (100 - PercentageToSubtract) / 100;
        }
    }
}
