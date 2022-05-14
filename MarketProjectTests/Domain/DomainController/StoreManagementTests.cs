using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using MarketProject.Domain.PurchasePackage.DiscountPackage;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class StoreManagementTests
    {
        StoreManagement _storeManagement;
        String storeName;
        String Username;
        String founder;
        String description;
        String category;
        double price;
        int itemId;
        int quantity;

        [TestInitialize]
        public void setup()
        {
            _storeManagement = new StoreManagement();
            storeName = "Krusty Krab";
            founder = "seeker";
            _storeManagement.OpenNewStore(new StoreFounder(founder, storeName), storeName, new PurchasePolicy(), new DiscountPolicy());
            Username = "Sandy Cheeks";
            itemId = 1;
            quantity = 10;
            description = "Delicious";
            category = "sea food";
            price = 5.0;
        }

        [TestMethod()]
        public void RateStore_StoreExists_NoException()
        {
            int rating = 10;
            String review = "I LOVE KRABS";
            try
            {
                _storeManagement.OpenNewStore(new StoreFounder("founder", storeName), storeName, null, null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _storeManagement.RateStore(Username, storeName, rating, review);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RateStore_StoreDoesntExist_ThrowsException()
        {
            int rating = 10;
            String review = "I LOVE KRABS";

            try
            {
                _storeManagement.RateStore(Username, storeName, rating, review);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void UpdateStockQuantityOfItem_StoreExistsItemExists_NoException()
        {
            int newQuantity = 15;
            try
            {
                _storeManagement.OpenNewStore(new StoreFounder("founder", storeName), storeName, null, null);
                _storeManagement.AddItemToStoreStock(storeName, itemId, Username, price, description, category, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _storeManagement.UpdateStockQuantityOfItem(storeName, itemId, newQuantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void UpdateStockQuantityOfItem_StoreDoesntExist_ThrowsException()
        {

            try
            {
                _storeManagement.UpdateStockQuantityOfItem(storeName, itemId, quantity);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void AddItemToStoreStock_StoreDoesntExist_ThrowsException()
        {
            try
            {
                _storeManagement.AddItemToStoreStock(storeName, itemId, Username, price, description, category, quantity);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void AddItemToStoreStock_StoreExists_NoException()
        {
            try
            {
                _storeManagement.OpenNewStore(new StoreFounder("founder", storeName), storeName, null, null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _storeManagement.AddItemToStoreStock(storeName, itemId, Username, price, description, category, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_StoreDoesntExist_ThrowsException()
        {
            try
            {
                _storeManagement.RemoveItemFromStore(storeName, itemId);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_StoreExists_NoException()
        {
            try
            {
                _storeManagement.OpenNewStore(new StoreFounder("founder", storeName), storeName, null, null);
                _storeManagement.AddItemToStoreStock(storeName, itemId, Username, price, description, category, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _storeManagement.RemoveItemFromStore(storeName, itemId);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void SendMessageToStore_StoreExist_Success()
        {
            String title = "reservation";
            String message = "Hey, I want to reserve a place for 6 diners today at 20:30.";

            try
            {
                _storeManagement.OpenNewStore(new StoreFounder("founder", storeName), storeName, null, null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _storeManagement.SendMessageToStore(Username, storeName, title, message, 20);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [TestMethod()]
        public void SendMessageToStore_StoreDoesntExist_Success()
        {
            String title = "reservation";
            String message = "Hey, I want to reserve a place for 6 diners today at 20:30.";

            try
            {
                _storeManagement.SendMessageToStore(Username, storeName, title, message, 5);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

        }

        [TestMethod()]
        public void AddStoreManager_AddManagerTwice_throwsExeption()
        {
            bool arrange = _storeManagement.AddStoreManager(new StoreManager(Username, storeName, founder), storeName);

            Assert.ThrowsException<Exception>(() => _storeManagement.AddStoreManager(new StoreManager(Username, storeName, founder), storeName));
        }

        [TestMethod]
        public void AddStoreManager_AddManagerWhileIsOwner_throwsExeption()
        {
            bool arrange = _storeManagement.AddStoreOwner(new StoreOwner(Username, storeName, founder), storeName);

            Assert.ThrowsException<Exception>(() => _storeManagement.AddStoreManager(new StoreManager(Username, storeName, founder), storeName));
        }

        [TestMethod]
        public void AddStoreManager_AddManager_returnsTrue()
        {
            bool act = _storeManagement.AddStoreManager(new StoreManager(Username, storeName, founder), storeName);
            Assert.IsTrue(act);
        }

        [TestMethod]
        public void AddStoreOwner_AddOwnerTwice_returnsFalse()
        {
            bool arrange = _storeManagement.AddStoreOwner(new StoreOwner(Username, storeName, founder), storeName);

            Assert.ThrowsException<Exception>(() => _storeManagement.AddStoreOwner(new StoreOwner(Username, storeName, founder), storeName));
        }

        [TestMethod]
        public void AddStoreOwner_AddOwnerWhileIsManager_returnsFalse()
        {
            bool arrange = _storeManagement.AddStoreManager(new StoreManager(Username, storeName, founder), storeName);

            Assert.ThrowsException<Exception>(() => _storeManagement.AddStoreOwner(new StoreOwner(Username, storeName, founder), storeName));
        }

        [TestMethod()]
        public void AddStoreOwner_removeAsManagerAddAsOwner_returnsTrue()
        {
            bool arrange = _storeManagement.AddStoreManager(new StoreManager(Username, storeName, founder), storeName);
            arrange = arrange & _storeManagement.RemoveStoreManager(Username, storeName, founder);

            bool act = _storeManagement.AddStoreOwner(new StoreOwner(Username, storeName, founder), storeName);

            Assert.IsTrue(arrange & act);
        }

        [TestMethod]
        public void AddStoreOwner_AddOwner_returnsTrue()
        {
            bool act = _storeManagement.AddStoreOwner(new StoreOwner(Username, storeName, founder), storeName);
            Assert.IsTrue(act);
        }

        [TestMethod()]
        public void RemoveStoreManager_addAndRemove_returnstrue()
        {
            string storeManager = "tommy shelby";
            bool add = _storeManagement.AddStoreManager(new StoreManager(storeManager, storeName, founder), storeName);

            bool actual = _storeManagement.RemoveStoreManager(storeManager, storeName, founder);
            Assert.IsTrue(add);
            Assert.IsTrue(actual);
        }
        [TestMethod()]
        public void RemoveStoreManager_removeFounder_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _storeManagement.RemoveStoreManager(founder, storeName, founder));
        }

        [TestMethod()]
        public void RemoveStoreManager_NonWorker_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _storeManagement.RemoveStoreManager("123", storeName, founder));
        }

        [TestMethod]
        public void RemoveStoreManager_NonStore_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _storeManagement.RemoveStoreManager(Username, storeName + "123", founder));
        }

        [TestMethod()]
        public void RemoveStoreOwner_addAndRemove_returnstrue()
        {
            string storeOwner = "amos";
            bool add = _storeManagement.AddStoreOwner(new StoreOwner(storeOwner, storeName, founder), storeName);

            bool actual = _storeManagement.RemoveStoreOwner(storeOwner, storeName, founder);
            Assert.IsTrue(add);
            Assert.IsTrue(actual);
        }
        [TestMethod()]
        public void RemoveStoreOwner_removeFounder_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _storeManagement.RemoveStoreOwner(founder, storeName, founder));
        }

        [TestMethod()]
        public void RemoveStoreOwner_NonWorker_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _storeManagement.RemoveStoreOwner("123", storeName, founder));
        }
    }
}