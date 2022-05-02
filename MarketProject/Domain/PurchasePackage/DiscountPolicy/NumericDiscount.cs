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
                _price_to_subtract = value;
            }
        }


        public NumericDiscount(double priceToSubtract, DiscountCondition _condition) : base(_condition)
        {
            _price_to_subtract = priceToSubtract;
        }

        internal override double GetTotalDiscount(ShoppingCart cart)
        {
            if(!_condition.Check())
                return 0;
            return cart.GetTotalPrice() - _price_to_subtract;
        }
    }
}
