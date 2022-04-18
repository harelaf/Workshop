using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Item
    {
        private Rating _rating;
        private ICollection<IDiscount> _discounts;
        private int _itemID;
        public int ItemID => _itemID;

        public Item(int id)
        {
            _rating = new Rating();
            _discounts = new List<IDiscount>();
            _itemID = id;
        }
    }
}
