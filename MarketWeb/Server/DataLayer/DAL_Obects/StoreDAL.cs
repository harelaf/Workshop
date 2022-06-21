﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MarketWeb.Shared;
using Newtonsoft.Json;

namespace MarketWeb.Server.DataLayer
{

    public class StoreDAL
    {
        [Key]
        public String _storeName { get; set; }
        [Required]
        public ICollection<RateDAL> _rating { get; set; }
        [Required]
        public ICollection<StockItemDAL> _stock { get; set; }

        [ForeignKey("RatingDAL")]
        [Required]
        public string _discountPolicyJSON { get; set; }
        [Required]
        public string _purchasePolicyJSON { get; set; }
        //public List<StoreManagerDAL> _managers { get; set; }
        //public List<StoreOwnerDAL> _owners { get; set; }
        //public StoreFounderDAL _founder { get; set; }
        public StoreState _state { get; set; }
        public IDictionary<string, List<BidDAL>> _biddedItems { get; set; }
        //[Required]
        //[ForeignKey("PurchasePolicyDAL")]
        //public PurchasePolicyDAL _purchasePolicy { get; set; }
        //[Required]
        //[ForeignKey("DiscountPolicyDAL")]
        //public DiscountPolicyDAL _discountPolicy { get; set; }

        public StoreDAL(string storeName, 
                        ICollection<StockItemDAL> stock, 
                        List<MessageToStoreDAL> messagesToStore, 
                        ICollection<RateDAL> rating, 
                        List<StoreManagerDAL> managers, 
                        List<StoreOwnerDAL> owners, 
                        StoreFounderDAL founder, 
                        StoreState state, 
                        string purchasePolicyJSON, 
                        string discountPolicyJSON,
                        IDictionary<string, List<BidDAL>> biddedItems)
        {
            _storeName = storeName;
            _stock = stock;
            _rating = rating;
            _state = state;
            _purchasePolicyJSON = purchasePolicyJSON;
            _discountPolicyJSON = discountPolicyJSON;
            _biddedItems = biddedItems;
        }

        public StoreDAL(string storeName, StoreState state)
        {
            _storeName = storeName;
            _state = state;
            _stock = new List<StockItemDAL>();
            _rating = new List<RateDAL>();
            //_managers = new List<StoreManagerDAL>();
            //_owners = new List<StoreOwnerDAL>();
            //_messagesToStore = new List<MessageToStoreDAL>();
            _purchasePolicyJSON = "";
            _discountPolicyJSON = "";
            _biddedItems = new Dictionary<string, List<BidDAL>>();
        }

        public StoreDAL()
        {
        }
    }
}
