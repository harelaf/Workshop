﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWeb.Server.DataLayer
{
    public class ShoppingBasketDAL
    {
        [Key]
        public int sbId { get; set; }
        [Required]
        [ForeignKey("StoreDAL")]
        public string _storeName { get; set; }
        [Required]
        public ICollection<BasketItemDAL> _items { get; set; }
        public PurchaseDetailsDAL _additionalDiscounts { get; set; }
        public ICollection<BidDAL> _bids { get; set; }

        public ShoppingBasketDAL(string store, IDictionary<int, PurchaseDetailsDAL> items, PurchaseDetailsDAL additionalDiscounts, ICollection<BidDAL> bids)
        {
            _storeName = store;
            _items = new List<BasketItemDAL>();
            foreach (KeyValuePair<int, PurchaseDetailsDAL> i_p in items)
            {
                _items.Add(new BasketItemDAL(i_p.Key, i_p.Value));
            }
            _additionalDiscounts = additionalDiscounts;
            _bids = bids;
        }

        public ShoppingBasketDAL(string store, ICollection<BasketItemDAL> items, PurchaseDetailsDAL additionalDiscounts, ICollection<BidDAL> bids)
        {
            _storeName = store;
            _items = items;
            _additionalDiscounts = additionalDiscounts;
            _bids = bids;
        }

        public IDictionary<int, PurchaseDetailsDAL> ConvertToDictionary()
        {
            IDictionary<int, PurchaseDetailsDAL> dic = new Dictionary<int, PurchaseDetailsDAL>();
            foreach (BasketItemDAL item in _items)
            {
                dic[item.itemID] = item.purchaseDetails;
            }
            return dic;
        }

        public ShoppingBasketDAL()
        {
            _items = new List<BasketItemDAL>();
            _additionalDiscounts = new PurchaseDetailsDAL();
            _bids = new List<BidDAL>();
        }
    }
}
