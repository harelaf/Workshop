using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class ShoppingCartDTO
    {
        private ICollection<ShoppingBasketDTO> _DTObaskets; 
        public ICollection<ShoppingBasketDTO> Baskets => _DTObaskets;
        public ShoppingCartDTO(ShoppingCart shoppingCart)
        {
            _DTObaskets = new List<ShoppingBasketDTO>();
            foreach (ShoppingBasket basket in shoppingCart._shoppingBaskets)
            {
                _DTObaskets.Add(new ShoppingBasketDTO(basket));
            }
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
