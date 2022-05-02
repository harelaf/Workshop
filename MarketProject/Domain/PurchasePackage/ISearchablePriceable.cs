using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public interface ISearchablePriceable
    {
        public int SearchItemAmount(String itemName);
        public int SearchCategoryAmount(String category);
        public double GetTotalPrice();
        public double GetItemPrice(String itemName);
        public double GetCategoryPrice(String category);

    }
}
