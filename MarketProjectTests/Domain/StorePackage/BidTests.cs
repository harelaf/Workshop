using MarketWeb.Server.DataLayer;
using MarketWeb.Server.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketProjectTests.Domain.Tests
{
    [TestClass]
    public class BidTests
    {
        private Bid bid;
        private string bidder;
        private int itemId;
        private int amount;
        private double biddedPrice;

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }
        
        [TestInitialize]
        public void setup()
        {
            bidder = "bidder";
            itemId = 1;
            amount = 100;
            biddedPrice = 1.5;
            bid = new Bid(bidder, itemId, amount, biddedPrice);
        }
        [TestMethod]
        public void acceptBid_acceptBidTwoUsernames_success()
        {
            string acceptor1 = "acceptor1";
            string acceptor2 = "acceptor2";
            bid.AcceptBid(acceptor1);
            bid.AcceptBid(acceptor2);
            Assert.IsTrue(bid.Acceptors.Contains(acceptor1));
            Assert.IsTrue(bid.Acceptors.Contains(acceptor2));
        }
        [TestMethod]
        public void counterOffer_counterOfferBidTwoUsernames_success()
        {
            string acceptor1 = "acceptor1";
            string acceptor2 = "acceptor2";
            double counterOffer1 = 1.7;
            double counterOffer2 = 1.9;
            bid.CounterOfferBid(acceptor1, counterOffer1);
            bid.CounterOfferBid(acceptor2, counterOffer2);
            Assert.IsTrue(bid.Acceptors.Contains(acceptor1));
            Assert.IsTrue(bid.Acceptors.Contains(acceptor2));
            Assert.IsTrue(bid.CounterOffer == counterOffer2);
        }
        [TestMethod]
        public void counterOffer_counterOfferBidTwoUsernames_takesMaxOffer()
        {
            string acceptor1 = "acceptor1";
            string acceptor2 = "acceptor2";
            double counterOffer1 = 1.9;
            double counterOffer2 = 1.7;
            bid.CounterOfferBid(acceptor1, counterOffer1);
            bid.CounterOfferBid(acceptor2, counterOffer2);
            Assert.IsTrue(bid.Acceptors.Contains(acceptor1));
            Assert.IsTrue(bid.Acceptors.Contains(acceptor2));
            Assert.IsTrue(bid.CounterOffer == counterOffer1);
        }
        [TestMethod]
        public void counterOffer_counterOfferBidLessThenBiddedPrice_success()
        {
            string acceptor1 = "acceptor1";
            double counterOffer1 = 0.5;
            bid.CounterOfferBid(acceptor1, counterOffer1);
            Assert.IsTrue(bid.Acceptors.Contains(acceptor1));
            Assert.IsTrue(bid.CounterOffer == -1);
        }
    }
}
