using MarketWeb.Server.DataLayer;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Shared;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain
{
    public class DalTRranslator
    {
        public ShoppingCartDAL CartDomainToDal(ShoppingCart cartDomain)
        {
            ICollection<ShoppingBasketDAL> baskets = new List<ShoppingBasketDAL>();
            foreach (ShoppingBasket basketDomain in cartDomain._shoppingBaskets)
                baskets.Add(BasketDomainToDal(basketDomain));
            return new ShoppingCartDAL(baskets);
        }
        public ShoppingBasketDAL BasketDomainToDal(ShoppingBasket basketDomain)
        {
            IDictionary<ItemDAL, PurchaseDetailsDAL> items = new Dictionary<ItemDAL, PurchaseDetailsDAL>();
            foreach (Item item in basketDomain._items.Keys)
                items.Add(ItemDomainToDal(item), PurchaseDetailsToDal(item.ItemID, basketDomain._items[item]));
            return new ShoppingBasketDAL(StoreDomainToDal(basketDomain._store), items);
        }

        private PurchaseDetailsDAL PurchaseDetailsToDal(int itemID, DiscountDetails discountDetails)
        {
            List<AtomicDiscountDAL> disList = new List<AtomicDiscountDAL>();
            foreach (AtomicDiscount dis in discountDetails.DiscountList)
                disList.Add(AtomicDiscountDomainToDal(dis));
            PurchaseDetailsDAL details = new PurchaseDetailsDAL(itemID, discountDetails.Amount, disList);
            return details;
        }

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
            StockDAL stock = StockDomainToDal(store.Stock);
            StoreFounderDAL founder = StoreFounderDomainToDal(store.GetFounder());
            StoreState state = store.State;
            RatingDAL rating = RatingDomainToDal(store.Rating);
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
            return new StoreDAL(storeName, stock, messagesToStoreDAL, rating, managers, owners, founder, state, PrchasePolicyDomainToDal(store.GetPurchasePolicy()), DiscountPolicyDomainToDal(store.GetDiscountPolicy()));
        }

        private DiscountPolicyDAL DiscountPolicyDomainToDal(DiscountPolicy discountPolicy)
        {
            List<DiscountDAL> disList = new List<DiscountDAL>();
            foreach (Discount dis in discountPolicy.Discounts.DiscountList)
                disList.Add(DiscountDomainToDal(dis));
            return new DiscountPolicyDAL(disList);
        }

        private PurchasePolicyDAL PrchasePolicyDomainToDal(PurchasePolicy purchasePolicy)
        {
            List<ConditionDAL> conditions = new List<ConditionDAL>();
            foreach(Condition cond in purchasePolicy.Conditions.ConditionList)
                conditions.Add(ConditionDomainToDal(cond));
            PurchasePolicyDAL ppDal = new PurchasePolicyDAL(conditions);
            return ppDal;
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

        public StockDAL StockDomainToDal(Stock stock)
        {
            ICollection<StockItemDAL> itemAndAmount = new List<StockItemDAL>();
            foreach (KeyValuePair<Item, int> i_a in stock.Items)
            {
                itemAndAmount.Add(new StockItemDAL(ItemDomainToDal(i_a.Key), i_a.Value));
            }
            return new StockDAL(itemAndAmount);
        }

        public StoreManagerDAL StoreManagerDomainToDal(StoreManager manager)
        {
            return new StoreManagerDAL(manager.StoreName, manager.Appointer, manager.Username);
        }

        public StoreOwnerDAL StoreOwnerDomainToDal(StoreOwner owner)
        {
            return new StoreOwnerDAL(owner.StoreName, owner.Appointer, owner.Username);
        }
        public StoreFounderDAL StoreFounderDomainToDal(StoreFounder founder)
        {
            return new StoreFounderDAL(founder.StoreName, founder.Username);
        }
        public MessageToStoreDAL MessageToStoreDomainToDAL(MessageToStore messageToStore)
        {
            return new MessageToStoreDAL(messageToStore.Id, messageToStore.StoreName,
                messageToStore.SenderUsername, messageToStore.Message, 
                messageToStore.Title, messageToStore.Reply, messageToStore.Replier);
        }

        public ItemDAL ItemDomainToDal(Item itemDomain)
        {
            return new ItemDAL(RatingDomainToDal(itemDomain.Rating), itemDomain.Name,
                itemDomain._price, itemDomain.Description, itemDomain.Category);
        }
        public RatingDAL RatingDomainToDal(Rating ratingDomain)
        {
            ICollection<RateDAL> rateSet = new List<RateDAL>();

            foreach (KeyValuePair<string, Tuple<int, string>> rate in ratingDomain.Ratings)
            {
                rateSet.Add(new RateDAL(rate.Key, rate.Value.Item1, rate.Value.Item2));
            }
            return new RatingDAL(rateSet);
        }

        public Store StoreDalToDomain(StoreDAL storeDAL)
        {
            Stock stock = StockDalToDomain(storeDAL._stock);
            PurchasePolicy purchasePolicy = PurchasePolicyDalToDomain(storeDAL._purchasePolicy);
            DiscountPolicy discountPolicy = DiscountPolicyDalToDomain(storeDAL._discountPolicy);
            StoreFounder founder = StoreFounderDalToDomain(storeDAL._founder);
            String storeName = storeDAL._storeName;
            StoreState state = storeDAL._state;
            Rating rating = RatingDalToDomain(storeDAL._rating);
            List<MessageToStore> messagesToStore = new List<MessageToStore>();
            foreach (MessageToStoreDAL messageToStoreDAL in storeDAL._messagesToStore)
            {
                messagesToStore.Add(MessageToStoreDALToDomain(messageToStoreDAL));
            }
            List<StoreManager> managers = new List<StoreManager>();
            foreach (StoreManagerDAL manager in storeDAL._managers)
            {
                managers.Add(StoreManagerDalToDomain(manager));
            }
            List<StoreOwner> owners = new List<StoreOwner>();
            foreach (StoreOwnerDAL owner in storeDAL._owners)
            {
                owners.Add(StoreOwnerDalToDomain(owner));
            }

            return new Store(stock, purchasePolicy, discountPolicy, messagesToStore, rating
                , managers, owners, founder,storeName ,state);
        }

        private DiscountPolicy DiscountPolicyDalToDomain(DiscountPolicyDAL discountPolicy)
        {
            throw new NotImplementedException();
        }

        private PurchasePolicy PurchasePolicyDalToDomain(PurchasePolicyDAL purchasePolicy)
        {
            throw new NotImplementedException();
        }

        public MessageToStore MessageToStoreDALToDomain(MessageToStoreDAL msg)
        {
            return new MessageToStore(msg._storeName, msg._senderUsername, 
                msg._title, msg._message, msg._reply, msg._replierFromStore, msg.mid);
        }
        public StoreFounder StoreFounderDalToDomain(StoreFounderDAL founderDAL)
        {
            return new StoreFounder(founderDAL.ConvertToSet(), founderDAL._username, founderDAL._storeName);
        }
        public StoreManager StoreManagerDalToDomain(StoreManagerDAL managerDAL)
        {
            return new StoreManager(managerDAL._appointer, managerDAL.ConvertToSet(), managerDAL._username, managerDAL._storeName);        
        }
        public StoreOwner StoreOwnerDalToDomain(StoreOwnerDAL ownerDAL)
        {
            return new StoreOwner(ownerDAL._appointer, ownerDAL.ConvertToSet(), ownerDAL._username, ownerDAL._storeName);
        }
        public Stock StockDalToDomain(StockDAL stockDAL)
        {
            Dictionary<Item, int> itemAndAmount = new Dictionary<Item, int>();  
            foreach (StockItemDAL stockItem in stockDAL._itemAndAmount)
            {
                itemAndAmount.Add(ItemDalToDomain(stockItem.item), stockItem.amount);
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
        public Rating RatingDalToDomain(RatingDAL ratingDAL)
        {
            Dictionary<string, Tuple<int, string>> ratings = new Dictionary<string, Tuple<int, string>>();
            foreach (RateDAL rate in ratingDAL._ratings)
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
            return new ShoppingCart(baskets);
        }
        public ShoppingBasket ShoppingBasketDALToDomain(ShoppingBasketDAL basketDAL)
        {
            IDictionary<Item, DiscountDetails> items = new Dictionary<Item, DiscountDetails>();
            foreach(KeyValuePair<ItemDAL, PurchaseDetailsDAL> i_a in basketDAL.ConvertToDictionary())
            {
                items.Add(ItemDalToDomain(i_a.Key), PurchaseDetailsDALToDomain(i_a.Value));
            }
            return new ShoppingBasket(StoreDalToDomain(basketDAL._store), items);
        }

        private DiscountDetails PurchaseDetailsDALToDomain(PurchaseDetailsDAL value)
        {
            ISet<AtomicDiscount> disList = new HashSet<AtomicDiscount>();
            foreach (AtomicDiscountDAL dis in value.discountList)
                disList.Add(AtomicDiscountDalToDomain(dis));
            DiscountDetails details = new DiscountDetails(value.amount, disList);
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
            foreach(MessageToStoreDAL repliedMessage in registeredDAL._repliedMessages)
            {
                repliedMessages.Add(MessageToStoreDalToDomain(repliedMessage));
            }
            ICollection<SystemRole> roles = new List<SystemRole>();
            foreach(SystemRoleDAL role in registeredDAL._roles)
            {
                roles.Add(SystemRoleDalToDomain(role));
            }
            IDictionary<int, Complaint> filedComplaints = new Dictionary<int, Complaint>();
            foreach (ComplaintDAL id_complaint in registeredDAL._filedComplaints)
                filedComplaints.Add(id_complaint._id, ComplaintDalToDomain(id_complaint));
            return new Registered(adminMessages, notifications, repliedMessages, username, password, 
                salt, birthDate,roles, filedComplaints, cart);
        }

        public AdminMessageToRegistered AdminMessageDalToDomain(AdminMessageToRegisteredDAL msgDal)
        {
           return new AdminMessageToRegistered(msgDal.mid,msgDal._receiverUsername, msgDal._senderUsername, msgDal._title, msgDal._message);
        }
        public NotifyMessage NotifyMessageDalToDomain(NotifyMessageDAL notifyDAL)
        {
            return new NotifyMessage(notifyDAL.mid, notifyDAL._storeName, notifyDAL._title, notifyDAL._message, notifyDAL._receiverUsername);
        }
        public MessageToStore MessageToStoreDalToDomain(MessageToStoreDAL msgDAL)
        {
            return new MessageToStore(msgDAL._storeName, msgDAL._senderUsername, msgDAL._title,
                msgDAL._message, msgDAL._reply, msgDAL._replierFromStore, msgDAL.mid);
        }
        public Complaint ComplaintDalToDomain(ComplaintDAL complaintDAL)
        {//int id, Registered complainer, int cartID, string message
            return new Complaint(complaintDAL._id,complaintDAL._complainer
                , complaintDAL._cartID, complaintDAL._message);
        }
        public SystemRole SystemRoleDalToDomain(SystemRoleDAL systemRoleDAL)
        {

            if (systemRoleDAL is SystemAdminDAL)
            {
                SystemAdminDAL systemAdminDAL = (SystemAdminDAL)systemRoleDAL;
                return new SystemAdmin(systemAdminDAL._username, systemAdminDAL.ConvertToSet());
            }
            if (systemRoleDAL is StoreFounderDAL)
            {
                StoreFounderDAL storeFounderDAL = (StoreFounderDAL)systemRoleDAL;
                return new StoreFounder(storeFounderDAL.ConvertToSet(), storeFounderDAL._username, storeFounderDAL._storeName);
            }
            if(systemRoleDAL is StoreOwnerDAL)
            {
                StoreOwnerDAL storeOwnerDAL = (StoreOwnerDAL)systemRoleDAL;
                return new StoreOwner(storeOwnerDAL._appointer, storeOwnerDAL.ConvertToSet(), storeOwnerDAL._username, storeOwnerDAL._storeName);
            }
            if (systemRoleDAL is StoreManagerDAL)
            {
                StoreManagerDAL storeManagerDAL = (StoreManagerDAL)systemRoleDAL;
                return new StoreManager(storeManagerDAL._appointer, storeManagerDAL.ConvertToSet(), storeManagerDAL._username, storeManagerDAL._storeName);
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
            IDictionary<ItemDAL, PurchaseDetailsDAL> items = new Dictionary<ItemDAL, PurchaseDetailsDAL>();
            foreach (KeyValuePair<Item, DiscountDetails> i_a in basket.Items)
            {
                items.Add(ItemDomainToDal(i_a.Key), PurchaseDetailsDomainToDal(i_a.Key.ItemID, i_a.Value));
            }
            return new ShoppingBasketDAL(StoreDomainToDal(basket._store), items);
        }

        private PurchaseDetailsDAL PurchaseDetailsDomainToDal(int itemID, DiscountDetails value)
        {
            List<AtomicDiscountDAL> disList = new List<AtomicDiscountDAL>();
            foreach (AtomicDiscount dis in value.DiscountList)
                disList.Add(AtomicDiscountDomainToDal(dis));
            return new PurchaseDetailsDAL(itemID, value.Amount, disList);
        }

        public Dictionary<string, Store> StoreListDalToDomain(ICollection<StoreDAL> storeDALs)
        {
            Dictionary<string, Store> stores = new Dictionary<string, Store>();
            foreach (StoreDAL storeDAL in storeDALs)
                stores.Add(storeDAL._storeName,StoreDalToDomain(storeDAL));
            return stores;
        }
        public RegisteredDAL RegisteredDomainToDAL(Registered registered)
        {
            string username = registered.Username;
            string password = registered._password;
            string salt = registered._salt;
            DateTime birthDate = registered._birthDate;
            ShoppingCartDAL cart = CartDomainToDal(registered.ShoppingCart);
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
                adminMessages, notifications, repliedMessages);
            ICollection<ComplaintDAL> filedComplaints = new List<ComplaintDAL>();
            foreach (KeyValuePair<int, Complaint> id_complaint in registered.FiledComplaints)
                filedComplaints.Add(ComplaintDomainToDal(id_complaint.Value));
            reg._filedComplaints = filedComplaints;
            return reg;
        }

        public ComplaintDAL ComplaintDomainToDal(Complaint value)
        {
            return new ComplaintDAL(value.ID, value._complainer, value.CartID, value.Message, value.Response);
        }

        public SystemRoleDAL SystemRoleDomainToDal(SystemRole role)
        {
            if (role is SystemAdmin)
            {
                SystemAdmin systemAdmin = (SystemAdmin)role;
                return new SystemAdminDAL(systemAdmin.operations, systemAdmin.Username,  "");
            }
            if (role is StoreFounder)
            {
                StoreFounder storeFounder = (StoreFounder)role;
                return new StoreFounderDAL(storeFounder.operations, storeFounder.Username, storeFounder.StoreName); ;
            }
            if (role is StoreOwner)
            {
                StoreOwner storeOwner = (StoreOwner)role;
                return new StoreOwnerDAL(storeOwner.operations, storeOwner.Username, storeOwner.StoreName, storeOwner.Appointer);
            }
            if (role is StoreManager)
            {
                StoreManager storeManager = (StoreManager)role;
                return new StoreManagerDAL(storeManager.operations, storeManager.Username, storeManager.StoreName, storeManager.Appointer);
            }
            else
                throw new Exception("can;t happen");
        }

        public MessageToStoreDAL MessageToStoreDomainToDal(MessageToStore repliedMessage)
        {
            return new MessageToStoreDAL(repliedMessage.Id, repliedMessage.StoreName, repliedMessage.SenderUsername,
                repliedMessage.Message, repliedMessage.Title, repliedMessage.Reply, repliedMessage.Replier);
        }

        public NotifyMessageDAL NotifyMessageDomainToDal(NotifyMessage notification)
        {
            return new NotifyMessageDAL(notification.Id, notification.StoreName, notification.Title,
                notification.Message, notification.ReceiverUsername);
        }

        public AdminMessageToRegisteredDAL AdminMessageDomainToDal(AdminMessageToRegistered adminMessage)
        {
            return new AdminMessageToRegisteredDAL(adminMessage.id, adminMessage.ReceiverUsername, adminMessage.SenderUsername, adminMessage.Title, adminMessage.Message);
        }
    }
}
