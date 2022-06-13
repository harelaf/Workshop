using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Shared.DTO;
using MarketWeb.Shared;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Server.Domain;

namespace MarketWeb.Service
{
    public class DTOtranslator
    {

        //private static DTOtranslator instance = null;
        //private DTOtranslator() {}
        //public static object getInstance()
        //{
        //    if(instance == null)
        //        lock (instance)
        //        {
        //            if (instance == null)
        //                instance = new DTOtranslator();
        //        }
        //    return instance;
        //}

        // ====================== from DTO translation for Conditions ======================
        public Condition translateCondition(IConditionDTO cond)
        {
            if (cond == null)
                return null;
            Type type = cond.GetType();
            if (type.Equals(typeof(AndCompositionDTO)))
                return translate((AndCompositionDTO)cond);
            if (type.Equals(typeof(DayOnWeekConditionDTO)))
                return translate((DayOnWeekConditionDTO)cond);
            if (type.Equals(typeof(HourConditionDTO)))
                return translate((HourConditionDTO)cond);
            if (type.Equals(typeof(OrCompositionDTO)))
                return translate((OrCompositionDTO)cond);
            if (type.Equals(typeof(PriceableConditionDTO)))
                return translate((PriceableConditionDTO)cond);
            if (type.Equals(typeof(SearchCategoryConditionDTO)))
                return translate((SearchCategoryConditionDTO)cond);
            if (type.Equals(typeof(SearchItemConditionDTO)))
                return translate((SearchItemConditionDTO)cond);
            if (type.Equals(typeof(XorCompositionDTO)))
                return translate((XorCompositionDTO)cond);
            else throw new NotImplementedException();
        }
        public IConditionDTO conditionToDTO(Condition cond)
        {
            if (cond == null)
                return null;
            Type type = cond.GetType();
            if (type.Equals(typeof(AndComposition)))
                return toDTO((AndComposition)cond);
            if (type.Equals(typeof(DayOnWeekCondition)))
                return toDTO((DayOnWeekCondition)cond);
            if (type.Equals(typeof(HourCondition)))
                return toDTO((HourCondition)cond);
            if (type.Equals(typeof(OrComposition)))
                return toDTO((OrComposition)cond);
            if (type.Equals(typeof(PriceableCondition)))
                return toDTO((PriceableCondition)cond);
            if (type.Equals(typeof(SearchCategoryCondition)))
                return toDTO((SearchCategoryCondition)cond);
            if (type.Equals(typeof(SearchItemCondition)))
                return toDTO((SearchItemCondition)cond);
            if (type.Equals(typeof(XorComposition)))
                return toDTO((XorComposition)cond);
            else throw new NotImplementedException();
        }

        private DayOnWeekConditionDTO toDTO(DayOnWeekCondition cond)
        {
            return new DayOnWeekConditionDTO(cond.DayOnWeek, cond.ToNegative);
        }

        private HourConditionDTO toDTO(HourCondition cond)
        {
            return new HourConditionDTO(cond.MinHour, cond.MaxHour, cond.ToNegative);
        }

        private OrCompositionDTO toDTO(OrComposition cond)
        {
            List<IConditionDTO> conditions = new List<IConditionDTO>();
            foreach (Condition innerCond in cond.ConditionList)
                conditions.Add(conditionToDTO(innerCond));
            return new OrCompositionDTO(cond.ToNegative, conditions);
        }

        private PriceableConditionDTO toDTO(PriceableCondition cond)
        {
            return new PriceableConditionDTO(cond.KeyWord, cond.MinValue, cond.MaxValue, cond.ToNegative);
        }

        private SearchCategoryConditionDTO toDTO(SearchCategoryCondition cond)
        {
            return new SearchCategoryConditionDTO(cond.KeyWord, cond.MinValue, cond.MaxValue, cond.ToNegative);
        }

        private SearchItemConditionDTO toDTO(SearchItemCondition cond)
        {
            return new SearchItemConditionDTO(cond.KeyWord, cond.MinValue, cond.MaxValue, cond.ToNegative);
        }

        private XorCompositionDTO toDTO(XorComposition cond)
        {
            List<IConditionDTO> conditions = new List<IConditionDTO>();
            foreach (Condition innerCond in cond.ConditionList)
                conditions.Add(conditionToDTO(innerCond));
            return new XorCompositionDTO(cond.ToNegative, conditions);
        }

