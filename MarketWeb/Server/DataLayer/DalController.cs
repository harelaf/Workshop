﻿using MarketWeb.Server.Domain;
using MarketWeb.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketWeb.Server.DataLayer
{
    public class DalController
    {
        private static DalController instance = null;
        private static string datasource;
        private static string initialcatalog;
        private static string userid;
        private static string password;
        public static DalController GetInstance()
        {
            if (instance == null)
                instance = new DalController();
            return instance;
        }
        private DalController()
        {
            
            MarketContext.datasource = datasource;
            MarketContext.initialcatalog = initialcatalog;
            MarketContext.userid = userid;
            MarketContext.password = password;

        }
        public static void InitializeContext(string _datasource, string _initialcatalog, string _userid, string _password)
        {
            datasource = _datasource;
            initialcatalog = _initialcatalog;
            userid = _userid;
            password = _password;
        }
        public List<StoreDAL> GetAllActiveStores() 
        {
            MarketContext context = new MarketContext();
            List<StoreDAL> stores= context.StoreDALs
                                                .Include(x => x._stock)
                                                .Include(x => x._bidsOfVisitors).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                .Include(x => x._rating)
                                                .Include(x => x._standbyOwners).ThenInclude(x => x._acceptors)
                                                .Where(store => store._state == StoreState.Active).ToList();
            if (stores == null)
                return new List<StoreDAL>();
            return stores; 
        }
        public void Register(string Username, string password,string salt,  DateTime dob) 
        {
            MarketContext context = new MarketContext();
            RegisteredDAL reg = new RegisteredDAL(Username, password, salt, dob);
            reg._cart = new ShoppingCartDAL();
            reg._adminMessages = new List<AdminMessageToRegisteredDAL>();
            reg._notifications = new List<NotifyMessageDAL>();
            context.RegisteredDALs.Add(reg);
            context.SaveChanges();
        }
        public RegisteredDAL GetRegistered(string username)
        {
            RegisteredDAL registeredDAL = null;
            MarketContext context = new MarketContext();
            if (IsUsernameExists(username))
            {
                registeredDAL = GetRegisteredDAL(context, username);
                return registeredDAL;
            }
                 
            throw new Exception($"there is no registered user with username: {username}");
        }
        public bool IsUsernameExists(string username)
        {
            MarketContext context = new MarketContext();
            return context.RegisteredDALs.Find(username) != null;
        }
        public void RemoveRegisteredVisitor(string username) 
        {
            MarketContext context = new MarketContext();
            RegisteredDAL toRemove = GetRegisteredDAL(context, username);
            if (toRemove != null) 
            {
                context.RegisteredDALs.Remove(toRemove);
                context.SaveChanges();
            }    
            else
            {
                throw new Exception("no such register in db");
            }
        }
        public void AddItemToCart(ShoppingBasketDAL shoppingBasket, String storeName, string userName, int itemID, int amount)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            ICollection<StockItemDAL> itemsNamunt = storeDAL._stock;
            //context = new MarketContext();
            foreach (StockItemDAL stockItem in itemsNamunt)
            {
                if (stockItem.itemID == itemID)
                {
                    stockItem.amount = stockItem.amount - amount;
                }
            }
            context.SaveChanges();

            context = new MarketContext();
            RegisteredDAL user = GetRegisteredDAL(context, userName);
            if (user._cart._shoppingBaskets.Where(b => b._store._storeName == storeName).Any())
            {
                ShoppingBasketDAL toModify = user._cart._shoppingBaskets.Where(b => b._store._storeName == storeName).FirstOrDefault();
                toModify._items = shoppingBasket._items;
            }
            else
            {
                ShoppingBasketDAL toAdd = new ShoppingBasketDAL();
                toAdd._items = shoppingBasket._items;
                toAdd._store = GetStoreDAL(context, storeName);
                user._cart._shoppingBaskets.Add(toAdd);
            }
            context.SaveChanges();
        }

        public void RemoveItemFromCart(int itemID, String storeName, string userName, int amount, ShoppingBasketDAL shoppingBasket)
        {
            MarketContext context = new MarketContext();
            RegisteredDAL user = GetRegisteredDAL(context, userName);
            
            ShoppingBasketDAL toRemove = user._cart._shoppingBaskets.Where(b => b._store._storeName == storeName).FirstOrDefault();
            if (shoppingBasket == null)//remove basket
            {
                user._cart._shoppingBaskets.Remove(toRemove);
            }
            else
            {
                toRemove._items = shoppingBasket._items;
            }
            context.Update(user);
            context.SaveChanges();
            
            context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            ICollection<StockItemDAL> itemsNamunt = storeDAL._stock;
            foreach (StockItemDAL stockItem in itemsNamunt)
            {
                if (stockItem.itemID == itemID)
                {
                    stockItem.amount = stockItem.amount + amount;
                }
            }
            context.SaveChanges();
        }

        public void RemoveAcceptedBidFromCart(int itemID, String storeName, string userName, int amount, ShoppingBasketDAL shoppingBasket)
        {
            MarketContext context = new MarketContext();
            RegisteredDAL user = GetRegisteredDAL(context, userName);

            ShoppingBasketDAL toRemove = user._cart._shoppingBaskets.Where(b => b._store._storeName == storeName).FirstOrDefault();
            if (shoppingBasket == null)//remove basket
            {
                user._cart._shoppingBaskets.Remove(toRemove);
            }
            else
            {
                toRemove._bids = shoppingBasket._bids;
            }
            context.Update(user);
            context.SaveChanges();

            context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            StockItemDAL itemNamunt = storeDAL._stock.FirstOrDefault(x => x.itemID == itemID);
            if(itemNamunt != null)
                itemNamunt.amount += amount;
            context.SaveChanges();
        }

        private RegisteredDAL GetRegisteredDAL(MarketContext context, string userName)
        {
            RegisteredDAL reg =  context.RegisteredDALs
                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._items).ThenInclude(x => x.purchaseDetails)
                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._store).ThenInclude(x => x._stock)
                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._store).ThenInclude(x => x._rating)
                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._additionalDiscounts)
                                .Include(x => x._adminMessages)
                                .Include(x => x._notifications)
                                .FirstOrDefault(s => s._username == userName);
            if (reg == null)
                throw new Exception($"there is no such registered visitor named '{userName}' in system.");
            return reg;
        }

        private StoreDAL GetStoreDAL(MarketContext context, string storeName)
        {
            StoreDAL store =  context.StoreDALs
                            .Include(x => x._stock)
                            .Include(x => x._bidsOfVisitors).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                            .Include(x => x._rating)
                            .Include(x => x._standbyOwners).ThenInclude(x => x._acceptors)
                            .FirstOrDefault(s => s._storeName == storeName);
            if (store == null)
                throw new Exception($"there is no such store: {storeName} in system.");
            return store;
        }

        public void UpdateQuantityOfItemInCart(int itemID, String storeName, int newQuantity, string userName, ShoppingBasketDAL shoppingBasketDAL, int amountDiff)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            if (storeDAL == null)
                throw new Exception($"store: {storeName} not in system");
            ICollection<StockItemDAL> itemsNamunt = storeDAL._stock;
            foreach (StockItemDAL stockItem in itemsNamunt)
            {
                if (stockItem.itemID == itemID)
                {
                    stockItem.amount = stockItem.amount + amountDiff;
                }

            }
            context.SaveChanges();

            context = new MarketContext();
            RegisteredDAL user = GetRegisteredDAL(context, userName);
            if (user == null)
                throw new Exception($"user: {userName} not in system");

            if (user._cart._shoppingBaskets.Where(b => b._store._storeName == storeName).Any())
            {
                ShoppingBasketDAL toModify = user._cart._shoppingBaskets.Where(b => b._store._storeName == storeName).FirstOrDefault();
                toModify._items = shoppingBasketDAL._items;
            }
            else
            {
                user._cart._shoppingBaskets.Add(shoppingBasketDAL);
            }
            context.Update(user);
            context.SaveChanges();
        }

        //public ShoppingCartDAL ViewMyCart(){} ---- no need. the field is updated
        public void addStorePurchase(ShoppingBasketDAL basket, DateTime date, string storename)
        {
            MarketContext context = new MarketContext();
            basket._store = GetStoreInformation(storename);
            if (!context.StorePurchaseHistory.Where(x => x._storeName == storename).Any())
            {
                StorePurchasedBasketDAL storePurchasedBasketDAL = new StorePurchasedBasketDAL();
                storePurchasedBasketDAL._storeName = storename;
                storePurchasedBasketDAL._PurchasedBaskets = new List<PurchasedBasketDAL>();
                context.StorePurchaseHistory.Add(storePurchasedBasketDAL);
                context.SaveChanges();
                context = new MarketContext();
            }

            PurchasedBasketDAL PurchasedBasket = new PurchasedBasketDAL();
            PurchasedBasket._PurchasedBasket = basket;
            PurchasedBasket._purchaseDate = date;

            context.StorePurchaseHistory.Include(x=> x._PurchasedBaskets).FirstOrDefault(x => x._storeName == storename)._PurchasedBaskets.Add(PurchasedBasket);
            context.SaveChanges();
        }
        public void addRegisteredPurchse(ShoppingCartDAL cart, DateTime date, string userName)
        {
            MarketContext context = new MarketContext();
            RegisteredDAL registeredDAL = GetRegisteredDAL(context, userName);
            if (registeredDAL == null)
                throw new Exception($"user: {userName} not in system");

            registeredDAL._cart = new ShoppingCartDAL();
            context.SaveChanges();


            context = new MarketContext();
            foreach (ShoppingBasketDAL basket in cart._shoppingBaskets)
            {
                basket._store = context.StoreDALs
                                            .Include(x => x._stock)
                                            .Include(x => x._rating)
                                            .FirstOrDefault(s => s._storeName == basket._store._storeName);
            }
            context = new MarketContext();
            RegisteredPurchasedCartDAL registeredPurchasedCartDAL = context.RegisteredPurchaseHistory
                                                                               .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._additionalDiscounts)
                                                                               .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._items).ThenInclude(x => x.purchaseDetails)
                                                                               .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                                               .FirstOrDefault(s => s.userName == userName);
            if (registeredPurchasedCartDAL == null)
            {
                registeredPurchasedCartDAL = new RegisteredPurchasedCartDAL();
                registeredPurchasedCartDAL._PurchasedCarts = new List<PurchasedCartDAL>();
                registeredPurchasedCartDAL.userName = userName;
                context.RegisteredPurchaseHistory.Add(registeredPurchasedCartDAL);
                context.SaveChanges();
                context = new MarketContext();
            }
            PurchasedCartDAL PurchasedCart = new PurchasedCartDAL();
            PurchasedCart._PurchasedCart = cart;
            PurchasedCart._purchaseDate = date;
            //registeredPurchasedCartDAL._PurchasedCarts.Add(PurchasedCart);

            context.RegisteredPurchaseHistory.Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._stock)
                                             .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._rating)
                                             .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._additionalDiscounts)
                                             .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._items).ThenInclude(x => x.purchaseDetails)
                                             .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                             .FirstOrDefault(s => s.userName == userName)._PurchasedCarts.Add(PurchasedCart);
            context.SaveChanges();

           
        }
        public void OpenNewStore(String storeName, string founderName)
        {
            MarketContext context = new MarketContext();
            StoreDAL store = new StoreDAL(storeName, StoreState.Active);
            store._stock = new List<StockItemDAL>();
            store._rating = new List<RateDAL>();
            store._purchasePolicyJSON = "";
            store._discountPolicyJSON = "";
            context.StoreDALs.Add(store);
            context.SaveChanges();
            context = new MarketContext();
            StoreFounderDAL founder = new StoreFounderDAL();
            founder._username = founderName;
            founder._storeName = storeName;
            context.StoreFounderDALs.Add(founder);
            context.SaveChanges();
        }

        internal List<MessageToStoreDAL> GetOpenMessagesToStoreByStoreName(string storeName)
        {
            MarketContext context = new MarketContext();
            List<MessageToStoreDAL> returnMessages = new List<MessageToStoreDAL>();
            List<MessageToStoreDAL> messages = context.MessageToStoreDALs.Where(msg => (msg._storeName == storeName)).ToList();
            foreach (MessageToStoreDAL message in messages)
            {
                if (message.Status() != StoreMessageStatus.Closed)
                {
                    returnMessages.Add(message);
                }
            }
            return returnMessages;
        }
        internal List<MessageToStoreDAL> GetRepliedMessagesToStoreByUserName(string userName)
        {
            MarketContext context = new MarketContext();
            List<MessageToStoreDAL> returnMessages = new List<MessageToStoreDAL>();
            List<MessageToStoreDAL> messages = context.MessageToStoreDALs.Where(msg => (msg._senderUsername == userName)).ToList();
            foreach (MessageToStoreDAL message in messages)
            {
                if (message.Status() != StoreMessageStatus.Open)
                {
                   returnMessages.Add(message);
                }
            }
            return returnMessages;
        }

        public void AddStoreManager(String managerUsername, String storeName, string appointer)
        {
            MarketContext context = new MarketContext();
            StoreManagerDAL storeManager = new StoreManagerDAL(appointer, managerUsername, storeName);
            context.storeManagerDALs.Add(storeManager);
            context.SaveChanges();  
        }
        public void AddStoreOwner(String ownerUsername, String storeName, string appointer)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            OwnerAcceptors oe = storeDAL._standbyOwners.FirstOrDefault(x => x._newOwner == ownerUsername);
            if (oe != null)
                storeDAL._standbyOwners.Remove(oe);
            StoreOwnerDAL storeOwner = new StoreOwnerDAL(appointer, ownerUsername, storeName);
            context.storeOwnerDALs.Add(storeOwner);
            context.SaveChanges();
        }
        /// <summary>
        /// only update the acceptors-table. not add to owners-table even if accepted by all.
        /// if you want to add an actual owner, call AddStoreOwner.
        /// </summary>
        /// <param name="ownerUsername"></param>
        /// <param name="storeName"></param>
        /// <param name="acceptor"></param>
        /// <exception cref="Exception"></exception>
        public void AcceptOwnerAppointment(String ownerUsername, String storeName, string acceptor)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            if (storeDAL == null)
                throw new Exception($"no such store called '{storeName}'.");
            OwnerAcceptors oe = storeDAL._standbyOwners.FirstOrDefault(x => x._newOwner == ownerUsername);
            if (oe != null)
            {
                oe._acceptors.Add(new StringData(acceptor));
            }
            else
            {
                ICollection<StringData> acceptors = new List<StringData>();
                storeDAL._standbyOwners.Add(new OwnerAcceptors(ownerUsername, acceptors, acceptor));
            }
            context.SaveChanges();
        }
        public void RejectOwnerAppointment(string storeName, string newOwner)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            OwnerAcceptors oe = storeDAL._standbyOwners.FirstOrDefault(x => x._newOwner == newOwner);
            if (oe != null)
            {
                storeDAL._standbyOwners.Remove(oe);
            }
            else
            {
                throw new Exception($"no such standby owner called '{newOwner}' to reject.");
            }
            context.SaveChanges();
        }
        public void RemoveStoreOwner(String ownerUsername, String storeName)
        {
            MarketContext context = new MarketContext();
            StoreOwnerDAL owner = context.storeOwnerDALs.Include(r => r._operationsWrappers).Where(r => r._storeName == storeName && r._username == ownerUsername).FirstOrDefault();
            context.SystemRoleDALs.Remove(owner);
            context.SaveChanges();
        }
        public void RemoveStoreManager(String managerUsername, String storeName)
        {
            MarketContext context = new MarketContext();
            StoreManagerDAL manager = context.storeManagerDALs.Include(r => r._operationsWrappers).Where((r) => (r._storeName == storeName && r._username == managerUsername)).FirstOrDefault();
            context.SystemRoleDALs.Remove(manager);
            context.SaveChanges();
        }
        public int AddItemToStoreStock(String storeName, String name, double price, String description, String category, int quantity)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            ItemDAL item = new ItemDAL(new List<RateDAL>(), name, price,
                description, category);

            context.itemDALs.Add(item);
            context.SaveChanges();
            context = new MarketContext();
            GetStoreDAL(context, storeName)._stock.Add(new StockItemDAL(item._itemID, quantity));
            context.SaveChanges();
            return item._itemID;
        }
        public void RemoveItemFromStore(String storeName, int itemID)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            foreach (StockItemDAL stockItem in storeDAL._stock)
            {
                if(stockItem.itemID == itemID)
                {
                    storeDAL._stock.Remove(stockItem);
                    break;
                }
            }
            context.SaveChanges();
        }
        public void UpdateStockQuantityOfItem(String storeName, int itemID, int newQuantity)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            foreach (StockItemDAL stockItem in storeDAL._stock)
            {
                if (stockItem.itemID == itemID)
                {
                    stockItem.amount = newQuantity;
                    break;
                }
            }
            context.SaveChanges();
        }
        public void EditItemPrice(String storeName, int itemID, double newPrice)
        {
            MarketContext context = new MarketContext();
            context.itemDALs.Find(itemID)._price = newPrice;
            context.SaveChanges();
        }
        public void EditItemName(String storeName, int itemID, String newName)
        {
            MarketContext context = new MarketContext();
            context.itemDALs.Find(itemID)._name = newName;
            context.SaveChanges();
        }
        public void EditItemDescription(String storeName, int itemID, String newDescription)
        {
            MarketContext context = new MarketContext();
            context.itemDALs.Find(itemID)._description = newDescription;
            context.SaveChanges();
        }

        public void EditStoreDiscountPolicy(string storeName, string newDiscountPolicyJSON)
        {
            MarketContext context = new MarketContext();
            context.StoreDALs.Find(storeName)._discountPolicyJSON = newDiscountPolicyJSON;
            context.SaveChanges();
        }

        public void EditStorePurchasePolicy(string storeName, string newPurchasePolicyJSON)
        {
            MarketContext context = new MarketContext();
            context.StoreDALs.Find(storeName)._purchasePolicyJSON = newPurchasePolicyJSON;
            context.SaveChanges();
        }

        internal ICollection<ComplaintDAL> GetRgisteredFiledComplaintsByUsername(string username)
        {
            MarketContext context = new MarketContext();
            return context.ComplaintDALs.Where(c => c._complainer == username).ToList();
        }

        internal ICollection<SystemRoleDAL> GetRegisteredRolesByUsername(string username)
        {
            MarketContext context = new MarketContext();
            return context.SystemRoleDALs.Include(r => r._operationsWrappers).Where(r => r._username == username).ToList();
        }

        public void RateItem(int itemID, String storeName, int rating, String review, string userName)
        {
            MarketContext context = new MarketContext();
            context.itemDALs.Find(itemID)._rating.Add(new RateDAL(userName, rating, review));
            context.SaveChanges();
        }
        public void RateStore(String storeName, int rating, String review, string userName)
        {
            MarketContext context = new MarketContext();
            StoreDAL storeDAL = GetStoreDAL(context, storeName);
            RateDAL rate = new RateDAL(userName, rating, review);
            storeDAL._rating.Add(rate);
            context.SaveChanges();
        }
        public StoreDAL GetStoreInformation(String storeName)
        {
            MarketContext context = new MarketContext();
            return GetStoreDAL(context, storeName);
        }
        public ItemDAL GetItem(int itemID)
        {
            MarketContext context = new MarketContext();
            return context.itemDALs.Where(x => x._itemID == itemID).FirstOrDefault();
        }
        public int SendMessageToStore(String storeName, String title, String message, string sender)
        {
            MarketContext context = new MarketContext();
            MessageToStoreDAL msg = new MessageToStoreDAL(sender, message, title, storeName);
            context.MessageToStoreDALs.Add(msg);
            context.SaveChanges();
            return msg.mid;
        }
        public int FileComplaint(int cartID, String message, string sender)
        {
            MarketContext context = new MarketContext();
            ComplaintDAL complaint = new ComplaintDAL(sender, cartID, message);

            context.ComplaintDALs.Add(complaint);
            context.SaveChanges();
            return complaint._id;
        }
        public List<Tuple<DateTime, ShoppingCartDAL>> GetMyPurchasesHistory(string userName)
        {
            MarketContext context = new MarketContext();
            List<Tuple<DateTime, ShoppingCartDAL>> history = new List<Tuple<DateTime, ShoppingCartDAL>>();
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._stock)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._rating)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._additionalDiscounts)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._items).ThenInclude(x => x.purchaseDetails)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                                                .FirstOrDefault(s => s.userName == userName);
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
            MarketContext context = new MarketContext();
            List<Tuple<DateTime, ShoppingBasketDAL>> history = new List<Tuple<DateTime, ShoppingBasketDAL>>();
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._stock)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._rating)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._additionalDiscounts)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._items).ThenInclude(x => x.purchaseDetails)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                                                .FirstOrDefault(s => s.userName == userName);
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
            MarketContext context = new MarketContext();
            RegisteredPurchasedCartDAL reg_history = context.RegisteredPurchaseHistory
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._stock)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._store).ThenInclude(s => s._rating)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._additionalDiscounts)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._items).ThenInclude(x => x.purchaseDetails)
                                                                                .Include(x => x._PurchasedCarts).ThenInclude(x => x._PurchasedCart).ThenInclude(x => x._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                                                .FirstOrDefault(s => s.userName == userName);

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
            MarketContext context = new MarketContext();
            return GetRegisteredDAL(context, userName);
        }
        public void EditVisitorPassword(String newPassword, string newSalt, string userName)
        {
            MarketContext context = new MarketContext();
            RegisteredDAL registered = GetRegisteredDAL(context, userName);
            registered._password = newPassword;
            registered._salt = newSalt;
            context.SaveChanges();
        }
        public void RemoveManagerPermission(String managerUsername, String storeName, Operation op)
        {
            MarketContext context = new MarketContext();
            StoreManagerDAL manager = context.storeManagerDALs
                                                        .Include(r => r._operationsWrappers)
                                                        .Where((r) => (r._storeName == storeName) && (r._username == managerUsername)).FirstOrDefault();

            manager._operations.Remove(op);
            manager._operationsWrappers.Remove(manager._operationsWrappers.Find(x => x.op.Equals(op)));
            context.SaveChanges();
            //since both register and store holds ptr to the same manager Obj, the removal will be seen in both.
        }
        public void AddManagerPermission(String managerUsername, String storeName, Operation op)
        {
            MarketContext context = new MarketContext();
            StoreManagerDAL manager = context.storeManagerDALs
                                                        .Include(r => r._operationsWrappers)
                                                        .Where((r) => (r._storeName == storeName) && (r._username == managerUsername)).FirstOrDefault();
            manager._operations.Add(op);
            manager._operationsWrappers.Add(new OperationWrapper(op));
            context.SaveChanges();
            //since both register and store holds ptr to the same manager Obj, the addition will be seen in both.
        }
        public void CloseStore(String storeName)
        {
            MarketContext context = new MarketContext();
            StoreDAL store = GetStoreDAL(context, storeName);
            store._state = StoreState.Inactive;
            context.SaveChanges();
        }
        public void ReopenStore(String storeName)
        {
            MarketContext context = new MarketContext();
            StoreDAL store = GetStoreDAL(context, storeName);
            store._state = StoreState.Active;
            context.SaveChanges();
        }
        public List<StoreOwnerDAL> GetStoreOwners(String storeName)
        {
            MarketContext context = new MarketContext();
            return context.storeOwnerDALs
                                        .Include(r => r._operationsWrappers)
                                        .Where(r => (r._storeName == storeName) ).ToList();
        }
        public List<StoreManagerDAL> GetStoreManagers(String storeName)
        {
            MarketContext context = new MarketContext();
            return context.storeManagerDALs
                                        .Include(r => r._operationsWrappers)
                                        .Where(r => (r._storeName == storeName)).ToList();
        }
        public StoreFounderDAL GetStoreFounder(String storeName)
        {
            MarketContext context = new MarketContext();
            return context.StoreFounderDALs
                                        .Include(r => r._operationsWrappers)
                                        .Where(r => (r._storeName == storeName)).FirstOrDefault();
        }

        public void AnswerStoreMesseage(int msgID, string storeName, string reply, string replier)
        {
            MarketContext context = new MarketContext();
            MessageToStoreDAL msg = context.MessageToStoreDALs.Find(msgID);
            if (msg == null)
                throw new Exception($"there is no message with id: {msgID} in db");
            msg._reply = reply;
            msg._replierFromStore = replier;
            context.SaveChanges();
        }
        public List<Tuple<DateTime, ShoppingBasketDAL>> GetStorePurchasesHistory(String storeName)
        {
            MarketContext context = new MarketContext();
            List<Tuple<DateTime, ShoppingBasketDAL>> history = new List<Tuple<DateTime, ShoppingBasketDAL>>();
            StorePurchasedBasketDAL store_basket = context.StorePurchaseHistory
                                                                            .Include(x => x._PurchasedBaskets).ThenInclude(x => x._PurchasedBasket).ThenInclude(x => x._store).ThenInclude(s => s._stock)
                                                                            .Include(x => x._PurchasedBaskets).ThenInclude(x => x._PurchasedBasket).ThenInclude(x => x._store).ThenInclude(s => s._rating)
                                                                            .Include(x => x._PurchasedBaskets).ThenInclude(x => x._PurchasedBasket).ThenInclude(x => x._additionalDiscounts)
                                                                            .Include(x => x._PurchasedBaskets).ThenInclude(x => x._PurchasedBasket).ThenInclude(x => x._items).ThenInclude(x => x.purchaseDetails)
                                                                            .Include(x => x._PurchasedBaskets).ThenInclude(x => x._PurchasedBasket).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                                            .FirstOrDefault(s => s._storeName == storeName);
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
            MarketContext context = new MarketContext();
            StoreDAL store = GetStoreDAL(context, storeName);
            store._state = StoreState.Closed;
            context.SaveChanges();
        }
        public ICollection<ComplaintDAL> GetRegisterdComplaints()
        {
            MarketContext context = new MarketContext();
            return context.ComplaintDALs.ToList();
        }
        public void ReplyToComplaint(int complaintID, String reply)
        {
            MarketContext context = new MarketContext();
            ComplaintDAL complaintDAL = context.ComplaintDALs.Find(complaintID);
            if(complaintDAL == null)
                throw new Exception("complaint not found in db");
            complaintDAL._response = reply;
            context.SaveChanges();
        }
        public ICollection<AdminMessageToRegisteredDAL> GetRegisteredMessagesFromAdmin(string username)
        {
            MarketContext context = new MarketContext();
            return GetRegisteredDAL(context, username)._adminMessages.ToList();
        }
        public ICollection<MessageToStoreDAL> GetRegisterAnsweredStoreMessages(string username)
        {
            MarketContext context = new MarketContext();
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
            MarketContext context = new MarketContext();
            return GetRegisteredDAL(context, username)._notifications.ToList();
        }
        public void AppointSystemAdmin(String adminUsername)
        {
            MarketContext context = new MarketContext();
            SystemAdminDAL systemAdminDAL = new SystemAdminDAL(adminUsername);
            context.SystemAdminDALs.Add(systemAdminDAL);
            context.SaveChanges();
        }
        public List<StoreDAL> GetStoresOfUser(string username)
        {
            MarketContext context = new MarketContext();
            List<string> storesnames = context.SystemRoleDALs
                                                        .Include(r => r._operationsWrappers)
                                                        .Where(r => (r._username == username) && (r._storeName != null))
                                                        .Select(r => r._storeName).ToList();
            List<StoreDAL> stores = new List<StoreDAL>();
            context = new MarketContext();
            foreach (string storeName in storesnames)
            {
                stores.Add(GetStoreDAL(context, storeName));
            }
            return stores;
        }
        public int SendAdminMessage(String receiverUsername, string senderUsername, String title, String message)
        {
            MarketContext context = new MarketContext();
            RegisteredDAL registeredDAL = GetRegisteredDAL(context, receiverUsername);
            if (registeredDAL == null)
                throw new Exception($"there is no user with username: {receiverUsername} in db");
            AdminMessageToRegisteredDAL msg = new AdminMessageToRegisteredDAL(receiverUsername, senderUsername, title, message);
            registeredDAL._adminMessages.Add(msg);
            context.SaveChanges();
            return msg.mid;
        }

        public bool StoreExists(string storeName)
        {
            MarketContext context = new MarketContext();
            return context.StoreDALs.Find(storeName) != null;
        }
        public int SendNotification(string storeName, string usernameReciever, String title, String message)
        {
            MarketContext context = new MarketContext();
            RegisteredDAL reg = GetRegisteredDAL(context, usernameReciever);
            if (usernameReciever == null)
                throw new Exception($"there is no such user with uaername: {usernameReciever}");
            NotifyMessageDAL notifyMessageDAL = new NotifyMessageDAL(title, message, usernameReciever);
            reg._notifications.Add(notifyMessageDAL);
            context.SaveChanges();
            return notifyMessageDAL.mid;
        }
        public ComplaintDAL GetComplaint(int id)
        {
            MarketContext context = new MarketContext();
            return context.ComplaintDALs.Find(id);
        }
        private bool CommonVarInLists<T>(List<T> l1, List<T> l2)
        {
            MarketContext context = new MarketContext();
            List<T> res = l1.Where(x => l2.Contains(x)).ToList();
            return res.Any();       
        }
        public String GetRoleUsername(int roleid)
        {
            MarketContext context = new MarketContext();
            SystemRoleDAL role = context.SystemRoleDALs
                                                    .Include(r=> r._operationsWrappers)
                                                    .FirstOrDefault(r=> r.id == roleid);
            if(role == null)
                throw new Exception($"role id {roleid} was not found in the database.");
            return role._username;
        }

        public String GetRoleStoreName(int roleid)
        {
            MarketContext context = new MarketContext();
            SystemRoleDAL role = context.SystemRoleDALs
                                                    .Include(r => r._operationsWrappers)
                                                    .FirstOrDefault(r => r.id == roleid);
            if (role == null)
                throw new Exception($"role id {roleid} was not found in the database.");
            return role._storeName;
        }

        public String GetReceiverOfAdminMessage(int mid)
        {
            MarketContext context = new MarketContext();
            List<RegisteredDAL> regs = context.RegisteredDALs
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._items).ThenInclude(i => i.purchaseDetails)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._store).ThenInclude(x => x._stock)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._store).ThenInclude(x => x._rating)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._additionalDiscounts)
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
            MarketContext context = new MarketContext();
            List<RegisteredDAL> regs = context.RegisteredDALs
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._items).ThenInclude(i => i.purchaseDetails)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._store).ThenInclude(x => x._stock)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(x => x._bids).ThenInclude(x => x._acceptors)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._store).ThenInclude(x => x._rating)
                                                .Include(x => x._cart).ThenInclude(c => c._shoppingBaskets).ThenInclude(b => b._additionalDiscounts)
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
            MarketContext context = new MarketContext();
            MessageToStoreDAL msg = context.MessageToStoreDALs.Find(mid);
            if(msg == null)
                throw new Exception($"message id {mid} was not found in the database.");
            return msg._storeName;
            
        }

        internal void BidItemInStore(string bidder, string storeName, int itemId, int amount, double newPrice)
        {
            MarketContext context = new MarketContext();
            StoreDAL store = GetStoreDAL(context, storeName);
            bool found = false;
            foreach(BidsOfVisitor bov in store._bidsOfVisitors)
            {
                if (bov._bidder == bidder)
                {
                    found = true;
                    bov._bids.Add(new BidDAL(bidder, itemId, amount, newPrice));
                    break;
                }
                    
            }
            if (!found)
            {
                List<BidDAL> lst = new List<BidDAL>();
                lst.Add(new BidDAL(bidder, itemId, amount, newPrice));
                store._bidsOfVisitors.Add(new BidsOfVisitor(bidder, lst));
            }
            context.SaveChanges();
        }

        internal void AddAcceptedBidToCart(string bidder, StoreDAL storeArg, int itemID, int amount, double price)
        {
            MarketContext context = new MarketContext();
            RegisteredDAL reg = GetRegisteredDAL(context, bidder);
            StoreDAL myStore = GetStoreDAL(context, storeArg._storeName);
            bool found = false;
            foreach (ShoppingBasketDAL basket in reg._cart._shoppingBaskets)
            {
                if (basket._store._storeName == myStore._storeName)
                {
                    found = true;
                    basket._bids.Add(new BidDAL(bidder, itemID, amount, price));
                    break;
                }
            }
            if (!found)
            {
                List<BidDAL> lst = new List<BidDAL>();
                lst.Add(new BidDAL(bidder, itemID, amount, price));
                reg._cart._shoppingBaskets.Add(new ShoppingBasketDAL(myStore, new List<BasketItemDAL>(), new PurchaseDetailsDAL(), lst));
            }
            BidsOfVisitor bov = myStore._bidsOfVisitors.FirstOrDefault(x => x._bidder == bidder);
            if (bov != null)
            {
                BidDAL bid = bov._bids.FirstOrDefault(x => x._itemId == itemID && x._amount == amount);
                if (bov._bids.Count <= 1 && bid != null)
                    myStore._bidsOfVisitors.Remove(bov);
                else if (bid != null)
                    bov._bids.Remove(bid);
            }
            context.SaveChanges();
        }

        internal void AcceptBid(string storeName, string acceptor, int itemId, string bidder, bool acceptedByAll)
        {
            MarketContext context = new MarketContext();
            StoreDAL store = GetStoreDAL(context, storeName);
            bool found = false;
            foreach (BidsOfVisitor bov in store._bidsOfVisitors)
            {
                if (bov._bidder == bidder)
                {
                    found = true;
                    try
                    {
                        BidDAL bid = bov._bids.Where(x => x._bidder == bidder && x._itemId == itemId).FirstOrDefault();
                        bid._acceptors.Add(new StringData(acceptor));
                        bid._acceptedByAll = acceptedByAll;
                        break;
                    }
                    catch (Exception)
                    {
                        throw new Exception($"no such bid with itemId '{itemId}'.");
                    }
                }
            }
            if (!found)
            {
                throw new Exception($"no bids in this store for this visitor.");
            }
            context.SaveChanges();
        }

        internal void CounterOffer(string storeName, string acceptor, int itemId, string bidder, double counterOffer, bool acceptedByAll)
        {
            MarketContext context = new MarketContext();
            StoreDAL store = GetStoreDAL(context, storeName);
            bool found = false;
            foreach (BidsOfVisitor bov in store._bidsOfVisitors)
            {
                if (bov._bidder == bidder)
                {
                    found = true;
                    try
                    {
                        BidDAL bid = bov._bids.Where(x => x._bidder == bidder && x._itemId == itemId).FirstOrDefault();
                        bid._acceptors.Add(new StringData(acceptor));
                        if (bid._counterOffer < counterOffer && bid._biddedPrice < counterOffer)
                            bid._counterOffer = counterOffer;
                        bid._acceptedByAll = acceptedByAll;
                        break;
                    }
                    catch (Exception)
                    {
                        throw new Exception($"no such bid with itemId '{itemId}'.");
                    }
                }
            }
            if (!found)
            {
                throw new Exception($"no bids in this store for this visitor.");
            }
            context.SaveChanges();
        }

        internal void RejectBid(string storeName, string rejector, int itemId, string bidder)
        {
            MarketContext context = new MarketContext();
            StoreDAL store = GetStoreDAL(context, storeName);
            bool found = false;
            foreach (BidsOfVisitor bov in store._bidsOfVisitors)
            {
                if (bov._bidder == bidder)
                {
                    found = true;
                    try
                    {
                        BidDAL bid = bov._bids.Where(x => x._bidder == bidder && x._itemId == itemId).FirstOrDefault();
                        bov._bids.Remove(bid);
                        break;
                    }
                    catch (Exception)
                    {
                        throw new Exception($"no such bid with itemId '{itemId}'.");
                    }
                }
            }
            if (!found)
            {
                throw new Exception($"no bids in this store for this visitor.");
            }
            context.SaveChanges();
        }
    }
}
