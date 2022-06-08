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
        }
        public void AddItem(Item item, int amount)
        {
            if (isItemInBasket(item)) 
            { 
                updateItemQuantity(item, amount);
                return;
            }
            else _items[item] = new DiscountDetails<AtomicDiscount>(amount);
            if (!_store.GetPurchasePolicy().checkPolicyConditions(this))
            {
                _items.Remove(item);
                throw new ArgumentException("this purchase is not compatible with the store's purchase policy.");
            }
            resetDiscounts();
            _store.GetDiscountPolicy().ApplyDiscounts(this);
        }

        public virtual int GetAmountOfItem(Item item)
        {
            String errorMessage;
            if (!isItemInBasket(item))
            {
                errorMessage = "item doesn't exist in basket";
                LogErrorMessage("GetAmountOfItem", errorMessage);
                throw new Exception(errorMessage);
            }
            return _items[item].Amount;
        }

        internal bool checkPurchasePolicy()
        {
            return _store.GetPurchasePolicy().checkPolicyConditions(this);
        }

        //returns the amount that was removed
        public int RemoveItem(Item item)
        {
            String errorMessage;
            if (isItemInBasket(item))
            {
                int amount = _items[item].Amount;
                _items.Remove(item);
                resetDiscounts();
                _store.GetDiscountPolicy().ApplyDiscounts(this);
                return amount;
            }
            errorMessage = "basket doesn't contain the item that was requested to be removed";
            LogErrorMessage("RemoveItem", errorMessage);
            throw new Exception(errorMessage);
        }

        public bool isItemInBasket(Item item)
        {
            return _items.ContainsKey(item);
        }

        public bool updateItemQuantity(Item item, int newQuantity)
        {
            if (isItemInBasket(item))
            {
                int oldAmount = _items[item].Amount;
                _items[item].Amount = newQuantity;
                if (!_store.GetPurchasePolicy().checkPolicyConditions(this))
                {
                    if(oldAmount < newQuantity)
                    {
                        _items[item].Amount = oldAmount;
                    }
                    throw new ArgumentException("this purchase is not compatible with the store's purchase policy.");
                }
                resetDiscounts();
                _store.GetDiscountPolicy().ApplyDiscounts(this);
                return true;
            }
            return false;
        }

        private void resetDiscounts()
        {
            foreach (DiscountDetails<AtomicDiscount> detail in Items.Values)
                detail.resetDiscounts();
            AdditionalDiscounts.resetDiscounts();
        }

        public bool IsBasketEmpty()
        {
            return _items.Count == 0;
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
                receipt += _store.GetDiscountPolicy().GetActualDiscountString(this);
            }
            return receipt;
        }
        public double GetTotalPrice()
        {
            double sum = 0;
            foreach (Item i in GetItems())
                sum += i._price * GetAmountOfItem(i);
            return sum;
        }
        internal double getActualPrice()
        {
            double totalDiscount = Store().GetDiscountPolicy().calculateDiscounts(this);
            double totalPrice = GetTotalPrice();
            return totalPrice - totalDiscount;
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

    }
}
