using MarketWeb.Server.Domain.PolicyPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public interface ISearchablePriceable
    {
        public int SearchItemAmount(String itemName);
        public int SearchCategoryAmount(String category);
        public double GetTotalPrice();
        public double GetItemPrice(String itemName);
        public double GetCategoryPrice(String category);
        public void SetAllProductsDiscount(AllProductsDiscount discount);
        public void SetCategoryDiscount(CategoryDiscount discount, String category);
        public void SetItemDiscount(ItemDiscount discount, String itemName);
        public void SetNumericDiscount(NumericDiscount discount);

    }
}
