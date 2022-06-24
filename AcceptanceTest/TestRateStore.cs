using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;

namespace AcceptanceTest
{
    // Acceptance tests for requirement: II.3.4
    [TestClass]
    public class TestRateStore
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        string storeName_inSystem = "Krusty Krab";
        string storeName_outSystem = "Chum Bucket";
        string username = "SpongeBob SquarePants";
        string guest_userToken = "";
        string registered_userToken = "";
        string review = "";
        int ratingInRange;
        public static readonly string paymentMethode_mock_flase = "mock_false";
        public static readonly string paymentMethode_mock_true = "mock_true";
        public static readonly string shippingMethode_mock_flase = "mock_false";
        public static readonly string shippingMethode_mock_true = "mock_true";

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

        [TestInitialize]
        public void setup()
        {
            guest_userToken = (marketAPI.EnterSystem()).Value;
            registered_userToken = (marketAPI.EnterSystem()).Value;
            marketAPI.Register(registered_userToken, username, "123456789", new DateTime(1992, 8, 4));
            registered_userToken = (marketAPI.Login(registered_userToken, username, "123456789")).Value;
            marketAPI.OpenNewStore(registered_userToken, storeName_inSystem);
            marketAPI.AddItemToStoreStock(registered_userToken, storeName_inSystem, 1, "Krabby Patty", 5.0, "Yummy", "Food", 5);
            review = "I LOVE KRABBY PATTIES";
            ratingInRange = 5;
        }

        [TestMethod]
        public void sad_UserHasntPurchasedInStore()
        {
            Response response = marketAPI.RateStore(registered_userToken, storeName_inSystem, ratingInRange, review);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_StoreDoesntExist()
        {
            Response response = marketAPI.RateStore(registered_userToken, storeName_outSystem, ratingInRange, review);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_ratingOutOfRange()
        {
            int ratingOutOfRange = -5;
            Response response = marketAPI.RateStore(registered_userToken, storeName_inSystem, ratingOutOfRange, review);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void happy_RateStoreSuccess()
        {
            Response response1 = marketAPI.AddItemToCart(registered_userToken, 1, storeName_inSystem, 1);
            Assert.IsFalse(response1.ErrorOccured);

            Response response2 = marketAPI.PurchaseMyCart(registered_userToken, "City Center", "Jerusalem", "Israel", "123456", username, paymentMethode_mock_true, shippingMethode_mock_true).Result;
            Assert.IsFalse(response2.ErrorOccured);

            Response response3 = marketAPI.RateStore(registered_userToken, storeName_inSystem, ratingInRange, review);
            Assert.IsFalse(response3.ErrorOccured);
        }
    }
}
