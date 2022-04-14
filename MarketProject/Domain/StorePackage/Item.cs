using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Item
    {
        private ICollection<Rating> _ratings;
        private ICollection<IDiscount> _discounts;
    }
}
