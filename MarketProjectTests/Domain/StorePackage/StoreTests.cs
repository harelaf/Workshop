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
            int newQuantity = 10;

            //_store.addItem(...)

            try
            {
                _store.UpdateStockQuantityOfItem(itemId, newQuantity);
            } catch(Exception)
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
    }
}