using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class History
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IDictionary<String, List<Tuple<DateTime, ShoppingBasket>>> _storePurchaseHistory; //storeName:String
        private IDictionary<String, List<Tuple<DateTime, ShoppingCart>>> _registeredPurchaseHistory; //Username:String
        private Market _market;
        public History(Market market)
        {
            _storePurchaseHistory = new Dictionary<String, List<Tuple<DateTime,ShoppingBasket>>>();
            _registeredPurchaseHistory = new Dictionary<String, List<Tuple<DateTime,ShoppingCart>>>();
            _market = market;
        }

        public bool CheckIfVisitorPurchasedInStore(String Username, String storeName)
        {
            if (!_registeredPurchaseHistory.ContainsKey(Username))
                return false;

            ICollection<Tuple<DateTime, ShoppingCart>> purchases = _registeredPurchaseHistory[Username];
            foreach (Tuple<DateTime, ShoppingCart> purchase in purchases)
            {
                ShoppingBasket basket = purchase.Item2.GetShoppingBasket(storeName);
                if(basket != null)
                {
                    return true;
                }
            }
            return false;
        }


        public bool CheckIfVisitorPurchasedItemInStore(String Username, String storeName, Item item)
        {
            if (!_registeredPurchaseHistory.ContainsKey(Username))
                return false;

            ICollection<Tuple<DateTime, ShoppingCart>> purchases = _registeredPurchaseHistory[Username];
            foreach (Tuple<DateTime, ShoppingCart> purchase in purchases)
            {
                ShoppingBasket basket = purchase.Item2.GetShoppingBasket(storeName);
                if (basket != null)
                {
                    if (basket.isItemInBasket(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchaseHistory(String storeName)
        {
            if (!_storePurchaseHistory.ContainsKey(storeName))
            {
                String errorMessage = $"There is purchase history for {storeName} yet.";
                LogErrorMessage("GetStorePurchaseHistory", errorMessage);
                throw new Exception(errorMessage);
            }
            return _storePurchaseHistory[storeName];
        }

        public void AddStoresPurchases(ShoppingCart shoppingCart)
        {
            foreach (ShoppingBasket shoppingBasket in shoppingCart._shoppingBaskets)
            {
                String storeName = shoppingBasket.Store().GetName();
                if (!_storePurchaseHistory.ContainsKey(storeName))
                    _storePurchaseHistory.Add(storeName, new List<Tuple<DateTime, ShoppingBasket>>());
                _storePurchaseHistory[storeName].Add(new Tuple<DateTime, ShoppingBasket>(DateTime.Now, shoppingBasket));

                // Send notification to store owner that a purchase was made.
                List<String> names = _market._storeManagement.GetStoreRolesByName(storeName);
                String title = $"Store: {storeName} is temporarily closing down: [{DateTime.Now.ToString()}].";
                String message = $"I am sad to inform you that {storeName} is temporarily closing down. " +
                    $"Your roles in the store will remain until we decide permanently close down." +
                    $"Yours Truly," +
                    $"{Username}.";
                foreach (String name in names)
                {
                    _market.SendNotification(storeName, name, title, message);
                }
            }
        }
        public void AddRegisterPurchases(ShoppingCart shoppingCart, String Username)
        {
            if (!_registeredPurchaseHistory.ContainsKey(Username))
                _registeredPurchaseHistory.Add(Username, new List<Tuple<DateTime, ShoppingCart>>());
            _registeredPurchaseHistory[Username].Add(new Tuple<DateTime, ShoppingCart>(DateTime.Now, shoppingCart));

        }
        public ICollection<Tuple<DateTime, ShoppingCart>> GetRegistreredPurchaseHistory(String Username)
        {
            if (!_registeredPurchaseHistory.ContainsKey(Username))
            {
                String errorMessage = $"Purchase History of Visitor: {Username} is Empty. Visitor has'nt purchased yet.";
                LogErrorMessage("GetRegistreredPurchaseHistory", errorMessage);
                return new List<Tuple<DateTime, ShoppingCart>>();
            }
            return _registeredPurchaseHistory[Username];
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in History.{functionName}. Cause: {message}.");
        }
    }
}
