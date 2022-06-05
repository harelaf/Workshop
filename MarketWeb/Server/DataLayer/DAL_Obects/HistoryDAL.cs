using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class HistoryDAL
    {
        internal IDictionary<String, List<Tuple<DateTime, ShoppingBasketDAL>>> _storePurchaseHistory { get; set; } //storeName:String
        internal IDictionary<String, List<Tuple<DateTime, ShoppingCartDAL>>> _registeredPurchaseHistory { get; set; } //Username:String
   
    }
}
