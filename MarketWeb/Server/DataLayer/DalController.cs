﻿using MarketWeb.Shared;
using Microsoft.EntityFrameworkCore;
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
            List<StoreDAL> stores= context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .Where(store => store._state == StoreState.Active).ToList();
            if (stores == null)
                return new List<StoreDAL>();
            return stores; 
        }
        public void Register(string Username, string password,string salt,  DateTime dob) 
        {
            RegisteredDAL reg = new RegisteredDAL(Username, password, salt, dob);
            reg._cart = new ShoppingCartDAL();
            reg._adminMessages = new List<AdminMessageToRegisteredDAL>();
            reg._notifications = new List<NotifyMessageDAL>();
            context.RegisteredDALs.Add(reg);
            context.SaveChanges();
        }
        public RegisteredDAL GetRegistered(string username)
        {
            if(IsUsernameExists(username))
                return context.RegisteredDALs.Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).FirstOrDefault(s => s._username == username);
            throw new Exception($"there is no registered user with username: {username}");
        }
        public bool IsUsernameExists(string username)
        {
            return context.RegisteredDALs.Find(username) != null;
        }
        public void RemoveRegisteredVisitor(string username) 
        {
            RegisteredDAL toRemove = context.RegisteredDALs
                                                .Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).FirstOrDefault(s => s._username == username);
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
            RegisteredDAL user = context.RegisteredDALs
                                                .Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).FirstOrDefault(s => s._username == userName);
            if (user == null)
                throw new Exception($"user: {userName} not in system");
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            ICollection<StockItemDAL> itemsNamunt = storeDAL._stock._itemAndAmount;
            ItemDAL itemToAdd = null;
            foreach (StockItemDAL stockItem in itemsNamunt)
            {
                if (stockItem.item._itemID == itemID)
                {
                    itemToAdd = stockItem.item;
                    stockItem.amount = stockItem.amount - amount;
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
            RegisteredDAL user = context.RegisteredDALs
                                                .Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).FirstOrDefault(s => s._username == userName);
            if (user == null)
                throw new Exception($"user: {userName} not in system");
            ItemDAL itemToRemove = null;
            ICollection<ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            if (shoppingBasketDALs.Where(b => b._store._storeName == storeName).Any())
                shoppingBasketDALs.Remove(shoppingBasketDALs.Where(b => b._store._storeName == storeName).FirstOrDefault());
            shoppingBasketDALs.Add(shoppingBasket);

            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            ICollection<StockItemDAL> itemsNamunt = storeDAL._stock._itemAndAmount;
            foreach (StockItemDAL stockItem in itemsNamunt)
            {
                if (stockItem.item._itemID == itemID)
                {
                    stockItem.amount = stockItem.amount + amount;
                }

            }
            context.SaveChanges();
        }
        public void UpdateQuantityOfItemInCart(int itemID, String storeName, int newQuantity, string userName, ShoppingBasketDAL shoppingBasketDAL, int amountDiff)
        {
            RegisteredDAL user = context.RegisteredDALs
                                                .Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).FirstOrDefault(s => s._username == userName);
            if (user == null)
                throw new Exception($"user: {userName} not in system");

            ICollection<ShoppingBasketDAL> shoppingBasketDALs = user._cart._shoppingBaskets;
            if (shoppingBasketDALs.Where(b => b._store._storeName == storeName).Any())
                shoppingBasketDALs.Remove(shoppingBasketDALs.Where(b => b._store._storeName == storeName).FirstOrDefault());
            shoppingBasketDALs.Add(shoppingBasketDAL);
           
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            ICollection<StockItemDAL> itemsNamunt = storeDAL._stock._itemAndAmount;
            foreach (StockItemDAL stockItem in itemsNamunt)
            {
                if (stockItem.item._itemID == itemID)
                {
                    stockItem.amount = stockItem.amount + amountDiff;
                }

            }
            context.SaveChanges();
        }
        //public ShoppingCartDAL ViewMyCart(){} ---- no need. the field is updated
        public void addStorePurchse(ShoppingBasketDAL basket, DateTime date, string storename)
        {
            PurchasedBasketDAL PurchasedBasket = new PurchasedBasketDAL(date, basket);
            StorePurchasedBasketDAL storePurchasedBasketDAL = context.StorePurchaseHistory.Include(x => x._PurchasedBaskets).FirstOrDefault(s => s._storeName == storename);
            if (storePurchasedBasketDAL == null)
                storePurchasedBasketDAL = new StorePurchasedBasketDAL(storename);
            storePurchasedBasketDAL._PurchasedBaskets.Add(PurchasedBasket);
            context.SaveChanges();
        }
        public void addRegisteredPurchse(ShoppingCartDAL cart, DateTime date, string userName)
        {
            RegisteredDAL registeredDAL = context.RegisteredDALs
                                                .Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).FirstOrDefault(s => s._username == userName);
            if (registeredDAL == null)
                throw new Exception($"user: {userName} not in system");
            PurchasedCartDAL PurchasedCart = new PurchasedCartDAL(date, registeredDAL._cart);
            RegisteredPurchasedCartDAL registeredPurchasedCartDAL = context.RegisteredPurchaseHistory.Include(x => x._PurchasedCarts).FirstOrDefault(s => s.userName == userName);
            if (registeredPurchasedCartDAL == null)
                registeredPurchasedCartDAL = new RegisteredPurchasedCartDAL(userName);

            registeredPurchasedCartDAL._PurchasedCarts.Add(PurchasedCart);
            registeredDAL._cart = new ShoppingCartDAL();
            context.SaveChanges();
        }
        public void OpenNewStore(String storeName, string founderName)
        {
            StoreDAL store = new StoreDAL(storeName, StoreState.Active);
            store._stock = new StockDAL();
            store._rating = new RatingDAL();
            store._purchasePolicy = new PurchasePolicyDAL();
            store._discountPolicy = new DiscountPolicyDAL();
            context.StoreDALs.Add(store);
            StoreFounderDAL founder = new StoreFounderDAL();
            founder._username = founderName;
            founder._storeName = storeName;
            context.StoreFounderDALs.Add(founder);
            context.SaveChanges();
        }

        internal List<MessageToStoreDAL> GetOpenMessagesToStoreByStoreName(string storeName)
        {
            List<MessageToStoreDAL> messages = context.MessageToStoreDALs.Where(msg => (msg._storeName == storeName)).ToList();
            foreach (MessageToStoreDAL message in messages)
            {
                if (message.Status() == StoreMessageStatus.Closed)
                {
                    messages.Remove(message);
                }
            }
            return messages;
        }
        internal List<MessageToStoreDAL> GetRepliedMessagesToStoreByUserName(string userName)
        {
            List<MessageToStoreDAL> messages = context.MessageToStoreDALs.Where(msg => (msg._senderUsername == userName)).ToList();
            foreach (MessageToStoreDAL message in messages)
            {
                if (message.Status() == StoreMessageStatus.Open)
                {
                    messages.Remove(message);
                }
            }
            return messages;
        }

        public void AddStoreManager(String managerUsername, String storeName, string appointer)
        {
            
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
           StoreManagerDAL storeManager = new StoreManagerDAL(appointer, managerUsername, storeName);
            context.storeManagerDALs.Add(storeManager);
            context.SaveChanges();  
        }

        public void AddStoreOwner(String ownerUsername, String storeName, string appointer)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            StoreOwnerDAL storeOwner = new StoreOwnerDAL(appointer, ownerUsername, storeName);
            context.storeOwnerDALs.Add(storeOwner);
            context.SaveChanges();
        }
        public void RemoveStoreOwner(String ownerUsername, String storeName)
        {
            StoreOwnerDAL owner = context.storeOwnerDALs.Include(r => r._operationsWrappers).Where((r) => (r._storeName == storeName)).First();
            context.SystemRoleDALs.Remove(owner);
            context.SaveChanges();
        }
        public void RemoveStoreManager(String managerUsername, String storeName)
        {
            StoreManagerDAL manager = context.storeManagerDALs.Include(r => r._operationsWrappers).Where((r) => (r._storeName == storeName)).First();
            context.SystemRoleDALs.Remove(manager);
            context.SaveChanges();
        }
        public int AddItemToStoreStock(String storeName, String name, double price, String description, String category, int quantity)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)                                                   
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            ItemDAL item = new ItemDAL(new RatingDAL(new List<RateDAL>()), name, price,
                description, category);
            storeDAL._stock._itemAndAmount.Add(new StockItemDAL(item, quantity));
            context.SaveChanges();
            return item._itemID;
        }
        public void RemoveItemFromStore(String storeName, int itemID)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            foreach (StockItemDAL stockItem in storeDAL._stock._itemAndAmount)
            {
                if(stockItem.item._itemID == itemID)
                {
                    storeDAL._stock._itemAndAmount.Remove(stockItem);
                    break;
                }
            }
            context.SaveChanges();
        }
        public void UpdateStockQuantityOfItem(String storeName, int itemID, int newQuantity)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            foreach (StockItemDAL stockItem in storeDAL._stock._itemAndAmount)
            {
                if (stockItem.item._itemID == itemID)
                {
                    stockItem.amount = newQuantity;
                    break;
                }
            }
            context.SaveChanges();
        }
        public void EditItemPrice(String storeName, int itemID, double newPrice)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            foreach (StockItemDAL stockItem in storeDAL._stock._itemAndAmount)
            {
                if (stockItem.item._itemID == itemID)
                {
                    stockItem.item._price = newPrice; 
                    break;
                }
            }
            context.SaveChanges();
        }
        public void EditItemName(String storeName, int itemID, String newName)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            foreach (StockItemDAL stockItem in storeDAL._stock._itemAndAmount)
            {
                if (stockItem.item._itemID == itemID)
                {
                    stockItem.item._name = newName;
                    break;
                }
            }
            context.SaveChanges();
        }
        public void EditItemDescription(String storeName, int itemID, String newDescription)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            foreach (StockItemDAL stockItem in storeDAL._stock._itemAndAmount)
            {
                if (stockItem.item._itemID == itemID)
                {
                    stockItem.item._description = newDescription;
                    break;
                }
            }
            context.SaveChanges();
        }

        internal ICollection<ComplaintDAL> GetRgisteredFiledComplaintsByUsername(string username)
        {
            return context.ComplaintDALs.Where(c => c._complainer == username).ToList();
        }

        internal ICollection<SystemRoleDAL> GetRegisteredRolesByUsername(string username)
        {
            return context.SystemRoleDALs.Include(r => r._operationsWrappers).Where(r => r._username == username).ToList();
        }

        public void RateItem(int itemID, String storeName, int rating, String review, string userName)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            RateDAL rate = new RateDAL(userName, rating, review);
            foreach (StockItemDAL stockItem in storeDAL._stock._itemAndAmount)
            {
                if (stockItem.item._itemID == itemID)
                {
                    stockItem.item._rating._ratings.Add(rate);
                    break;
                }
            }
            context.SaveChanges();
        }
        public void RateStore(String storeName, int rating, String review, string userName)
        {
            StoreDAL storeDAL = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            RateDAL rate = new RateDAL(userName, rating, review);
            storeDAL._rating._ratings.Add(rate);
            context.SaveChanges();
        }
        public StoreDAL GetStoreInformation(String storeName)
        {
            StoreDAL store = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if(store == null)
                throw new Exception($"there is no such store: {storeName} in system.");
            return store;
        }
        public ItemDAL GetItem(int itemID, String storeName)
        {
            try
            {
                return context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName)._stock._itemAndAmount.Where(i => i.item._itemID == itemID).First().item;
            }
            catch (Exception ex)
            {
                throw new Exception($"there is no such item: {itemID} in store: {storeName}.");
            }   
        }
        public int SendMessageToStore(String storeName, String title, String message, string sender)
        {

            MessageToStoreDAL msg = new MessageToStoreDAL(sender, message, title, storeName);
            context.MessageToStoreDALs.Add(msg);
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
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory.Include(x => x._PurchasedCarts).FirstOrDefault(s => s.userName == userName);
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
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory.Include(x => x._PurchasedCarts).FirstOrDefault(s => s.userName == userName);
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
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory.Include(x => x._PurchasedCarts).FirstOrDefault(s => s.userName == userName);

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
            return context.RegisteredDALs
                                .Include(x => x._cart)
                                .Include(x => x._adminMessages)
                                .Include(x => x._notifications).FirstOrDefault(s => s._username == userName);
        }
        public void EditVisitorPassword(String newPassword, string newSalt, string userName)
        {
            RegisteredDAL registered = context.RegisteredDALs
                                .Include(x => x._cart)
                                .Include(x => x._adminMessages)
                                .Include(x => x._notifications).FirstOrDefault(s => s._username == userName);
            registered._password = newPassword;
            registered._salt = newSalt;
            context.SaveChanges();
        }
        public void RemoveManagerPermission(String managerUsername, String storeName, Operation op)
        {

            StoreManagerDAL manager = context.storeManagerDALs.Include(r => r._operationsWrappers).Where((r) => (r._storeName == storeName) && (r._username == managerUsername)).First();

            manager._operations.Remove(op);
            manager._operationsWrappers.Remove(manager._operationsWrappers.Find(x => x.op.Equals(op)));
            context.SaveChanges();
            //since both register and store holds ptr to the same manager Obj, the removal will be seen in both.
        }
        public void AddManagerPermission(String managerUsername, String storeName, Operation op)
        {
            StoreManagerDAL manager = context.storeManagerDALs.Include(r => r._operationsWrappers).Where((r) => (r._storeName == storeName) && (r._username == managerUsername)).First();
            manager._operations.Add(op);
            manager._operationsWrappers.Add(new OperationWrapper(op));
            context.SaveChanges();
            //since both register and store holds ptr to the same manager Obj, the addition will be seen in both.
        }
        public void CloseStore(String storeName)
        {
            StoreDAL store = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            store._state = StoreState.Inactive;
            context.SaveChanges();
        }
        public void ReopenStore(String storeName)
        {
            StoreDAL store = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");

            store._state = StoreState.Active;
            context.SaveChanges();
        }
        public List<StoreOwnerDAL> GetStoreOwners(String storeName)
        {
            return context.storeOwnerDALs.Include(r => r._operationsWrappers).Where(r => (r._storeName == storeName) ).ToList();
        }
        public List<StoreManagerDAL> GetStoreManagers(String storeName)
        {
            return context.storeManagerDALs.Include(r => r._operationsWrappers).
                Where(r => (r._storeName == storeName)).ToList();
        }
        public StoreFounderDAL GetStoreFounder(String storeName)
        {
            return context.StoreFounderDALs.Include(r => r._operationsWrappers).Where(r => (r._storeName == storeName)).FirstOrDefault();
        }

        public void AnswerStoreMesseage(int msgID, string storeName, string reply, string replier)
        {
            MessageToStoreDAL msg = context.MessageToStoreDALs.Find(msgID);
            if (msg == null)
                throw new Exception($"there is no message with id: {msgID} in db");
            msg._reply = reply;
            msg._replierFromStore = replier;
            context.SaveChanges();
        }
        public List<Tuple<DateTime, ShoppingBasketDAL>> GetStorePurchasesHistory(String storeName)
        {
            List<Tuple<DateTime, ShoppingBasketDAL>> history = new List<Tuple<DateTime, ShoppingBasketDAL>>();
            StorePurchasedBasketDAL store_basket = context.StorePurchaseHistory.Include(x => x._PurchasedBaskets).FirstOrDefault(s => s._storeName == storeName);
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
            StoreDAL store = context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName);
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
            return context.RegisteredDALs
                                .Include(x => x._cart)
                                .Include(x => x._adminMessages)
                                .Include(x => x._notifications).FirstOrDefault(s => s._username == username)._adminMessages.ToList();
        }
        public ICollection<MessageToStoreDAL> GetRegisterAnsweredStoreMessages(string username)
        {
            ICollection<MessageToStoreDAL> messages = context.MessageToStoreDALs.Where(msg => (msg._senderUsername == username)).ToList();
            foreach(MessageToStoreDAL message in messages)
            {
                if(message.Status() == StoreMessageStatus.Open)
                {
                    messages.Remove(message);
                }
            }
            return messages;
        }
        public ICollection<NotifyMessageDAL> GetRegisteredMessagesNotofication(string username)
        {
            return context.RegisteredDALs
                                .Include(x => x._cart)
                                .Include(x => x._adminMessages)
                                .Include(x => x._notifications).FirstOrDefault(s => s._username == username)._notifications.ToList();
        }
        public void AppointSystemAdmin(String adminUsername)
        {
            SystemAdminDAL systemAdminDAL = new SystemAdminDAL(adminUsername);
            context.SystemAdminDALs.Add(systemAdminDAL);
            context.SaveChanges();
        }
        public List<StoreDAL> GetStoresOfUser(string username)
        {
            List<string> storesnames = context.SystemRoleDALs.Include(r => r._operationsWrappers).Where(r => (r._username == username) && (r._storeName != null)).Select(r => r._storeName).ToList();
            List<StoreDAL> stores = new List<StoreDAL>();
            foreach (string storeName in storesnames)
            {
                stores.Add(context.StoreDALs.Include(x => x._stock)
                                                    .Include(x => x._rating)
                                                    .Include(x => x._purchasePolicy)
                                                    .Include(x => x._discountPolicy)
                                                    .FirstOrDefault(s => s._storeName == storeName));
            }
                   
            return stores;
        }
        public int SendAdminMessage(String receiverUsername, string senderUsername, String title, String message)
        {
            RegisteredDAL registeredDAL = context.RegisteredDALs
                                                    .Include(x => x._cart)
                                                    .Include(x => x._adminMessages)
                                                    .Include(x => x._notifications).FirstOrDefault(s => s._username == receiverUsername); ;
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
            RegisteredDAL reg = context.RegisteredDALs
                                            .Include(x => x._cart)
                                            .Include(x => x._adminMessages)
                                            .Include(x => x._notifications).FirstOrDefault(s => s._username == usernameReciever); ;
            if (usernameReciever == null)
                throw new Exception($"there is no such user with uaername: {usernameReciever}");
            NotifyMessageDAL notifyMessageDAL = new NotifyMessageDAL(title, message, usernameReciever);
            reg._notifications.Add(notifyMessageDAL);
            context.SaveChanges();
            return notifyMessageDAL.mid;
        }
        public ComplaintDAL GetComplaint(int id)
        {
            return context.ComplaintDALs.Find(id);
        }


        private bool CommonVarInLists<T>(List<T> l1, List<T> l2)
        {
            List<T> res = l1.Where(x => l2.Contains(x)).ToList();
            return res.Any();       
        }
        //add pp
        //add dp
        //add d
        public String GetRoleUsername(int roleid)
        {
            SystemRoleDAL role = context.SystemRoleDALs.Include(r=> r._operationsWrappers).FirstOrDefault(r=> r.id == roleid);
            if(role == null)
                throw new Exception($"role id {roleid} was not found in the database.");
            return role._username;
        }

        public String GetRoleStoreName(int roleid)
        {
            SystemRoleDAL role = context.SystemRoleDALs.Include(r => r._operationsWrappers).FirstOrDefault(r => r.id == roleid);
            if (role == null)
                throw new Exception($"role id {roleid} was not found in the database.");
            return role._storeName;
        }

        public String GetReceiverOfAdminMessage(int mid)
        {
            List<RegisteredDAL> regs = context.RegisteredDALs
                                                .Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).ToList();
            foreach (RegisteredDAL registered in regs)
            {
                List<int> messagesIDs = registered._adminMessages.Select(x => x.mid).ToList();

                if (messagesIDs.Contains(mid))
                {
                    return registered._username;
                }
            }
            throw new Exception($"message id {mid} was not found in the database.");
        }

        public String GetReceiverOfNotificationMessage(int mid)
        {
            List<RegisteredDAL> regs = context.RegisteredDALs
                                                .Include(x => x._cart)
                                                .Include(x => x._adminMessages)
                                                .Include(x => x._notifications).ToList();
            foreach (RegisteredDAL registered in regs)
            {
                List<int> messagesIDs = registered._notifications.Select(x => x.mid).ToList();

                if (messagesIDs.Contains(mid))
                {
                    return registered._username;
                }
            }
            throw new Exception($"message id {mid} was not found in the database.");
        }

        public String GetStoreNameOfMessageToStore(int mid)
        {
            MessageToStoreDAL msg = context.MessageToStoreDALs.Find(mid);
            if(msg == null)
                throw new Exception($"message id {mid} was not found in the database.");
            return msg._storeName;
            
        }
    }
}