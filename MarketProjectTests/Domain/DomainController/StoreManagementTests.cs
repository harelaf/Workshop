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
            double price = 5.0;
            int quantity = 5;
            int newQuantity = 10;
            try
            {
                _storeManagement.OpenNewStore(null, storeName, null, null);
                _storeManagement.AddItemToStoreStock(storeName, itemId, name, price, description, quantity);
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
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void AddItemToStoreStock_StoreDoesntExist_ThrowsException()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String storeName = "Krusty Krab";
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;

            try
            {
                _storeManagement.AddItemToStoreStock(storeName, itemId, name, price, description, quantity);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void AddItemToStoreStock_StoreExists_NoException()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String storeName = "Krusty Krab";
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;
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
                _storeManagement.AddItemToStoreStock(storeName, itemId, name, price, description, quantity);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_StoreDoesntExist_ThrowsException()
        {
            StoreManagement _storeManagement = new StoreManagement();
            String storeName = "Krusty Krab";
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;

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
            StoreManagement _storeManagement = new StoreManagement();
            String storeName = "Krusty Krab";
            int itemId = 1;
            String name = "Krabby Patty";
            String description = "yummy";
            double price = 5.0;
            int quantity = 10;
            try
            {
                _storeManagement.OpenNewStore(null, storeName, null, null);
                _storeManagement.AddItemToStoreStock(storeName, itemId, name, price, description, quantity);
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
    }
}