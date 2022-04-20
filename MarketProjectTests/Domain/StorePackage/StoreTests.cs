using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class StoreTests
    {

        Store _store;
        string storeFounder;
        int itemId;
        String name;
        String description;
        String category;
        double price;
        int quantity;

        [TestInitialize()]
        public void setup()
        {
            storeFounder = "founder";
            _store = new Store("Krusty Krab", new StoreFounder(storeFounder, "Krusty Krab"), null, null);
            itemId = 1;
            name = "Krabby Patty";
            description = "yummy";
            category = "computing";
            price = 5.0;
            quantity = 10;
        }


        [TestMethod()]
        public void RateStore_UserHasntRatedStore_NoException()
        {
            String username = "Squidward Tentacles";
            int rating = 1;
            String review = "NOOOOOOOOOOOO";

            try
            {
                _store.RateStore(username, rating, review);
                Assert.AreEqual(_store.GetRating(), "" + rating);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RateStore_UserHasRatedStore_ThrowsException()
        {
            String username = "Squidward Tentacles";
            int rating = 1;
            String review = "NOOOOOOOOOOOO";

            try
            {
                _store.RateStore(username, rating, review);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _store.RateStore(username, rating, review);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void UpdateStockQuantityOfItem_ItemExists_NoException()
        {
            String description = "Delicious";
            int newQuantity = 15;
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, category, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _store.UpdateStockQuantityOfItem(itemId, newQuantity);
                Item i = _store.GetItem(itemId);
                Assert.AreEqual(_store.Stock.GetItemAmount(i), newQuantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void UpdateStockQuantityOfItem_ItemDoesntExist_ThrowsException()
        {
            int newQuantity = 15;

            try
            {
                _store.UpdateStockQuantityOfItem(itemId, newQuantity);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void AddItemToStoreStock_ItemIdIsUnique_NoException()
        {
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, category, quantity);
                Assert.IsNotNull(_store.GetItem(itemId));
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void AddItemToStoreStock_ItemIdIsNotUnique_ThrowsException()
        {
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, category, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, category, quantity);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_ItemDoesntExist_ThrowsException()
        {
            try
            {
                _store.RemoveItemFromStore(itemId);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_ItemExists_NoException()
        {
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, category, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _store.RemoveItemFromStore(itemId);
                Item i = _store.GetItem(itemId);
                Assert.IsNull(i);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void AddStoreManager_AddManagerTwice_returnsFalse()
        {
            bool arrange = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));

            bool act = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));

            Assert.IsFalse(act);
        }

        [TestMethod]
        public void AddStoreManager_AddManagerWhileIsOwner_returnsFalse()
        {
            bool arrange = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));

            bool act = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));

            Assert.IsFalse(act);
        }

        [TestMethod]
        public void AddStoreManager_AddManager_returnsTrue()
        {
            bool act = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));
            Assert.IsTrue(act);
        }

        [TestMethod]
        public void AddStoreOwner_AddOwnerTwice_returnsFalse()
        {
            bool arrange = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));

            bool act = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));

            Assert.IsFalse(act);
        }

        [TestMethod]
        public void AddStoreOwner_AddOwnerWhileIsManager_returnsFalse()
        {
            bool arrange = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));

            bool act = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));

            Assert.IsFalse(act);
        }

        [TestMethod]
        public void AddStoreOwner_AddOwner_returnsTrue()
        {
            bool act = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));
            Assert.IsTrue(act);
        }
    }
}