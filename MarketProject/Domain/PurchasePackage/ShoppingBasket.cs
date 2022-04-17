using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class ShoppingBasket
    {
        private Store _store;
        private IDictionary<Item, int> _items;
        public Store Store => _store;
         
        public ShoppingBasket(Store store)
        {
            _store = store;
            _items = new Dictionary<Item, int>();
        }
        public void addItem(Item item, int amount)
        {
            if (_items.ContainsKey(item))
                _items[item] += amount;
            else _items.Add(item, amount);
        }


    }
}
