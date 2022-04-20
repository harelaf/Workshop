using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    class ShoppingCartDTO
    {
        private ICollection<ShoppingBasketDTO> _DTObaskets { get; set; }
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
    }
}
