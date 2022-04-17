using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    abstract class User
    {
        private ShoppingCart _shoppingCart;
        public void addItemToCart(Store store, Item item, int amount)
        {
            ShoppingBasket basket= _shoppingCart.GetShoppingBasket(store);
            if (basket == null)
                basket = new ShoppingBasket(store);
            basket.addItem(item, amount);
        }
    }
}
