﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class PlusDiscount : ComposedDiscount
    {
        public PlusDiscount(List<Discount> discounts) : base(discounts){}

        public PlusDiscount(List<Discount> discounts, Condition condition) : base(discounts, condition){}

        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            if (!CheckCondition(searchablePriceable) || GetExpirationDate(searchablePriceable) < DateTime.Now)
                return 0;
            double totalPrice = searchablePriceable.GetTotalPrice();
            double totalPriceAfterDiscount = totalPrice;
            foreach(Discount dis in DiscountList)
                totalPriceAfterDiscount = dis.calcPriceFromCurrPrice(searchablePriceable, totalPriceAfterDiscount);
            return totalPrice - totalPriceAfterDiscount;
        }
        public override void applyDiscount(ISearchablePriceable searchablePriceable)
        {
            foreach(Discount dis in DiscountList)
                dis.applyDiscount(searchablePriceable);
        }
        public override String GetDiscountString(int indent)
        {
            String pad = newLine(indent);
            String pad2 = newLine(indent + 1);
            String str = pad + "sum all of the following:";
            int index = 0;
            foreach(Discount discount in DiscountList)
            {
                str += $"{pad2}{++index}. {discount.GetDiscountString(indent + 2)}";
            }
            str += ConditionToString(indent);
            return str;
        }
        public override DateTime GetExpirationDate(ISearchablePriceable searchablePriceable)
        {
            DateTime ans = DateTime.MaxValue;
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
        public override string GetActualDiscountString(ISearchablePriceable searchablePriceable, int indent)
        {
            String pad2 = newLine(indent + 1);
            String str = "sum all of the following:\n";
            int index = 0;
            foreach (Discount discount in DiscountList)
            {
                str += $"{pad2}{++index}. {discount.GetActualDiscountString(searchablePriceable, indent + 1)}";
            }
            return str;
        }
        public override double calcPriceFromCurrPrice(ISearchablePriceable searchablePriceable, double currPrice)
        {
            if (!CheckCondition(searchablePriceable))
                return currPrice;
            foreach (Discount dis in DiscountList)
                currPrice = dis.calcPriceFromCurrPrice(searchablePriceable, currPrice);
            return currPrice;
        }
    }
}
