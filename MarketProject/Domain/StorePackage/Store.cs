using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class Store
    {
        private String _storeName;
        private Stock _stock;
        private PurchasePolicy _purchasePolicy;
        private DiscountPolicy _discountPolicy;
        private Queue<MessageToStore> _messagesToStore;
        private Rating _rating;
        private List<StoreManager> _managers;
        private List<StoreOwner> _owners;
        private StoreFounder _founder;

        public Store(String storeName, StoreFounder founder, PurchasePolicy purchasePolicy, DiscountPolicy discountPolicy)
        {
            _storeName = storeName;
            _stock = new Stock();
            _purchasePolicy = purchasePolicy;
            _discountPolicy = discountPolicy;
            _messagesToStore = new Queue<MessageToStore>();
            _rating = new Rating();
            _managers = new List<StoreManager>();
            _owners = new List<StoreOwner>();
            _founder = founder;
        }

        public String getName()
        {
            return _storeName;
        }
    }
}
