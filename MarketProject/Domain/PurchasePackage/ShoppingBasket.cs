using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class ShoppingBasket
    {
        private Store _store;
        private IDictionary<Item, int> _items;
        public Store Store => _store;
         
        public ShoppingBasket(Store store)
        {
            _store = store;
            _items = new Dictionary<Item, int>();
        }
        public void AddItem(Item item, int amount)
        {
            if (isItemInBasket(item))
                _items[item] = _items[item] + amount;
            else _items.Add(item, amount);
        }
        public int GetAmountOfItem(Item item)
        {
            if (!isItemInBasket(item))
                throw new Exception("item doesn't exist in basket");
            return _items[item];
        }
        public int RemoveItem(Item item)
        {
            int amount = -1;
            if (isItemInBasket(item))
            {
                amount = _items[item];
                _items.Remove(item);
            }
            return amount;   
        }
        public bool isItemInBasket(Item item)
        {
            return _items.ContainsKey(item);
        }

    }
}
