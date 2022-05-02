using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class MaxDiscount : ComposedDiscount
    {
        public MaxDiscount(List<Discount> _discountsList) : base(_discountsList){}
        public MaxDiscount(List<Discount> discounts, DiscountCondition condition) : base(discounts, condition){}

        public override string GetDiscountString(ShoppingCart cart)
        {
            Discount maxDis = GetMaxDiscount(cart);
            return maxDis.GetDiscountString(cart);
        }

        private Discount GetMaxDiscount(ShoppingCart cart)
        {
            Discount maxDis = new NumericDiscount(0, DateTime.MaxValue);
            foreach (Discount dis in _discountList)
            {
                maxDis = Math.Max(maxDis.GetTotalDiscount(cart), dis.GetTotalDiscount(cart)) == maxDis.GetTotalDiscount(cart) ?
                    maxDis : dis;
            }
            return maxDis;
        }

        public override double GetTotalDiscount(ShoppingCart cart)
        {
            double totalDis = 0;
            foreach(Discount dis in _discountList)
            {
                totalDis = Math.Max(totalDis, dis.GetTotalDiscount(cart));
            }
            return totalDis;
        }

        public override DateTime GetExpirationDate(ShoppingCart cart)
        {
            return GetMaxDiscount(cart).GetExpirationDate();
        }
    }
}
