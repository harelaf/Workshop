
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
        public RatingDAL _rating { get; set; }
        [Required]
        public String _discountPolicyJSON { get; set; }
        [Required]
        public String _purchasePolicyJSON { get; set; }
        //public List<StoreManagerDAL> _managers { get; set; }
        //public List<StoreOwnerDAL> _owners { get; set; }
        //public StoreFounderDAL _founder { get; set; }
        public StoreState _state { get; set; }
        //[Required]
        //[ForeignKey("PurchasePolicyDAL")]
        //public PurchasePolicyDAL _purchasePolicy { get; set; }
        //[Required]
        //[ForeignKey("DiscountPolicyDAL")]
        //public DiscountPolicyDAL _discountPolicy { get; set; }

        public StoreDAL(string storeName, ICollection<StockItemDAL> stock, List<MessageToStoreDAL> messagesToStore, ICollection<RateDAL> rating, List<StoreManagerDAL> managers, List<StoreOwnerDAL> owners, StoreFounderDAL founder, StoreState state, PurchasePolicyDAL purchasePolicy, DiscountPolicyDAL discountPolicy)
        {
            _storeName = storeName;
            _stock = stock;
            _rating = rating;
            _state = state;
            _purchasePolicyJSON = purchasePolicy;
            _discountPolicyJSON = discountPolicy;
        }

        public StoreDAL(string storeName, StoreState state)
        {
            _storeName = storeName;
            _state = state;
            _stock = new List<StockItemDAL>();
            _rating = new List<RateDAL>();
            _purchasePolicy = new PurchasePolicyDAL(new List<ConditionDAL>());
            _discountPolicy = new DiscountPolicyDAL(new List<DiscountDAL>());
            //_managers = new List<StoreManagerDAL>();
            //_owners = new List<StoreOwnerDAL>();
            //_messagesToStore = new List<MessageToStoreDAL>();
            _stock = new StockDAL(new List<StockItemDAL>());
            _rating = new RatingDAL(new List<RateDAL>());
            _purchasePolicyJSON = "";
            _discountPolicyJSON = "";
        }

        public StoreDAL()
        {
        }
    }
}
