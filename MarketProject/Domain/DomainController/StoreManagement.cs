using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class StoreManagement
    {
        private ICollection<Store> _stores;

        public Store GetStore(String storeName)
        {
            foreach (Store store in _stores)
            {
                if (store.StoreName == storeName)
                    return store;
            }
            return null;
        }
        public bool IsStoreExist(String storeName)
        {
           return GetStore(storeName) != null;
        }
        public Item ReserveItemFromStore(String storeName, int itemID, int amount)
        {
            Store store = GetStore(storeName);
            return store.ReserveItem(itemID, amount);
        }
        public Item GetItem(String storeName, int itemID)
        {
            Store store = GetStore(storeName);
            if (store == null)
                throw new Exception("there is no store in system with the givn storeid");
            Item item= store.GetItem(itemID);
            if (item == null)
                throw new Exception("there is no item: "+itemID+" in the given store");
            return item;    
        }
        public void UnreserveItemInStore(String storeName, Item item, int amountToAdd)
        {
            Store store = GetStore(storeName);
            store.UnReserveItem(item, amountToAdd);
        }

        

        public void EditItemPrice(String storeName, int itemID, float newPrice)
        {
            Item item = GetItem(storeName, itemID);
            item.SetPrice(newPrice);
        }
        public void EditItemName(String storeName, int itemID, String newName)
        {
            Item item = GetItem(storeName, itemID);
            item.SetName(newName);
        }
        public void EditItemDescription(String storeName, int itemID, String newDescription)
        {
            Item item = GetItem(storeName, itemID);
            item.SetDescription(newDescription);
        }
    }
}
