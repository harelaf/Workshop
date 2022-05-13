using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class ShoppingBasketDTO
    {
        private String _storeName;
        private IDictionary<ItemDTO, int> _items;


        public String StoreName => _storeName;
        public IDictionary<ItemDTO, int> Items => _items;

        public String GetStoreName => _storeName;

        public ShoppingBasketDTO()
        {
            _storeName = "storeeeee";
            _items = new Dictionary<ItemDTO, int>();
            _items.Add(new ItemDTO("item1", 0.5), 10);
            _items.Add(new ItemDTO("item2", 10.5), 15);
            _items.Add(new ItemDTO("item3", 6.2), 32);
        }

        public string ToString()
        {
            //_name;
            string toString = $"Shopping Basket of store: {_storeName}:\n";
            toString += string.Format($"|{0,-30}|{1,-4}|{2,-15}|{3,-30}\n", "Item ID", "Item Category", "Item Price", "Amount");
            foreach (KeyValuePair<ItemDTO, int> entry in _items)
            {
                toString += string.Format($"|{0,-30}|{1,-4}|{2,-15}|{3,-30}\n",
                    entry.Key.ItemID, entry.Key.Category, entry.Key.Price, entry.Value);
            }
            return toString;
        }
    }
}
