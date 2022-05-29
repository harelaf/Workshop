using MarketProject.Domain.PurchasePackage.PolicyPackage;
using System;
using System.Collections.Generic;

namespace MarketProject.Domain
{
    public class ShoppingBasket : ISearchablePriceable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual Store _store { get; set; }
        //public virtual IDictionary<Item, int> _items { get; set; }
        //public IDictionary<Item, int> Items => _items;
        private IDictionary<Item, DiscountDetails> _itemDiscounts;
        public IDictionary<Item, DiscountDetails> ItemDiscounts
        {
            get { return _itemDiscounts; }
            private set { _itemDiscounts = value; }
        }
        private DiscountDetails _additionalDiscounts;
        public DiscountDetails AdditionalDiscounts => _additionalDiscounts;

        public ShoppingBasket(Store store)
        {
            _store = store;
            ItemDiscounts = new Dictionary<Item, DiscountDetails>();
        }

        public void AddItem(Item item, int amount)
        {
            if (isItemInBasket(item))
                ItemDiscounts[item].AddAmount(amount);
            else ItemDiscounts[item] = new DiscountDetails(amount);
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
            return ItemDiscounts[item].Amount;
        }

        //returns the amount that was removed
        public int RemoveItem(Item item)
        {
            String errorMessage;
            if (isItemInBasket(item))
            {
                int amount = ItemDiscounts[item].Amount;
                ItemDiscounts.Remove(item);
                return amount;
            }
            errorMessage = "basket doesn't contain the item that was requested to be removed";
            LogErrorMessage("RemoveItem", errorMessage);
            throw new Exception(errorMessage);
        }

        public bool isItemInBasket(Item item)
        {
            return ItemDiscounts.ContainsKey(item);
        }

        public bool updateItemQuantity(Item item, int newQuantity)
        {
            if (isItemInBasket(item))
            {
                ItemDiscounts[item].UpdateAmount(newQuantity);
                return true;
            }
            return false;
        }

        public bool IsBasketEmpty()
        {
            return ItemDiscounts.Count == 0;
        }

        public virtual ICollection<Item> GetItems()
        {
            return ItemDiscounts.Keys;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in ShoppingBasket.{functionName}. Cause: {message}.");
        }


        public int SearchItemAmount(string itemName)
        {
            int result = 0;
            foreach(Item item in ItemDiscounts.Keys)
                if(itemName == item.Name)
                    result += ItemDiscounts[item].Amount;
            return result;
        }
        public int SearchCategoryAmount(string category)
        {
            int result = 0;
            foreach (Item item in ItemDiscounts.Keys)
                if (category == item.Category)
                    result += ItemDiscounts[item].Amount;
            return result;
        }
        public double GetTotalPrice()
        {
            double sum = 0;
            foreach(Item i in GetItems())
                sum += i._price * GetAmountOfItem(i);
            return sum;
        }
        internal double getActualPrice()
        {
            double totalDiscount = Store().GetDiscountPolicy().calculateDiscounts(this);
            double totalPrice = GetTotalPrice();
            return totalPrice - totalDiscount;
        }
        public string GetBasketReceipt()
        {
            return _store.GetDiscountPolicy().GetActualDiscountString(this);
        }
        public double GetItemPrice(String itemName)
        {
            double sum = 0;
            foreach (Item item in ItemDiscounts.Keys)
                if (itemName == item.Name)
                    sum += item._price * ItemDiscounts[item].Amount;
            return sum;
        }
        public double GetCategoryPrice(String category)
        {
            double sum = 0;
            foreach(Item item in ItemDiscounts.Keys)
                if (category == item.Category)
                    sum += item._price * ItemDiscounts[item].Amount;
            return sum;
        }
        public void SetAllProductsDiscount(AtomicDiscount discount)
        {
            foreach (Item item in ItemDiscounts.Keys)
                ItemDiscounts[item].AddDiscount(discount);
        }
        public void SetCategoryDiscount(AtomicDiscount discount, String category)
        {
            foreach (Item item in ItemDiscounts.Keys)
                if (item.Category == category)
                    ItemDiscounts[item].AddDiscount(discount);
        }
        public void SetItemDiscount(AtomicDiscount discount, String itemName)
        {
            foreach (Item item in ItemDiscounts.Keys)
                if (item.Name == itemName)
                    ItemDiscounts[item].AddDiscount(discount);
        }
        public void SetNumericDiscount(AtomicDiscount discount)
        {
            AdditionalDiscounts.AddDiscount(discount);
        }
        public IDictionary<Item, DiscountDetails> GetDetailsByItem()
        {
            return ItemDiscounts;
        }
        public DiscountDetails GetAdditionalDiscounts()
        {
            return AdditionalDiscounts;
        }
        public virtual Store Store()
        {
            return _store;
        }
    }
}
