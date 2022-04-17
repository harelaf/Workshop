using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Store
    {
        private Stock _stock;
        private PurchasePolicy _purchasePolicy;
        private Queue<MessageToStore> _messagesToStore;
        private Rating _rating;
        private List<StoreManager> _managers;
        private List<StoreOwner> _owners;
        private StoreFounder _founder;

        public Store()
        {
            _stock = new Stock();
            _purchasePolicy = new PurchasePolicy();
            _messagesToStore = new Queue<MessageToStore>();
            _rating = new Rating();
            _managers
        }
    }
}