        private AndCompositionDTO toDTO(AndComposition cond)
        {
            List<IConditionDTO> conditions = new List<IConditionDTO>();
            foreach (Condition innerCond in cond.ConditionList)
                conditions.Add(conditionToDTO(innerCond));
            return new AndCompositionDTO(cond.ToNegative, conditions);
        }

        public AndComposition translate(AndCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<Condition> conditions = new List<Condition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new AndComposition(negative, conditions);
        }
        public DayOnWeekCondition translate(DayOnWeekConditionDTO condition_dto)
        {
            String day = condition_dto.DayOnWeek;
            bool negative = condition_dto.Negative;
            return new DayOnWeekCondition(day, negative);
        }
        public Condition translate(HourConditionDTO condition_dto)
        {
            int minHour = condition_dto.MinHour;
            int maxHour = condition_dto.MaxHour;
            bool negative = condition_dto.Negative;
            return new HourCondition(minHour, maxHour, negative);
        }
        public Condition translate(OrCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<Condition> conditions = new List<Condition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new OrComposition(negative, conditions);
        }
        public Condition translate(PriceableConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new PriceableCondition(keyWord, minAmount, maxAmount, negative);
        }
        public Condition translate(SearchCategoryConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchCategoryCondition(keyWord, minAmount, maxAmount, negative);
        }
        public Condition translate(SearchItemConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchItemCondition(keyWord, minAmount, maxAmount, negative);
        }
        public Condition translate(XorCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<Condition> conditions = new List<Condition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new XorComposition(negative, conditions);
        }

