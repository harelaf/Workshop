using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Item
    {
        private Rating _rating;
        private ICollection<IDiscount> _discounts;
        private String _name;


        public String GetName()
        {
            return _name;
        }
    }
}
