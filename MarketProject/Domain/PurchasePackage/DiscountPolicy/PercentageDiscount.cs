using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class PercentageDiscount : AtomicDiscount
    {
        private double _percentage_to_subtract;
        public double PercentageToSubtract
        {
            get { return _percentage_to_subtract; }
            private set
            {
                _percentage_to_subtract = value;
            }
        }


        public PercentageDiscount(double percentage_to_subtract, DiscountCondition _condition) : base(_condition)
        {
            _percentage_to_subtract = percentage_to_subtract;
        }

        internal override double GetTotalDiscount(ShoppingCart cart)
        {
            if(!_condition.Check())
                return 0;
            return cart.GetTotalPrice() * (100 - _percentage_to_subtract);
        }
    }
}
