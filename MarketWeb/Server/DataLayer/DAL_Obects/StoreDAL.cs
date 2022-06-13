
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
        public String _storeName { get; set; }
        [Required]
        public StockDAL _stock { get; set; }
        public List<MessageToStoreDAL> _messagesToStore { get; set; }
        public RatingDAL _rating { get; set; }
        public List<StoreManagerDAL> _managers { get; set; }
        public List<StoreOwnerDAL> _owners { get; set; }
        public StoreFounderDAL _founder { get; set; }
        public StoreState _state { get; set; }
        public PurchasePolicyDAL _purchasePolicy { get; set; }
        public DiscountPolicyDAL _discountPolicy { get; set; }

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
            _stock = new StockDAL(new List<StockItemDAL>());
            _rating = new RatingDAL(new List<RateDAL>());
            _purchasePolicy = new PurchasePolicyDAL(new List<ConditionDAL>());
            _discountPolicy = new DiscountPolicyDAL(new List<DiscountDAL>());
        }

        public StoreDAL()
        {
            _stock = new StockDAL();
            _messagesToStore = new List<MessageToStoreDAL>();
            _rating = new RatingDAL();
            _managers = new List<StoreManagerDAL>();
            _owners = new List<StoreOwnerDAL>();
            //_founder = new StoreFounderDAL("test", "test");
            _purchasePolicy = new PurchasePolicyDAL();
            _discountPolicy = new DiscountPolicyDAL();
        }

    }
}
