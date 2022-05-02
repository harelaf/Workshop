using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal class PlusDiscount : ComposedDiscount
    {
        public PlusDiscount(List<Discount> discounts) : base(discounts){}

        public PlusDiscount(List<Discount> discounts, DiscountCondition condition) : base(discounts, condition){}

        public override double GetTotalDiscount(ShoppingCart cart)
        {
            double sumDis = 0;
            foreach(Discount dis in _discountList)
                sumDis += dis.GetTotalDiscount(cart);
            return sumDis;
        }

        public override String GetDiscountString(ShoppingCart cart)
        {
            String str = "";
            foreach(Discount discount in _discountList)
            {
                str += discount.GetDiscountString(cart) + "\n";
            }
            return str;
        }

        public override DateTime GetExpirationDate(ShoppingCart cart)
        {
            DateTime ans = DateTime.MinValue;
            foreach (Discount dis in _discountList)
            {
                if (dis.GetTotalDiscount(cart) > 0)
                {
                    DateTime date = dis.GetExpirationDate(cart);
                    ans = Math.Min(date.Ticks, ans.Ticks) == ans.Ticks ?
                        ans : date;
                }
            }
            return ans;
        }
    }
}
