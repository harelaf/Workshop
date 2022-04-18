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
        public void UnreserveItemInStore(String storeName, Item item, int amount_to_add)
        {
            Store store = GetStore(storeName);
            store.UnReserveItem(item, amount_to_add);
        }

   
        public Boolean UpdateStockQuantityOfItem(String username, String storeName, int itemID, uint newQuantity)
        {//II.4.1
            Item item = GetItem(storeName, itemID);
            item.SetQuantity(newQuantity);
            return true;
        }
        public Boolean EditItemPrice(String username, String storeName, int itemID, float newPrice)
        {//II.4.1
            Item item = GetItem(storeName, itemID);
            item.SetPrice(newPrice);
            return true;
        }
        public Boolean EditItemName(String username, String storeName, int itemID, String newName)
        {//II.4.1
            Item item = GetItem(storeName, itemID);
            item.SetName(newName);
            return true;
        }
        public Boolean EditItemDescription(String username, String storeName, int itemID, String newDescription)
        {//II.4.1
            Item item = GetItem(storeName, itemID);
            item.SetDescription(newDescription);
            return true;
        }
    }
}
