using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
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
        [JsonConstructor]
        public PercentageDiscount(double percentageToSubtract, Condition condition, DateTime expiration) : base(condition, expiration)
        {
            _percentage_to_subtract = percentageToSubtract;
        }

        public PercentageDiscount(double percentageToSubtract, DateTime expiration) : base(expiration)
        {
            _percentage_to_subtract = percentageToSubtract;
        }
        public override double calcPriceFromCurrPrice(ISearchablePriceable searchablePriceable, double currPrice)
        {
            if (!CheckCondition(searchablePriceable))
                return currPrice;
            return currPrice * (100 - PercentageToSubtract) / 100;
        }
    }
}
