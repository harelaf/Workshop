using MarketWeb.Server.Domain.PolicyPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class ShoppingBasket : ISearchablePriceable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual Store _store { get; set; }
        public virtual IDictionary<Item, DiscountDetails<AtomicDiscount>> _items { get; set; }
        public IDictionary<Item, DiscountDetails<AtomicDiscount>> Items => _items;
        private List<Bid> _biddedItems;
        public List<Bid> BiddedItems => _biddedItems;
        private DiscountDetails<NumericDiscount> _additionalDiscounts;
        public DiscountDetails<NumericDiscount> AdditionalDiscounts => _additionalDiscounts;

        public ShoppingBasket(Store store, IDictionary<Item, DiscountDetails<AtomicDiscount>> items) : this(store)
        {
            _items = items;
        }

        public ShoppingBasket(Store store)
        {
            _store = store;
            _items = new Dictionary<Item, DiscountDetails<AtomicDiscount>>();
            _additionalDiscounts = new DiscountDetails<NumericDiscount>(0);
            _biddedItems = new List<Bid>();
        }
        public void AddItem(Item item, int amount)
        {
            if (isItemInBasket(item))
            {
                //updateItemQuantity(item, amount + Items[GetItem(item.ItemID)].Amount);
                updateItemQuantity(item, amount + Items[GetItem(item.ItemID)].Amount);
                return;
            }
            else _items[GetItem(item.ItemID)] = new DiscountDetails<AtomicDiscount>(amount);
            if (!Store().GetPurchasePolicy().checkPolicyConditions(this))
            {
                _items.Remove(item);
                throw new ArgumentException("this purchase is not compatible with the store's purchase policy.");
            }
            resetDiscounts();
            Store().GetDiscountPolicy().ApplyDiscounts(this);
        }

        public virtual int GetAmountOfItem(Item item)
        {
            String errorMessage;
            int amount = Items[GetItem(item.ItemID)].Amount;
            foreach(Bid bid in BiddedItems)
            {
                if(bid.ItemID == item.ItemID)
                {
                    amount += bid.Amount;
                }
            }
            if (amount == 0)
            {
                errorMessage = "item doesn't exist in basket";
                LogErrorMessage("GetAmountOfItem", errorMessage);
                throw new Exception(errorMessage);
            }
            return amount;
        }
        public virtual int GetAmountOfItemNoBids(Item item)
        {
            String errorMessage;
            int amount =Items[ GetItem(item.ItemID)].Amount;
            if (amount == 0)
            {
                errorMessage = "item doesn't exist in basket";
                LogErrorMessage("GetAmountOfItem", errorMessage);
                throw new Exception(errorMessage);
            }
            return amount;
        }

        internal int RemoveAcceptedBid(int itemID)
        {
            int amount = 0;
            Bid myBid = null;
            foreach(Bid bid in BiddedItems)
            {
                if(bid.ItemID == itemID)
                {
                    myBid = bid;   
                }
            }
            if(myBid != null)
            {
                amount = myBid.Amount;
                BiddedItems.Remove(myBid);
            }
            return amount;
        }

        internal bool checkPurchasePolicy()
        {
            return Store().GetPurchasePolicy().checkPolicyConditions(this);
        }
        //returns the amount that was removed
        public int RemoveItem(Item item)
        {
            String errorMessage;
            if (isItemInBasket(item))
            {
                int amount = _items[GetItem(item._itemID)].Amount;
                _items.Remove(item);
                resetDiscounts();
                Store().GetDiscountPolicy().ApplyDiscounts(this);
                return amount;
            }
            errorMessage = "basket doesn't contain the item that was requested to be removed";
            LogErrorMessage("RemoveItem", errorMessage);
            throw new Exception(errorMessage);
        }

        public bool isItemInBasket(Item item)
        {
            foreach(Item i in Items.Keys)
            {
                if(i.ItemID == item.ItemID)
                {
                    return true;
                }
            }
            return false;
            //return _items.ContainsKey(item);
        }

        public bool updateItemQuantity(Item item, int newQuantity)
        {
            if (isItemInBasket(item))
            {
                int oldAmount = _items[GetItem(item.ItemID)].Amount;
                _items[GetItem(item.ItemID)].Amount = newQuantity;
                if (!Store().GetPurchasePolicy().checkPolicyConditions(this))
                {
                    if (oldAmount < newQuantity)
                    {
                        _items[GetItem(item.ItemID)].Amount = oldAmount;
                    }
                    throw new ArgumentException("this purchase is not compatible with the store's purchase policy.");
                }
                resetDiscounts();
                Store().GetDiscountPolicy().ApplyDiscounts(this);
                return true;
            }
            throw new Exception("there is no option to change amount of bidded bargain. you may add items from the store's page.");
        }

        internal void AddAcceptedBid(int itemId, int amount, double price)
        {
            _biddedItems.Add(new Bid("", itemId, amount, price));
        }

        private void resetDiscounts()
        {
            foreach (DiscountDetails<AtomicDiscount> detail in Items.Values)
                detail.resetDiscounts();
            AdditionalDiscounts.resetDiscounts();
        }

        public bool IsBasketEmpty()
        {
            return _items.Count == 0 && BiddedItems.Count == 0;
        }

        public virtual ICollection<Item> GetItems()
        {
            return _items.Keys;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in ShoppingBasket.{functionName}. Cause: {message}.");
        }


        public int SearchItemAmount(string itemName)
        {
            int result = 0;
            foreach(Item item in _items.Keys)
            {
                if(itemName == item.Name)
                {
                    result += _items[item].Amount;
                }
            }
            return result;
        }

        public int SearchCategoryAmount(string category)
        {
            int result = 0;
            foreach (Item item in _items.Keys)
            {
                if (category == item.Category)
                {
                    result += _items[item].Amount;
                }
            }
            return result;
        }
        internal string GetBasketReceipt()
        {
            String receipt = "";
            foreach(Item item in Items.Keys)
            {
                receipt += $"{Items[item]} {item._price} -> {Items[item].Amount * item._price}\n";
                receipt += Store().GetDiscountPolicy().GetActualDiscountString(this);
            }
            return receipt;
        }
        public double GetTotalPrice()
        {
            double sum = 0;
            foreach (Item i in GetItems())
                sum += i._price * Items[i].Amount;
            return sum;
        }
        internal double getActualPrice()
        {
            return Store().GetDiscountPolicy().calcActualPrice(this);
        }
        public double GetItemPrice(String itemName)
        {
            double sum = 0;
            foreach (Item item in Items.Keys)
                if (itemName == item.Name)
                    sum += item._price * Items[item].Amount;
            return sum;
        }
        public double GetCategoryPrice(String category)
        {
            double sum = 0;
            foreach (Item item in Items.Keys)
                if (category == item.Category)
                    sum += item._price * Items[item].Amount;
            return sum;
        }
        public void SetAllProductsDiscount(AllProductsDiscount discount)
        {
            foreach (Item item in Items.Keys)
                Items[item].AddDiscount(discount);
        }
        public void SetCategoryDiscount(CategoryDiscount discount, String category)
        {
            foreach (Item item in Items.Keys)
                if (item.Category == category)
                    Items[item].AddDiscount(discount);
        }
        public void SetItemDiscount(ItemDiscount discount, String itemName)
        {
            foreach (Item item in Items.Keys)
                if (item.Name == itemName)
                    Items[item].AddDiscount(discount);
        }
        public void SetNumericDiscount(NumericDiscount discount)
        {
            AdditionalDiscounts.AddDiscount(discount);
        }
        public IDictionary<Item, DiscountDetails<AtomicDiscount>> GetDetailsByItem()
        {
            return Items;
        }
        public ISet<NumericDiscount> GetAdditionalDiscounts()
        {
            return AdditionalDiscounts.DiscountList;
        }
        public double GetAdditionalDiscountsPrice()
        {
            double sum = 0;
            foreach (NumericDiscount discount in AdditionalDiscounts.DiscountList)
                sum += discount.PriceToSubtract;
            return sum;
        }
        public virtual Store Store()
        {
            return _store;
        }

        private Item GetItem(int itemId)
        {
            foreach(Item item in Items.Keys)
            {
                if(item.ItemID == itemId)
                {
                    return item;
                }
            }
            return null;
        }

    }
}
