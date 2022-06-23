using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class DiscountPolicy
    {
        private PlusDiscount _discounts;
        public PlusDiscount Discounts => _discounts;
        public DiscountPolicy()
        {
            _discounts = new PlusDiscount(new List<Discount>());
        }
        [JsonConstructor]
        public DiscountPolicy(PlusDiscount discounts)
        {
            _discounts = discounts;
        }
        public DiscountPolicy(DiscountPolicy policy)
        {
            _discounts = policy._discounts;
        }
        public void AddDiscount(Discount discount)
        {
            lock (Discounts)
            {
                Discounts.AddDiscount(discount);
            }
        }
        public virtual double calculateDiscounts(ISearchablePriceable searchablePriceable)
        {
            return Discounts.GetTotalDiscount(searchablePriceable);
        }
        public virtual double calcActualPrice(ISearchablePriceable searchablePriceable)
        {
            return Discounts.calcPriceFromCurrPrice(searchablePriceable, searchablePriceable.GetTotalPrice());
        }
        public string GetActualDiscountString(ISearchablePriceable searchablePriceable)
        {
            return Discounts.GetActualDiscountString(searchablePriceable, 0);
        }

        public void ApplyDiscounts(ISearchablePriceable searchablePriceable)
        {
            Discounts.applyDiscount(searchablePriceable);
        }

        internal void Reset()
        {
            lock (Discounts)
            {
                Discounts.DiscountList.Clear();
            }
        }

        internal List<string> GetDiscountsStrings()
        {
            List<string> discountsStrings = new List<string>();
            foreach (Discount dis in Discounts.DiscountList)
                discountsStrings.Add(dis.GetDiscountString(0));
            return discountsStrings;
        }
    }
}
