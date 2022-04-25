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
        string storeName;
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
            storeFounder = "Mr. Krabs";
            storeName = "Krusty Krab";
            _store = new Store(storeName, new StoreFounder(storeFounder, "Krusty Krab"), new PurchasePolicy(), new DiscountPolicy());
            itemId = 1;
            name = "Krabby Patty";
            description = "yummy";
            category = "computing";
            price = 5.0;
            quantity = 10;
        }


        [TestMethod()]
        public void RateStore_VisitorHasntRatedStore_NoException()
        {
            String Username = "Squidward Tentacles";
            int rating = 1;
            String review = "NOOOOOOOOOOOO";

            try
            {
                _store.RateStore(Username, rating, review);
                Assert.AreEqual(_store.GetRating(), "" + rating);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RateStore_VisitorHasRatedStore_ThrowsException()
        {
            String Username = "Squidward Tentacles";
            int rating = 1;
            String review = "NOOOOOOOOOOOO";

            try
            {
                _store.RateStore(Username, rating, review);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _store.RateStore(Username, rating, review);
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
            int quantity = 5;
            double price = 5.0;
            String category = "";
            int newQuantity = 10;
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

            Assert.ThrowsException<Exception>(() => _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder)));
        }

        [TestMethod]
        public void AddStoreManager_AddManagerWhileIsOwner_returnsFalse()
        {
            bool arrange = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));

            Assert.ThrowsException<Exception>(() => _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder)));
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

            Assert.ThrowsException<Exception>(() => _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder)));
        }

        [TestMethod]
        public void AddStoreOwner_AddOwnerWhileIsManager_returnsFalse()
        {
            bool arrange = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));

            Assert.ThrowsException<Exception>(() => _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder)));
        }

        [TestMethod]
        public void AddStoreOwner_AddOwner_returnsTrue()
        {
            bool act = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));
            Assert.IsTrue(act);
        }

        [TestMethod]
        public void TestReserveItem_moreThanAmountInStock()
        {
            //item exists in stock and there is more than amount in stock
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "item1", 20, "banana", category);
            store.Stock.AddItem(item, inStock);
            int amountToReserve = 10;
            //action
            store.ReserveItem(itemID, amountToReserve);
            int expectedAmountInStock = inStock - amountToReserve;
            // Assert
            Assert.AreEqual(store.Stock.GetItemAmount(item), expectedAmountInStock);
        }
        [TestMethod]
        public void TestReserveItem_notEnoughtInStock()
        {
            //item exists in stock but there is not enought in stock
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToReserve = inStock + 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
            Assert.AreEqual(store.Stock.GetItemAmount(item), inStock);
        }
        [TestMethod]
        public void TestReserveItem_NoSuchItemInStock()
        {
            //item does'nt exists in stock.
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            int amountToReserve = 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
        }
        [TestMethod]
        public void TestReserveItem_nonPosigtiveAmountToReserve()
        {
            //trying reserve amount<=0
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToReserve = 0;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
        }
        [TestMethod]
        public void TestUnreserveItem_positiveAmount()
        {
            //item exists in stock and the given amount>0
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 0;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToUneserve = 10;
            //action
            store.UnReserveItem(item, amountToUneserve);
            int expectedAmountInStock = inStock + amountToUneserve;
            // Assert
            Assert.AreEqual(store.Stock.GetItemAmount(item), expectedAmountInStock);
        }
        [TestMethod]
        public void TestUnreserveItem_NoSuchItemInStock()
        {
            //item does'nt exists in stock.
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            int amountToUnreserve = 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.UnReserveItem(item, amountToUnreserve));
        }
        [TestMethod]
        public void TestUnreserveItem_nonPositiveAmount()
        {
            //trying unureserve amount_to_add<=0
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToUnreserve = 0;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.UnReserveItem(item, amountToUnreserve));
        }

        [TestMethod()]
        public void AddStoreOwner_removeAsManagerAddAsOwner_returnsTrue()
        {
            bool arrange = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));
            arrange = arrange & _store.RemoveStoreManager(name);

            bool act = _store.AddStoreOwner(new StoreOwner(name, _store.StoreName, storeFounder));

            Assert.IsTrue(arrange & act);
        }

        [TestMethod()]
        public void RemoveStoreManager_addAndRemove_returnstrue()
        {
            string storeManager = "tommy shelby";
            bool add = _store.AddStoreManager(new StoreManager(storeManager, storeName, storeFounder));

            bool actual = _store.RemoveStoreManager(storeManager);
            Assert.IsTrue(add);
            Assert.IsTrue(actual);
        }
        [TestMethod()]
        public void RemoveStoreManager_removeFounder_returnsfalse()
        {
            bool actual = _store.RemoveStoreManager(storeFounder);
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void RemoveStoreManager_NonWorker_returnsfalse()
        {
            bool actual = _store.RemoveStoreManager("123");
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void RemoveStoreOwner_addAndRemove_returnstrue()
        {
            string storeOwner = "amos";
            bool add = _store.AddStoreOwner(new StoreOwner(storeOwner, storeName, storeFounder));

            bool actual = _store.RemoveStoreOwner(storeOwner);
            Assert.IsTrue(add);
            Assert.IsTrue(actual);
        }
        [TestMethod()]
        public void RemoveStoreOwner_removeFounder_returnsfalse()
        {
            bool actual = _store.RemoveStoreOwner(storeFounder);
            Assert.IsFalse(actual);
        }

        [TestMethod()]
        public void RemoveStoreOwner_NonWorker_returnsfalse()
        {
            bool actual = _store.RemoveStoreOwner("123");
            Assert.IsFalse(actual);
           // Assert.ThrowsException<Exception>(() => store.UnReserveItem(item, amountToUnreserve));
        }
    }
}
