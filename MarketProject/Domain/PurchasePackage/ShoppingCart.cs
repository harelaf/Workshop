using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class ShoppingCart
    {
        private ICollection<ShoppingBasket> _shoppingBaskets;

        public ShoppingCart()
        {
            _shoppingBaskets = new List<ShoppingBasket>();
        }

        public ShoppingBasket GetShoppingBasket(Store store)
        {
            foreach (ShoppingBasket basket in _shoppingBaskets)
            {
                if (basket.Store.StoreName == store.StoreName)
                    return basket;
            }
            return null;
        }
        public void AddShoppingBasket(ShoppingBasket shoppingBasket)
        {
            _shoppingBaskets.Add(shoppingBasket);
        }

    }
}
