using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    class ShoppingBasketDTO
    {
        private String _storeName;
        private IDictionary<ItemDTO, int> _items;

        public String StoreName => _storeName;
        public IDictionary<ItemDTO, int> Items => _items;

        public ShoppingBasketDTO(ShoppingBasket shoppingBasket)
        {
            _storeName = shoppingBasket.Store.StoreName;
            _items = new Dictionary<ItemDTO, int>();

            foreach (KeyValuePair<Item, int> entry in shoppingBasket.Items)
            {
                ItemDTO dto = new ItemDTO(entry.Key);
                _items[dto] = entry.Value;
            }
        }
    }
}
