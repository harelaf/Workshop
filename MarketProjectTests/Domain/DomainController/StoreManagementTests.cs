using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class StoreManagementTests
    {
        [TestMethod()]
        public void RateStore_StoreExists_NoException()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String username = "Sandy Cheeks";
            String storeName = "Krusty Krab";
            int rating = 10;
            String review = "I LOVE KRABS";
            try
            {
                _storeManagement.OpenNewStore(null, storeName, null, null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _storeManagement.RateStore(username, storeName, rating, review);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RateStore_StoreDoesntExist_ThrowsException()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String username = "Sandy Cheeks";
            String storeName = "Krusty Krab";
            int rating = 10;
            String review = "I LOVE KRABS";

            try
            {
                _storeManagement.RateStore(username, storeName, rating, review);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void UpdateStockQuantityOfItem_StoreExistsItemExists_NoException()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String username = "Sandy Cheeks";
            String storeName = "Krusty Krab";
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "Delicious";
            int quantity = 5;
            int newQuantity = 10;
            try
            {
                _storeManagement.OpenNewStore(null, storeName, null, null);
                //_store.addItem(...);
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
            StoreManagement _storeManagement = new StoreManagement();
            String storeName = "Krusty Krab";
            int itemId = 1;
            int newQuantity = 10;

            try
            {
                _storeManagement.UpdateStockQuantityOfItem(storeName, itemId, newQuantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }


        public void SendMessageToStore_StoreExist_Success()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String username = "Sandy Cheeks";
            String storeName = "Krusty Krab";
            String title = "reservation";
            String message = "Hey, I want to reserve a place for 6 diners today at 20:30.";

            try
            {
                _storeManagement.OpenNewStore(null, storeName, null, null);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            try
            {
                _storeManagement.SendMessageToStore(username, storeName, title, message);
            }
            catch (Exception)
            {
                Assert.Fail();
            }

            Assert.Fail();
        }

        public void SendMessageToStore_StoreDoesntExist_Success()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String username = "Sandy Cheeks";
            String storeName = "Krusty Krab";
            String title = "reservation";
            String message = "Hey, I want to reserve a place for 6 diners today at 20:30.";

            try
            {
                _storeManagement.SendMessageToStore(username, storeName, title, message);
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }

            Assert.Fail();
        }
    }
}