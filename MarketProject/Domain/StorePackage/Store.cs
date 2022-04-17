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
        private String _storeName;
        public String StoreName => _storeName;

        public Store()
        {
            _stock = new Stock();
            _purchasePolicy = new PurchasePolicy();
            _managers = new List<StoreManager>();
            _owners = new List<StoreOwner>();
            _founder = new StoreFounder();
        }
        public Item ReserveItem(int itemID, int amount)
        {
            Item item  = _stock.GetItem(itemID);
            if (item == null)
                throw new Exception("there is no such item: " + itemID + " is store");
            if (amount <= 0)
                throw new Exception("cannt reserve item with amount<1");
            if (!_stock.ReserveItem(item, amount))
                throw new Exception("can't reseve amount: " + amount+" of " + itemID + ".the avalable amount is: " + _stock.GetItemAmount(item));
            return item; // else: reservation done secsussfully-> return reserved item
        }

    }
}
