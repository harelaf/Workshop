
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Shared;

namespace MarketWeb.Server.Domain
{

    public class Store
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private Stock _stock;
        private static int messageId=0;
		public Stock Stock => _stock;
        private PurchasePolicy _purchasePolicy;
        private DiscountPolicy _discountPolicy;
        private List<MessageToStore> _messagesToStore;
        public List<MessageToStore> MessagesToStore => _messagesToStore;
        private Rating _rating;
        public Rating Rating => _rating;
        private List<StoreManager> _managers;
        private List<StoreOwner> _owners;
        private StoreFounder _founder;
        private String _storeName;
        private StoreState _state;
        // bidder (if registered -> username, else -> authentication token) to bids
        private IDictionary<String, List<Bid>> _biddedItems;

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
            _messagesToStore = new List<MessageToStore>();
            _rating = new Rating();
            _managers = new List<StoreManager>();
            _owners = new List<StoreOwner>();
            _founder = founder;
            _state = StoreState.Active;
            _biddedItems = new Dictionary<String, List<Bid>>();
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
            lock (_managers)
            {
                lock (_owners)
                {
                    if (!hasRoleInStore(newManager.Username))
                    {
                        _managers.Add(newManager);
                        return true;
                    }
                }
            }
            errorMessage = "already has a role in this store.";
            LogErrorMessage("AddStoreManager", errorMessage);
            throw new Exception(errorMessage);
        }
        public bool AddStoreOwner(StoreOwner newOwner)
        {
            String errorMessage;
            lock (_managers)
            {
                lock (_owners)
                {
                    if (!hasRoleInStore(newOwner.Username))
                    {
                        _owners.Add(newOwner);
                        return true;
                    }
                }
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

        internal void AddConditionToPurchasePolicy(Condition condition)
        {
            _purchasePolicy.AddCondition(condition);
        }

        public void AddMessage(MessageToStore message)
        {

            messageId++;
            _messagesToStore.Add(message);
            message.Setid(messageId);

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

        public Tuple<List<string>, List<string>> RemoveStoreOwner(string ownerUsername, String appointerUsername)
        {
            Tuple<List<string>, List<string>> apointee_OwnersNManagers;
            String errorMessage = null;
            StoreOwner owner = GetOwner(ownerUsername);
            if (owner == null)
                errorMessage = $"{ownerUsername} is not a owner in this store.";
            else if (!owner.isAppointer(appointerUsername))
                errorMessage = "this visitor has no permission to remove this owner.";
            else
            {
                apointee_OwnersNManagers = new Tuple<List<string>, List<string>>
                    (GetAllApointeeOwners(ownerUsername), GetAllApointeeManagers(ownerUsername));
                _owners.Remove(owner);
                return apointee_OwnersNManagers;
            }
                
            LogErrorMessage("RemoveStoreOwner", errorMessage);
            throw new Exception(errorMessage);

        }
        private List<string> GetAllApointeeOwners(string appinterOwner) 
        { 
            List<string> owners = new List<string>();
            foreach (StoreOwner owner in _owners)
            {
                if (owner.isAppointer(appinterOwner))
                    owners.Add(owner.Username);
            }
            return owners;
        }
        private List<string> GetAllApointeeManagers(string appinterOwner)
        {
            List<string> managers = new List<string>();
            foreach (StoreManager manager in _managers)
            {
                if (manager.isAppointer(appinterOwner))
                    managers.Add(manager.Username);
            }
            return managers;
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

        internal List<string> GetPurchasePolicyStrings()
        {
            return _purchasePolicy.GetConditionsStrings();
        }

        internal void BidItem(int itemId, double biddedPrice, string bidder)
        {
            if (!_biddedItems.ContainsKey(bidder))
                _biddedItems.Add(bidder, new List<Bid>());
            _biddedItems[bidder].Add(new Bid(itemId, biddedPrice));
        }

        /// <summary>
        /// records that this role-holder accepted this bid
        /// </summary>
        /// <param name="acceptor"></param>
        /// <param name="itemId"></param>
        /// <param name="bidder"></param>
        /// <returns>true when all parties accepted the bid. false otherwise.</returns>
        /// <exception cref="Exception"></exception>
        internal bool AcceptBid(string acceptor, int itemId, string bidder)
        {
            Bid bid = GetBid(itemId, bidder);
            if (bid == null)
                throw new Exception("no such bid to accept.");
            bid.AcceptBid(acceptor);
            return CheckBidAcceptance(bid);
        }

        internal bool CounterOfferBid(string acceptor, int itemId, string bidder, double counterOffer)
        {
            Bid bid = GetBid(itemId, bidder);
            if (bid == null)
                throw new Exception("no such bid to counter-offer.");
            bid.CounterOfferBid(acceptor, counterOffer);
            return CheckBidAcceptance(bid);
        }

        internal void RejectBid(string rejector, int itemId, string bidder)
        {
            if(GetOwner(rejector) == null && _founder.Username != rejector)
            {
                StoreManager manager = GetManager(rejector);
                if(manager == null || !manager.hasAccess(StoreName, Operation.MANAGE_INVENTORY)) 
                {
                    throw new Exception("this visitor has no permission for this operation.");
                }
            }
            Bid bid = GetBid(itemId, bidder);
            if (bid == null)
                throw new Exception("no such bid to reject.");
            _biddedItems[bidder].Remove(bid);
            if(_biddedItems.Count == 0)
                _biddedItems.Remove(bidder);
        }

        internal Bid GetBid(int itemId, string bidder)
        {
            foreach (Bid bid in _biddedItems[bidder])
                if (bid.ItemId == itemId)
                    return bid;
            return null;
        }

        internal bool CheckBidAcceptance(Bid bid)
        {
            if (!bid.Acceptors.Contains(_founder.Username))
                return false;
            foreach (StoreOwner owner in _owners)
                if (!bid.Acceptors.Contains(owner.Username))
                    return false;
            foreach (StoreManager manger in _managers)
                if (manger.hasAccess(StoreName, Operation.STOCK_EDITOR) && !bid.Acceptors.Contains(manger.Username))
                    return false;
            return true;
        }

        internal List<string> GetDiscountPolicyStrings()
        {
            return _discountPolicy.GetDiscountsStrings();
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
                    _messagesToStore.Remove(msg);
                    return msg;
                }
            }
            return null;
        }


        public virtual DiscountPolicy GetDiscountPolicy()
        {
            return this._discountPolicy;
        }
        public virtual PurchasePolicy GetPurchasePolicy()
        {
            return this._purchasePolicy;
        }
        public void ResetDiscountPolicy()
        {
            _discountPolicy.Reset();
        }
        internal void ResetPurchasePolicy()
        {
            _purchasePolicy.Reset();
        }
    }
}
