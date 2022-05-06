using System;
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

        public override String GetDiscountString(int indent)
        {
            String pad = newLine(indent);
            String pad2 = newLine(indent + 1);
            String str = pad + "sum all of the following:\n";
            int index = 0;
            foreach(Discount discount in DiscountList)
            {
                str += $"{pad2}{++index}. {discount.GetDiscountString(indent + 1)}";
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
    }
}
