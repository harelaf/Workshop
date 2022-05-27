using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class ShoppingBasketDTO
    {
        private String _storeName;
        private Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> _items;
        public String StoreName => _storeName;
        public Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> Items => _items;
        public ShoppingBasketDTO(String storeName, Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> items)
        {
            _storeName = storeName;
            _items = items;
        }

    }
}
