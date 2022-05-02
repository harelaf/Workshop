using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class NumericDiscount : AtomicDiscount
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

        public override double GetTotalDiscount(ShoppingCart cart)
        {
            if(!CheckCondition() || GetExpirationDate(cart) < DateTime.Now)
                return 0;
            double totalDis = cart.GetTotalPrice() > PriceToSubtract ? PriceToSubtract : 0;
            return totalDis;
        }

        public override string GetDiscountString(ShoppingCart cart)
        {
            return PriceToSubtract + " off total price." +
                "\n\n" + ExpirationToString() +
                "\n\n" + ConditionToString();
        }
    }
}
