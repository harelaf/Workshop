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
        [TestMethod()]
        public void RateStore_UserHasntRatedStore_NoException()
        {
            Store _store = new Store("Krusty Krab", null, null, null);
            String username = "Squidward Tentacles";
            int rating = 1;
            String review = "NOOOOOOOOOOOO";

            try
            {
                _store.RateStore(username, rating, review);
                Assert.AreEqual(_store.GetRating(), rating);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RateStore_UserHasRatedStore_ThrowsException()
        {
            Store _store = new Store("Krusty Krab", null, null, null);
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
            Store _store = new Store("Krusty Krab", null, null, null);
            String username = "Squidward Tentacles";
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "Delicious";
            int quantity = 5;
            double price = 5.0;
            int newQuantity = 10;
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, quantity);
            }
            catch(Exception)
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
            Store _store = new Store("Krusty Krab", null, null, null);
            int itemId = 1;
            int newQuantity = 10;

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
            Store _store = new Store("Krusty Krab", null, null, null);
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;

            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, quantity);
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
            Store _store = new Store("Krusty Krab", null, null, null);
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, quantity);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_ItemDoesntExist_ThrowsException()
        {
            Store _store = new Store("Krusty Krab", null, null, null);
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;

            try
            {
                _store.RemoveItemFromStore(itemId);
                Assert.Fail();
            } 
            catch(Exception)
            {
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_ItemExists_NoException()
        {
            Store _store = new Store("Krusty Krab", null, null, null);
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, quantity);
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

        [TestMethod]
        public void TestReserveItem_moreThanAmountInStock()
        {
            //item exists in stock and there is more than amount in stock
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "itwm1", 20, "banana");
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
            Item item = new Item(itemID, "itwm1", 20, "banana");
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
            Item item = new Item(itemID, "itwm1", 20, "banana");
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
            Item item = new Item(itemID, "itwm1", 20, "banana");
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
            Item item = new Item(itemID, "itwm1", 20, "banana");
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
            Item item = new Item(itemID, "itwm1", 20, "banana");
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
            Item item = new Item(itemID, "itwm1", 20, "banana");
            store.Stock.AddItem(item, inStock);
            int amountToUnreserve = 0;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.UnReserveItem(item,amountToUnreserve ));
        }
    }
}