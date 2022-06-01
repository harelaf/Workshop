using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class ShoppingCart
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual ICollection<ShoppingBasket> _shoppingBaskets { get; set; }

        public ShoppingCart()
        {
            _shoppingBaskets = new List<ShoppingBasket>();
        }

        public ShoppingBasket GetShoppingBasket(String storeName)
        {
            foreach (ShoppingBasket basket in _shoppingBaskets)
            {
                if (basket.Store().StoreName == storeName)
                    return basket;
            }
            return null;
        }

        public void AddShoppingBasket(ShoppingBasket shoppingBasket)
        {
            _shoppingBaskets.Add(shoppingBasket);
        }

        public bool isCartEmpty()
        {
            return _shoppingBaskets.Count == 0;
        }

        public void RemoveBasketFromCart(ShoppingBasket basket)
        {
            String errorMessage;
            if (!isCartEmpty() && _shoppingBaskets.Contains(basket))
                _shoppingBaskets.Remove(basket);
            else
            {
                errorMessage = "there is no such basket in cart to remove.";
                LogErrorMessage("RemoveBasketFromCart", errorMessage);
                throw new Exception(errorMessage);
            }
        }

        public virtual void RelaseItemsOfCart()
        {
            foreach (ShoppingBasket basket in _shoppingBaskets)
                basket.Store().RestockBasket(basket);
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in ShoppingCart.{functionName}. Cause: {message}.");
        }

        public int SearchItemAmount(string itemName)
        {
            int result = 0;
            foreach(ShoppingBasket shoppingBasket in _shoppingBaskets)
            {
                result += shoppingBasket.SearchItemAmount(itemName);
            }
            return result;
        }

        public int SearchCategoryAmount(string category)
        {
            int result = 0;
            foreach (ShoppingBasket shoppingBasket in _shoppingBaskets)
            {
                result += shoppingBasket.SearchCategoryAmount(category);
            }
            return result;
        }

        //before discounts
        public double GetTotalPrice()
        {
            double result = 0;
            foreach (ShoppingBasket shoppingBasket in _shoppingBaskets)
            {
                result += shoppingBasket.GetTotalPrice();
            }
            return result;
        }

        public double GetItemPrice(string itemName)
        {
            double result = 0;
            foreach (ShoppingBasket shoppingBasket in _shoppingBaskets)
            {
                result += shoppingBasket.GetItemPrice(itemName);
            }
            return result;
        }

        public double GetCategoryPrice(string category)
        {
            double result = 0;
            foreach (ShoppingBasket shoppingBasket in _shoppingBaskets)
            {
                result += shoppingBasket.GetCategoryPrice(category);
            }
            return result;
        }

        internal double getActualPrice()
        {
            double actualPrice = 0;
            foreach(ShoppingBasket shoppingBasket in _shoppingBaskets)
            {
                actualPrice += shoppingBasket.getActualPrice();
            }
            return actualPrice;
        }

        internal string GetCartReceipt()
        {
            String cartReceipt = "";
            ////////////////////////
            foreach (ShoppingBasket shoppingBasket in _shoppingBaskets)
            {
                cartReceipt += shoppingBasket.GetBasketReceipt() + "\n";
            }
            return cartReceipt;
        }
    }
}
