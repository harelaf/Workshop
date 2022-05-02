using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal abstract class ComposedDiscount : Discount
    {
        protected List<Discount> _discountList
        {
            get { return _discountList; }
            private set
            {
                if(value == null)
                    throw new ArgumentNullException("discount list must contain at least one discount.");
                _discountList = value;
            }
        }

        public ComposedDiscount(List<Discount> discounts, DiscountCondition condition) : base(condition)
        {
            _discountList = discounts;
        }

        public ComposedDiscount(List<Discount> discounts) : base()
        {
            _discountList = discounts;
        }
    }
}
