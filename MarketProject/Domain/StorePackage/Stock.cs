using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Stock
    {
        private IDictionary<Item, int> _itemAndAmount;

        public bool ReserveItem(Item item, int amount)
        {
            if (GetItemAmount(item) < amount)
                return false;
            else
            {
                _itemAndAmount[item]-= amount;
                return true;
            }
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
    }
}
