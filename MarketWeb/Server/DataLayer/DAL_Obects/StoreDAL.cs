﻿
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{

    public class StoreDAL
    {
        [Key]
        internal String _storeName;
        [Required]
        internal StockDAL _stock;
        internal List<MessageToStoreDAL> _messagesToStore;
        internal RatingDAL _rating;
        internal List<StoreManagerDAL> _managers;
        internal List<StoreOwnerDAL> _owners;
        internal StoreFounderDAL _founder;
        [Required]
        internal StoreState _state;
        internal PurchasePolicyDAL _purchasePolicy;
        internal DiscountPolicyDAL _discountPolicy;

        public StoreDAL(string storeName, StockDAL stock, List<MessageToStoreDAL> messagesToStore, RatingDAL rating, List<StoreManagerDAL> managers, List<StoreOwnerDAL> owners, StoreFounderDAL founder, StoreState state, PurchasePolicyDAL purchasePolicy, DiscountPolicyDAL discountPolicy)
        {
            _storeName = storeName;
            _stock = stock;
            _messagesToStore = messagesToStore;
            _rating = rating;
            _managers = managers;
            _owners = owners;
            _founder = founder;
            _state = state;
            _purchasePolicy = purchasePolicy;
            _discountPolicy = discountPolicy;
        }

        public StoreDAL(string storeName, StoreFounderDAL founder, StoreState state)
        {
            _storeName = storeName;
            _founder = founder;
            _state = state;
            _managers = new List<StoreManagerDAL>();
            _owners = new List<StoreOwnerDAL>();
            _messagesToStore = new List<MessageToStoreDAL>();
            _stock = new StockDAL(new Dictionary<ItemDAL, int>());
            _rating = new RatingDAL(new List<RateDAL>());
            _purchasePolicy = new PurchasePolicyDAL(new List<ConditionDAL>());
            _discountPolicy = new DiscountPolicyDAL(new List<DiscountDAL>());
        }
        //private PurchasePolicy _purchasePolicy;
        //private DiscountPolicy _discountPolicy;

    }
}
