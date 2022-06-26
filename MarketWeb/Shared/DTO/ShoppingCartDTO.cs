﻿
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class ShoppingCartDTO
    {
        private ICollection<ShoppingBasketDTO> _DTObaskets;
        public ICollection<ShoppingBasketDTO> Baskets => _DTObaskets;
        private int scid;
        public int Scid => scid;
        public ShoppingCartDTO(ICollection<ShoppingBasketDTO> basketDTOs, int scid)
        {
            _DTObaskets = basketDTOs == null ? new List<ShoppingBasketDTO>() : basketDTOs;
            this.scid = scid;
        }
        public ShoppingBasketDTO GetBasket(String StoreName)
        {
            ShoppingBasketDTO basket = null;
            foreach (ShoppingBasketDTO b in _DTObaskets)
            {
                if (b.StoreName == StoreName)
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
                toString += basket.ToString() + "\n";
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
                    foreach (int itemid in shoppingBasketDTO.Items.Keys)
                    {
                        if (itemid == itemID)
                        {
                            totalAmountInCart += shoppingBasketDTO.Items[itemid].Item2.Amount;
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
