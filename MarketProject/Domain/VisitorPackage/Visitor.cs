using System;

namespace MarketProject.Domain
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
            return shoppingBasket.GetAmountOfItem(item);
        }
        public ShoppingCart PurchaseMyCart(String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode)
        {
            ShoppingCart cart = _shoppingCart;
            PurchaseProcess.GetInstance().Purchase(address, city, country, zip, purchaserName, cart, paymentMethode, shipmentMethode);
            _shoppingCart = new ShoppingCart();
            return cart;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in Visitor.{functionName}. Cause: {message}.");
        }
    }
}
