using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class DiscountPolicy
    {
        private ICollection<IDiscount> _discounts;

        public DiscountPolicy()
        {
            _discounts = new List<IDiscount>();
        }
    }
}
