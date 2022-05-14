
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class ShoppingCartDTO
    {
        private ICollection<ShoppingBasketDTO> _DTObaskets; 
        public ICollection<ShoppingBasketDTO> Baskets => _DTObaskets;
        public ShoppingCartDTO(ICollection<ShoppingBasketDTO> basketDTOs)
        {
            _DTObaskets = basketDTOs;
        }
        public ShoppingBasketDTO GetBasket(String StoreName)
        {
            ShoppingBasketDTO basket = null;
            foreach(ShoppingBasketDTO b in _DTObaskets)
            {
                if(basket.StoreName == StoreName)
                {
                    basket = b;
                }
            }
            return basket;
        }
        public string ToString()
        {
            if (_DTObaskets.Count <= 0)
                return "Shopping Cart Is Empty!\n";
            string toString = "Shopping Cart Details:\n";
            foreach (ShoppingBasketDTO basket in _DTObaskets)
            {
                toString+= basket.ToString() + "\n";
            }
            return toString;
        }
        public int getAmountOfItemInCart(string storeName, int itemID)
        {
            int totalAmountInCart = 0;
            foreach (ShoppingBasketDTO shoppingBasketDTO in _DTObaskets)
            {
                if (shoppingBasketDTO.StoreName == storeName)
                {
                    foreach (ItemDTO item in shoppingBasketDTO.Items.Keys)
                    {
                        if (item.ItemID == itemID)
                        {
                            totalAmountInCart += shoppingBasketDTO.Items[item];
                            break;
                        }

                    }
                    break;
                }

            }
            return totalAmountInCart;
        }
    }
}
