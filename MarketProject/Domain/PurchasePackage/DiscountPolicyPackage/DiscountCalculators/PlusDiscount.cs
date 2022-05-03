﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class PlusDiscount : ComposedDiscount
    {
        public PlusDiscount(List<Discount> discounts) : base(discounts){}

        public PlusDiscount(List<Discount> discounts, DiscountCondition condition) : base(discounts, condition){}

        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            double sumDis = 0;
            foreach(Discount dis in DiscountList)
                sumDis += dis.GetTotalDiscount(searchablePriceable);
            return sumDis;
        }

        public override String GetDiscountString(ISearchablePriceable searchablePriceable)
        {
            String str = "";
            foreach(Discount discount in DiscountList)
            {
                str += discount.GetDiscountString(searchablePriceable) + "\n";
            }
            return str;
        }

        public override DateTime GetExpirationDate(ISearchablePriceable searchablePriceable)
        {
            DateTime ans = DateTime.MinValue;
            foreach (Discount dis in DiscountList)
            {
                if (dis.GetTotalDiscount(searchablePriceable) > 0)
                {
                    DateTime date = dis.GetExpirationDate(searchablePriceable);
                    ans = Math.Min(date.Ticks, ans.Ticks) == ans.Ticks ?
                        ans : date;
                }
            }
            return ans;
        }

        public void AddDiscount(Discount discount)
        {
            DiscountList.Add(discount);
        }
    }
}
