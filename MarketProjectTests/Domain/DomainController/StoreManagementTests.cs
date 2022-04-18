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
            } catch (Exception) 
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
    }
}