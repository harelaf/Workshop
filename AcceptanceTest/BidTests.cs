using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketWeb.Service;
using MarketWeb.Shared.DTO;
using MarketWeb.Shared;

namespace AcceptanceTest
{
    [TestClass]
    public class BidTests
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        string storeName = "Bidding Shop";
        //string storeName_outSystem = "bla";
        string bidder_VisitorToken;
        string owner1_token;
        string owner1_username = "owner1";
        int itemId1;
        int amount1;
        int itemId2;
        int amount2;
        //int itemID_outStock = 1111111;

        [TestInitialize()]
        public void setup()
        {
            DateTime dob = new DateTime(2001, 7, 30);
            bidder_VisitorToken = (marketAPI.EnterSystem()).Value;
            owner1_token = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(owner1_token, owner1_username, "123456789", dob);
            owner1_token = (marketAPI.Login(owner1_token, owner1_username, "123456789")).Value;// reg
            marketAPI.OpenNewStore(owner1_token, storeName);
            itemId1 = 1; amount1 = 20;
            itemId2 = 2; amount2 = 50;
            marketAPI.AddItemToStoreStock(owner1_token, storeName, itemId1,
                "banana", 3.5, "", "fruit", amount1);
            marketAPI.AddItemToStoreStock(owner1_token, storeName, itemId2,
                "banana2", 3.5, "", "fruit", amount2);

        }
        [TestMethod]
        public void bidItem_bidAndCheck_success()
        {
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId, amount, biddedPrice);
            List<BidDTO> bids = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName).Value;
            Assert.IsTrue(bids != null && bids.Count > 0 && bids[0].ItemID == itemId && bids[0].Amount == amount);
            Assert.IsTrue(bids?[0].Acceptors.Count == 0);
        }
        [TestMethod]
        public void acceptBid_acceptBid_success()
        {
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId, amount, biddedPrice);
            marketAPI.AcceptBid(owner1_token, storeName, itemId, bidder_VisitorToken);
            Response<List<BidDTO>> res = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName);
            Assert.IsFalse(res.ErrorOccured, res.ErrorMessage);
            ISet<string> acceptors = res.Value[0].Acceptors;
            Assert.IsTrue(acceptors.Contains(owner1_username));
            List<string> lst = marketAPI.GetUsernamesWithInventoryPermissionInStore(owner1_token, storeName).Value;
            Assert.IsTrue(lst.Count == 1 && lst.Contains(owner1_username));
            Assert.IsTrue(res.Value[0].AcceptedByAll);
        }
        [TestMethod]
        public void addAcceptedBidToCart_counterOfferBidAndAddToCart_success()
        {
            int amount = 10;
            double biddedPrice = 1.5;
            double counterOffer = 1.9;
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            marketAPI.CounterOfferBid(owner1_token, storeName, itemId1, bidder_VisitorToken, counterOffer);
            Response res = marketAPI.AddAcceptedBidToCart(bidder_VisitorToken, itemId1, storeName, amount);
            Assert.IsFalse(res.ErrorOccured, res.ErrorMessage);
            ShoppingBasketDTO basket = marketAPI.ViewMyCart(bidder_VisitorToken).Value.GetBasket(storeName);
            Assert.IsTrue(basket.BiddedItems.Find(x => x.ItemID == itemId1 && x.Amount == amount && x.BiddedPrice == counterOffer) != null);
        }
        [TestMethod]
        public void addAcceptedBidToCart_rejectBidAndAddToCart_failure()
        {
            int amount = 10;
            double biddedPrice = 1.5;
            marketAPI.AddItemToCart(bidder_VisitorToken, itemId1, storeName, 5);
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            marketAPI.RejectBid(owner1_token, storeName, itemId1, bidder_VisitorToken);
            Response res = marketAPI.AddAcceptedBidToCart(bidder_VisitorToken, itemId1, storeName, amount);
            Assert.IsTrue(res.ErrorOccured);
            ShoppingBasketDTO basket = marketAPI.ViewMyCart(bidder_VisitorToken).Value.GetBasket(storeName);
            Assert.IsNull(basket.BiddedItems.Find(x => x.ItemID == itemId1 && x.Amount == amount && x.BiddedPrice == biddedPrice));
        }
        [TestMethod]
        public void acceptRejectAndCounterOfferBid_acceptBeforeBid_throwsException()
        {
            Response res1 = marketAPI.AcceptBid(owner1_token, storeName, itemId1, bidder_VisitorToken);
            Response res2 = marketAPI.RejectBid(owner1_token, storeName, itemId1, bidder_VisitorToken);
            Response res3 = marketAPI.CounterOfferBid(owner1_token, storeName, itemId1, bidder_VisitorToken, 1.7);
            Assert.IsTrue(res1.ErrorOccured);
            Assert.IsTrue(res2.ErrorOccured);
            Assert.IsTrue(res3.ErrorOccured);
        }
        [TestMethod]
        public void acceptAndReject_acceptRejectAndAccept_throwsException()
        {
            int amount = 10;
            double biddedPrice = 1.5;
            string owner2_username = "owner2";
            string owner2_token = marketAPI.EnterSystem().Value;
            marketAPI.Register(owner2_token, owner2_username, "123", new DateTime(2001, 7, 30));
            owner2_token = marketAPI.Login(owner2_token, owner2_username, "123").Value;
            marketAPI.AcceptOwnerAppointment(owner1_token, owner2_username, storeName);
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            marketAPI.AcceptBid(owner1_token, storeName, itemId1, bidder_VisitorToken);
            ISet<string> acceptors = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName).Value[0].Acceptors;
            Assert.IsTrue(acceptors.Contains(owner1_username));
            marketAPI.RejectBid(owner2_token, storeName, itemId1, bidder_VisitorToken);
            Response response = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName);
            Assert.IsTrue(response.ErrorOccured);
            Response response1 = marketAPI.AcceptBid(owner1_token, storeName, itemId1, bidder_VisitorToken);
            Assert.IsTrue(response1.ErrorOccured);
        }
        [TestMethod]
        public void counterOffer_counterOfferBidTwoUsernames_takesMaxOffer()
        {
            int amount = 10;
            double biddedPrice = 1.5;
            string owner2_username = "owner2";
            string owner2_token = marketAPI.EnterSystem().Value;
            marketAPI.Register(owner2_token, owner2_username, "123", new DateTime(2001, 7, 30));
            owner2_token = marketAPI.Login(owner2_token, owner2_username, "123").Value;
            marketAPI.AcceptOwnerAppointment(owner1_token, owner2_username, storeName); 
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            double counterOffer1 = 1.9;
            double counterOffer2 = 1.7;
            Response response1 = marketAPI.CounterOfferBid(owner2_token, storeName, itemId1, bidder_VisitorToken, counterOffer1);
            Response response2 = marketAPI.CounterOfferBid(owner1_token, storeName, itemId1, bidder_VisitorToken, counterOffer2);
            Assert.IsFalse(response1.ErrorOccured || response2.ErrorOccured);
            List<BidDTO> bids = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName).Value;
            Assert.IsTrue(bids[0].Acceptors.Contains(owner1_username));
            Assert.IsTrue(bids[0].Acceptors.Contains(owner2_username));
            Assert.AreEqual(bids[0].CounterOffer, counterOffer1);
            Assert.IsTrue(bids[0].AcceptedByAll);
        }
        [TestMethod]
        public void counterOffer_counterOfferBidLessThenBiddedPrice_success()
        {
            int amount = 10;
            double biddedPrice = 1.5;
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            double counterOffer1 = 0.5;
            Response response1 = marketAPI.CounterOfferBid(owner1_token, storeName, itemId1, bidder_VisitorToken, counterOffer1);
            Assert.IsFalse(response1.ErrorOccured);
            List<BidDTO> bids = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName).Value;
            Assert.IsTrue(bids[0].Acceptors.Contains(owner1_username));
            Assert.IsTrue(bids[0].CounterOffer == -1);
            Assert.IsTrue(bids[0].BiddedPrice == biddedPrice);
            Assert.IsTrue(bids[0].AcceptedByAll);
        }
        [TestMethod]
        public void rejectBid_bidThenReject_success()
        {
            int amount = 100;
            double biddedPrice = 1.5;
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            List<BidDTO> bids1 = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName).Value;
            Assert.IsTrue(bids1.Find(x => x.ItemID == itemId1 && x.Amount == amount && x.Bidder == bidder_VisitorToken) != null);
            marketAPI.RejectBid(owner1_token, storeName, itemId1, bidder_VisitorToken);
            Response<List<BidDTO>> response = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName);
            Assert.IsTrue(response.ErrorOccured);
        }
        [TestMethod]
        public void purchaseAcceptedBid_counterOfferAddToCartAndPurchase_success()
        {
            int amount = 10;
            double biddedPrice = 1.5;
            double counterOffer = 1.9;
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            marketAPI.CounterOfferBid(owner1_token, storeName, itemId1, bidder_VisitorToken, counterOffer);
            Response res = marketAPI.AddAcceptedBidToCart(bidder_VisitorToken, itemId1, storeName, amount);
            Assert.IsFalse(res.ErrorOccured, res.ErrorMessage);
            ShoppingBasketDTO basket = marketAPI.ViewMyCart(bidder_VisitorToken).Value.GetBasket(storeName);
            Assert.IsTrue(basket.BiddedItems.Find(x => x.ItemID == itemId1 && x.Amount == amount && x.BiddedPrice == counterOffer) != null);
            Task<Response> purchaseRes = marketAPI.PurchaseMyCart(bidder_VisitorToken, "1", "1", "1", "1", bidder_VisitorToken, "mock_true", "mock_true");
            purchaseRes.Wait();
            Assert.IsFalse (purchaseRes.Result.ErrorOccured, purchaseRes.Result.ErrorMessage);
        }
        [TestMethod]
        public void counterOffer_counterOfferBidConcurrently_takesMaxOffer()
        {
            int iterations = 50;
            int amount = 10;
            double biddedPrice = 1.5;
            string owner_username = "owner_";
            string[] owner_tokens = new string[iterations];
            for(int i = 0; i < iterations; i++)
            {
                owner_tokens[i] = marketAPI.EnterSystem().Value;
                marketAPI.Register(owner_tokens[i], owner_username + i, "123", new DateTime(2001, 7, 30));
                owner_tokens[i] = marketAPI.Login(owner_tokens[i], owner_username + i, "123").Value;
                marketAPI.AddStoreOwnerForTestPurposes(owner1_token, owner_username + i, storeName);
            }
            
            marketAPI.BidItemInStore(bidder_VisitorToken, storeName, itemId1, amount, biddedPrice);
            double counterOffer = 2;
            Thread thread1 = new Thread(() => {
                for (int i = 0; i < iterations / 2; i++)
                {
                    Response res1 = marketAPI.CounterOfferBid(owner_tokens[i], storeName, itemId1, bidder_VisitorToken, counterOffer++);
                    Assert.IsFalse(res1.ErrorOccured, res1.ErrorMessage);
                }
            });
            Thread thread2 = new Thread(() => {
                for (int i = iterations / 2; i < iterations; i++)
                {
                    Response res2 = marketAPI.CounterOfferBid(owner_tokens[i], storeName, itemId1, bidder_VisitorToken, counterOffer++);
                    Assert.IsFalse(res2.ErrorOccured, res2.ErrorMessage);
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            Assert.AreEqual(counterOffer, iterations + 2);
            List<BidDTO> bids = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName).Value;
            Assert.AreEqual(bids[0].Acceptors.Count, iterations);
            Assert.AreEqual(bids[0].CounterOffer, counterOffer - 1);
            Assert.IsFalse(bids[0].AcceptedByAll);
            Response res = marketAPI.AcceptBid(owner1_token, storeName, itemId1, bidder_VisitorToken);
            Assert.IsFalse(res.ErrorOccured, res.ErrorMessage);
            List<BidDTO> bids2 = marketAPI.GetVisitorBidsAtStore(bidder_VisitorToken, storeName).Value;
            Assert.AreEqual(bids2[0].Acceptors.Count, iterations + 1);
            Assert.AreEqual(bids2[0].CounterOffer, counterOffer - 1);
            Assert.IsTrue(bids2[0].AcceptedByAll);
        }
    }
}
