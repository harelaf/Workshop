using MarketProject.Domain.PurchasePackage.PolicyPackage;
using System;

namespace MarketProject.Domain
{
    public interface ISearchablePriceable
    {
        public int SearchItemAmount(String itemName);
        public int SearchCategoryAmount(String category);
        public double GetTotalPrice();
        public double GetItemPrice(String itemName);
        public double GetCategoryPrice(String category);
        public void SetAllProductsDiscount(AtomicDiscount discount);
        public void SetCategoryDiscount(AtomicDiscount discount, String category);
        public void SetItemDiscount(AtomicDiscount discount, String itemName);
        public void SetNumericDiscount(AtomicDiscount discount);

    }
}
