using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class ShoppingBasketDTO
    {
        private String _storeName;
        private IDictionary<ItemDTO, DiscountDetailsDTO> _items;
        public String StoreName => _storeName;
        public IDictionary<ItemDTO, DiscountDetailsDTO> Items => _items;
        public ShoppingBasketDTO(String storeName, Dictionary<ItemDTO, DiscountDetailsDTO> items)
        {
            _storeName = storeName;
            _items = items;
        }
        public string ToString()
        {
            //_name;
            string toString = $"Shopping Basket of store: {_storeName}:\n";
            toString += string.Format($"|{0,-30}|{1,-4}|{2,-15}|{3,-30}\n", "Item ID", "Item Category", "Item Price", "Amount");
            foreach (KeyValuePair<ItemDTO, DiscountDetailsDTO> entry in _items)
            {
                toString += string.Format($"|{0,-30}|{1,-4}|{2,-15}|{3,-30}\n",
                    entry.Key.ItemID, entry.Key.Category, entry.Key.Price, entry.Value.Amount);
            }
            return toString;
        }
    }
}
