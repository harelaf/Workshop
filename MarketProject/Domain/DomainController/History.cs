using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class History
    {
        private IDictionary<int, ICollection<(DateTime,ShoppingBasket)>> _storePurchaseHistory;
        private IDictionary<int, ICollection<(DateTime, ShoppingCart)>> _registeredPurchaseHistory;
    }
}
