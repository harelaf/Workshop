﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{

    public class StoreDAL
    {
        [Key]
        public String _storeName { get; set; }
        [Required]
        [ForeignKey("StockDAL")]
        public StockDAL _stock { get; set; }
        //public List<MessageToStoreDAL> _messagesToStore { get; set; }
        [Required]
        [ForeignKey("RatingDAL")]
        public RatingDAL _rating { get; set; }
        //public List<StoreManagerDAL> _managers { get; set; }
        //public List<StoreOwnerDAL> _owners { get; set; }
        //public StoreFounderDAL _founder { get; set; }
        public StoreState _state { get; set; }
        [Required]
        [ForeignKey("PurchasePolicyDAL")]
        public PurchasePolicyDAL _purchasePolicy { get; set; }
        [Required]
        [ForeignKey("DiscountPolicyDAL")]
        public DiscountPolicyDAL _discountPolicy { get; set; }

        public StoreDAL(string storeName, StockDAL stock, List<MessageToStoreDAL> messagesToStore, RatingDAL rating, List<StoreManagerDAL> managers, List<StoreOwnerDAL> owners, StoreFounderDAL founder, StoreState state, PurchasePolicyDAL purchasePolicy, DiscountPolicyDAL discountPolicy)
        {
            _storeName = storeName;
            _stock = stock;
            //_messagesToStore = messagesToStore;
            _rating = rating;
            //_managers = managers;
            //_owners = owners;
            //_founder = founder;
            _state = state;
            _purchasePolicy = purchasePolicy;
            _discountPolicy = discountPolicy;
        }

        public StoreDAL(string storeName, StoreState state)
        {
            _storeName = storeName;
            _state = state;
            //_managers = new List<StoreManagerDAL>();
            //_owners = new List<StoreOwnerDAL>();
            //_messagesToStore = new List<MessageToStoreDAL>();
            _stock = new StockDAL(new List<StockItemDAL>());
            _rating = new RatingDAL(new List<RateDAL>());
            _purchasePolicy = new PurchasePolicyDAL(new List<ConditionDAL>());
            _discountPolicy = new DiscountPolicyDAL(new List<DiscountDAL>());
        }

        public StoreDAL()
        {
        }
    }
}