using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Item
    {
        private ICollection<Rating> _ratings;
        private ICollection<IDiscount> _discounts;
        private int _itemID;
        public int ItemID => _itemID;

        public Item(int id)
        {
            _ratings = new List<Rating>();
            _discounts = new List<IDiscount>();
            _itemID = id;
        }
    }
}
