

using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class StoreDTO
    {
        private StockDTO _stock;
        public StockDTO Stock => _stock;

        private PurchasePolicyDTO _purchasePolicy;
        public PurchasePolicyDTO PurchasePolicy => _purchasePolicy;

        private DiscountPolicyDTO _discountPolicy;
        public DiscountPolicyDTO DiscountPolicy => _discountPolicy;

        private Queue<MessageToStoreDTO> _messagesToStore;
        public Queue<MessageToStoreDTO> MessagesToStore => _messagesToStore;

        private RatingDTO _rating;
        public RatingDTO Rating => _rating;

        private List<StoreManagerDTO> _managers;
        public List<StoreManagerDTO> Managers => _managers;

        private List<StoreOwnerDTO> _owners;
        public List<StoreOwnerDTO> Owners => _owners;

        private StoreFounderDTO _founder;
        public StoreFounderDTO Founder => _founder;

        private String _storeName;
        public String StoreName => _storeName;

        private StoreState _state;
        public StoreState State => _state;

        public StoreDTO(String storeName, StoreFounderDTO founder, PurchasePolicyDTO purchasePolicy, DiscountPolicyDTO discountPolicy, StockDTO stock, Queue<MessageToStoreDTO> messages, RatingDTO rating, List<StoreManagerDTO> managers, List<StoreOwnerDTO> owners, StoreState state)
        {
            _storeName = storeName;
            _stock = stock;
            _purchasePolicy = purchasePolicy;
            _discountPolicy = discountPolicy;
            _messagesToStore = messages;
            _rating = rating;
            _managers = managers;
            _owners = owners;
            _founder = founder;
            _state = state; 
        }
        //public StoreDTO(Store store)
        //{
        //    _stock = new StockDTO(store.Stock);
        //    _purchasePolicy = store.GetPurchasePolicy();
        //    _discountPolicy = store.GetDiscountPolicy();
        //    _messagesToStore = new Queue<MessageToStoreDTO>();
        //    foreach (MessageToStore msg in store.MessagesToStore)
        //    { //Might be in reverse order...
        //        MessageToStoreDTO dto = new MessageToStoreDTO(msg);
        //        _messagesToStore.Enqueue(dto);
        //    }
        //    _rating = new RatingDTO(store.Rating);
        //    _managers = new List<StoreManagerDTO>();
        //    foreach (StoreManager manager in store.GetManagers())
        //    {
        //        StoreManagerDTO dto = new StoreManagerDTO(manager);
        //        _managers.Add(dto);
        //    }
        //    _owners = new List<StoreOwnerDTO>();
        //    foreach (StoreOwner owner in store.GetOwners())
        //    {
        //        StoreOwnerDTO dto = new StoreOwnerDTO(owner);
        //        _owners.Add(dto);
        //    }
        //    _founder = new StoreFounderDTO(store.GetFounder());
        //    _storeName = store.GetName();
        //    _state = store.State;
        //}
    }
}
