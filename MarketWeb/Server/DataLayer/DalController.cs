
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
        private static DalController instance = null;
        public static DalController GetInstance()
        {
            if (instance == null)
                instance = new DalController();
            return instance;
        }
        private DalController()
        {
            context = new MarketContext();
        }
        public List<StoreDAL> GetAllActiveStores() 
        {
            List<StoreDAL> stores= context.StoreDALs.Where(store => store._state == StoreState.Active).ToList();
            if (stores == null)
                return new List<StoreDAL>();
            return stores; 
        }
        public void Register(string Username, string password,string salt,  DateTime dob) 
        {
            context.RegisteredDALs.Add(
                new RegisteredDAL(Username, password, salt, dob));
            context.SaveChanges();
        }
        public RegisteredDAL GetRegistered(string username)
        {
            if(IsUsernameExists(username))
                return context.RegisteredDALs.Find(username);
            throw new Exception($"there is no registered user with username: {username}");
        }
        public bool IsUsernameExists(string username)
        {
            return context.RegisteredDALs.Find(username)!=null;
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
        public void AddItemToCart(ShoppingBasketDAL shoppingBasket ,String storeName,string userName, int itemID, int amount)
        {
            string errMsg = "";
            RegisteredDAL user = context.RegisteredDALs.Find(userName);
            if (user == null)
                throw new Exception($"user: {userName} not in system");
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            ICollection<ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            if (shoppingBasketDALs.Where(b => b._store._storeName == storeName).Any())
                shoppingBasketDALs.Remove(shoppingBasketDALs.Where(b => b._store._storeName == storeName).FirstOrDefault());
           shoppingBasketDALs.Add(shoppingBasket);
            context.SaveChanges();
        }
        public void RemoveItemFromCart(int itemID, String storeName, string userName, int amount, ShoppingBasketDAL shoppingBasket)
        {
            RegisteredDAL user = context.RegisteredDALs.Find(userName);
            if (user == null)
                throw new Exception($"user: {userName} not in system");
            ItemDAL itemToRemove = null;
            ICollection<ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            if (shoppingBasketDALs.Where(b => b._store._storeName == storeName).Any())
                shoppingBasketDALs.Remove(shoppingBasketDALs.Where(b => b._store._storeName == storeName).FirstOrDefault());
            shoppingBasketDALs.Add(shoppingBasket);

            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
        public void UpdateQuantityOfItemInCart(int itemID, String storeName, int newQuantity, string userName, ShoppingBasketDAL shoppingBasketDAL, int amountDiff)
        {
            RegisteredDAL user = context.RegisteredDALs.Find(userName);
            if (user == null)
                throw new Exception($"user: {userName} not in system");

            ICollection<ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            if (shoppingBasketDALs.Where(b => b._store._storeName == storeName).Any())
                shoppingBasketDALs.Remove(shoppingBasketDALs.Where(b => b._store._storeName == storeName).FirstOrDefault());
            shoppingBasketDALs.Add(shoppingBasketDAL);
           
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            if (registeredDAL == null)
                throw new Exception($"user: {userName} not in system");
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
            RegisteredDAL reg = context.RegisteredDALs.Find(founderName);
            if(reg == null)
                throw new Exception($"user: {founderName} not in system");
            StoreFounderDAL founder = new StoreFounderDAL(storeName, founderName);
            StoreDAL store = new StoreDAL(storeName, founder, StoreState.Active);
            context.StoreDALs.Add(store);
            reg._roles.Add(founder);
            context.SaveChanges();
        }
        public void AddStoreManager(String managerUsername, String storeName, string appointer)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(managerUsername);
            if (registeredDAL == null)
                throw new Exception($"user: {managerUsername} not in system");
            StoreManagerDAL storeManager = new StoreManagerDAL(storeName, appointer, managerUsername);
            storeDAL._managers.Add(storeManager);
            registeredDAL._roles.Add(storeManager);
            context.SaveChanges();  
        }
        public void AddStoreOwner(String ownerUsername, String storeName, string appointer)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(ownerUsername);
            if (registeredDAL == null)
                throw new Exception($"user: {ownerUsername} not in system");
            StoreOwnerDAL storeOwner = new StoreOwnerDAL(storeName, appointer, ownerUsername);
            storeDAL._owners.Add(storeOwner);
            registeredDAL._roles.Add(storeOwner);
            context.SaveChanges();
        }
        public void RemoveStoreOwner(String ownerUsername, String storeName)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            foreach (StoreOwnerDAL owner in storeDAL._owners)
            {
                if (owner._username == ownerUsername)
                {
                    storeDAL._owners.Remove(owner);
                    break;
                }
            }
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(ownerUsername);
            if (registeredDAL == null)
                throw new Exception($"user: {ownerUsername} not in system");
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
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            foreach (StoreManagerDAL manager in storeDAL._managers)
            {
                if (manager._username == managerUsername)
                {
                    storeDAL._managers.Remove(manager);
                    break;
                }
            }
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(managerUsername);
            if (registeredDAL == null)
                throw new Exception($"user: {managerUsername} not in system");
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
        public int AddItemToStoreStock(String storeName, String name, double price, String description, String category, int quantity)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            ItemDAL item = new ItemDAL(new RatingDAL(new List<RateDAL>()), name, price,
                description, category);
            storeDAL._stock._itemAndAmount.Add(item, quantity);
            context.SaveChanges();
            return item._itemID;
        }
        public void RemoveItemFromStore(String storeName, int itemID)
        {
            StoreDAL storeDAL = context.StoreDALs.Find(storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
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
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            RateDAL rate = new RateDAL(userName, rating, review);
            storeDAL._rating._ratings.Add(rate);
            context.SaveChanges();
        }
        public StoreDAL GetStoreInformation(String storeName)
        {
            StoreDAL store =  context.StoreDALs.Find(storeName);
            if(store == null)
                throw new Exception($"there is no such store: {storeName} in system.");
            return store;
        }
        public ItemDAL GetItem(int itemID, String storeName)
        {
            try
            {
                return context.StoreDALs.Find(storeName)._stock._itemAndAmount.Keys.Where(i => i._itemID == itemID).First();
            }
            catch (Exception ex)
            {
                throw new Exception($"there is no such item: {itemID} in store: {storeName}.");
            }   
        }
        public int SendMessageToStore(String storeName, String title, String message, string sender)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");
            MessageToStoreDAL msg = new MessageToStoreDAL(storeName, sender, message, title);
            store._messagesToStore.Add(msg);
            context.SaveChanges();
            return msg.mid;
        }
        public int FileComplaint(int cartID, String message, string sender)
        {
            ComplaintDAL complaint = new ComplaintDAL(sender, cartID, message);
            context.ComplaintDALs.Add(complaint);
            context.SaveChanges();
            return complaint._id;
        }
        public List<Tuple<DateTime, ShoppingCartDAL>> GetMyPurchasesHistory(string userName)
        {
            List<Tuple<DateTime, ShoppingCartDAL>> history = new List<Tuple<DateTime, ShoppingCartDAL>>();
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory.Find(userName);
            if (reg_history == null)
                return null;
            foreach (PurchasedCartDAL cart in reg_history._PurchasedCarts)
            {
                history.Add(new Tuple<DateTime, ShoppingCartDAL>(cart._purchaseDate, cart._PurchasedCart));
            }
            return history;
        }
        public List<Tuple<DateTime, ShoppingBasketDAL>> GetRegisterPurchasesInStore(string userName, string storeName)
        {
            List<Tuple<DateTime, ShoppingBasketDAL>> history = new List<Tuple<DateTime, ShoppingBasketDAL>>();
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory.Find(userName);
            if (reg_history == null)
                return null;
            foreach (PurchasedCartDAL purchasedCart in reg_history._PurchasedCarts)
            {
                ShoppingCartDAL cart = purchasedCart._PurchasedCart;
                foreach(ShoppingBasketDAL purchasedBasket in cart._shoppingBaskets)
                {
                    if (purchasedBasket._store._storeName == storeName)
                        history.Add(new Tuple<DateTime, ShoppingBasketDAL>(purchasedCart._purchaseDate, purchasedBasket));
                }
            }
            return history;
        }
        public bool DidRegisterPurchasedInStore(string userName, string storeName)
        {
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory.Find(userName);

            if (reg_history == null)
                return false;
            foreach (PurchasedCartDAL purchasedCart in reg_history._PurchasedCarts)
            {
                ShoppingCartDAL cart = purchasedCart._PurchasedCart;
                foreach (ShoppingBasketDAL purchasedBasket in cart._shoppingBaskets)
                {
                    if (purchasedBasket._store._storeName == storeName)
                        return true;
                }
            }
            return false;
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
            if (storeDAL == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            foreach (StoreManagerDAL managerDAL in storeDAL._managers)
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
            if (storeDAL == null)
                throw new Exception($"there is no such store: {storeName} in system.");

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
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            store._state = StoreState.Inactive;
            context.SaveChanges();
        }
        public void ReopenStore(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            store._state = StoreState.Active;
            context.SaveChanges();
        }
        public List<StoreOwnerDAL> GetStoreOwners(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            return store._owners;
        }
        public List<StoreManagerDAL> GetStoreManagers(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            return store._managers;
        }
        public StoreFounderDAL GetStoreFounder(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            return store._founder;
        }
        public List<MessageToStoreDAL> GetStoreMessages(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            return store._messagesToStore;
        }
        public void AnswerStoreMesseage(int msgID, string storeName, string reply, string replier)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");
            MessageToStoreDAL msg = store._messagesToStore.Find(m=> m.mid == msgID);
            if (msg == null)
                throw new Exception($"there is no message with id: {msgID} in db");
            msg._reply = reply;
            msg._replierFromStore = replier;
            context.SaveChanges();
        }
        public List<Tuple<DateTime, ShoppingBasketDAL>> GetStorePurchasesHistory(String storeName)
        {
            List<Tuple<DateTime, ShoppingBasketDAL>> history = new List<Tuple<DateTime, ShoppingBasketDAL>>();
            StorePurchasedBasketDAL store_basket = context.StorePurchaseHistory.Find(storeName);
            if (store_basket == null)
                return null;
            foreach (PurchasedBasketDAL basket in store_basket._PurchasedBaskets)
            {
                history.Add(new Tuple<DateTime, ShoppingBasketDAL>(basket._purchaseDate, basket._PurchasedBasket));
            }
            return history;
        }
        public void CloseStorePermanently(String storeName)
        {
            StoreDAL store = context.StoreDALs.Find(storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");
            store._state = StoreState.Closed;
            context.SaveChanges();
        }
        public ICollection<ComplaintDAL> GetRegisterdComplaints()
        {
            return context.ComplaintDALs.ToList();
        }
        public void ReplyToComplaint(int complaintID, String reply)
        {
            ComplaintDAL complaintDAL = context.ComplaintDALs.Find(complaintID);
            if(complaintDAL == null)
                throw new Exception("complaint not found in db");
            complaintDAL._response = reply;
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
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(adminUsername);
            if (registeredDAL == null)
                throw new Exception($"there is no user with username: {adminUsername} in db");
            registeredDAL._roles.Add(new SystemAdminDAL(adminUsername));
            context.SaveChanges();
        }
        public List<StoreDAL> GetStoresOfUser(string username)
        {
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(username);
            if (registeredDAL == null)
                throw new Exception($"there is no user with username: {username} in db");
            List<string> storesNames = registeredDAL._roles.Select(role => role._storeName).ToList();
            List<StoreDAL> stores = new List<StoreDAL>();
            foreach (string storeName in storesNames)
                stores.Add(context.StoreDALs.Find(storeName));
            return stores;
        }
        public int SendAdminMessage(String receiverUsername, string senderUsername, String title, String message)
        {
            RegisteredDAL registeredDAL = context.RegisteredDALs.Find(receiverUsername);
            if (registeredDAL == null)
                throw new Exception($"there is no user with username: {receiverUsername} in db");
            AdminMessageToRegisteredDAL msg = new AdminMessageToRegisteredDAL(receiverUsername, senderUsername, title, message);
            registeredDAL._adminMessages.Add(msg);
            context.SaveChanges();
            return msg.mid;
        }

        public bool StoreExists(string storeName)
        {
            return context.StoreDALs.Find(storeName) != null;
        }
        public int SendNotification(string storeName, string usernameReciever, String title, String message)
        {
            RegisteredDAL reg = context.RegisteredDALs.Find(usernameReciever);
            if (usernameReciever == null)
                throw new Exception($"there is no such user with uaername: {usernameReciever}");
            NotifyMessageDAL notifyMessageDAL = new NotifyMessageDAL(storeName, title, message, usernameReciever);
            reg._notifications.Add(notifyMessageDAL);
            context.SaveChanges();
            return notifyMessageDAL.mid;
        }
        public ComplaintDAL GetComplaint(int id)
        {
            return context.ComplaintDALs.Find(id);
        }
        //add pp
        //add dp
        //add d

    }
}
