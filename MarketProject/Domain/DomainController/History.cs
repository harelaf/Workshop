using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class History
    {
        private IDictionary<String, List<Tuple<DateTime, ShoppingBasket>>> _storePurchaseHistory; //storeName:String
        private IDictionary<String, List<Tuple<DateTime, ShoppingCart>>> _registeredPurchaseHistory; //Username:String

        public History()
        {
            _storePurchaseHistory = new Dictionary<String, List<Tuple<DateTime,ShoppingBasket>>>();
            _registeredPurchaseHistory = new Dictionary<String, List<Tuple<DateTime,ShoppingCart>>>();
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
                throw new Exception($"There is purchase history for {storeName} yet.");
            return _storePurchaseHistory[storeName];
        }

        public void AddStoresPurchases(ShoppingCart shoppingCart)
        {
            foreach (ShoppingBasket shoppingBasket in shoppingCart._shoppingBaskets)
            {
                String storeName = shoppingBasket.Store.GetName();
                if (!_storePurchaseHistory.ContainsKey(storeName))
                    _storePurchaseHistory.Add(storeName, new List<Tuple<DateTime, ShoppingBasket>>());
                _storePurchaseHistory[storeName].Add(new Tuple<DateTime, ShoppingBasket>(DateTime.Now, shoppingBasket));              
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
                throw new Exception($"Purchase History of Visitor: {Username} is Empty. Visitor has'nt purchased yet.");
            return _registeredPurchaseHistory[Username];
        }
    }
}
