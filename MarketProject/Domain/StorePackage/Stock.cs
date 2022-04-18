using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Stock
    {
        private IDictionary<Item, int> _itemAndAmount;

        public Stock()
        {
            _itemAndAmount = new Dictionary<Item, int>();
        }

        public bool ReserveItem(Item item, int amount)
        {
            int newAmount = GetItemAmount(item) - amount;
            if ( newAmount >= 0)
            {
                _itemAndAmount[item] = newAmount;
                return true;
            }
            return false;
        }
        public Item GetItem(int itemId)
        {
            ICollection<Item> items = _itemAndAmount.Keys;
            foreach (Item item in items)
            {
                if (item.ItemID == itemId)
                    return item;

            }
            return null;
        }
        public int GetItemAmount(Item item)
        {
            return _itemAndAmount[item];
        }
 
        public bool UnreserveItem(Item item, int amount)
        {
            if (_itemAndAmount.ContainsKey(item))
            {
                _itemAndAmount[item] = _itemAndAmount[item] + amount;
                return true;
            }
            return false;   
        }
    }
}
