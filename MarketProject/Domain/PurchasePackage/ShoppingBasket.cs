﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class ShoppingBasket
    {
        public virtual Store _store { get; set; }
        public virtual IDictionary<Item, int> _items { get; set; }
        public Store Store => _store;
         
        public ShoppingBasket(Store store)
        {
            _store = store;
            _items = new Dictionary<Item, int>();
        }
        public void AddItem(Item item, int amount)
        {
            if (isItemInBasket(item))
                _items[item] = _items[item] + amount;
            else _items.Add(item, amount);
        }
        public virtual int GetAmountOfItem(Item item)
        {
            if (!isItemInBasket(item))
                throw new Exception("item doesn't exist in basket");
            return _items[item];
        }
        //returns the amount that was removed
        public int RemoveItem(Item item)
        {
            if (isItemInBasket(item))
            {
                int amount = _items[item];
                _items.Remove(item);
                return amount;
            }
            throw new Exception("basket does'nt contain the item that was requested to be removed");   
        }
        public bool isItemInBasket(Item item)
        {
            return _items.ContainsKey(item);
        }

        public bool updateItemQuantity(Item item, int newQuantity)
        {
            if (isItemInBasket(item))
            {
                _items[item] = newQuantity;
                return true;
            }
            return false;
        }
        public bool IsBasketEmpty()
        {
            return _items.Count == 0;
        }
        public virtual ICollection<Item> GetItems()
        {
            return _items.Keys;
        }
    }
}
