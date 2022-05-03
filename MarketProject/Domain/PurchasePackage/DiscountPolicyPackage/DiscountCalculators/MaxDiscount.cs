using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class MaxDiscount : ComposedDiscount
    {
        public MaxDiscount(List<Discount> _discountsList) : base(_discountsList){}
        public MaxDiscount(List<Discount> discounts, DiscountCondition condition) : base(discounts, condition){}

        public override string GetDiscountString(ISearchablePriceable searchablePriceable)
        {
            Discount maxDis = GetMaxDiscount(searchablePriceable);
            return maxDis.GetDiscountString(searchablePriceable);
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
            {
                totalDis = Math.Max(totalDis, dis.GetTotalDiscount(searchablePriceable));
            }
            return totalDis;
        }

        public override DateTime GetExpirationDate(ISearchablePriceable searchablePriceable)
        {
            return GetMaxDiscount(searchablePriceable).GetExpirationDate(searchablePriceable);
        }
    }
}
