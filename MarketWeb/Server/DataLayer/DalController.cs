
using MarketWeb.Server.Domain;
using MarketWeb.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketWeb.Server.DataLayer
{
    public class DalController
    {
        MarketContext context;

        public DalController()
        {
            context = new MarketContext();
        }
        public List<StoreDAL> GetAllActiveStores() 
        {
            return context.StoreDALs.Where(store => store._state == StoreState.Active).ToList();
        }
        public void Register(string Username, string password,string salt,  DateTime dob) 
        {
            context.RegisteredDALs.Add(
                new RegisteredDAL(Username, password, salt, dob));
            context.SaveChanges();
        }
        public void RemoveRegisteredVisitor(string username) 
        {
            RegisteredDAL toRemove = context.RegisteredDALs.Find(username);
            if (toRemove != null) 
            {
                context.RegisteredDALs.Remove(toRemove);
                context.SaveChanges();
            }    
            else
                throw new Exception("no such register in db");
        }
        public void AddItemToCart(int itemID, String storeName, int amount, string userName)
        {
            RegisteredDAL user = context.RegisteredDALs.Find(userName);
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            Dictionary<ItemDAL, int> itemsNamunt = storeDAL._stock._itemAndAmount;
            ItemDAL itemToAdd = null;
            foreach (ItemDAL item in itemsNamunt.Keys)
            {
                if (item._itemID == itemID)
                {
                    itemToAdd = item;
                    itemsNamunt[item] = itemsNamunt[item] - amount;
                }
                    
            }
            bool hasStoreBasket = false;
            ICollection<ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            foreach (ShoppingBasketDAL shoppingBasket in shoppingBasketDALs)
            {
                if(shoppingBasket._store._storeName == storeName)
                {
                    shoppingBasket._items.Add(itemToAdd, amount);
                    hasStoreBasket = true;
                }
            }
            if (!hasStoreBasket)
            {
                ShoppingBasketDAL basketDAL = new ShoppingBasketDAL(storeDAL, new Dictionary<ItemDAL, int>());
                basketDAL._items.Add(itemToAdd, amount);
            }
            context.SaveChanges();
        }
        public void RemoveItemFromCart(int itemID, String storeName, string userName)
        {
            RegisteredDAL user = context.RegisteredDALs.Find(userName);
            int amount = 0;
            ItemDAL itemToRemove = null;
            ICollection <ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            foreach (ShoppingBasketDAL shoppingBasket in shoppingBasketDALs)
            {
                if (shoppingBasket._store._storeName == storeName)
                {
                    IDictionary<ItemDAL, int> basket = shoppingBasket._items;
                    foreach (ItemDAL item in basket.Keys)
                    {
                        if (item._itemID == itemID)
                        {
                            amount = basket[item];
                            itemToRemove = item;
                            break;
                        }
                    }
                    basket.Remove(itemToRemove);
                    if(basket.Count <= 0)
                        shoppingBasketDALs.Remove(shoppingBasket); 
                }
            }
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            Dictionary<ItemDAL, int> itemsNamunt = storeDAL._stock._itemAndAmount;
            foreach (ItemDAL item in itemsNamunt.Keys)
            {
                if (item._itemID == itemID)
                {
                    itemsNamunt[item] = itemsNamunt[item] + amount;
                }

            }
            context.SaveChanges();
        }
        public void UpdateQuantityOfItemInCart(int itemID, String storeName, int newQuantity, string userName)
        {
            RegisteredDAL user = context.RegisteredDALs.Find(userName);
            int amountDiff = 0;
            ICollection<ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            foreach (ShoppingBasketDAL shoppingBasket in shoppingBasketDALs)
            {
                if (shoppingBasket._store._storeName == storeName)
                {
                    IDictionary<ItemDAL, int> basket = shoppingBasket._items;
                    foreach (ItemDAL item in basket.Keys)
                    {
                        if (item._itemID == itemID)
                        {
                            amountDiff =newQuantity- basket[item];
                            basket[item] = newQuantity;
                            break;
                        }
                    }
                }
            }
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            Dictionary<ItemDAL, int> itemsNamunt = storeDAL._stock._itemAndAmount;
            foreach (ItemDAL item in itemsNamunt.Keys)
            {
                if (item._itemID == itemID)
                {
                    itemsNamunt[item] = itemsNamunt[item] + amountDiff;
                }

            }
            context.SaveChanges();
        }
        //public ShoppingCartDAL ViewMyCart(){} ---- no need. the field is updated
        public void addStorePurchse(ShoppingBasketDAL basket, DateTime date, string storename)
        {
            PurchasedBasketDAL PurchasedBasket = new PurchasedBasketDAL(date, basket);
            StorePurchasedBasketDAL storePurchasedBasketDAL = context.StorePurchaseHistory.Find(storename);
            if (storePurchasedBasketDAL == null)
                storePurchasedBasketDAL = new StorePurchasedBasketDAL(storename);
            storePurchasedBasketDAL._PurchasedBaskets.Add(PurchasedBasket);
            context.SaveChanges();
        }
        public void addRegisteredPurchse(ShoppingCartDAL cart, DateTime date, string userName)
        {
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(userName);
            PurchasedCartDAL PurchasedCart = new PurchasedCartDAL(date, registeredDAL._cart);
            RegisteredPurchasedCartDAL registeredPurchasedCartDAL = context.RegisteredPurchaseHistory.Find(userName);
            if (registeredPurchasedCartDAL == null)
                registeredPurchasedCartDAL = new RegisteredPurchasedCartDAL(userName);

            registeredPurchasedCartDAL._PurchasedCarts.Add(PurchasedCart);
            registeredDAL._cart = new ShoppingCartDAL();
            context.SaveChanges();
        }
        public void OpenNewStore(String storeName, string founderName)
        {
            StoreDAL store = new StoreDAL(storeName, new StoreFounderDAL(storeName, founderName), StoreState.Active);
            context.StoreDALs.Add(store);   
            context.SaveChanges();
        }
        public void AddStoreManager(String managerUsername, String storeName, string appointer)
        {
            StoreManagerDAL storeManager = new StoreManagerDAL(managerUsername, storeName, appointer);
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            storeDAL._managers.Add(storeManager);
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(managerUsername);
            registeredDAL._roles.Add(storeManager);
            context.SaveChanges();  
        }
        public void AddStoreOwner(String ownerUsername, String storeName, string appointer)
        {
            StoreOwnerDAL storeOwner = new StoreOwnerDAL(ownerUsername, storeName, appointer);
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            storeDAL._owners.Add(storeOwner);
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(ownerUsername);
            registeredDAL._roles.Add(storeOwner);
            context.SaveChanges();
        }
        public void RemoveStoreOwner(String ownerUsername, String storeName)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            foreach (StoreOwnerDAL owner in storeDAL._owners)
            {
                if (owner._username == ownerUsername)
                {
                    storeDAL._owners.Remove(owner);
                    break;
                }
            }
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(ownerUsername);
            foreach (SystemRoleDAL role in registeredDAL._roles)
            {
                if (role._storeName == storeName)
                {
                    registeredDAL._roles.Remove(role);
                    break;
                }
            }
            context.SaveChanges();
        }
        public void RemoveStoreManager(String managerUsername, String storeName)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            foreach (StoreManagerDAL manager in storeDAL._managers)
            {
                if (manager._username == managerUsername)
                {
                    storeDAL._managers.Remove(manager);
                    break;
                }
            }
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(managerUsername);
            foreach (SystemRoleDAL role in registeredDAL._roles)
            {
                if (role._storeName == storeName)
                {
                    registeredDAL._roles.Remove(role);
                    break;
                }
            }
            context.SaveChanges();
        }
        public void AddItemToStoreStock(String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            ItemDAL item = new ItemDAL(new RatingDAL(new List<RateDAL>()), name, price, description, category);
            storeDAL._stock._itemAndAmount.Add(item, quantity);
            context.SaveChanges();
        }
        public void RemoveItemFromStore(String storeName, int itemID)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            foreach (ItemDAL item in storeDAL._stock._itemAndAmount.Keys)
            {
                if(item._itemID == itemID)
                {
                    storeDAL._stock._itemAndAmount.Remove(item);
                    break;
                }
            }
            context.SaveChanges();
        }
        public void UpdateStockQuantityOfItem(String storeName, int itemID, int newQuantity)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            foreach (ItemDAL item in storeDAL._stock._itemAndAmount.Keys)
            {
                if (item._itemID == itemID)
                {
                    storeDAL._stock._itemAndAmount[item] = newQuantity;
                    break;
                }
            }
            context.SaveChanges();
        }
        public void EditItemPrice(String storeName, int itemID, double newPrice)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            foreach (ItemDAL item in storeDAL._stock._itemAndAmount.Keys)
            {
                if (item._itemID == itemID)
                {
                    item._price = newPrice; 
                    break;
                }
            }
            context.SaveChanges();
        }
        public void EditItemName(String storeName, int itemID, String newName)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            foreach (ItemDAL item in storeDAL._stock._itemAndAmount.Keys)
            {
                if (item._itemID == itemID)
                {
                    item._name = newName;
                    break;
                }
            }
            context.SaveChanges();
        }
        public void EditItemDescription(String storeName, int itemID, String newDescription)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            foreach (ItemDAL item in storeDAL._stock._itemAndAmount.Keys)
            {
                if (item._itemID == itemID)
                {
                    item._description = newDescription;
                    break;
                }
            }
            context.SaveChanges();
        }
        public void RateItem(int itemID, String storeName, int rating, String review, string userName)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            RateDAL rate = new RateDAL(userName, rating, review);
            foreach (ItemDAL item in storeDAL._stock._itemAndAmount.Keys)
            {
                if (item._itemID == itemID)
                {
                    item._rating._ratings.Add(rate);
                    break;
                }
            }
            context.SaveChanges();
        }
        public void RateStore(String storeName, int rating, String review, string userName)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            RateDAL rate = new RateDAL(userName, rating, review);
            storeDAL._rating._ratings.Add(rate);
            context.SaveChanges();
        }
        public StoreDAL GetStoreInformation(String storeName)
        {
            return context.StoreDALs.Find(storeName);
        }
        public ItemDAL GetItem(int itemID, String storeName)
        {
            return context.StoreDALs.Find(storeName)._stock._itemAndAmount.Keys.Where(i => i._itemID == itemID).FirstOrDefault();
        }
        public void SendMessageToStore(String storeName, String title, String message, string sender)
        {
            MessageToStoreDAL msg = new MessageToStoreDAL(storeName, sender, message, title);
            context.StoreDALs.Find(storeName)._messagesToStore.Add(msg);
            context.SaveChanges();
        }
        public void FileComplaint(int cartID, String message, RegisteredDAL sender)
        {
            ComplaintDAL complaint = new ComplaintDAL(sender, cartID, message);
            context.ComplaintDALs.Add(complaint);
            context.SaveChanges();
        }
        public List<Tuple<DateTime, ShoppingCartDAL>> GetMyPurchasesHistory(string userName)
        {
            List<Tuple<DateTime, ShoppingCartDAL>> history = new List<Tuple<DateTime, ShoppingCartDAL>>();
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory.Find(userName);
            foreach (PurchasedCartDAL cart in reg_history._PurchasedCarts)
            {
                history.Add(new Tuple<DateTime, ShoppingCartDAL>(cart._purchaseDate, cart._PurchasedCart));
            }
            return history;
        }
        public RegisteredDAL GetVisitorInformation(string userName) 
        {
            return context.RegisteredDALs.Find(userName);
        }
        public void EditVisitorPassword(String newPassword, string newSalt, string userName)
        {
            RegisteredDAL registered = context.RegisteredDALs.Find(userName);
            registered._password = newPassword;
            registered._salt = newSalt;
            context.SaveChanges();
        }
        public void RemoveManagerPermission(String managerUsername, String storeName, Operation op)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(managerUsername);
            foreach(StoreManagerDAL managerDAL in storeDAL._managers)
            {
                if(managerDAL._username == managerUsername)
                {
                    if (managerDAL._operations.Contains(op))
                        managerDAL._operations.Remove(op);
                    break;
                }
            }
            context.SaveChanges();
            //since both register and store holds ptr to the same manager Obj, the removal will be seen in both.
        }
        public void AddManagerPermission(String managerUsername, String storeName, Operation op)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(managerUsername);
            foreach (StoreManagerDAL managerDAL in storeDAL._managers)
            {
                if (managerDAL._username == managerUsername)
                {
                    if (!managerDAL._operations.Contains(op))
                        managerDAL._operations.Add(op);
                    break;
                }
            }
            context.SaveChanges();
            //since both register and store holds ptr to the same manager Obj, the addition will be seen in both.
        }
        public void CloseStore(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            store._state = StoreState.Inactive;
            context.SaveChanges();
        }
        public void ReopenStore(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            store._state = StoreState.Active;
            context.SaveChanges();
        }
        public List<StoreOwnerDAL> GetStoreOwners(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            return store._owners;
        }
        public List<StoreManagerDAL> GetStoreManagers(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            return store._managers;
        }
        public StoreFounderDAL GetStoreFounder(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            return store._founder;
        }
        public List<MessageToStoreDAL> GetStoreMessages(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            return store._messagesToStore;
        }
        public void AnswerStoreMesseage(int msgID, string storeName, string reply, string replier)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            MessageToStoreDAL msg = store._messagesToStore.Find(m=> m.mid == msgID); 
            msg._reply = reply;
            msg._replierFromStore = replier;
            context.SaveChanges();
        }
        public List<Tuple<DateTime, ShoppingBasketDAL>> GetStorePurchasesHistory(String storeName)
        {
            List<Tuple<DateTime, ShoppingBasketDAL>> history = new List<Tuple<DateTime, ShoppingBasketDAL>>();
            StorePurchasedBasketDAL store_basket = context.StorePurchaseHistory.Find(storeName);
            foreach (PurchasedBasketDAL basket in store_basket._PurchasedBaskets)
            {
                history.Add(new Tuple<DateTime, ShoppingBasketDAL>(basket._purchaseDate, basket._PurchasedBasket));
            }
            return history;
        }
        public void CloseStorePermanently(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            store._state = StoreState.Closed;
            context.SaveChanges();
        }
        public ICollection<ComplaintDAL> GetRegisterdComplaints()
        {
            return context.ComplaintDALs.ToList();
        }
        public void ReplyToComplaint(int complaintID, String reply)
        {
            context.ComplaintDALs.Find(complaintID)._response = reply;
            context.SaveChanges();
        }
        public ICollection<AdminMessageToRegisteredDAL> GetRegisteredMessagesFromAdmin(string username)
        {
            return context.RegisteredDALs.Find(username)._adminMessages.ToList();
        }
        public ICollection<MessageToStoreDAL> GetRegisterAnsweredStoreMessages(string username)
        {
            return context.RegisteredDALs.Find(username)._repliedMessages.ToList();
        }
        public ICollection<NotifyMessageDAL> GetRegisteredMessagesNotofication(string username)
        {
            return context.RegisteredDALs.Find(username)._notifications.ToList();
        }
        public void AppointSystemAdmin(String adminUsername)
        {
            context.RegisteredDALs.Find(adminUsername)._roles.Add(new SystemAdminDAL(adminUsername, new Dictionary<int, ComplaintDAL>()));
            context.SaveChanges();

        }
        public List<StoreDAL> GetStoresOfUser(string username)
        {
            List<string> storesNames = context.RegisteredDALs.Find(username)._roles.Select(role => role._storeName).ToList();
            List<StoreDAL> stores = new List<StoreDAL>();
            foreach (string storeName in storesNames)
                stores.Add(context.StoreDALs.Find(storeName));
            return stores;
        }
        public void SendAdminMessage(String receiverUsername, string senderUsername, String title, String message)
        {
            AdminMessageToRegisteredDAL msg = new AdminMessageToRegisteredDAL(receiverUsername, senderUsername, title, message);
            context.RegisteredDALs.Find(receiverUsername)._adminMessages.Add(msg);
            context.SaveChanges();
        }

        
    }
}
