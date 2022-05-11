using MarketProject.Domain.PurchasePackage.DiscountPackage;
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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            String errorMessage = null;
            Item item  = _stock.GetItem(itemID);
            if (item == null)
                errorMessage = "there is no such item: " + itemID + " is store";
            else if (amount <= 0)
                errorMessage = "cannt reserve item with amount<1";
            else if (!_stock.ReserveItem(item, amount))
                errorMessage = "can't reseve amount: " + amount + " of " + itemID + ".the avalable amount is: " + _stock.GetItemAmount(item);
            if (errorMessage != null)
            {
                LogErrorMessage("ReserveItem", errorMessage);
                throw new Exception(errorMessage);
            }
            return item; // else: reservation done secsussfully-> return reserved item
        }
        public Item GetItem(int itemID)
        {
            return _stock.GetItem(itemID);
        }

        public bool AddStoreManager(StoreManager newManager)
        {
            String errorMessage;
            if (!hasRoleInStore(newManager.Username))
            {
                lock (_managers)
                {
                    _managers.Add(newManager);
                }
                return true;
            }
            errorMessage = "already has a role in this store.";
            LogErrorMessage("AddStoreManager", errorMessage);
            throw new Exception(errorMessage);
        }

        public bool AddStoreOwner(StoreOwner newOwner)
        {
            String errorMessage;
            if (!hasRoleInStore(newOwner.Username))
            {
                lock (_owners)
                {
                    _owners.Add(newOwner);
                }
                return true;
            }
            errorMessage = "already has a role in this store.";
            LogErrorMessage("AddStoreOwner", errorMessage);
            throw new Exception(errorMessage);
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
            String errorMessage = null;
            if (amount_to_add <= 0)
                errorMessage = "cannt unreserve item with amount<1";
            else if (!_stock.UnreserveItem(item, amount_to_add))
                errorMessage = "can't unreserve item from that doesn't exists is store stock";
            if (errorMessage != null)
            {
                LogErrorMessage("UnReserveItem", errorMessage);
                throw new Exception(errorMessage);
            }
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
            String errorMessage;
            bool result = _rating.AddRating(Username, rating, review);
            if (!result)
            {
                errorMessage = $"Visitor {Username} already rated this store.";
                LogErrorMessage("RateStore", errorMessage);
                throw new Exception(errorMessage);
            }
        }

        public void UpdateStockQuantityOfItem(int itemId, int newQuantity)
        {
            String errorMessage;
            if (_stock.GetItem(itemId) == null)
            {
                errorMessage = $"An item with ID {itemId} doesnt exist in the stock.";
                LogErrorMessage("UpdateStockQuantityOfItem", errorMessage);
                throw new Exception(errorMessage);
            }
            _stock.ChangeItemQuantity(itemId, newQuantity);
        }

        public void AddItemToStoreStock(int itemId, String name, double price, String description, String category, int quantity)
        {
            String errorMessage;
            if (_stock.GetItem(itemId) != null)
            {
                errorMessage = $"An item with ID {itemId} already exists in the stock.";
                LogErrorMessage("AddItemToStoreStock", errorMessage);
                throw new Exception(errorMessage);
            }
            Item newItem = new Item(itemId, name, price, description, category);
            _stock.AddItem(newItem, quantity);
        }

        public void RemoveItemFromStore(int itemId)
        {
            String errorMessage;
            if (_stock.GetItem(itemId) == null)
            {
                errorMessage = $"An item with ID {itemId} doesnt exist in the stock.";
                LogErrorMessage("RemoveItemFromStore", errorMessage);
                throw new Exception(errorMessage);
            }
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

        public bool RemoveStoreOwner(string ownerUsername, String appointerUsername)
        {
            String errorMessage = null;
            StoreOwner owner = GetOwner(ownerUsername);
            if (owner == null)
                errorMessage = $"{ownerUsername} is not a owner in this store.";
            else if (!owner.isAppointer(appointerUsername))
                errorMessage = "this visitor has no permission to remove this owner.";
            else
                return _owners.Remove(owner);
            LogErrorMessage("RemoveStoreOwner", errorMessage);
            throw new Exception(errorMessage);

        }

        public bool RemoveStoreManager(string managerUsername, String appointerUsername)
        {
            String errorMessage = null;
            StoreManager manager = GetManager(managerUsername);
            if (manager == null)
                errorMessage = $"{managerUsername} is not a manager in this store.";
            else if (!manager.isAppointer(appointerUsername))
                errorMessage = "this visitor has no permission to remove this manager.";
            else
                return _managers.Remove(manager);
            LogErrorMessage("RemoveStoreManager", errorMessage);
            throw new Exception(errorMessage);
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

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in Store.{functionName}. Cause: {message}.");
        }

        public void AddDiscount(Discount discount)
        {
            lock (_discountPolicy)
            {
                _discountPolicy.AddDiscount(discount);
            }
        }

        internal MessageToStore AnswerMessage(int msgID)
        {
            foreach( MessageToStore msg in _messagesToStore)
            {
                if(msg.Id == msgID)
                {
                    _messagesToStore.Enqueue(msg);
                    return msg;
                }
            }
            return null;
        }
    }
}
