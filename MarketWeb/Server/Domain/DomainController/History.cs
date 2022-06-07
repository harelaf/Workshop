using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.DataLayer;

namespace MarketWeb.Server.Domain
{
    public class History
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private IDictionary<String, List<Tuple<DateTime, ShoppingBasket>>> _storePurchaseHistory; //storeName:String
        //private IDictionary<String, List<Tuple<DateTime, ShoppingCart>>> _registeredPurchaseHistory; //Username:String
        private DalTRranslator _dalTRranslator;
        private DalController _dalController = DalController.GetInstance();
        public History()
        {
            //_storePurchaseHistory = new Dictionary<String, List<Tuple<DateTime,ShoppingBasket>>>();
            //_registeredPurchaseHistory = new Dictionary<String, List<Tuple<DateTime,ShoppingCart>>>();
            _dalTRranslator = new DalTRranslator();
        }

        public bool CheckIfVisitorPurchasedInStore(String Username, String storeName)
        {
            return _dalController.DidRegisterPurchasedInStore(Username, storeName);
        }


        public bool CheckIfVisitorPurchasedItemInStore(String Username, String storeName, Item item)
        {
            ICollection<Tuple<DateTime, ShoppingBasket>> purchasesInStore = _dalTRranslator.PurchasedBasketDalToDomain(_dalController.GetRegisterPurchasesInStore(Username, storeName));
            if(purchasesInStore== null)
                return false;
            foreach (Tuple<DateTime, ShoppingBasket> purchase in purchasesInStore)
            {
                if (purchase.Item2.isItemInBasket(item))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchaseHistory(String storeName)
        {
            List<Tuple<DateTime, ShoppingBasket>> tuples = _dalTRranslator.PurchasedBasketDalToDomain(_dalController.GetStorePurchasesHistory(storeName));
            if (tuples == null)
            {
                String errorMessage = $"There is purchase history for {storeName} yet.";
                LogErrorMessage("GetStorePurchaseHistory", errorMessage);
                throw new Exception(errorMessage);
            }
            return tuples;
        }

        public void AddStoresPurchases(ShoppingCart shoppingCart)
        {
            foreach (ShoppingBasket shoppingBasket in shoppingCart._shoppingBaskets)
            {
                String storeName = shoppingBasket.Store().GetName();
                _dalController.addStorePurchse(_dalTRranslator.BasketDomainToDal(shoppingBasket), DateTime.Now, storeName);
            }
        }
        public void AddRegisterPurchases(ShoppingCart shoppingCart, String Username)
        {
            _dalController.addRegisteredPurchse(_dalTRranslator.CartDomainToDal(shoppingCart), DateTime.Now, Username);
        }
        public ICollection<Tuple<DateTime, ShoppingCart>> GetRegistreredPurchaseHistory(String Username)
        {
            ICollection<Tuple<DateTime, ShoppingCart>> history =
                _dalTRranslator.PurchasedCartDalToDomain(_dalController.GetMyPurchasesHistory(Username));
            if (history == null)
            {
                String errorMessage = $"Purchase History of Visitor: {Username} is Empty. Visitor has'nt purchased yet.";
                LogErrorMessage("GetRegistreredPurchaseHistory", errorMessage);
                return new List<Tuple<DateTime, ShoppingCart>>();
            }
            return history;
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in History.{functionName}. Cause: {message}.");
        }
    }
}
