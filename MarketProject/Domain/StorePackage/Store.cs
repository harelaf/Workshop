using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public enum StoreState
    {
        Active,
        Inactive,
        Closed
    }

    public class Store
    {
        private Stock _stock;
        public Stock Stock => _stock;
        private PurchasePolicy _purchasePolicy;
        public PurchasePolicy PurchasePolicy => _purchasePolicy;
        private DiscountPolicy _discountPolicy;
        public DiscountPolicy DiscountPolicy => _discountPolicy;
        private Queue<MessageToStore> _messagesToStore;
        public Queue<MessageToStore> MessagesToStore => _messagesToStore;
        private Rating _rating;
        public Rating Rating => _rating;
        private List<StoreManager> _managers;
        private List<StoreOwner> _owners;
        private StoreFounder _founder;
        private String _storeName;
        private StoreState _state;

        public String StoreName => _storeName;

        public StoreState State => _state;

        public bool isActive()
        {
            return _state == StoreState.Active;
        }

        public Store(String storeName, StoreFounder founder, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            _storeName = storeName;
            _stock = new Stock();
            _purchasePolicy = purchasePolicy;
            _discountPolicy = discountPolicy;
            _messagesToStore = new Queue<MessageToStore>();
            _rating = new Rating();
            _managers = new List<StoreManager>();
            _owners = new List<StoreOwner>();
            _founder = founder;
            _state = StoreState.Active;
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

        public bool AddStoreManager(StoreManager newManager)
        {
            if (!hasRoleInStore(newManager.Username))
            {
                _managers.Add(newManager);
                return true;
            }
            throw new Exception("already has a role in this store.");
        }

        public bool AddStoreOwner(StoreOwner newOwner)
        {
            if (!hasRoleInStore(newOwner.Username))
            {
                _owners.Add(newOwner);
                return true;
            }
            throw new Exception("already has a role in this store.");
        }

        private bool hasRoleInStore(string Username)
        {
            return GetOwner(Username) != null || GetManager(Username) != null || _founder.Username.Equals(Username);
        }

        private StoreManager GetManager(string ownerUsername)
        {
            foreach(StoreManager manager in _managers)
                if(manager.Username == ownerUsername)
                    return manager;
            return null;
        }

        private StoreOwner GetOwner(string managerUsername)
        {
            foreach (StoreOwner owner in _owners)
                if (owner.Username == managerUsername)
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

        public String GetName()
        {
            return _storeName;
        }

        public String GetRating()
        {
            return "" + _rating.GetRating();
        }

        public List<String> GetItemNames()
        {
            return _stock.GetItemNames();
        }

        public List<Item> getItemsByName(String itemName)
        {
            return _stock.GetItemsByName(itemName);
        }

        public List<String> GetStoreRolesByName()
        {
            List<String> names = new List<String>();
            names.Add(_founder.Username);
            foreach (StoreManager manager in _managers)
            {
                names.Add(manager.Username);
            }
            foreach (StoreOwner owner in _owners)
            {
                names.Add(owner.Username);
            }
            return names;
        }

        // Deprecated function. No longer used by GetStoreInformation.
        public String GetInformation()
        {
            String info = $"{_storeName}\n";
            info += $"- Founded by {_founder.Username}\n";
            String ownerNames = "";
            foreach (StoreOwner owner in _owners)
            {
                ownerNames += owner.Username + ", ";
            }
            info += $"- Owners: {ownerNames}\n";
            String managerNames = "";
            foreach (StoreManager manager in _managers)
            {
                managerNames += manager.Username + ", ";
            }
            info += $"- Managers: {managerNames}\n";
            info += $"- Has a rating of {GetRating()}\n";
            info += "\n";
            String itemNames = "";
            foreach (String name in GetItemNames())
            {
                itemNames += name + ", ";
            }
            info += $"List of items: {itemNames}\n";
            return info;
        }

        public void RateStore(String Username, int rating, String review)
        {
            bool result = _rating.AddRating(Username, rating, review);
            if (!result)
                throw new Exception($"Visitor {Username} already rated this store.");
        }

        public void UpdateStockQuantityOfItem(int itemId, int newQuantity)
        {
            if (_stock.GetItem(itemId) == null)
                throw new Exception($"An item with ID {itemId} doesnt exist in the stock.");
            _stock.ChangeItemQuantity(itemId, newQuantity);
        }

        public void AddItemToStoreStock(int itemId, String name, double price, String description, String category, int quantity)
        {
            if (_stock.GetItem(itemId) != null)
                throw new Exception($"An item with ID {itemId} already exists in the stock.");
            Item newItem = new Item(itemId, name, price, description, category);
            _stock.AddItem(newItem, quantity);
        }

        public void RemoveItemFromStore(int itemId)
        {
            if (_stock.GetItem(itemId) == null)
                throw new Exception($"An item with ID {itemId} doesnt exist in the stock.");
            _stock.RemoveItem(itemId);
        }

        public void AddMessage(MessageToStore message)
        {
            _messagesToStore.Enqueue(message);
        }

        public void CloseStore()
        {
            _state = StoreState.Inactive;
        }

        public void ReopenStore()
        {
            _state = StoreState.Active;
        }

        public void CloseStorePermanently()
        {
            _state = StoreState.Closed;
            _founder = null;
            _managers = new List<StoreManager>();
            _owners = new List<StoreOwner>();
        }
        public void RestockBasket(ShoppingBasket basket)
        {
            foreach (Item item in basket.GetItems())
                _stock.UnreserveItem(item, basket.GetAmountOfItem(item));
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove all of a Visitor's roles from this store.</para>
        /// </summary>
        /// <param name="Username"> The Visitor to revoke the roles of.</param>
        public void RemoveRoles(String Username)
        {
            RemoveManager(Username);
            RemoveOwner(Username);
            if (_founder.Username == Username)
            {
                // TODO: Decide what to do if founder is removed.
                return;
            }
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Removes Visitor as a manager if he is a manager.</para>
        /// </summary>
        /// <param name="Username"> The Visitor to revoke the role of.</param>
        private void RemoveManager(String Username)
        {
            StoreManager manager = GetManager(Username);
            if (manager != null)
                _managers.Remove(manager);
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Removes Visitor as an owner if he is an owner.</para>
        /// </summary>
        /// <param name="Username"> The Visitor to revoke the role of.</param>
        private void RemoveOwner(String Username)
        {
            StoreOwner owner = GetOwner(Username);
            if (owner != null)
                _owners.Remove(owner);
        }

        public bool RemoveStoreOwner(string ownerUsername)
        {
            return _owners.Remove(GetOwner(ownerUsername));
        }

        public bool RemoveStoreManager(string managerUsername)
        {
            return _managers.Remove(GetManager(managerUsername));
        }

        internal List<StoreManager> GetManagers()
        {
            return _managers;
        }

        internal List<StoreOwner> GetOwners()
        {
            return _owners;
        }

        internal StoreFounder GetFounder()
        {
            return _founder;
        }

        //public List<SystemRole> getRoles()
        //{
        //    List<SystemRole> roles = new List<SystemRole>();
        //    roles.AddRange(_managers);
        //    roles.AddRange(_owners);
        //    roles.Add(_founder);
        //    return roles;
        //}
    }
}
