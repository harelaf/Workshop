﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public abstract class Visitor
    {
        private ShoppingCart _shoppingCart;
        public ShoppingCart ShoppingCart => _shoppingCart;
        protected Visitor()
        {
            _shoppingCart = new ShoppingCart();
        }

        public void AddItemToCart(Store store, Item item, int amount)
        {
            ShoppingBasket basket= _shoppingCart.GetShoppingBasket(store.StoreName);
            if (basket == null)
            {
                basket = new ShoppingBasket(store);
                _shoppingCart.AddShoppingBasket(basket);
            }
            basket.AddItem(item, amount);
        }
        public int RemoveItemFromCart(Item item, Store store)
        {
            ShoppingBasket basket = _shoppingCart.GetShoppingBasket(store.StoreName);
            if (basket == null)
                throw new Exception("your cart doesnt contain any basket with the given store");
            int amount = basket.RemoveItem(item);
            if (basket.IsBasketEmpty())
                _shoppingCart.RemoveBasketFromCart(basket);
            return amount;
        }
        public void UpdateItemInCart(Store store, Item item, int newQuantity)
        {
            if (newQuantity <= 0)// newQuantity==0 -> remove.
                throw new Exception("can't update item quantity to a non-positive amount");
            ShoppingBasket basket = _shoppingCart.GetShoppingBasket(store.StoreName);
            if (basket == null)
                throw new Exception("your cart doesnt contain any basket with the given store");
            if (!basket.updateItemQuantity(item, newQuantity))
                throw new Exception("your cart doesn't contain the given item in the given store basket. ");
        }
        public int GetQuantityOfItemInCart(Store store, Item item)
        {
            ShoppingBasket shoppingBasket = _shoppingCart.GetShoppingBasket(store.StoreName);
            if (shoppingBasket == null)
                throw new Exception("your cart doesnt contain any basket with the given store");
            return shoppingBasket.GetAmountOfItem(item);
        }
        public ShoppingCart PurchaseMyCart(String address, String city, String country, String zip, String purchaserName)
        {
            ShoppingCart cart = _shoppingCart;
            PurchaseProcess.GetInstance().Purchase(address, city, country, zip, purchaserName, cart);
            _shoppingCart = new ShoppingCart();
            return cart;
        }


    }
}