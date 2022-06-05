
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;
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

        public StoreDAL(string storeName, StockDAL stock, List<MessageToStoreDAL> messagesToStore, RatingDAL rating, List<StoreManagerDAL> managers, List<StoreOwnerDAL> owners, StoreFounderDAL founder, StoreState state)
        {
            _storeName = storeName;
            _stock = stock;
            _messagesToStore = messagesToStore;
            _rating = rating;
            _managers = managers;
            _owners = owners;
            _founder = founder;
            _state = state;
        }

        public StoreDAL(string storeName, StoreFounderDAL founder, StoreState state)
        {
            _storeName = storeName;
            _founder = founder;
            _state = state;
        }
        //private PurchasePolicy _purchasePolicy;
        //private DiscountPolicy _discountPolicy;

    }
}
