using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class MaxDiscount : ComposedDiscount
    {
        public MaxDiscount(List<Discount> _discountsList) : base(_discountsList){}
        public MaxDiscount(List<Discount> discounts, Condition condition) : base(discounts, condition){}
        public override string GetActualDiscountString(ISearchablePriceable searchablePriceable, int indent)
        {
            Discount maxDis = GetMaxDiscount(searchablePriceable);
            return maxDis.GetActualDiscountString(searchablePriceable, indent);
        }
        public override String GetDiscountString(int indent)
        {
            String pad2 = newLine(indent + 1);
            String str = "apply the maximal discount out of the following:\n";
            int index = 0;
            foreach (Discount discount in DiscountList)
                str += $"{pad2}{++index}. {discount.GetDiscountString(indent + 1)}";
            return str;
        }
        private Discount GetMaxDiscount(ISearchablePriceable searchablePriceable)
        {
            Discount maxDis = new NumericDiscount(0, DateTime.MaxValue);
            foreach (Discount dis in DiscountList)
            {
                maxDis = Math.Max(maxDis.GetTotalDiscount(searchablePriceable), 
                            dis.GetTotalDiscount(searchablePriceable)) 
                            == maxDis.GetTotalDiscount(searchablePriceable) ?
                            maxDis : dis;
            }
            return maxDis;
        }
        public override double GetTotalDiscount(ISearchablePriceable searchablePriceable)
        {
            double totalDis = 0;
            foreach(Discount dis in DiscountList)
                totalDis = Math.Max(totalDis, dis.GetTotalDiscount(searchablePriceable));
            return totalDis;
        }
        public override void applyDiscount(ISearchablePriceable searchablePriceable)
        {
            Discount maxDis = GetMaxDiscount(searchablePriceable);
            maxDis.applyDiscount(searchablePriceable);
        }
        public override DateTime GetExpirationDate(ISearchablePriceable searchablePriceable)
        {
            return GetMaxDiscount(searchablePriceable).GetExpirationDate(searchablePriceable);
        }
        public override double calcPriceFromCurrPrice(ISearchablePriceable searchablePriceable, double currPrice)
        {
            if (!CheckCondition(searchablePriceable))
                return currPrice;
            Discount dis = GetMaxDiscount(searchablePriceable);
            return dis.calcPriceFromCurrPrice(searchablePriceable, currPrice);
        }
    }
}
