
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
        public ICollection<RateDAL> _rating { get; set; }
        [Required]
        public ICollection<StockItemDAL> _stock { get; set; }

        public StoreState _state { get; set; }
        [Required]
        [ForeignKey("PurchasePolicyDAL")]
        public PurchasePolicyDAL _purchasePolicy { get; set; }
        [Required]
        [ForeignKey("DiscountPolicyDAL")]
        public DiscountPolicyDAL _discountPolicy { get; set; }

        public StoreDAL(string storeName, ICollection<StockItemDAL> stock, List<MessageToStoreDAL> messagesToStore, ICollection<RateDAL> rating, List<StoreManagerDAL> managers, List<StoreOwnerDAL> owners, StoreFounderDAL founder, StoreState state, PurchasePolicyDAL purchasePolicy, DiscountPolicyDAL discountPolicy)
        {
            _storeName = storeName;
            _stock = stock;
            _rating = rating;
            _state = state;
            _purchasePolicy = purchasePolicy;
            _discountPolicy = discountPolicy;
        }

        public StoreDAL(string storeName, StoreState state)
        {
            _storeName = storeName;
            _state = state;
            _stock = new List<StockItemDAL>();
            _rating = new List<RateDAL>();
            _purchasePolicy = new PurchasePolicyDAL(new List<ConditionDAL>());
            _discountPolicy = new DiscountPolicyDAL(new List<DiscountDAL>());
        }

        public StoreDAL()
        {
        }
    }
}
