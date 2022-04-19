using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Store
    {
        private Stock _stock;
        public Stock Stock => _stock;
        private PurchasePolicy _purchasePolicy;
        private ICollection<MessageToStore> _messagesToStore;
        private ICollection<Rating> _ratings;
        private ICollection<string> _managers;
        private ICollection<string> _owners;
        private string _founder;
        private string _storeName;
        public string StoreName => _storeName;

        public Store(string storeName, string storeFounder)
        {
            _stock = new Stock();
            _purchasePolicy = new PurchasePolicy();
            _managers = new HashSet<string>();
            _owners = new HashSet<string>();
            _founder = storeFounder;
            _storeName = storeName;
        }
        public Item ReserveItem(int itemID, int amount)
        {
            Item item  = _stock.GetItem(itemID);
            if (item == null)
                throw new Exception("there is no such item: " + itemID + " is store");
            if (amount <= 0)
                throw new Exception("cannt reserve item with amount<1");
            if (!_stock.ReserveItem(item, amount))
                throw new Exception("can't reseve amount: " + amount+" of " + itemID + ".the avalable amount is: " + _stock.GetItemAmount(item));
            return item; // else: reservation done secsussfully-> return reserved item
        }
        public Item GetItem(int itemID)
        {
            return _stock.GetItem(itemID);
        }

        internal bool AddStoreOwner(string ownerUserName)
        {
            if(!hasRoleInStore(ownerUserName))
            {
                _owners.Add(ownerUserName);
                return true;
            }
            return false;
        }

        internal bool AddStoreManager(string managerUsername)
        {
            if (!hasRoleInStore(managerUsername))
            {
                _owners.Add(managerUsername);
                return true;
            }
            return false;
        }

        private bool hasRoleInStore(string username)
        {
            return GetOwner(username) != null && GetOwner(username) != null && _founder.Equals(username);
        }

        private string GetManager(string ownerUserName)
        {
            foreach(string manager in _managers)
                if(manager.Equals(ownerUserName))
                    return manager;
            return null;
        }

        private object GetOwner(string managerUsername)
        {
            foreach (string owner in _owners)
                if (owner.Equals(managerUsername))
                    return owner;
            return null;
        }
        
        public void UnReserveItem(Item item, int amount_to_add)
        {
            if (amount_to_add <= 0)
                throw new Exception("cannt unreserve item with amount<1");
            if (!_stock.UnreserveItem(item, amount_to_add))
                throw new Exception("can't unreserve item from that doesn't exists is store stock");
        }


    }
}
