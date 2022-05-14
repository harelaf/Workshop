using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWebProject.DTO
{
    public enum StoreState
    {
        Active,
        Inactive,
        Closed
    }
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

        private StoreState _state;
        public StoreState State => _state;
        private IDictionary<string, StoreState> _statesName;
        public StoreDTO()
        {
            initStateName();
            _storeName = "store1";
            _founder = new StoreFounderDTO();
            _stock = new StockDTO();
            _rating = new RatingDTO();
            _state = StoreState.Active;
        }

        public StoreDTO(StoreFounderDTO founder, string storeName, string state)
        {
            initStateName();
            _founder = founder;
            _storeName = storeName;
            _state = _statesName[state];
            _managers = new List<StoreManagerDTO>();
            _owners = new List<StoreOwnerDTO>();
            _messagesToStore = new Queue<MessageToStoreDTO>();
            _stock = new StockDTO();
            _rating = new RatingDTO();
        }

        public StoreDTO(StockDTO stock, Queue<MessageToStoreDTO> messagesToStore, RatingDTO rating,
            List<StoreManagerDTO> managers, List<StoreOwnerDTO> owners, StoreFounderDTO founder,
            string storeName, string state)
        {
            initStateName();
            _stock = stock;
            _messagesToStore = messagesToStore;
            _rating = rating;
            _managers = managers;
            _owners = owners;
            _founder = founder;
            _storeName = storeName;
            _state = _statesName[state];
        }

        public bool isActive()
        {
            return _state == StoreState.Active;
        }

        private void initStateName()
        {
            _statesName= new Dictionary<string, StoreState>();
            _statesName.Add("Active", StoreState.Active);
            _statesName.Add("Inactive", StoreState.Inactive);
            _statesName.Add("Closed", StoreState.Closed);
        }
    }
}
