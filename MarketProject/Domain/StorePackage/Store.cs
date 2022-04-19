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
        private DiscountPolicy _discountPolicy;
        private Queue<MessageToStore> _messagesToStore;
        private Rating _rating;
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

        public void UnReserveItem(Item item, int amountToAdd)
        {
            if (amountToAdd <= 0)
                throw new Exception("cannt unreserve item with amount<1");
            if (!_stock.UnreserveItem(item, amountToAdd))
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

        public String GetInformation()
        {
            String info = $"{_storeName}\n";
            // founder.name
            info += $"- Founded by {"founder.name"}\n";
            String ownerNames = "";
            foreach (StoreOwner owner in _owners)
            {
                //ownerNames += owner.name + ", ";
            }
            info += $"- Owners: {ownerNames}\n";
            String managerNames = "";
            foreach (StoreManager manager in _managers)
            {
                //managerNames += manager.name + ", ";
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

        public void RateStore(String username, int rating, String review)
        {
            bool result = _rating.AddRating(username, rating, review);
            if (!result)
                throw new Exception($"User {username} already rated this store.");
        }

        public void UpdateStockQuantityOfItem(int itemId, int newQuantity)
        {
            if (_stock.GetItem(itemId) == null)
                throw new Exception($"An item with ID {itemId} doesnt exist in the stock.");
            _stock.ChangeItemQuantity(itemId, newQuantity);
        }

        public void AddItemToStoreStock(int itemId, String name, double price, String description, int quantity)
        {
            if (_stock.GetItem(itemId) != null)
                throw new Exception($"An item with ID {itemId} already exists in the stock.");
            Item newItem = new Item(itemId, name, price, description);
            _stock.AddItem(newItem, quantity);
        }

        public void RemoveItemFromStore(int itemId)
        {
            if (_stock.GetItem(itemId) == null)
                throw new Exception($"An item with ID {itemId} doesnt exist in the stock.");
            _stock.RemoveItem(itemId);
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
        }
    }
}
