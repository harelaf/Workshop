using MarketWeb.Server.DataLayer;
using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;
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
            IDictionary<ItemDAL, int> items = new Dictionary<ItemDAL, int>();
            foreach (Item item in basketDomain._items.Keys)
                items.Add(ItemDomainToDal(item), basketDomain._items[item]);
            return new ShoppingBasketDAL(StoreDomainToDal(basketDomain._store), items);
        }

        private StoreDAL StoreDomainToDal(Store store)
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
            return new StoreDAL(storeName, stock, messagesToStoreDAL, rating, managers, owners, founder, state);
        }

        private StockDAL StockDomainToDal(Stock stock)
        {
            Dictionary<ItemDAL, int> itemAndAmount = new Dictionary<ItemDAL, int>();
            foreach (KeyValuePair<Item, int> i_a in stock.Items)
            {
                itemAndAmount.Add(ItemDomainToDal(i_a.Key), i_a.Value);
            }
            return new StockDAL(itemAndAmount);
        }

        private StoreManagerDAL StoreManagerDomainToDal(StoreManager manager)
        {
            return new StoreManagerDAL(manager.StoreName, manager.Appointer, manager.Username);
        }

        private StoreOwnerDAL StoreOwnerDomainToDal(StoreOwner owner)
        {
            return new StoreOwnerDAL(owner.StoreName, owner.Appointer, owner.Username);
        }
        private StoreFounderDAL StoreFounderDomainToDal(StoreFounder founder)
        {
            return new StoreFounderDAL(founder.StoreName, founder.Username);
        }
        private MessageToStoreDAL MessageToStoreDomainToDAL(MessageToStore messageToStore)
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
            PurchasePolicy purchasePolicy = null;// ron&avishi
            DiscountPolicy discountPolicy = null;// ron&avishi
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
        public MessageToStore MessageToStoreDALToDomain(MessageToStoreDAL msg)
        {
            return new MessageToStore(msg._storeName, msg._senderUsername, 
                msg._title, msg._message, msg._reply, msg._replierFromStore, msg.mid);
        }
        public StoreFounder StoreFounderDalToDomain(StoreFounderDAL founderDAL)
        {
            return new StoreFounder(founderDAL._operations ,founderDAL._username, founderDAL._storeName);
        }
        public StoreManager StoreManagerDalToDomain(StoreManagerDAL managerDAL)
        {
            return new StoreManager(managerDAL._appointer, managerDAL._operations, managerDAL._username, managerDAL._storeName);        
        }
        public StoreOwner StoreOwnerDalToDomain(StoreOwnerDAL ownerDAL)
        {
            return new StoreOwner(ownerDAL._appointer, ownerDAL._operations, ownerDAL._username, ownerDAL._storeName);
        }
        public Stock StockDalToDomain(StockDAL stockDAL)
        {
            Dictionary<Item, int> itemAndAmount = new Dictionary<Item, int>();  
            foreach (KeyValuePair<ItemDAL, int> i_a in stockDAL._itemAndAmount)
            {
                itemAndAmount.Add(ItemDalToDomain(i_a.Key), i_a.Value);
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
            IDictionary<Item, int> items = new Dictionary<Item, int>();
            foreach(KeyValuePair<ItemDAL, int> i_a in basketDAL._items)
            {
                items.Add(ItemDalToDomain(i_a.Key), i_a.Value);
            }
            return new ShoppingBasket(StoreDalToDomain(basketDAL._store), items);
        }
        public Registered RegisteredDALToDomain(RegisteredDAL registeredDAL)
        {
            string username = registeredDAL._username;
            string password = registeredDAL._password;
            string salt = registeredDAL._salt;
            DateTime birthDate = registeredDAL._birthDate;
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
            foreach (KeyValuePair<int, ComplaintDAL> id_complaint in registeredDAL._filedComplaints)
                filedComplaints.Add(id_complaint.Key, ComplaintDalToDomain(id_complaint.Value));
            return new Registered(adminMessages, notifications, repliedMessages, username, password, 
                salt, birthDate,roles,  filedComplaints);
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
            return new Complaint(complaintDAL._id, RegisteredDALToDomain(complaintDAL._complainer)
                , complaintDAL._cartID, complaintDAL._message);
        }
        public SystemRole SystemRoleDalToDomain(SystemRoleDAL systemRoleDAL)
        {

            if (systemRoleDAL is SystemAdminDAL)
            {
                SystemAdminDAL systemAdminDAL = (SystemAdminDAL)systemRoleDAL;
                IDictionary<int, Complaint> complaints = new Dictionary<int, Complaint>();
                foreach (KeyValuePair<int, ComplaintDAL> id_complaint in systemAdminDAL._receivedComplaints)
                    complaints.Add(id_complaint.Key, ComplaintDalToDomain(id_complaint.Value));
                return new SystemAdmin(systemAdminDAL._username, systemAdminDAL._operations, complaints);
            }
            if (systemRoleDAL is StoreFounderDAL)
            {
                StoreFounderDAL storeFounderDAL = (StoreFounderDAL)systemRoleDAL;
                return new StoreFounder(storeFounderDAL._operations, storeFounderDAL._username, storeFounderDAL._storeName);
            }
            if(systemRoleDAL is StoreOwnerDAL)
            {
                StoreOwnerDAL storeOwnerDAL = (StoreOwnerDAL)systemRoleDAL;
                return new StoreOwner(storeOwnerDAL._appointer, storeOwnerDAL._operations, storeOwnerDAL._username, storeOwnerDAL._storeName);
            }
            if (systemRoleDAL is StoreManagerDAL)
            {
                StoreManagerDAL storeManagerDAL = (StoreManagerDAL)systemRoleDAL;
                return new StoreManager(storeManagerDAL._appointer, storeManagerDAL._operations, storeManagerDAL._username, storeManagerDAL._storeName);
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

        private ShoppingCartDAL ShoppingCartDomainToDAL(ShoppingCart cart)
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

        private ShoppingBasketDAL ShoppingBasketDomainToDAL(ShoppingBasket basket)
        {
            IDictionary<ItemDAL, int> items = new Dictionary<ItemDAL, int>();
            foreach (KeyValuePair<Item, int> i_a in basket.Items)
            {
                items.Add(ItemDomainToDal(i_a.Key), i_a.Value);
            }
            return new ShoppingBasketDAL(StoreDomainToDal(basket._store), items);
        }
    }
}
