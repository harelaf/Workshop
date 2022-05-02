using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal abstract class ComplexDiscount : Discount
    {
        protected List<Discount> _discountList;

        public ComplexDiscount(List<Discount> discounts, DiscountCondition condition) : base(condition)
        {
            _discountList = discounts;
        }

        public ComplexDiscount(List<Discount> discounts) : base(null)
        {
            _discountList = discounts;
        }

        public abstract Discount calcDiscount();
    }
}
