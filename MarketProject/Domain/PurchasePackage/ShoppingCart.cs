using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class ShoppingCart
    {
        private ICollection<ShoppingBasket> _shoppingBaskets;

        public ShoppingBasket GetShoppingBasket(Store store)
        {
            foreach (ShoppingBasket basket in _shoppingBaskets)
            {
                if (basket.Store.StoreName == store.StoreName)
                    return basket;
            }
            return null;
        }

    }
}
