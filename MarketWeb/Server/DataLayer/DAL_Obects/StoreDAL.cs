
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

        [Required]
        public string _discountPolicyJSON { get; set; }
        [Required]
        public string _purchasePolicyJSON { get; set; }
        //public List<StoreManagerDAL> _managers { get; set; }
        //public List<StoreOwnerDAL> _owners { get; set; }
        //public StoreFounderDAL _founder { get; set; }
        public StoreState _state { get; set; }
        public ICollection<BidsOfVisitor> _bidsOfVisitors{ get; set; }
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
                        ICollection<BidsOfVisitor> bidsOfVisitors)
        {
            _storeName = storeName;
            _stock = stock;
            _rating = rating;
            _state = state;
            _purchasePolicyJSON = purchasePolicyJSON;
            _discountPolicyJSON = discountPolicyJSON;
            if(bidsOfVisitors != null)
                _bidsOfVisitors = bidsOfVisitors;
            else _bidsOfVisitors = new List<BidsOfVisitor>();
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
            _bidsOfVisitors = new List<BidsOfVisitor>();
        }

        public StoreDAL()
        { 
            _stock = new List<StockItemDAL>();
            _rating = new List<RateDAL>();
            //_managers = new List<StoreManagerDAL>();
            //_owners = new List<StoreOwnerDAL>();
            //_messagesToStore = new List<MessageToStoreDAL>();
            _purchasePolicyJSON = "";
            _discountPolicyJSON = "";
            _bidsOfVisitors = new List<BidsOfVisitor>();
        }
    }
}
