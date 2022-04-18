using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class History
    {
        private IDictionary<String, ICollection<(DateTime, ShoppingBasket)>> _storePurchaseHistory; //storeName:String
        private IDictionary<String, ICollection<(DateTime, ShoppingCart)>> _registeredPurchaseHistory; //username:String

        // Harel: TODO: FINISH THIS FUNCITON.
        public bool CheckIfUserPurchasedInStore(String username, String storeName)
        {
            if (!_registeredPurchaseHistory.ContainsKey(username))
                return false;
            
            ICollection<(DateTime, ShoppingCart)> purchases = _registeredPurchaseHistory[username];

            return false;
        }

        // Harel: TODO: FINISH THIS FUNCTION
        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchaseHistory(String username, String storeName)
        {
            List<Tuple<DateTime, ShoppingBasket>> purchases = new List<Tuple<DateTime, ShoppingBasket>>();
            foreach (KeyValuePair<String, ICollection<(DateTime, ShoppingBasket)>> entry in _storePurchaseHistory)
            {
                
            }
            return purchases;
        }
    }
}
