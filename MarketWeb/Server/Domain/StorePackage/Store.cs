
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
		internal Stock _stock;
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
        internal StoreState _state;
        // bidder (if registered -> username, else -> authentication token) to bids
        private IDictionary<String, List<Bid>> _biddedItems;
        public IDictionary<String, List<Bid>> BiddedItems => _biddedItems;
        private Dictionary<String, List<String>> _standbyOwners;

        public String StoreName => _storeName;
        public StoreState State => _state;

        public bool isActive()
        {
            return _state == StoreState.Active;
        }

        public Store(
            Stock stock, 
            PurchasePolicy purchasePolicy, 
            DiscountPolicy discountPolicy, 
            List<MessageToStore> messagesToStore, 
            Rating rating, 
            List<StoreManager> managers, 
            List<StoreOwner> owners, 
            StoreFounder founder, 
            string storeName, 
            StoreState state, 
            IDictionary<string, List<Bid>> biddedItems, 
            Dictionary<string, List<string>> standbyOwners)
        {
            _stock = stock;
            _purchasePolicy = purchasePolicy;
            _discountPolicy = discountPolicy;
            _messagesToStore = messagesToStore;
            _rating = rating;
            _managers = managers;
            _owners = owners;
            _founder = founder;
            _storeName = storeName;
            _state = state;
            _biddedItems = biddedItems;
            _standbyOwners = standbyOwners;
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
            _standbyOwners = new Dictionary<String, List<String>>();
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

        public StoreOwner AddStoreOwner(StoreOwner newOwner)
        {
            String errorMessage;
            lock (_managers)
            {
                lock (_owners)
                {
                    if (!hasRoleInStore(newOwner.Username))
                    {
                        _owners.Add(newOwner);
                        return newOwner;
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

        internal void markAcceptedBidAsUsed(string bidder, int itemID)
        {
            Bid bid = GetBid(itemID, bidder);
            _biddedItems[bidder].Remove(bid);
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
            _messagesToStore.Add(message);
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

        internal void AddConditionToPurchasePolicy(Condition condition)
        {
            _purchasePolicy.AddCondition(condition);
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

        public List<string> GetPurchasePolicyStrings()
        {
            return _purchasePolicy.GetConditionsStrings();
        }

        public List<string> GetUsernamesWithPermission(Operation op)
        {
            List<string> usernames = new List<string>();
            if (_founder.hasAccess(StoreName, op))
                usernames.Add(_founder.Username);
            foreach(StoreOwner owner in _owners)
                if (owner.hasAccess(StoreName, op))
                    usernames.Add(owner.Username);
            foreach (StoreManager manager in _managers)
                if (manager.hasAccess(StoreName, op))
                    usernames.Add(manager.Username);
            return usernames;
        }

        public StoreOwner AcceptOwnerAppointment(string acceptor, string newOwner)
        {
            String errorMessage = null;
            lock (_standbyOwners)
            {
                if (!hasRoleInStore(newOwner))
                {
                    if (_standbyOwners.ContainsKey(newOwner))
                    {
                        if (!_standbyOwners[newOwner].Contains(acceptor))
                            _standbyOwners[newOwner].Add(acceptor);
                        else errorMessage = "you already accepted this owner";
                    }
                    else
                    {
                        _standbyOwners[newOwner] = new List<string>();
                        _standbyOwners[newOwner].Add(acceptor);
                    }
                }
                else
                {
                    errorMessage = "this visitor has a role in this store already.";
                }
            }
            if(errorMessage != null)
            {
                LogErrorMessage("AcceptOwnerAppointment", errorMessage);
                throw new Exception(errorMessage);
            }
            if (checkOwnerAcceptance(newOwner))
            {
                String appointer = _standbyOwners[newOwner][0];
                _standbyOwners.Remove(newOwner);
                return AddStoreOwner(new StoreOwner(newOwner, StoreName, appointer));
            }
            return null;
        }

        private bool checkOwnerAcceptance(string newOwner)
        {
            lock (_standbyOwners)
            {
                if (!_standbyOwners.ContainsKey(newOwner))
                    throw new Exception("this username is not in standby for ownership.");
                List<string> workers = GetUsernamesWithPermission(Operation.APPOINT_OWNER);
                foreach (string worker in workers)
                    if (!_standbyOwners[newOwner].Contains(worker))
                        return false;
            }
            return true;
        }

        public void RejectOwnerAppointment(string rejector, string newOwner)
        {
            lock (_standbyOwners)
            {
                if (_standbyOwners.ContainsKey(newOwner))
                    _standbyOwners.Remove(newOwner);
                else
                {
                    String errorMessage = $"no such owner to reject in store '{StoreName}'.";
                    LogErrorMessage("RejectOwnerAppointment", errorMessage);
                    throw new Exception(errorMessage);
                }
            }
        }

        public Dictionary<string, List<string>> GetStandbyOwnersInStore()
        {
            return _standbyOwners;
        }

        public void BidItem(int itemId, int amount, double biddedPrice, string bidder)
        {
            if (!_biddedItems.ContainsKey(bidder))
                _biddedItems.Add(bidder, new List<Bid>());
            if (GetBid(itemId, bidder) == null)
                _biddedItems[bidder].Add(new Bid(bidder, itemId, amount, biddedPrice));
            else throw new Exception("this visitor already bid this item.");
        }

        /// <summary>
        /// records that this role-holder accepted this bid
        /// </summary>
        /// <param name="acceptor"></param>
        /// <param name="itemId"></param>
        /// <param name="bidder"></param>
        /// <returns>true when all parties accepted the bid. false otherwise.</returns>
        /// <exception cref="Exception"></exception>
        public bool AcceptBid(string acceptor, int itemId, string bidder)
        {
            Bid bid = GetBid(itemId, bidder);
            if (bid == null)
                throw new Exception("no such bid to accept.");
            if (CheckBidAcceptance(bid))
                throw new Exception("this bid is already accepted.");
            bid.AcceptBid(acceptor);
            return CheckBidAcceptance(bid);
        }

        public List<Bid> GetBids()
        {
            List<Bid> bids = new List<Bid>();
            foreach (List<Bid> bidList in _biddedItems.Values)
                bids.AddRange(bidList);
            return bids;
        }

        public List<Bid> GetVisitorBids(String bidder)
        {
            if (_biddedItems.ContainsKey(bidder))
                return _biddedItems[bidder];
            else return null;
        }

        public bool CounterOfferBid(string acceptor, int itemId, string bidder, double counterOffer)
        {
            Bid bid = GetBid(itemId, bidder);
            if (bid == null)
                throw new Exception("no such bid to counter-offer.");
            lock (bid)
            {
                if (CheckBidAcceptance(bid))
                    throw new Exception("this bid is already accepted.");
                bid.CounterOfferBid(acceptor, counterOffer);
                return CheckBidAcceptance(bid);
            }
        }

        public void RejectBid(string rejector, int itemId, string bidder)
        {
            Bid bid = GetBid(itemId, bidder);
            if (bid == null)
                throw new Exception("no such bid to reject.");
            if (CheckBidAcceptance(bid))
                throw new Exception("this bid is already accepted.");
            _biddedItems[bidder].Remove(bid);
            if(_biddedItems.Count == 0)
                _biddedItems.Remove(bidder);
        }

        public Bid GetBid(int itemId, string bidder)
        {
            if(_biddedItems.ContainsKey(bidder))
                foreach (Bid bid in _biddedItems[bidder])
                    if (bid.ItemID == itemId)
                        return bid;
            return null;
        }

        public double GetBidAcceptedPrice(string bidder, int itemID, int amount)
        {
            Bid bid = GetBid(itemID, bidder);
            if (bid == null)
                throw new Exception("this bid does not exist."); 
            if (!CheckBidAcceptance(bid))
                throw new Exception("this bid is not yet accepted");
            if (bid.Amount != amount)
                throw new Exception($"the amount on wich the bid was accepted is {bid.Amount}.");
            return bid.GetFinalPrice();
        }

        public bool CheckBidAcceptance(Bid bid)
        {
            if (bid == null)
                throw new Exception("this bid does not exist.");
            if(bid.AcceptedByAll)
                return true;
            List<string> lst = GetUsernamesWithPermission(Operation.STOCK_EDITOR);
            foreach (string s in lst)
                if (!bid.Acceptors.Contains(s))
                    return false;
            bid.AcceptedByAll = true;
            return true;
        }

        public List<string> GetDiscountPolicyStrings()
        {
            return _discountPolicy.GetDiscountsStrings();
        }

        public List<StoreManager> GetManagers()
        {
            return _managers;
        }

        public List<StoreOwner> GetOwners()
        {
            return _owners;
        }

        public StoreFounder GetFounder()
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
