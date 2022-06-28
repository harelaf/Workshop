using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using System.Threading.Tasks;

namespace AcceptanceTest
{
    [TestClass]
    public class TestRateItem
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        String username = "SpongeBob SquarePants";
        String password = "ILoveKrabbyPatties123";
        String username2 = "Patrick Star";
        String password2 = "IAmVerySmart";
        String storename = "Krusty Krab";
        String itemname = "Krabby Patty";
        String category = "Patties";
        String description = "Tasty";
        double price = 15.0;
        int quantity = 100;
        DateTime dob = DateTime.Now;
        int itemId1;
        int itemId2;
        String authToken;

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

        [TestInitialize]
        public void setup()
        {
            authToken = (marketAPI.EnterSystem()).Value;
            marketAPI.Register(authToken, username, password, dob);
            authToken = (marketAPI.Login(authToken, username, password)).Value;
            marketAPI.OpenNewStore(authToken, storename);
            itemId1 = marketAPI.AddItemToStoreStock(authToken, storename, itemname, price, description, category, quantity).Value;
            itemId2 = marketAPI.AddItemToStoreStock(authToken, storename, itemname + "2", price, description + "2", category + "2", quantity).Value;
        }

        [TestMethod]
        public void PurchaseCartAndRateItem_Success()
        {
            Response res = marketAPI.AddItemToCart(authToken, itemId1, storename, 20);
            Assert.IsFalse(res.ErrorOccured);
            Task<Response> task = marketAPI.PurchaseMyCart(authToken, "address", "city", "country", "zip", "Bob Man", "WSIE", "WSEP", "123456789", "1", "2000", "???", "999", "1111111");
            task.Wait();
            res = task.Result;
            Assert.IsFalse(res.ErrorOccured);

            int rating = 5;
            String review = "Yummy";
            res = marketAPI.RateItem(authToken, itemId1, storename, rating, review);
            Assert.IsFalse(res.ErrorOccured);

            Response<StoreDTO> res2 = marketAPI.GetStoreInformation(authToken, storename);
            Assert.IsFalse(res2.ErrorOccured);
            Assert.IsTrue(res2.Value.Stock.Items[itemId1].Item1.Rating.Ratings.Count == 1);
            Assert.IsTrue(res2.Value.Stock.Items[itemId1].Item1.Rating.Ratings[username].Item1 == rating);
            Assert.IsTrue(res2.Value.Stock.Items[itemId1].Item1.Rating.Ratings[username].Item2 == review);
        }

        [TestMethod]
        public void PurchaseCartWithManyItemsAndRateItem_Success()
        {
            Response res = marketAPI.AddItemToCart(authToken, itemId1, storename, 20);
            Assert.IsFalse(res.ErrorOccured);
            res = marketAPI.AddItemToCart(authToken, itemId2, storename, 20);
            Assert.IsFalse(res.ErrorOccured);
            Task<Response> task = marketAPI.PurchaseMyCart(authToken, "address", "city", "country", "zip", "Bob Man", "WSIE", "WSEP", "123456789", "1", "2000", "???", "999", "1111111");
            task.Wait();
            res = task.Result;
            Assert.IsFalse(res.ErrorOccured);

            int rating = 5;
            String review = "Yummy";
            res = marketAPI.RateItem(authToken, itemId1, storename, rating, review);
            Assert.IsFalse(res.ErrorOccured);

            Response<StoreDTO> res2 = marketAPI.GetStoreInformation(authToken, storename);
            Assert.IsFalse(res2.ErrorOccured);
            Assert.IsTrue(res2.Value.Stock.Items[itemId1].Item1.Rating.Ratings.Count == 1);
            Assert.IsTrue(res2.Value.Stock.Items[itemId1].Item1.Rating.Ratings[username].Item1 == rating);
            Assert.IsTrue(res2.Value.Stock.Items[itemId1].Item1.Rating.Ratings[username].Item2 == review);

            rating = 7;
            review = "Not Yummy";
            res = marketAPI.RateItem(authToken, itemId2, storename, rating, review);
            Assert.IsFalse(res.ErrorOccured);

            res2 = marketAPI.GetStoreInformation(authToken, storename);
            Assert.IsFalse(res2.ErrorOccured);
            Assert.IsTrue(res2.Value.Stock.Items[itemId2].Item1.Rating.Ratings.Count == 1);
            Assert.IsTrue(res2.Value.Stock.Items[itemId2].Item1.Rating.Ratings[username].Item1 == rating);
            Assert.IsTrue(res2.Value.Stock.Items[itemId2].Item1.Rating.Ratings[username].Item2 == review);
        }

        [TestMethod]
        public void DontPurchaseCartAndRateItem_Failure()
        {
            Response res = marketAPI.AddItemToCart(authToken, itemId1, storename, 20);
            Assert.IsFalse(res.ErrorOccured);

            int rating = 5;
            String review = "Yummy";
            res = marketAPI.RateItem(authToken, itemId1, storename, rating, review);
            Assert.IsTrue(res.ErrorOccured);
        }

        [TestMethod]
        public void PurchaseCartWrongItemAndRateItem_Failure()
        {
            Response res = marketAPI.AddItemToCart(authToken, itemId2, storename, 20);
            Assert.IsFalse(res.ErrorOccured);

            int rating = 5;
            String review = "Yummy";
            res = marketAPI.RateItem(authToken, itemId1, storename, rating, review);
            Assert.IsTrue(res.ErrorOccured);
        }
    }
}
