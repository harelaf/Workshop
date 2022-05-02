using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public class PriceableCondition : AtomicDiscountCondition
    {
        protected IPriceable _priceable;
        protected int _minPrice, _maxPrice;

        public PriceableCondition(IPriceable priceable, int minPrice, int maxPrice)
        {
            _priceable = priceable;
            _minPrice = minPrice;
            _maxPrice = maxPrice;
        }
        public override bool Check()
        {
            return IsInRange(_priceable.GetTotalPrice(), _minPrice, _maxPrice);
        }
    }
}