        // ====================== from DTO translation for Discounts ======================
        public Discount translateDiscount(IDiscountDTO dis)
        {
            if (dis == null)
                return null;
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscountDTO)))
                return translate((AllProductsDiscountDTO)dis);
            if (type.Equals(typeof(CategoryDiscountDTO)))
                return translate((CategoryDiscountDTO)dis);
            if (type.Equals(typeof(ItemDiscountDTO)))
                return translate((ItemDiscountDTO)dis);
            if (type.Equals(typeof(MaxDiscountDTO)))
                return translate((MaxDiscountDTO)dis);
            //if (type.Equals(typeof(NumericDiscountDTO)))
            //    return translate((NumericDiscountDTO)dis);
            if (type.Equals(typeof(PlusDiscountDTO)))
                return translate((PlusDiscountDTO)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }
        public AtomicDiscountDTO discountToDTO(Discount dis)
        {
            if (dis == null)
                return null;
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscount)))
                return toDTO((AllProductsDiscount)dis);
            if (type.Equals(typeof(CategoryDiscount)))
                return toDTO((CategoryDiscount)dis);
            if (type.Equals(typeof(ItemDiscount)))
                return toDTO((ItemDiscount)dis);
            if (type.Equals(typeof(NumericDiscount)))
                return toDTO((NumericDiscount)dis);
            //if (type.Equals(typeof(MaxDiscount)))
            //    return toDTO((MaxDiscount)dis);
            //if (type.Equals(typeof(PlusDiscount)))
            //    return toDTO((PlusDiscount)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }
        public Discount translate(AllProductsDiscountDTO discount_dto)
        {
            double percentage = discount_dto.Percentage;
            Condition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new AllProductsDiscount(percentage, condition, expiration);
        }
        public Discount translate(CategoryDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.Percentage_to_subtract;
            String category = discount_dto.Category;
            Condition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new CategoryDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount translate(ItemDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.PercentageToSubtract;
            String category = discount_dto.ItemName;
            Condition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new ItemDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount translate(MaxDiscountDTO discount_dto)
        {
            List<Discount> discount_list = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discount_list.Add(translateDiscount(discountDTO));
            Condition condition = translateCondition(discount_dto.Condition);
            return new MaxDiscount(discount_list, condition);
        }
        //public Discount translate(NumericDiscountDTO discount_dto)
        //{
        //    double priceToSubtract = discount_dto.PriceToSubtract;
        //    Condition condition = translateCondition(discount_dto.Condition);
        //    DateTime expiration = discount_dto.Expiration;
        //    return new NumericDiscount(priceToSubtract, condition, expiration);
        //}
        public Discount translate(PlusDiscountDTO discount_dto)
        {
            List<Discount> discounts = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discounts.Add(translateDiscount(discountDTO));
            Condition condition = translateCondition(discount_dto.Condition);
            return new PlusDiscount(discounts, condition);
        }

        // ====================== to DTO convertion for all dto's ======================
        public AdminMessageToRegisteredDTO toDTO(AdminMessageToRegistered msg)
        {
            return new AdminMessageToRegisteredDTO(msg.ReceiverUsername, msg.SenderUsername, msg.Title, msg.Message);
        }
        public ItemDTO toDTO(Item itm, String storeName)
        {
            return new ItemDTO(
                itm.ItemID,
                itm.Name,
                itm._price,
                itm.Description,
                itm.Category,
                toDTO(itm.Rating),
                storeName);
        }
        public RatingDTO toDTO(Rating rate)
        {
            return new RatingDTO(rate.Ratings);
        }
        public MessageToStoreDTO toDTO(MessageToStore messageToStore)
        {
            return new MessageToStoreDTO(
                messageToStore.StoreName,
                messageToStore.SenderUsername,
                messageToStore.Title,
                messageToStore.Message,
                messageToStore.Reply,
                messageToStore.Replier,
                messageToStore.Id);
        }
        public NotifyMessageDTO toDTO(NotifyMessage notifyMessage)
        {
            return new NotifyMessageDTO(
                notifyMessage.StoreName,
                notifyMessage.Title,
                notifyMessage.Message,
                notifyMessage.ReceiverUsername,
                notifyMessage.Id);
        }
        public PurchasedCartDTO toDTO(DateTime date, ShoppingCart shoppingCart)
        {
            return new PurchasedCartDTO(date, toDTO(shoppingCart));
        }
        public RegisteredDTO toDTO(Registered registered)
        {
            ICollection<AdminMessageToRegisteredDTO> adminMessages = new List<AdminMessageToRegisteredDTO>();
            ICollection<MessageToStoreDTO> repliedMessages = new List<MessageToStoreDTO>();
            ICollection<NotifyMessageDTO> notifications = new List<NotifyMessageDTO>();
            IDictionary<int, ComplaintDTO> filedComplaints = new Dictionary<int, ComplaintDTO>();
            foreach (AdminMessageToRegistered msg in registered.AdminMessages)
                adminMessages.Add(toDTO(msg));
            foreach (MessageToStore msg in registered.messageToStores)
                repliedMessages.Add(toDTO(msg));
            foreach (NotifyMessage msg in registered.Notifcations)
                notifications.Add(toDTO(msg));
            foreach (KeyValuePair<int, Complaint> pair in registered.FiledComplaints)
                filedComplaints.Add(pair.Key, toDTO(pair.Value));
            return new RegisteredDTO(
                registered.Username,
                toDTO(registered.ShoppingCart),
                adminMessages,
                notifications,
                repliedMessages,
                filedComplaints,
                registered._birthDate);
        }

        public ComplaintDTO toDTO(Complaint msg)
        {
            return new ComplaintDTO(msg.ID, msg.CartID, msg.GetComplainer(), msg.Message, msg.Response);
        }

        public ShoppingBasketDTO toDTO(ShoppingBasket shoppingBasket)
        {
            Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> items = new Dictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>>();
            List<NumericDiscountDTO> additionalDiscounts = new List<NumericDiscountDTO>();
            List<BidDTO> biddedItems = new List<BidDTO>();
            foreach (Bid bid in shoppingBasket.BiddedItems)
                biddedItems.Add(toDTO(bid, shoppingBasket.Store().GetItem(bid.ItemID)._price));
            foreach (KeyValuePair<Item, DiscountDetails<AtomicDiscount>> entry in shoppingBasket.Items)
            {
                ItemDTO dto = toDTO(entry.Key, shoppingBasket.Store().StoreName);
                items[entry.Key.ItemID] = new Tuple<ItemDTO, DiscountDetailsDTO>(dto, toDTO(shoppingBasket, entry.Value, entry.Key._price));
            }
            foreach (NumericDiscount dis in shoppingBasket.AdditionalDiscounts.DiscountList)
                additionalDiscounts.Add(toDTO(dis));
            return new ShoppingBasketDTO(shoppingBasket.Store().StoreName, items, additionalDiscounts, biddedItems);
        }

        public DiscountDetailsDTO toDTO(ISearchablePriceable searchablePriceable, DiscountDetails<AtomicDiscount> discountDetails, double itemPrice)
        {
            List<String> disList = new List<String>();
            foreach (AtomicDiscount discount in discountDetails.DiscountList)
                disList.Add(discount.GetDiscountString(0));
            double actualPrice = discountDetails.calcPriceFromCurrPrice(searchablePriceable, itemPrice);
            return new DiscountDetailsDTO(
                discountDetails.Amount,
                disList,
                actualPrice);
        }

        public ItemDiscountDTO toDTO(ItemDiscount discount)
        {
            return new ItemDiscountDTO(discount.PercentageToSubtract, discount.ItemName, conditionToDTO(discount.Condition), discount.Expiration);
        }

        public AllProductsDiscountDTO toDTO(AllProductsDiscount discount)
        {
            return new AllProductsDiscountDTO(discount.PercentageToSubtract, conditionToDTO(discount.Condition), discount.Expiration);
        }
        public CategoryDiscountDTO toDTO(CategoryDiscount discount)
        {
            return new CategoryDiscountDTO(discount.PercentageToSubtract, discount.Category, conditionToDTO(discount.Condition), discount.Expiration);
        }
        public NumericDiscountDTO toDTO(NumericDiscount discount)
        {
            return new NumericDiscountDTO(discount.PriceToSubtract, conditionToDTO(discount.Condition), discount.Expiration);
        }
        public MaxDiscountDTO toDTO(MaxDiscount dis)
        {
            throw new NotImplementedException();
        }
        public ShoppingCartDTO toDTO(ShoppingCart shoppingCart)
        {
            ICollection<ShoppingBasketDTO> _DTObaskets = new List<ShoppingBasketDTO>();
            foreach (ShoppingBasket basket in shoppingCart._shoppingBaskets)
                _DTObaskets.Add(toDTO(basket));
            return new ShoppingCartDTO(_DTObaskets);
        }
        public StockDTO toDTO(Stock stock, String storeName)
        {
            Dictionary<int, Tuple<ItemDTO, int>> itemAndAmount = new Dictionary<int, Tuple<ItemDTO, int>>();
            foreach (KeyValuePair<Item, int> entry in stock.Items)
            {
                ItemDTO dto = toDTO(entry.Key, storeName);
                itemAndAmount[entry.Key.ItemID] = new Tuple<ItemDTO, int>(dto, entry.Value);
            }
            return new StockDTO(itemAndAmount);
        }
        public StoreDTO toDTO(Store store)
        {
            List<MessageToStoreDTO> messagesToStore = new List<MessageToStoreDTO>();
            foreach (MessageToStore msg in store.MessagesToStore) //Might be in reverse order...
                messagesToStore.Add(toDTO(msg));
            List<StoreManagerDTO> managers = new List<StoreManagerDTO>();
            foreach (StoreManager manager in store.GetManagers())
                managers.Add(toDTO(manager));
            List<StoreOwnerDTO> owners = new List<StoreOwnerDTO>();
            foreach (StoreOwner owner in store.GetOwners())
                owners.Add(toDTO(owner));
            return new StoreDTO(
                store.StoreName,
                toDTO(store.GetFounder()),
                toDTO(store.GetPurchasePolicy()),
                toDTO(store.GetDiscountPolicy()),
                toDTO(store.Stock, store.StoreName),
                messagesToStore,
                toDTO(store.Rating),
                managers,
                owners,
                store.State);
        }
        public StoreFounderDTO toDTO(StoreFounder storeFounder)
        {
            return new StoreFounderDTO(storeFounder.StoreName, storeFounder.Username);
        }
        public StoreManagerDTO toDTO(StoreManager storeManager)
        {
            return new StoreManagerDTO(
                new HashSet<Operation>(storeManager.operations),
                storeManager.Username,
                storeManager.StoreName,
                storeManager.Appointer);
        }
        public StoreOwnerDTO toDTO(StoreOwner storeOwner)
        {
            return new StoreOwnerDTO(
                storeOwner.operations,
                storeOwner.StoreName,
                storeOwner.Username,
                storeOwner.Appointer);
        }
        public PurchasePolicyDTO toDTO(PurchasePolicy policy)
        {
            return new PurchasePolicyDTO();
        }
        public DiscountPolicyDTO toDTO(DiscountPolicy policy)
        {
            return new DiscountPolicyDTO(toDTO(policy.Discounts));
        }
        // to be implemented when needed
        public PlusDiscountDTO toDTO(PlusDiscount discounts)
        {
            //List<IDiscountDTO> discountDTOs = new List<IDiscountDTO>();
            //List<IConditionDTO> conditionDTOs = new List<IConditionDTO>();
            //foreach (Discount dis in discounts.DiscountList)
            //    discountDTOs.Add(toDTO(dis));
            return null;
        }
        public BidDTO toDTO(Bid bid, double originalPrice)
        {
            return new BidDTO(
                bid.Bidder,
                bid.ItemID,
                bid.Amount,
                bid.BiddedPrice,
                bid.CounterOffer,
                bid.Acceptors,
                originalPrice,
                bid.AccepttedByAll);
        }
    }
}