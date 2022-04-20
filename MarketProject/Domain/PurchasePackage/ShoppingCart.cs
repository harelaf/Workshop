using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class ShoppingCart
    {
        public virtual ICollection<ShoppingBasket> _shoppingBaskets { get; set; }

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
        public bool isCartEmpty()
        {
            return _shoppingBaskets.Count == 0;
        }
        public void RemoveBasketFromCart(ShoppingBasket basket)
        {
            if (!isCartEmpty() && _shoppingBaskets.Contains(basket))
                _shoppingBaskets.Remove(basket);
            throw new Exception("there is no such basket in cart to remove.");
        }

        public virtual void RelaseItemsOfCart()
        {
            foreach (ShoppingBasket basket in _shoppingBaskets)
                basket.Store.RestockBasket(basket);
        }
    }
}
