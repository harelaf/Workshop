using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public interface ISearchable
    {
        public int SearchItemAmount(String itemName);
        public int SearchCategoryAmount(String itemName);
    }
}
