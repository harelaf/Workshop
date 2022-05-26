using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Shared.DTO;
using MarketWeb.Shared;
using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;
using MarketWeb.Server.Domain;

namespace MarketWeb.Service
{
    public static class DTOtranslator
    {

        //private static DTOtranslator instance = null;
        //private DTOtranslator() {}
        //public static static object getInstance()
        //{
        //    if(instance == null)
        //        lock (instance)
        //        {
        //            if (instance == null)
        //                instance = new DTOtranslator();
        //        }
        //    return instance;
        //}
        public static DiscountCondition translateCondition(IConditionDTO cond)
        {
            if (cond == null)
			{
                return null;
			}
            Type type = cond.GetType();
            if(type.Equals(typeof(AndCompositionDTO)))
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
        public static Discount translateDiscount(IDiscountDTO dis)
        {
            Type type = dis.GetType();
            if (type.Equals(typeof(AllProductsDiscountDTO)))
                return translate((AllProductsDiscountDTO)dis);
            if (type.Equals(typeof(CategoryDiscountDTO)))
                return translate((CategoryDiscountDTO)dis);
            if (type.Equals(typeof(ItemDiscountDTO)))
                return translate((ItemDiscountDTO)dis);
            if (type.Equals(typeof(MaxDiscountDTO)))
                return translate((MaxDiscountDTO)dis);
            if (type.Equals(typeof(NumericDiscountDTO)))
                return translate((NumericDiscountDTO)dis);
            if (type.Equals(typeof(PlusDiscountDTO)))
                return translate((PlusDiscountDTO)dis);
            else throw new NotImplementedException($"need an implementation for {type} discount type.");
        }
        public static AndComposition translate(AndCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new AndComposition(negative, conditions);
        }
        public static DayOnWeekCondition translate(DayOnWeekConditionDTO condition_dto)
        {
            String day = condition_dto.DayOnWeek;
            bool negative = condition_dto.Negative;
            return new DayOnWeekCondition(day, negative);
        }
        public static DiscountCondition translate(HourConditionDTO condition_dto)
        {
            int minHour = condition_dto.MinHour;
            int maxHour = condition_dto.MaxHour;
            bool negative = condition_dto.Negative;
            return new HourCondition(minHour, maxHour, negative);
        }
        public static DiscountCondition translate(OrCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new OrComposition(negative, conditions);
        }
        public static DiscountCondition translate(PriceableConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new PriceableCondition(keyWord, minAmount, maxAmount, negative);
        }
        public static DiscountCondition translate(SearchCategoryConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchCategoryCondition(keyWord, minAmount, maxAmount, negative);
        }
        public static DiscountCondition translate(SearchItemConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchItemCondition(keyWord, minAmount, maxAmount, negative);
        }
        public static DiscountCondition translate(XorCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new XorComposition(negative, conditions);
        }
        public static Discount translate(AllProductsDiscountDTO discount_dto)
        {
            double percentage = discount_dto.Percentage;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new AllProductsDiscount(percentage, condition, expiration);
        }
        public static Discount translate(CategoryDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.Percentage_to_subtract;
            String category = discount_dto.Category;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new CategoryDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public static Discount translate(ItemDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.PercentageToSubtract;
            String category = discount_dto.ItemName;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new ItemDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public static Discount translate(MaxDiscountDTO discount_dto)
        {
            List<Discount> discount_list = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discount_list.Add(translateDiscount(discountDTO));
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            return new MaxDiscount(discount_list, condition);
        }
        public static Discount translate(NumericDiscountDTO discount_dto)
        {
            double priceToSubtract = discount_dto.PriceToSubtract;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new NumericDiscount(priceToSubtract, condition, expiration);
        }
        public static Discount translate(PlusDiscountDTO discount_dto)
        {
            List<Discount> discounts = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discounts.Add(translateDiscount(discountDTO));
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            return new PlusDiscount(discounts, condition);
        }
        public static AdminMessageToRegisteredDTO toDTO(AdminMessageToRegistered msg)
        {
            return new AdminMessageToRegisteredDTO(msg.ReceiverUsername, msg.SenderUsername, msg.Title, msg.Message);
        }

        public static ItemDTO toDTO(Item itm, String storeName)
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
        public static RatingDTO toDTO(Rating rate)
        {
            return new RatingDTO(rate.Ratings);
        }
        public static MessageToStoreDTO toDTO(MessageToStore messageToStore)
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
        public static NotifyMessageDTO toDTO(NotifyMessage notifyMessage)
        {
            return new NotifyMessageDTO(
                notifyMessage.StoreName,
                notifyMessage.Title,
                notifyMessage.Message,
                notifyMessage.ReceiverUsername,
                notifyMessage.Id);
        }
        public static PurchasedCartDTO toDTO(DateTime date, ShoppingCart shoppingCart)
        {
            return new PurchasedCartDTO(date, toDTO(shoppingCart));
        }
        public static RegisteredDTO toDTO(Registered registered)
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
            foreach (KeyValuePair<int, Complaint> row in registered.FiledComplaints)
                filedComplaints[row.Key] = toDTO(row.Value);
            return new RegisteredDTO(
                registered.Username,
                toDTO(registered.ShoppingCart),
                adminMessages,
                notifications,
                repliedMessages,
                filedComplaints,
                registered._birthDate);
        }
        public static ComplaintDTO toDTO(Complaint complaint)
        {
            return new ComplaintDTO(complaint.ID, complaint.CartID, complaint.GetComplainer(), complaint.Message, complaint.Response);
        }
        public static ShoppingBasketDTO toDTO(ShoppingBasket shoppingBasket)
        {
            Dictionary<int, Tuple<ItemDTO, int>> items = new Dictionary<int, Tuple<ItemDTO, int>>();
            foreach (Item key in shoppingBasket.Items.Keys)
            {
                ItemDTO dto = toDTO(key, shoppingBasket._store.StoreName);
                items[dto.ItemID] = new Tuple<ItemDTO, int>(dto, shoppingBasket.Items[key]);
            }
           
            return new ShoppingBasketDTO(shoppingBasket.Store().StoreName, items);
        }
        public static ShoppingCartDTO toDTO(ShoppingCart shoppingCart)
        {
            ICollection<ShoppingBasketDTO> _DTObaskets = new List<ShoppingBasketDTO>();
            foreach (ShoppingBasket basket in shoppingCart._shoppingBaskets)
                _DTObaskets.Add(toDTO(basket));
            return new ShoppingCartDTO(_DTObaskets);
        }
        public static StockDTO toDTO(Stock stock, string storeName)
        {
            Dictionary<int, Tuple<ItemDTO, int>> itemAndAmount = new Dictionary<int, Tuple<ItemDTO, int>>();
            foreach (Item key in stock.Items.Keys)
            {
                ItemDTO dto = toDTO(key, storeName);
                itemAndAmount[dto.ItemID] = new Tuple<ItemDTO, int>(dto, stock.Items[key]);
            }
            return new StockDTO(itemAndAmount);
        }
        public static StoreDTO toDTO(Store store)
        {
            List<MessageToStoreDTO> messagesToStore = new List<MessageToStoreDTO>();
            foreach (MessageToStore msg in store.MessagesToStore)
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
        public static StoreFounderDTO toDTO(StoreFounder storeFounder)
        {
            return new StoreFounderDTO(storeFounder.Username, storeFounder.StoreName);
        }
        public static StoreManagerDTO toDTO(StoreManager storeManager)
        {
            return new StoreManagerDTO(
                new HashSet<Operation>(storeManager.operations),
                storeManager.Username,
                storeManager.StoreName,
                storeManager.Appointer);
        }
        public static StoreOwnerDTO toDTO(StoreOwner storeOwner)
        {
            return new StoreOwnerDTO(
                storeOwner.operations,
                storeOwner.Username,
                storeOwner.StoreName,
                storeOwner.Appointer);
        }
        public static PurchasePolicyDTO toDTO(PurchasePolicy policy)
        {
            return new PurchasePolicyDTO();
        }
        public static DiscountPolicyDTO toDTO(DiscountPolicy policy)
        {
            return new DiscountPolicyDTO();
        }

        public static MessageToRegisteredDTO toDTO(MessageToRegistered MessageToRegistered)
        {

            return new MessageToRegisteredDTO(
                MessageToRegistered.StoreName,
                MessageToRegistered.Username,
                MessageToRegistered.Title,
                MessageToRegistered.Message);
        }
    }
}
