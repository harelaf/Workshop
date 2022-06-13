using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MarketWeb.Service;
using MarketWeb.Shared;

namespace AcceptanceTest
{
    [TestClass]
    public class PurchasePolicyTests
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        String storeName = "test's Shop";
        //String storeName_outSystem = "bla";
        String guest_VisitorToken;
        String store_founder_token;
        String store_founder_name;
        DateTime expiration = DateTime.Now.AddDays(1);
        int itemID = 1;
        double itemprice = 10;
        String itemDesc = "Yummy";
        String itemName = "item";
        String category = "category";
        DateTime bDay = new DateTime(1992, 8, 4);

        String address = "Earth";
        String city = "Mars";
        String country = "The Sun";
        String zip = "Milky Way";
        String name = "Peter Griffin";
        String paymentMethod = "Alien Technology";
        String shipmentMethod = "Spacecraft";


        [TestInitialize]
        public void setup()
        {
            guest_VisitorToken = (marketAPI.EnterSystem()).Value;
            store_founder_token = (marketAPI.EnterSystem()).Value;// guest
            store_founder_name = "afik";
            marketAPI.Register(store_founder_token, store_founder_name, "123456789", bDay);
            store_founder_token = (marketAPI.Login(store_founder_token, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(store_founder_token, storeName);
            marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemID, itemName, itemprice, itemDesc, category, 100);
        }

        [TestMethod]
        public void TryPurchaseCart_SimplePurchasePolicyAllows_success()
        {
            String policy = $"TotalBasketPriceFrom_{25}";
            Response res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);

            Response res2 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, 5);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);

            Response res3 = marketAPI.PurchaseMyCart(guest_VisitorToken, address, city, country, zip, name, paymentMethod, shipmentMethod).Result;
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);
        }

        [TestMethod]
        public void TryPurchaseCart_SimplePurchasePolicyDenies_failure()
        {
            String policy = $"TotalBasketPriceFrom_{300}";
            Response res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);

            Response res2 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, 5);
            Assert.IsTrue(res2.ErrorOccured, "res2 " + res2.ErrorMessage);

            Response res3 = marketAPI.PurchaseMyCart(guest_VisitorToken, address, city, country, zip, name, paymentMethod, shipmentMethod).Result;
            Assert.IsTrue(res3.ErrorOccured, "res3 " + res3.ErrorMessage);
        }

        [TestMethod]
        public void TryPurchaseCart_ComplicatedPurchasePolicyAllows_success()
        {
            String policy = $"TotalBasketPriceFrom_{25}";
            Response res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            policy = $"ItemTotalAmountInBasketFrom_{itemName}_{4}";
            res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            policy = $"(AND DayOfWeek_{((int)DateTime.Now.DayOfWeek + 1) % 7} Hour_0_24)";
            res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            policy = $"(OR TotalBasketPriceTo_{75})";
            res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);

            Response res2 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, 5);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);

            Response res3 = marketAPI.PurchaseMyCart(guest_VisitorToken, address, city, country, zip, name, paymentMethod, shipmentMethod).Result;
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);
        }

        [TestMethod]
        public void TryPurchaseCart_ComplicatedPurchasePolicyDenies_failure()
        {
            String policy = $"TotalBasketPriceFrom_{25}";
            Response res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            policy = $"ItemTotalAmountInBasketFrom_{itemName}_{4}";
            res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            policy = $"(AND DayOfWeek_{((int)DateTime.Now.Day + 1) % 7} Hour_{(DateTime.Now.Hour + 23) % 24}_{(DateTime.Now.Hour + 1) % 24})";
            res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            policy = $"(OR TotalBasketPriceTo_{10})"; // False
            res1 = marketAPI.AddStorePurchasePolicy(store_founder_token, storeName, policy);
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);

            Response res2 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, 5);
            Assert.IsTrue(res2.ErrorOccured, "res2 " + res2.ErrorMessage);

            Response res3 = marketAPI.PurchaseMyCart(guest_VisitorToken, address, city, country, zip, name, paymentMethod, shipmentMethod).Result;
            Assert.IsTrue(res3.ErrorOccured, "res3 " + res3.ErrorMessage);
        }
    }
}
