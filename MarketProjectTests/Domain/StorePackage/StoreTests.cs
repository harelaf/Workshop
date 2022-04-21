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
            String category = "";
            int newQuantity = 10;
            try
            {
                _store.AddItemToStoreStock(itemId, name, price, description, category, quantity);
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
            String category = "";
            double price = 5.0;
            int quantity = 10;

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
            Store _store = new Store("Krusty Krab", null, null, null);
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            String category = "";
            double price = 5.0;
            int quantity = 10;
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
            String category = "";
            double price = 5.0;
            int quantity = 10;
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
    }
}