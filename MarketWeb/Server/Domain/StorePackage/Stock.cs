using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class Stock
    {
        private Dictionary<Item, int> _itemAndAmount;
        public Dictionary<Item, int> Items => _itemAndAmount;

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

        public void ChangeItemQuantity(int itemId, int quantity)
        {
            Item item = GetItem(itemId);
            _itemAndAmount[item] = quantity;
        }

        public int GetItemAmount(Item item)
        {
            return _itemAndAmount[item];
        }

        public void AddItem(Item item, int amount)
        {
            _itemAndAmount.Add(item, amount);
        }

        public void RemoveItem(int itemId)
        {
            Item item = GetItem(itemId);
            _itemAndAmount.Remove(item);
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

        public List<String> GetItemNames()
        {
            List<Item> keyList = new List<Item>(_itemAndAmount.Keys);
            List<String> names = new List<String>();

            foreach (Item item in keyList)
            {
                names.Add(item.Name);
            }

            return names;
        }

        public List<Item> GetItemsByName(String itemName)
        {
            List<Item> items = new List<Item>();
            foreach(Item item in _itemAndAmount.Keys)
            {
                if (item.Name.Contains(itemName))
                {
                    items.Add(item);
                }
            }
            return items;
        }
    }
}
