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
            if (GetItemAmount(item) < amount)
                return false;
            else
            {
                _itemAndAmount[item]= _itemAndAmount[item] -amount;
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
        public void AddItem(Item item, int amount)
        {
            _itemAndAmount.Add(item, amount);
        }
        public void UnreserveItem(Item item, int amount)
        {
            if (_itemAndAmount.ContainsKey(item))
                _itemAndAmount[item] = _itemAndAmount[item] + amount;
            else
                throw new Exception("can't unreserve item from that doesn't exists is store stock");
        }
    }
}
