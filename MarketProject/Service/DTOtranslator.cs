using System;
using System.Collections.Generic;
using System.Text;
using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using MarketProject.Service.DTO;

namespace MarketProject.Service
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
        public DiscountCondition translateCondition(IConditionDTO cond)
        {
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
        public Discount translateDiscount(IDiscountDTO dis)
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
        public AndComposition translate(AndCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
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
        public DiscountCondition translate(HourConditionDTO condition_dto)
        {
            int minHour = condition_dto.MinHour;
            int maxHour = condition_dto.MaxHour;
            bool negative = condition_dto.Negative;
            return new HourCondition(minHour, maxHour, negative);
        }
        public DiscountCondition translate(OrCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new OrComposition(negative, conditions);
        }
        public DiscountCondition translate(PriceableConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new PriceableCondition(keyWord, minAmount, maxAmount, negative);
        }
        public DiscountCondition translate(SearchCategoryConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchCategoryCondition(keyWord, minAmount, maxAmount, negative);
        }
        public DiscountCondition translate(SearchItemConditionDTO condition_dto)
        {
            String keyWord = condition_dto.KeyWord;
            int minAmount = condition_dto.MinAmount;
            int maxAmount = condition_dto.MaxAmount;
            bool negative = condition_dto.Negative;
            return new SearchItemCondition(keyWord, minAmount, maxAmount, negative);
        }
        public DiscountCondition translate(XorCompositionDTO condition_dto)
        {
            bool negative = condition_dto.Negative;
            List<DiscountCondition> conditions = new List<DiscountCondition>();
            foreach (IConditionDTO cond in condition_dto.Conditions)
                conditions.Add(translateCondition(cond));
            return new XorComposition(negative, conditions);
        }
        public Discount translate(AllProductsDiscountDTO discount_dto)
        {
            double percentage = discount_dto.Percentage;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new AllProductsDiscount(percentage, condition, expiration);
        }
        public Discount translate(CategoryDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.Percentage_to_subtract;
            String category = discount_dto.Category;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new CategoryDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount translate(ItemDiscountDTO discount_dto)
        {
            double percentage_to_subtract = discount_dto.PercentageToSubtract;
            String category = discount_dto.ItemName;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new ItemDiscount(percentage_to_subtract, category, condition, expiration);
        }
        public Discount translate(MaxDiscountDTO discount_dto)
        {
            List<Discount> discount_list = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discount_list.Add(translateDiscount(discountDTO));
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            return new MaxDiscount(discount_list, condition);
        }
        public Discount translate(NumericDiscountDTO discount_dto)
        {
            double priceToSubtract = discount_dto.PriceToSubtract;
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            DateTime expiration = discount_dto.Expiration;
            return new NumericDiscount(priceToSubtract, condition, expiration);
        }
        public Discount translate(PlusDiscountDTO discount_dto)
        {
            List<Discount> discounts = new List<Discount>();
            foreach (IDiscountDTO discountDTO in discount_dto.Discounts)
                discounts.Add(translateDiscount(discountDTO));
            DiscountCondition condition = translateCondition(discount_dto.Condition);
            return new PlusDiscount(discounts, condition);
        }
        public AdminMessageToRegisteredDTO toDTO(AdminMessageToRegistered msg)
        {
            return new AdminMessageToRegisteredDTO(msg.ReceiverUsername, msg.SenderUsername, msg.Title, msg.Message);
        }
        public ItemDTO toDTO(Item itm)
        {
            return new ItemDTO(
                itm.ItemID,
                itm.Name,
                itm._price,
                itm.Description,
                itm.Category,
                toDTO(itm.Rating));
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
            foreach (AdminMessageToRegistered msg in registered.AdminMessages)
                adminMessages.Add(toDTO(msg));
            foreach (MessageToStore msg in registered.messageToStores)
                repliedMessages.Add(toDTO(msg));
            foreach (NotifyMessage msg in registered.Notifcations)
                notifications.Add(toDTO(msg));
            return new RegisteredDTO(
                registered.Username,
                toDTO(registered.ShoppingCart),
                adminMessages,
                notifications,
                repliedMessages);
        }
        public ShoppingBasketDTO toDTO(ShoppingBasket shoppingBasket)
        {
            Dictionary<ItemDTO, int> items = new Dictionary<ItemDTO, int>();
            foreach (KeyValuePair<Item, int> entry in shoppingBasket.Items)
            {
                ItemDTO dto = toDTO(entry.Key);
                items[dto] = entry.Value;
            }
            return new ShoppingBasketDTO(shoppingBasket.Store().StoreName, items);
        }
        public ShoppingCartDTO toDTO(ShoppingCart shoppingCart)
        {
            ICollection<ShoppingBasketDTO> _DTObaskets = new List<ShoppingBasketDTO>();
            foreach (ShoppingBasket basket in shoppingCart._shoppingBaskets)
                _DTObaskets.Add(toDTO(basket));
            return new ShoppingCartDTO(_DTObaskets);
        }
        public StockDTO toDTO(Stock stock)
        {
            Dictionary<ItemDTO, int> itemAndAmount = new Dictionary<ItemDTO, int>();
            foreach (KeyValuePair<Item, int> entry in stock.Items)
            {
                ItemDTO dto = toDTO(entry.Key);
                itemAndAmount[dto] = entry.Value;
            }
            return new StockDTO(itemAndAmount);
        }
        public StoreDTO toDTO(Store store)
        {
            Queue<MessageToStoreDTO> messagesToStore = new Queue<MessageToStoreDTO>();
            foreach (MessageToStore msg in store.MessagesToStore) //Might be in reverse order...
                messagesToStore.Enqueue(toDTO(msg));
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
                toDTO(store.Stock),
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
            return new DiscountPolicyDTO();
        }
    }
}
