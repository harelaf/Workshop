using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Store
    {
        private Stock _stock;
        private PurchasePolicy _purchasePolicy;
        private ICollection<MessageToStore> _messagesToStore;
        private ICollection<Rating> _ratings;
        private ICollection<StoreManager> _managers;
        private ICollection<StoreOwner> _owners;
        private StoreFounder _founder;

    }
}
