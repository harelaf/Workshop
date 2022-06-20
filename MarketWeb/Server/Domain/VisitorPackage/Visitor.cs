using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketWeb.Server.Domain
{
    public abstract class Visitor
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ShoppingCart _shoppingCart;
        public ShoppingCart ShoppingCart => _shoppingCart;
        protected Visitor()
        {
            _shoppingCart = new ShoppingCart();
        }

        protected Visitor(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public void AddItemToCart(Store store, Item item, int amount)
        {
            ShoppingBasket basket= _shoppingCart.GetShoppingBasket(store.StoreName);
            if (basket == null)
            {
                basket = new ShoppingBasket(store);
                _shoppingCart.AddShoppingBasket(basket);
            }
            basket.AddItem(item, amount);
        }
        public int RemoveItemFromCart(Item item, Store store)
        {
            String errorMessage;
            ShoppingBasket basket = _shoppingCart.GetShoppingBasket(store.StoreName);
            if (basket == null)
            {
                errorMessage = "your cart doesnt contain any basket with the given store";
                LogErrorMessage("RemoveItemFromCart", errorMessage);
                throw new Exception(errorMessage);
            }
            int amount = basket.RemoveItem(item);
            if (basket.IsBasketEmpty())
                _shoppingCart.RemoveBasketFromCart(basket);
            return amount;
        }
        internal int RemoveAcceptedBidFromCart(int itemID, String storeName)
        {
            String errorMessage;
            ShoppingBasket basket = _shoppingCart.GetShoppingBasket(storeName);
            if (basket == null)
            {
                errorMessage = "your cart doesnt contain any basket with the given store";
                LogErrorMessage("RemoveItemFromCart", errorMessage);
                throw new Exception(errorMessage);
            }
            int amount = basket.RemoveAcceptedBid(itemID);
            if (basket.IsBasketEmpty())
                _shoppingCart.RemoveBasketFromCart(basket);
            return amount;
        }
        public void UpdateItemInCart(Store store, Item item, int newQuantity)
        {
            String errorMessage = null;
            if (newQuantity <= 0)// newQuantity==0 -> remove.
            {
                errorMessage = "can't update item quantity to a non-positive amount";
                LogErrorMessage("UpdateItemInCart", errorMessage);
                throw new Exception(errorMessage);
            }
            ShoppingBasket basket = _shoppingCart.GetShoppingBasket(store.StoreName);
            if (basket == null)
                errorMessage = "your cart doesnt contain any basket with the given store";
            else if (!basket.updateItemQuantity(item, newQuantity))
                errorMessage = "your cart doesn't contain the given item in the given store basket. ";
            if (errorMessage != null)
            {
                LogErrorMessage("UpdateItemInCart", errorMessage);
                throw new Exception(errorMessage);
            }
        }
        public int GetQuantityOfItemInCart(Store store, Item item)
        {
            String errorMessage;
            ShoppingBasket shoppingBasket = _shoppingCart.GetShoppingBasket(store.StoreName);
            if (shoppingBasket == null)
            {
                errorMessage = "your cart doesnt contain any basket with the given store";
                LogErrorMessage("GetQuantityOfItemInCart", errorMessage);
                throw new Exception(errorMessage);
            }
            return shoppingBasket.GetAmountOfItemNoBids(item);
        }
        public async Task<ShoppingCart> PurchaseMyCartAsync(String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode,   string cardNumber = null, string month = null, string year = null, string holder = null, string ccv = null, string id = null)
        {
            ShoppingCart cart = _shoppingCart;
            await PurchaseProcess.GetInstance().Purchase(address, city, country, zip, purchaserName, cart, paymentMethode, shipmentMethode, cardNumber, month, year, holder, ccv, id);
            _shoppingCart = new ShoppingCart();
            return cart;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in Visitor.{functionName}. Cause: {message}.");
        }

        internal void AddAcceptedBidToCart(Store store, int itemId, int amount, double price)
        {
            ShoppingBasket basket = _shoppingCart.GetShoppingBasket(store.StoreName);
            if (basket == null)
            {
                basket = new ShoppingBasket(store);
                _shoppingCart.AddShoppingBasket(basket);
            }
            basket.AddAcceptedBid(itemId, amount, price);
        }
    }
}
