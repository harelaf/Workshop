using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class ShoppingBasketDTO
    {
        private String _storeName;
        private Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> _items;
        private List<NumericDiscountDTO> _additionalDiscounts;

        public String StoreName => _storeName;
        public Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> Items => _items;
        public List<NumericDiscountDTO> AdditionalDiscounts => _additionalDiscounts;
        public ShoppingBasketDTO(String storeName, Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> items, List<NumericDiscountDTO> additionalDiscounts)
        {
            _storeName = storeName;
            _items = items;
            _additionalDiscounts = additionalDiscounts;
        }

    }
}
