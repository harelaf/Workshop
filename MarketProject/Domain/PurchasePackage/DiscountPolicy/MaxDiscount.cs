using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class MaxDiscount : ComplexDiscount
    {
        public MaxDiscount(List<Discount> _discountsList) : base(_discountsList){}
        public override Discount calcDiscount()
        {
            Discount max = new Discount(0.0);
            foreach (Discount dis in _discountList)
            {
                max = Math.Max(max.GetTotalDiscount(), dis.GetTotalDiscount()) == max.GetTotalDiscount() ? max : dis;
            }
        }
    }
}
