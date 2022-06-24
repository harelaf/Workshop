using MarketWeb.Server.DataLayer;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain
{
    public class DalTRranslator
    {
        public static StoreManagement StoreManagement;

        private AtomicDiscountDAL AtomicDiscountDomainToDal(AtomicDiscount dis)
        {
            if (dis == null)
                return null;
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscount)))
                return DomainToDal((AllProductsDiscount)dis);
            if (type.Equals(typeof(CategoryDiscount)))
                return DomainToDal((CategoryDiscount)dis);
            if (type.Equals(typeof(ItemDiscount)))
                return DomainToDal((ItemDiscount)dis);
            if (type.Equals(typeof(NumericDiscount)))
                return DomainToDal((NumericDiscount)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }

        private DiscountDAL DiscountDomainToDal(Discount dis)
        {
            if (dis == null)
                return null;
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscount)))
                return DomainToDal((AllProductsDiscount)dis);
            if (type.Equals(typeof(CategoryDiscount)))
                return DomainToDal((CategoryDiscount)dis);
            if (type.Equals(typeof(ItemDiscount)))
                return DomainToDal((ItemDiscount)dis);
            if (type.Equals(typeof(NumericDiscount)))
                return DomainToDal((NumericDiscount)dis);
            if (type.Equals(typeof(MaxDiscount)))
                return DomainToDal((MaxDiscount)dis);
            if (type.Equals(typeof(PlusDiscount)))
                return DomainToDal((PlusDiscount)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }
        private AllProductsDiscountDAL DomainToDal(AllProductsDiscount dis)
        {
            return new AllProductsDiscountDAL(dis.PercentageToSubtract, dis.Expiration, ConditionDomainToDal(dis.Condition));
        }
        private CategoryDiscountDAL DomainToDal(CategoryDiscount dis)
        {
            return new CategoryDiscountDAL(dis.Category, dis.PercentageToSubtract, dis.Expiration, ConditionDomainToDal(dis.Condition));
        }
        private ItemDiscountDAL DomainToDal(ItemDiscount dis)
        {
            return new ItemDiscountDAL(dis.ItemName, dis.PercentageToSubtract, dis.Expiration, ConditionDomainToDal(dis.Condition));
        }
        private NumericDiscountDAL DomainToDal(NumericDiscount dis)
        {
            return new NumericDiscountDAL(dis.PriceToSubtract, dis.Expiration, ConditionDomainToDal(dis.Condition));
        }
        private MaxDiscountDAL DomainToDal(MaxDiscount dis)
        {
            List<DiscountDAL> disList = new List<DiscountDAL>();
            foreach(Discount innerDis in dis.DiscountList)
                disList.Add(DiscountDomainToDal(innerDis));
            ConditionDAL cond = ConditionDomainToDal(dis.Condition);
            return new MaxDiscountDAL(disList, cond);
        }
        private PlusDiscountDAL DomainToDal(PlusDiscount dis)
        {
            List<DiscountDAL> disList = new List<DiscountDAL>();
            foreach (Discount innerDis in dis.DiscountList)
                disList.Add(DiscountDomainToDal(innerDis));
            ConditionDAL cond = ConditionDomainToDal(dis.Condition);
            return new PlusDiscountDAL(disList, cond);
        }
        public StoreDAL StoreDomainToDal(Store store)
        {
            String storeName = store.StoreName;
            ICollection<StockItemDAL> stock = StockDomainToDal(store.Stock);
            StoreFounderDAL founder = StoreFounderDomainToDal(store.GetFounder());
            StoreState state = store.State;
            ICollection<RateDAL> rating = RatingDomainToDal(store.Rating);
            List<MessageToStoreDAL> messagesToStoreDAL = new List<MessageToStoreDAL>();
            foreach (MessageToStore messageToStore in store.MessagesToStore)
            {
                messagesToStoreDAL.Add(MessageToStoreDomainToDAL(messageToStore));
            }
            List<StoreOwnerDAL> owners = new List<StoreOwnerDAL>();
            foreach (StoreOwner owner in store.GetOwners())
            {
                owners.Add(StoreOwnerDomainToDal(owner));
            }
            List<StoreManagerDAL> managers = new List<StoreManagerDAL>();
            foreach (StoreManager manager in store.GetManagers())
            {
                managers.Add(StoreManagerDomainToDal(manager));
            }
            ICollection<BidsOfVisitor> bidsOfVisitors = new List<BidsOfVisitor>();
            foreach(string bidder in store.BiddedItems.Keys)
            {
                ICollection<BidDAL> currBidList = new List<BidDAL>();
                foreach(Bid bid in store.BiddedItems[bidder])
                {
                    currBidList.Add(BidDomainToDal(bid));
                }
                bidsOfVisitors.Add(new BidsOfVisitor(bidder, currBidList));
            }
            ICollection<OwnerAcceptors> standbyOwners = new List<OwnerAcceptors>();
            Dictionary<string, List<string>> standbyOwnersDict = store.GetStandbyOwnersInStore();
            foreach (string newOwner in standbyOwnersDict.Keys)
            {
                ICollection<StringData> acceptors = new List<StringData>();
                string appointer = standbyOwnersDict[newOwner][0];
                foreach (string acceptor in standbyOwnersDict[newOwner])
                {
                    if(acceptor != appointer)
                        acceptors.Add(new StringData(acceptor));
                }
                standbyOwners.Add(new OwnerAcceptors(newOwner, acceptors, appointer));
            }
            return new StoreDAL(storeName, stock, messagesToStoreDAL, rating, managers, owners, founder, state, 
                                                            PurchasePolicyDomainToDal(store.GetPurchasePolicy()), 
                                                            DiscountPolicyDomainToDal(store.GetDiscountPolicy()),
                                                            bidsOfVisitors, standbyOwners);
        }

        private String DiscountPolicyDomainToDal(DiscountPolicy discountPolicy)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(discountPolicy, settings);
        }

        private String PurchasePolicyDomainToDal(PurchasePolicy purchasePolicy)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(purchasePolicy, settings);
        }

        private ConditionDAL ConditionDomainToDal(Condition cond)
        {
            if (cond == null)
                return null;
            Type type = cond.GetType();
            if (type.Equals(typeof(AndComposition)))
                return DomainToDal((AndComposition)cond);
            if (type.Equals(typeof(DayOnWeekCondition)))
                return DomainToDal((DayOnWeekCondition)cond);
            if (type.Equals(typeof(HourCondition)))
                return DomainToDal((HourCondition)cond);
            if (type.Equals(typeof(OrComposition)))
                return DomainToDal((OrComposition)cond);
            if (type.Equals(typeof(PriceableCondition)))
                return DomainToDal((PriceableCondition)cond);
            if (type.Equals(typeof(SearchCategoryCondition)))
                return DomainToDal((SearchCategoryCondition)cond);
            if (type.Equals(typeof(SearchItemCondition)))
                return DomainToDal((SearchItemCondition)cond);
            if (type.Equals(typeof(XorComposition)))
                return DomainToDal((XorComposition)cond);
            else throw new NotImplementedException();
        }

        private ConditionDAL DomainToDal(AndComposition cond)
        {
            List<ConditionDAL> condList = new List<ConditionDAL>();
            foreach (Condition innerCond in cond.ConditionList)
                condList.Add(ConditionDomainToDal(innerCond));
            return new AndCompositionDAL(condList, cond.ToNegative);
        }

        private DayOnWeekConditionDAL DomainToDal(DayOnWeekCondition cond)
        {
            return new DayOnWeekConditionDAL(cond.DayOnWeek, cond.ToNegative);
        }

        private HourConditionDAL DomainToDal(HourCondition cond)
        {
            return new HourConditionDAL(cond.MinHour, cond.MaxHour, cond.ToNegative);
        }

        private OrCompositionDAL DomainToDal(OrComposition cond)
        {
            List<ConditionDAL> condList = new List<ConditionDAL>();
            foreach (Condition innerCond in cond.ConditionList)
                condList.Add(ConditionDomainToDal(innerCond));
            return new OrCompositionDAL(condList, cond.ToNegative);
        }

        private PriceableConditionDAL DomainToDal(PriceableCondition cond)
        {
            return new PriceableConditionDAL(cond.MinValue, cond.MaxValue, cond.ToNegative);
        }

        private SearchCategoryConditionDAL DomainToDal(SearchCategoryCondition cond)
        {
            return new SearchCategoryConditionDAL(cond.KeyWord, cond.MinValue, cond.MaxValue, cond.ToNegative);
        }

        private SearchItemConditionDAL DomainToDal(SearchItemCondition cond)
        {
            return new SearchItemConditionDAL(cond.KeyWord, cond.MinValue, cond.MaxValue, cond.ToNegative);
        }

        private XorCompositionDAL DomainToDal(XorComposition cond)
        {
            List<ConditionDAL> condList = new List<ConditionDAL>();
            foreach (Condition innerCond in cond.ConditionList)
                condList.Add(ConditionDomainToDal(innerCond));
            return new XorCompositionDAL(condList, cond.ToNegative);
        }

        public ICollection<StockItemDAL> StockDomainToDal(Stock stock)
        {
            ICollection<StockItemDAL> itemAndAmount = new List<StockItemDAL>();
            foreach (KeyValuePair<Item, int> i_a in stock.Items)
            {
                itemAndAmount.Add(new StockItemDAL(i_a.Key.ItemID, i_a.Value));
            }
            return itemAndAmount;
        }

        public StoreManagerDAL StoreManagerDomainToDal(StoreManager manager)
        {
            return new StoreManagerDAL(manager.Appointer, manager.Username, manager.StoreName);
        }

        public StoreOwnerDAL StoreOwnerDomainToDal(StoreOwner owner)
        {
            return new StoreOwnerDAL(owner.Appointer, owner.Username, owner.StoreName);
        }
        public StoreFounderDAL StoreFounderDomainToDal(StoreFounder founder)
        {
            return new StoreFounderDAL(founder.Username, founder.StoreName);
        }
        public MessageToStoreDAL MessageToStoreDomainToDAL(MessageToStore messageToStore)
        {
            return new MessageToStoreDAL(messageToStore.Id, messageToStore.SenderUsername, 
                messageToStore.Message, messageToStore.Title, messageToStore.Reply, messageToStore.Replier, messageToStore.StoreName);
        }

        public ICollection<PopulationStatistics> PopulationStatisticsListDalToDomain(Dictionary<PopulationSection,int> statistics, DateTime dateTime)
        {
            ICollection<PopulationStatistics> populationStatisticsDomain = new List<PopulationStatistics>();
            foreach(KeyValuePair<PopulationSection,int> populationStatisticsDAL in statistics)
            {
                populationStatisticsDomain.Add(new PopulationStatistics(populationStatisticsDAL.Key, dateTime, populationStatisticsDAL.Value));
            }
            return populationStatisticsDomain;

        }
        public ItemDAL ItemDomainToDal(Item itemDomain)
        {
            return new ItemDAL(itemDomain.ItemID,RatingDomainToDal(itemDomain.Rating), itemDomain.Name,
                itemDomain._price, itemDomain.Description, itemDomain.Category);
        }
        public ICollection<RateDAL> RatingDomainToDal(Rating ratingDomain)
        {
            ICollection<RateDAL> rateSet = new List<RateDAL>();

            foreach (KeyValuePair<string, Tuple<int, string>> rate in ratingDomain.Ratings)
            {
                rateSet.Add(new RateDAL(rate.Key, rate.Value.Item1, rate.Value.Item2));
            }
            return rateSet;
        }

        public Store StoreDalToDomain(StoreDAL storeDAL)
        {
            Stock stock = StockDalToDomain(storeDAL._stock);
            PurchasePolicy purchasePolicy = PurchasePolicyDalToDomain(storeDAL._purchasePolicyJSON);
            DiscountPolicy discountPolicy = DiscountPolicyDalToDomain(storeDAL._discountPolicyJSON);
            StoreFounderDAL founderDAL = DalController.GetInstance().GetStoreFounder(storeDAL._storeName);
            StoreFounder founder = StoreFounderDalToDomain(founderDAL);
            String storeName = storeDAL._storeName;
            StoreState state = storeDAL._state;
            Rating rating = RatingDalToDomain(storeDAL._rating);
            List<MessageToStore> messagesToStore = new List<MessageToStore>();
            List<MessageToStoreDAL> messagesToStoreDAL = DalController.GetInstance().GetOpenMessagesToStoreByStoreName(storeDAL._storeName); ;
            foreach (MessageToStoreDAL messageToStoreDAL in messagesToStoreDAL)
            {
                messagesToStore.Add(MessageToStoreDALToDomain(messageToStoreDAL));
            }
            List<StoreManagerDAL> storeManagerDALs= DalController.GetInstance().GetStoreManagers(storeDAL._storeName); ;
            List<StoreManager> managers = new List<StoreManager>();
            foreach (StoreManagerDAL manager in storeManagerDALs)
            {
                managers.Add(StoreManagerDalToDomain(manager));
            }
            List<StoreOwnerDAL> ownersDAL = DalController.GetInstance().GetStoreOwners(storeDAL._storeName); ;
            List<StoreOwner> owners = new List<StoreOwner>();
            foreach (StoreOwnerDAL owner in ownersDAL)
            {
                owners.Add(StoreOwnerDalToDomain(owner));
            }
            IDictionary<string, List<Bid>> biddedItems = new Dictionary<string, List<Bid>>();
            if (storeDAL._bidsOfVisitors != null)
            {
                foreach (BidsOfVisitor bov in storeDAL._bidsOfVisitors)
                {
                    biddedItems[bov._bidder] = new List<Bid>();
                    foreach (BidDAL bid in bov._bids)
                    {
                        biddedItems[bov._bidder].Add(BidDalToDomain(bid));
                    }
                }
            }
            Dictionary<string, List<string>> standbyOwners = new Dictionary<string, List<string>>();
            if(storeDAL._standbyOwners != null)
            {
                foreach (OwnerAcceptors oe in storeDAL._standbyOwners)
                {
                    standbyOwners[oe._newOwner] = new List<string>();
                    standbyOwners[oe._newOwner].Add(oe._appointer);
                    foreach (StringData acceptor in oe._acceptors)
                    {
                        standbyOwners[oe._newOwner].Add(acceptor.data);
                    }
                }
            }
            return new Store(stock, purchasePolicy, discountPolicy, messagesToStore, rating
                , managers, owners, founder,storeName ,state, biddedItems, standbyOwners);
        }

        private DiscountPolicy DiscountPolicyDalToDomain(string discountPolicyJSON)
        {
            if(discountPolicyJSON == null || discountPolicyJSON == "")
            {
                return new DiscountPolicy();
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            return JsonConvert.DeserializeObject<DiscountPolicy>(discountPolicyJSON, settings);
            /*
              PlusDiscount plusDiscount =  JsonConvert.DeserializeObject<PlusDiscount>(discountPolicyJSON, settings);
            DiscountPolicy discountPolicy = new DiscountPolicy();
            foreach(Discount d in plusDiscount.DiscountList)
            {
                discountPolicy.AddDiscount(d);
            }
            return discountPolicy;
             */
        }

        private PurchasePolicy PurchasePolicyDalToDomain(string purchasePolicyJSON)
        {
            if (purchasePolicyJSON == null || purchasePolicyJSON == "")
            {
                return new PurchasePolicy();
            }
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            return JsonConvert.DeserializeObject<PurchasePolicy>(purchasePolicyJSON, settings);
        }

        public MessageToStore MessageToStoreDALToDomain(MessageToStoreDAL msg)
        {
            return new MessageToStore(DalController.GetInstance().GetStoreNameOfMessageToStore(msg.mid), msg._senderUsername, 
                msg._title, msg._message, msg._reply, msg._replierFromStore, msg.mid);
        }
        public StoreFounder StoreFounderDalToDomain(StoreFounderDAL founderDAL)
        {
            return new StoreFounder(founderDAL.ConvertToSet(), DalController.GetInstance().GetRoleUsername(founderDAL.id), DalController.GetInstance().GetRoleStoreName(founderDAL.id));
        }
        public StoreManager StoreManagerDalToDomain(StoreManagerDAL managerDAL)
        {
            return new StoreManager(managerDAL._appointer, managerDAL.ConvertToSet(), DalController.GetInstance().GetRoleUsername(managerDAL.id), DalController.GetInstance().GetRoleStoreName(managerDAL.id));        
        }
        public StoreOwner StoreOwnerDalToDomain(StoreOwnerDAL ownerDAL)
        {
            return new StoreOwner(ownerDAL._appointer, ownerDAL.ConvertToSet(), DalController.GetInstance().GetRoleUsername(ownerDAL.id), DalController.GetInstance().GetRoleStoreName(ownerDAL.id));
        }
        public Stock StockDalToDomain(ICollection<StockItemDAL> stockDAL)
        {
            Dictionary<Item, int> itemAndAmount = new Dictionary<Item, int>();  
            foreach (StockItemDAL stockItem in stockDAL)
            {
                ItemDAL item = DalController.GetInstance().GetItem(stockItem.itemID);
                itemAndAmount.Add(ItemDalToDomain(item), stockItem.amount);
            }
            return new Stock(itemAndAmount);
        }
        public Item ItemDalToDomain(ItemDAL itemDal)
        {
            Item item = new Item(itemDal._itemID, itemDal._name, itemDal._price, itemDal._description, itemDal._category);
            //set rating:
            item._rating = RatingDalToDomain(itemDal._rating);
            return item;
        }
        public Rating RatingDalToDomain(ICollection<RateDAL> ratingDAL)
        {
            Dictionary<string, Tuple<int, string>> ratings = new Dictionary<string, Tuple<int, string>>();
            foreach (RateDAL rate in ratingDAL)
            {
                ratings.Add(rate.username, new Tuple<int, string>(rate.rate, rate.review));
            }
            return new Rating(ratings);
        }

        public ShoppingCart ShoppingCartDALToDomain(ShoppingCartDAL cartDAL)
        {
            ICollection<ShoppingBasket> baskets = new List<ShoppingBasket>();
            foreach(ShoppingBasketDAL basketDAL in cartDAL._shoppingBaskets)
            {
                baskets.Add(ShoppingBasketDALToDomain(basketDAL));
            }
            return new ShoppingCart(baskets, cartDAL.scId);
        }
        public ShoppingBasket ShoppingBasketDALToDomain(ShoppingBasketDAL basketDAL)
        {
            IDictionary<Item, DiscountDetails<AtomicDiscount>> items = new Dictionary<Item, DiscountDetails<AtomicDiscount>>();
            Store store = StoreManagement.GetActiveStore(basketDAL._storeName);
            if(store == null)
            {
                store = StoreDalToDomain(DalController.GetInstance().GetStoreInformation(basketDAL._storeName));
            }
            foreach(KeyValuePair<int, PurchaseDetailsDAL> i_a in basketDAL.ConvertToDictionary())
            {
                Item item = store.GetItem(i_a.Key);
                items.Add(item, PurchaseDetailsDALToDomain<AtomicDiscount>(i_a.Value));
            }
            DiscountDetails<NumericDiscount> additionalDiscounts = PurchaseDetailsDALToDomain<NumericDiscount>(basketDAL._additionalDiscounts);
            List<Bid> bids = new List<Bid>();
            foreach (BidDAL bid in basketDAL._bids)
                bids.Add(BidDalToDomain(bid));
            return new ShoppingBasket(store, items, additionalDiscounts, bids);
        }

        private Bid BidDalToDomain(BidDAL bid)
        {
            ISet<string> acceptors = new HashSet<string>();
            if (bid._acceptors != null)
            {
                foreach (StringData acceptor in bid._acceptors)
                    acceptors.Add(acceptor.data);
            }
            return new Bid(bid._bidder, bid._itemId, bid._amount, bid._biddedPrice, bid._counterOffer, acceptors, bid._acceptedByAll);
        }

        private DiscountDetails<T> PurchaseDetailsDALToDomain<T>(PurchaseDetailsDAL value) where T : AtomicDiscount
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            ISet<T> disList = JsonConvert.DeserializeObject<HashSet<T>>(value.discountListJSON, settings);
            if(disList == null)
                disList = new HashSet<T>();
            DiscountDetails<T> details = new DiscountDetails<T>(value.amount, disList);
            return details;
        }

        private Discount DiscountDalToDomain(DiscountDAL dis)
        {
                if (dis == null)
                    return null;
                Type type = dis.GetType();
                if (type.Equals(typeof(AllProductsDiscountDAL)))
                    return DalToDomain((AllProductsDiscountDAL)dis);
                if (type.Equals(typeof(CategoryDiscountDAL)))
                    return DalToDomain((CategoryDiscountDAL)dis);
                if (type.Equals(typeof(ItemDiscountDAL)))
                    return DalToDomain((ItemDiscountDAL)dis);
                if (type.Equals(typeof(MaxDiscountDAL)))
                    return DalToDomain((MaxDiscountDAL)dis);
                if (type.Equals(typeof(NumericDiscountDAL)))
                    return DalToDomain((NumericDiscountDAL)dis);
                if (type.Equals(typeof(PlusDiscountDAL)))
                    return DalToDomain((PlusDiscountDAL)dis);
                else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }
        private AtomicDiscount AtomicDiscountDalToDomain(AtomicDiscountDAL dis)
        {
            if (dis == null)
                return null;
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscountDAL)))
                return DalToDomain((AllProductsDiscountDAL)dis);
            if (type.Equals(typeof(CategoryDiscountDAL)))
                return DalToDomain((CategoryDiscountDAL)dis);
            if (type.Equals(typeof(ItemDiscountDAL)))
                return DalToDomain((ItemDiscountDAL)dis);
            if (type.Equals(typeof(NumericDiscountDAL)))
                return DalToDomain((NumericDiscountDAL)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }

        private AllProductsDiscount DalToDomain(AllProductsDiscountDAL dis)
        {
            Condition cond = ConditionDalToDomain(dis._condition);
            return new AllProductsDiscount(dis._percents, cond, dis._expiration);
        }

        private CategoryDiscount DalToDomain(CategoryDiscountDAL dis)
        {
            Condition cond = ConditionDalToDomain(dis._condition);
            return new CategoryDiscount(dis._percents, dis._categoryName, cond, dis._expiration);
        }

        private AtomicDiscount DalToDomain(ItemDiscountDAL dis)
        {
            Condition cond = ConditionDalToDomain(dis._condition);
            return new CategoryDiscount(dis._percents, dis._itemName, cond, dis._expiration);
        }

        private MaxDiscount DalToDomain(MaxDiscountDAL dis)
        {
            List<Discount> disList = new List<Discount>();
            foreach(DiscountDAL innerDis in dis._discounts)
                disList.Add(DiscountDalToDomain(innerDis));
            Condition cond = ConditionDalToDomain(dis._condition);
            return new MaxDiscount(disList, cond);
        }

        private NumericDiscount DalToDomain(NumericDiscountDAL dis)
        {
            Condition cond = ConditionDalToDomain(dis._condition);
            return new NumericDiscount(dis._priceToSubtract, cond, dis._expiration);
        }

        private PlusDiscount DalToDomain(PlusDiscountDAL dis)
        {
            List<Discount> disList = new List<Discount>();
            foreach (DiscountDAL innerDis in dis._discounts)
                disList.Add(DiscountDalToDomain(innerDis));
            Condition cond = ConditionDalToDomain(dis._condition);
            return new PlusDiscount(disList, cond);
        }

        private Condition ConditionDalToDomain(ConditionDAL cond)
        {
            if (cond == null)
                return null;
            Type type = cond.GetType();
            if (type.Equals(typeof(AndCompositionDAL)))
                return DalToDomain((AndCompositionDAL)cond);
            if (type.Equals(typeof(DayOnWeekConditionDAL)))
                return DalToDomain((DayOnWeekConditionDAL)cond);
            if (type.Equals(typeof(HourConditionDAL)))
                return DalToDomain((HourConditionDAL)cond);
            if (type.Equals(typeof(OrCompositionDAL)))
                return DalToDomain((OrCompositionDAL)cond);
            if (type.Equals(typeof(PriceableConditionDAL)))
                return DalToDomain((PriceableConditionDAL)cond);
            if (type.Equals(typeof(SearchCategoryConditionDAL)))
                return DalToDomain((SearchCategoryConditionDAL)cond);
            if (type.Equals(typeof(SearchItemConditionDAL)))
                return DalToDomain((SearchItemConditionDAL)cond);
            if (type.Equals(typeof(XorCompositionDAL)))
                return DalToDomain((XorCompositionDAL)cond);
            else throw new NotImplementedException();
        }

        private AndComposition DalToDomain(AndCompositionDAL cond)
        {
            List<Condition> condList = new List<Condition>();
            foreach (ConditionDAL innerCond in cond._conditionList)
                condList.Add(ConditionDalToDomain(innerCond));
            return new AndComposition(cond._negative, condList);
        }

        private DayOnWeekCondition DalToDomain(DayOnWeekConditionDAL cond)
        {
            return new DayOnWeekCondition(cond._day.ToString(), cond._negative);
        }

        private HourCondition DalToDomain(HourConditionDAL cond)
        {
            return new HourCondition(cond._minHour, cond._maxHour, cond._negative);
        }

        private OrComposition DalToDomain(OrCompositionDAL cond)
        {
            List<Condition> condList = new List<Condition>();
            foreach (ConditionDAL innerCond in cond._conditionList)
                condList.Add(ConditionDalToDomain(innerCond));
            return new OrComposition(cond._negative, condList);
        }

        private PriceableCondition DalToDomain(PriceableConditionDAL cond)
        {
            return new PriceableCondition(cond._keyWord, cond._minValue, cond._maxValue, cond._negative);
        }

        private SearchCategoryCondition DalToDomain(SearchCategoryConditionDAL cond)
        {
            return new SearchCategoryCondition(cond._keyWord, cond._minValue, cond._maxValue, cond._negative);
        }

        private SearchItemCondition DalToDomain(SearchItemConditionDAL cond)
        {
            return new SearchItemCondition(cond._keyWord, cond._minValue, cond._maxValue, cond._negative);
        }

        private XorComposition DalToDomain(XorCompositionDAL cond)
        {
            List<Condition> condList = new List<Condition>();
            foreach (ConditionDAL innerCond in cond._conditionList)
                condList.Add(ConditionDalToDomain(innerCond));
            return new XorComposition(cond._negative, condList);
        }

        public Registered RegisteredDALToDomain(RegisteredDAL registeredDAL)
        {
            string username = registeredDAL._username;
            string password = registeredDAL._password;
            string salt = registeredDAL._salt;
            DateTime birthDate = registeredDAL._birthDate;
            ShoppingCart cart = ShoppingCartDALToDomain(registeredDAL._cart);
            ICollection<AdminMessageToRegistered> adminMessages = new List<AdminMessageToRegistered>();
            foreach(AdminMessageToRegisteredDAL adminMessage in registeredDAL._adminMessages)
            {
                adminMessages.Add(AdminMessageDalToDomain(adminMessage));
            }
            ICollection<NotifyMessage> notifications = new List<NotifyMessage>();
            foreach(NotifyMessageDAL notification in registeredDAL._notifications)
            {
                notifications.Add(NotifyMessageDalToDomain(notification));
            }
            ICollection<MessageToStore> repliedMessages = new List<MessageToStore>();
            ICollection<MessageToStoreDAL> repliedMessagesDAL =DalController.GetInstance().GetRepliedMessagesToStoreByUserName(registeredDAL._username);
            foreach (MessageToStoreDAL repliedMessage in repliedMessagesDAL)
            {
                repliedMessages.Add(MessageToStoreDalToDomain(repliedMessage));
            }
            ICollection<SystemRoleDAL> rolesDAL = DalController.GetInstance().GetRegisteredRolesByUsername(registeredDAL._username);
            ICollection<SystemRole> roles = new List<SystemRole>();
            foreach(SystemRoleDAL role in rolesDAL)
            {
                roles.Add(SystemRoleDalToDomain(role));
            }
            IDictionary<int, Complaint> filedComplaints = new Dictionary<int, Complaint>();
            ICollection<ComplaintDAL> fieldComplaintsDAL = DalController.GetInstance().GetRgisteredFiledComplaintsByUsername(registeredDAL._username);
            foreach (ComplaintDAL id_complaint in fieldComplaintsDAL)
                filedComplaints.Add(id_complaint._id, ComplaintDalToDomain(id_complaint));
            return new Registered(adminMessages, notifications, repliedMessages, username, password, 
                salt, birthDate,roles, filedComplaints, cart);
        }

        public AdminMessageToRegistered AdminMessageDalToDomain(AdminMessageToRegisteredDAL msgDal)
        {
           return new AdminMessageToRegistered(msgDal.mid, DalController.GetInstance().GetReceiverOfAdminMessage(msgDal.mid), msgDal._senderUsername, msgDal._title, msgDal._message);
        }
        public NotifyMessage NotifyMessageDalToDomain(NotifyMessageDAL notifyDAL)
        {
            return new NotifyMessage(notifyDAL.mid, notifyDAL._storeName, notifyDAL._title, notifyDAL._message, DalController.GetInstance().GetReceiverOfNotificationMessage(notifyDAL.mid));
        }
        public MessageToStore MessageToStoreDalToDomain(MessageToStoreDAL msgDAL)
        {
            return new MessageToStore(DalController.GetInstance().GetStoreNameOfMessageToStore(msgDAL.mid), msgDAL._senderUsername, msgDAL._title,
                msgDAL._message, msgDAL._reply, msgDAL._replierFromStore, msgDAL.mid);
        }
        public Complaint ComplaintDalToDomain(ComplaintDAL complaintDAL)
        {
            return new Complaint(complaintDAL._id,complaintDAL._complainer
                , complaintDAL._cartID, complaintDAL._message, complaintDAL._response);
        }
        public SystemRole SystemRoleDalToDomain(SystemRoleDAL systemRoleDAL)
        {
            if (systemRoleDAL is SystemAdminDAL)
            {
                SystemAdminDAL systemAdminDAL = (SystemAdminDAL)systemRoleDAL;
                return new SystemAdmin(DalController.GetInstance().GetRoleUsername(systemAdminDAL.id), systemAdminDAL.ConvertToSet());
            }
            if (systemRoleDAL is StoreFounderDAL)
            {
                StoreFounderDAL storeFounderDAL = (StoreFounderDAL)systemRoleDAL;
                return new StoreFounder(storeFounderDAL.ConvertToSet(), DalController.GetInstance().GetRoleUsername(storeFounderDAL.id), DalController.GetInstance().GetRoleStoreName(storeFounderDAL.id));
            }
            if(systemRoleDAL is StoreOwnerDAL)
            {
                StoreOwnerDAL storeOwnerDAL = (StoreOwnerDAL)systemRoleDAL;
                return new StoreOwner(storeOwnerDAL._appointer, storeOwnerDAL.ConvertToSet(), DalController.GetInstance().GetRoleUsername(storeOwnerDAL.id), DalController.GetInstance().GetRoleStoreName(storeOwnerDAL.id));
            }
            if (systemRoleDAL is StoreManagerDAL)
            {
                StoreManagerDAL storeManagerDAL = (StoreManagerDAL)systemRoleDAL;
                return new StoreManager(storeManagerDAL._appointer, storeManagerDAL.ConvertToSet(), DalController.GetInstance().GetRoleUsername(storeManagerDAL.id), DalController.GetInstance().GetRoleStoreName(storeManagerDAL.id));
            }
            else
                throw new Exception("can;t happen");
        }
        public List<Tuple<DateTime, ShoppingCart>> PurchasedCartDalToDomain(List<Tuple<DateTime, ShoppingCartDAL>> purchasedCartDAL)
        {
            List<Tuple<DateTime, ShoppingCart>> cartHistory = new List<Tuple<DateTime, ShoppingCart>>();
            foreach (Tuple<DateTime, ShoppingCartDAL> purchase in purchasedCartDAL)
            {
                cartHistory.Add(new Tuple<DateTime, ShoppingCart>(purchase.Item1, ShoppingCartDALToDomain(purchase.Item2)));
            }
            return cartHistory;
        }
        public List<Tuple<DateTime, ShoppingCartDAL>> PurchasedCartDomainToDal(List<Tuple<DateTime, ShoppingCart>> purchasedCart)
        {
            List<Tuple<DateTime, ShoppingCartDAL>> cartHistory = new List<Tuple<DateTime, ShoppingCartDAL>>();
            foreach (Tuple<DateTime, ShoppingCart> purchase in purchasedCart)
            {
                cartHistory.Add(new Tuple<DateTime, ShoppingCartDAL>(purchase.Item1, ShoppingCartDomainToDAL(purchase.Item2)));
            }
            return cartHistory;
        }

        public ShoppingCartDAL ShoppingCartDomainToDAL(ShoppingCart cart)
        {
            ICollection<ShoppingBasketDAL> baskets = new List<ShoppingBasketDAL>();
            foreach (ShoppingBasket basketDAL in cart._shoppingBaskets)
            {
                baskets.Add(ShoppingBasketDomainToDAL(basketDAL));
            }
            return new ShoppingCartDAL(baskets);
        }

        public List<Tuple<DateTime, ShoppingBasket>> PurchasedBasketDalToDomain(List<Tuple<DateTime, ShoppingBasketDAL>> purchasedBasketDAL)
        {
            List<Tuple<DateTime, ShoppingBasket>> basketHistory = new List<Tuple<DateTime, ShoppingBasket>>();
            foreach (Tuple<DateTime, ShoppingBasketDAL> purchase in purchasedBasketDAL)
            {
                basketHistory.Add(new Tuple<DateTime, ShoppingBasket>(purchase.Item1, ShoppingBasketDALToDomain(purchase.Item2)));
            }
            return basketHistory;
        }
        public List<Tuple<DateTime, ShoppingBasketDAL>> PurchasedBasketDomainToDal(List<Tuple<DateTime, ShoppingBasket>> purchasedBasket)
        {
            List<Tuple<DateTime, ShoppingBasketDAL>> basketHistory = new List<Tuple<DateTime, ShoppingBasketDAL>>();
            foreach (Tuple<DateTime, ShoppingBasket> purchase in purchasedBasket)
            {
                basketHistory.Add(new Tuple<DateTime, ShoppingBasketDAL>(purchase.Item1, ShoppingBasketDomainToDAL(purchase.Item2)));
            }
            return basketHistory;
        }
        public IDictionary<int, Complaint> ComplaintsListDalToDomain(ICollection<ComplaintDAL> complaintDALs)
        {
            IDictionary<int, Complaint> complaints = new Dictionary<int, Complaint>();
            foreach (ComplaintDAL complaintDAL in complaintDALs)
                complaints.Add(complaintDAL._id, ComplaintDalToDomain(complaintDAL));
            return complaints;
        }
        public ShoppingBasketDAL ShoppingBasketDomainToDAL(ShoppingBasket basket)
        {
            IDictionary<int, PurchaseDetailsDAL> items = new Dictionary<int, PurchaseDetailsDAL>();
            foreach (KeyValuePair<Item, DiscountDetails<AtomicDiscount>> i_a in basket.Items)
            {
                items.Add(i_a.Key.ItemID, PurchaseDetailsDomainToDal(i_a.Key.ItemID, i_a.Value));
            }
            PurchaseDetailsDAL additionalDiscounts = PurchaseDetailsDomainToDal(0, basket.AdditionalDiscounts);
            List<BidDAL> bids = new List<BidDAL>();
            foreach (Bid bid in basket.BiddedItems)
                bids.Add(BidDomainToDal(bid));
            return new ShoppingBasketDAL(basket._store.StoreName, items, additionalDiscounts, bids);
        }

        private BidDAL BidDomainToDal(Bid bid)
        {
            ICollection<StringData> acceptors = new List<StringData>();
            foreach(string str in bid.Acceptors)
                acceptors.Add(new StringData(str));
            return new BidDAL(
                bid.Bidder, 
                bid.ItemID, 
                bid.Amount, 
                bid.BiddedPrice, 
                bid.CounterOffer, 
                acceptors,
                bid.AcceptedByAll);
        }

        private PurchaseDetailsDAL PurchaseDetailsDomainToDal<T>(int itemID, DiscountDetails<T> discountDetails) where T : AtomicDiscount
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            String disListJSON = JsonConvert.SerializeObject(discountDetails.DiscountList, settings);
            PurchaseDetailsDAL details = new PurchaseDetailsDAL(itemID, discountDetails.Amount, disListJSON);
            return details;
        }

        public Dictionary<string, Store> StoreListDalToDomain(ICollection<StoreDAL> storeDALs)
        {
            Dictionary<string, Store> stores = new Dictionary<string, Store>();
            foreach (StoreDAL storeDAL in storeDALs)
                stores.Add(storeDAL._storeName,StoreDalToDomain(storeDAL));
            return stores;
        }
        /*
        public RegisteredDAL RegisteredDomainToDAL(Registered registered)
        {
            string username = registered.Username;
            string password = registered._password;
            string salt = registered._salt;
            DateTime birthDate = registered._birthDate;
            ShoppingCartDAL cart = ShoppingCartDomainToDAL(registered.ShoppingCart);
            ICollection<AdminMessageToRegisteredDAL> adminMessages = new List<AdminMessageToRegisteredDAL>();
            foreach (AdminMessageToRegistered adminMessage in registered.AdminMessages)
            {
                adminMessages.Add(AdminMessageDomainToDal(adminMessage));
            }
            ICollection<NotifyMessageDAL> notifications = new List<NotifyMessageDAL>();
            foreach (NotifyMessage notification in registered.Notifcations)
            {
                notifications.Add(NotifyMessageDomainToDal(notification));
            }
            ICollection<MessageToStoreDAL> repliedMessages = new List<MessageToStoreDAL>();
            foreach (MessageToStore repliedMessage in registered.messageToStores)
            {
                repliedMessages.Add(MessageToStoreDomainToDal(repliedMessage));
            }
            ICollection<SystemRoleDAL> roles = new List<SystemRoleDAL>();
            foreach (SystemRole role in registered.Roles)
            {
                roles.Add(SystemRoleDomainToDal(role));
            }
            RegisteredDAL reg = new RegisteredDAL(username, password, salt, cart, birthDate, null, roles, 
                adminMessages, notifications, repliedMessages, );
            ICollection<ComplaintDAL> filedComplaints = new List<ComplaintDAL>();
            foreach (KeyValuePair<int, Complaint> id_complaint in registered.FiledComplaints)
                filedComplaints.Add(ComplaintDomainToDal(id_complaint.Value));
            return reg;
        }*/

        public ComplaintDAL ComplaintDomainToDal(Complaint value)
        {
            return new ComplaintDAL(value.ID, value._complainer, value.CartID, value.Message, value.Response);
        }

        public SystemRoleDAL SystemRoleDomainToDal(SystemRole role)
        {
            if (role is SystemAdmin)
            {
                SystemAdmin systemAdmin = (SystemAdmin)role;
                return new SystemAdminDAL(systemAdmin.Username);
            }
            if (role is StoreFounder)
            {
                StoreFounder storeFounder = (StoreFounder)role;
                return new StoreFounderDAL(storeFounder.Username, storeFounder.StoreName);
            }
            if (role is StoreOwner)
            {
                StoreOwner storeOwner = (StoreOwner)role;
                return new StoreOwnerDAL(storeOwner.Appointer, storeOwner.Username, storeOwner.StoreName);
            }
            if (role is StoreManager)
            {
                StoreManager storeManager = (StoreManager)role;
                return new StoreManagerDAL(storeManager.Appointer, storeManager.Username, storeManager.StoreName);
            }
            else
                throw new Exception("can;t happen");
        }

        public MessageToStoreDAL MessageToStoreDomainToDal(MessageToStore repliedMessage)
        {
            return new MessageToStoreDAL(repliedMessage.Id, repliedMessage.SenderUsername,
                repliedMessage.Message, repliedMessage.Title, repliedMessage.Reply, repliedMessage.Replier, repliedMessage.StoreName);
        }

        public NotifyMessageDAL NotifyMessageDomainToDal(NotifyMessage notification)
        {
            return new NotifyMessageDAL(notification.Id, notification.StoreName, notification.Title,
                notification.Message);
        }

        public AdminMessageToRegisteredDAL AdminMessageDomainToDal(AdminMessageToRegistered adminMessage)
        {
            return new AdminMessageToRegisteredDAL(adminMessage.id, adminMessage.ReceiverUsername, adminMessage.SenderUsername, adminMessage.Title, adminMessage.Message);
        }
    }
}
