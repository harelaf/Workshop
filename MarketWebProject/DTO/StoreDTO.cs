using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public class StoreDTO
    {
        private StockDTO _stock;
        public StockDTO Stock => _stock;

        //private PurchasePolicy _purchasePolicy;
        //public PurchasePolicy PurchasePolicy => _purchasePolicy;

        //private DiscountPolicy _discountPolicy;
        //public DiscountPolicy DiscountPolicy => _discountPolicy;

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

        //private StoreState _state;
        //public StoreState State => _state;

        public StoreDTO()
        {
            _storeName = "store1";
            _founder = new StoreFounderDTO();
        }
        
    }
}
