using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public abstract class User
    {
        private ShoppingCart _shoppingCart;
        public ShoppingCart ShoppingCart => _shoppingCart;
        protected User()
        {
            _shoppingCart = new ShoppingCart();
        }

        public void addItemToCart(Store store, Item item, int amount)
        {
            ShoppingBasket basket= _shoppingCart.GetShoppingBasket(store);
            if (basket == null)
            {
                basket = new ShoppingBasket(store);
                _shoppingCart.AddShoppingBasket(basket);
            }
            basket.AddItem(item, amount);
        }
        public int RemoveItemFromCart(Item item, Store store)
        {
            ShoppingBasket basket = _shoppingCart.GetShoppingBasket(store);
            if (basket == null)
                throw new Exception("your cart doesnt contain any basket with the given store");
            int amount = basket.RemoveItem(item);
            if (amount < 0)
                throw new Exception("basket does'nt contain the item that was requested to be removed");
            return amount;
        }

    }
}
