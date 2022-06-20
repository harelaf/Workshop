using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain;
using MarketWeb.Server.Domain.PolicyPackage;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class StoreTests
    {

        Store _store;
        string storeName;
        string storeFounder;
        int itemId;
        String name;
        String description;
        String category;
        double price;
        int quantity;

        [TestInitialize()]
        public void setup()
        {
            storeFounder = "Mr. Krabs";
            storeName = "Krusty Krab";
            _store = new Store(storeName, new StoreFounder(storeFounder, "Krusty Krab"), new PurchasePolicy(), new DiscountPolicy());
            itemId = 1;
            name = "Krabby Patty";
            description = "yummy";
            category = "computing";
            price = 5.0;
            quantity = 10;
        }


        [TestMethod()]
        public void RateStore_UserHasntRatedStore_NoException()
        {
            String username = "Squidward Tentacles";
            int rating = 1;
            String review = "NOOOOOOOOOOOO";

            try
            {
                _store.RateStore(username, rating, review);
                Assert.AreEqual(_store.GetRating(), "" + rating);
            }
            catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void RateStore_UserHasRatedStore_ThrowsException()
        {
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
            String description = "Delicious";
            int quantity = 5;
            double price = 5.0;
            String category = "";
            int newQuantity = 10;
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
            int newQuantity = 15;

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
            try
            {
                _store.RemoveItemFromStore(itemId);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [TestMethod()]
        public void RemoveItemFromStore_ItemExists_NoException()
        {
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

        [TestMethod()]
        public void AddStoreManager_AddManagerTwice_returnsFalse()
        {
            bool arrange = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));

            Assert.ThrowsException<Exception>(() => _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder)));
        }

        [TestMethod]
        public void AddStoreManager_AddManagerWhileIsOwner_returnsFalse()
        {
            StoreOwner arrange = _store.AcceptOwnerAppointment(storeFounder, name);//success. first owner's appointment.

            Assert.ThrowsException<Exception>(() => _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder)));
        }

        [TestMethod]
        public void AddStoreManager_AddManager_returnsTrue()
        {
            bool act = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));
            Assert.IsTrue(act);
        }

        [TestMethod]
        public void AddStoreOwner_AddOwnerTwice_returnsFalse()
        {
            StoreOwner arrange = _store.AcceptOwnerAppointment(storeFounder, name);

            Assert.ThrowsException<Exception>(() => _store.AcceptOwnerAppointment(storeFounder, name));
        }

        [TestMethod]
        public void AddStoreOwner_AddOwnerWhileIsManager_returnsFalse()
        {
            bool arrange = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));

            Assert.ThrowsException<Exception>(() => _store.AcceptOwnerAppointment(storeFounder, name));
        }

        [TestMethod]
        public void AddStoreOwner_AddOwner_returnsTrue()
        {
            StoreOwner act = _store.AcceptOwnerAppointment(storeFounder, name);
            Assert.IsTrue(act != null);
        }

        [TestMethod]
        public void TestReserveItem_moreThanAmountInStock()
        {
            //item exists in stock and there is more than amount in stock
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "item1", 20, "banana", category);
            store.Stock.AddItem(item, inStock);
            int amountToReserve = 10;
            //action
            store.ReserveItem(itemID, amountToReserve);
            int expectedAmountInStock = inStock - amountToReserve;
            // Assert
            Assert.AreEqual(store.Stock.GetItemAmount(item), expectedAmountInStock);
        }
        [TestMethod]
        public void TestReserveItem_notEnoughtInStock()
        {
            //item exists in stock but there is not enought in stock
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToReserve = inStock + 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
            Assert.AreEqual(store.Stock.GetItemAmount(item), inStock);
        }
        [TestMethod]
        public void TestReserveItem_NoSuchItemInStock()
        {
            //item does'nt exists in stock.
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            int amountToReserve = 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
        }
        [TestMethod]
        public void TestReserveItem_nonPosigtiveAmountToReserve()
        {
            //trying reserve amount<=0
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToReserve = 0;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
        }
        [TestMethod]
        public void TestUnreserveItem_positiveAmount()
        {
            //item exists in stock and the given amount>0
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 0;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToUneserve = 10;
            //action
            store.UnReserveItem(item, amountToUneserve);
            int expectedAmountInStock = inStock + amountToUneserve;
            // Assert
            Assert.AreEqual(store.Stock.GetItemAmount(item), expectedAmountInStock);
        }
        [TestMethod]
        public void TestUnreserveItem_NoSuchItemInStock()
        {
            //item does'nt exists in stock.
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            int amountToUnreserve = 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.UnReserveItem(item, amountToUnreserve));
        }
        [TestMethod]
        public void TestUnreserveItem_nonPositiveAmount()
        {
            //trying unureserve amount_to_add<=0
            // Arrange
            Store store = new Store("Store1", null, null, null);
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID, "itwm1", 20, "banana", "category");
            store.Stock.AddItem(item, inStock);
            int amountToUnreserve = 0;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.UnReserveItem(item, amountToUnreserve));
        }

        [TestMethod()]
        public void AddStoreOwner_removeAsManagerAddAsOwner_returnsTrue()
        {
            bool arrange = _store.AddStoreManager(new StoreManager(name, _store.StoreName, storeFounder));
            arrange = arrange & _store.RemoveStoreManager(name, storeFounder);

            StoreOwner act = _store.AcceptOwnerAppointment(storeFounder, name);
            bool isAdded = _store.GetOwners().Contains(act);
            Assert.IsTrue(arrange & act != null && isAdded);
        }

        [TestMethod()]
        public void RemoveStoreManager_addAndRemove_returnstrue()
        {
            string storeManager = "tommy shelby";
            bool add = _store.AddStoreManager(new StoreManager(storeManager, storeName, storeFounder));

            bool actual = _store.RemoveStoreManager(storeManager, storeFounder);
            Assert.IsTrue(add);
            Assert.IsTrue(actual);
        }
        [TestMethod()]
        public void RemoveStoreManager_removeFounder_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _store.RemoveStoreManager(storeFounder, null));
        }

        [TestMethod()]
        public void RemoveStoreManager_NonWorker_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _store.RemoveStoreManager("123", null));
        }

        [TestMethod()]
        public void RemoveStoreOwner_addAndRemove_returnstrue()
        {
            string storeOwner = "amos";
            StoreOwner add = _store.AcceptOwnerAppointment(storeFounder, storeOwner);
            bool isAdded = _store.GetOwners().Contains(add);
            try
            {
                _store.RemoveStoreOwner(storeOwner, storeFounder);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.IsTrue(add != null && isAdded);
        }
        [TestMethod()]
        public void RemoveStoreOwner_removeFounder_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _store.RemoveStoreOwner(storeFounder, storeFounder));
        }

        [TestMethod()]
        public void RemoveStoreOwner_NonWorker_returnsfalse()
        {
            Assert.ThrowsException<Exception>(() => _store.RemoveStoreOwner("123", null));
        }
        [TestMethod()]
        public void acceptStoreOwner_add2ownersCheckstandby_returnstrue()
        {
            string storeOwner = "amos";
            string storeOwner2 = "haim";
            StoreOwner add1 = _store.AcceptOwnerAppointment(storeFounder, storeOwner);//success. first owner's appointment.
            StoreOwner add2 = _store.AcceptOwnerAppointment(storeFounder, storeOwner2);// awaiting ownerUsername1 to approve appointment
            bool isAdded = _store.GetOwners().Contains(add1);
            bool isNotAdded = !_store.GetOwners().Contains(add2);
            bool isAdded2 = _store.GetStandbyOwnersInStore().ContainsKey(storeOwner2) && _store.GetStandbyOwnersInStore()[storeOwner2].Contains(storeFounder);
            Assert.IsTrue(add1 != null && isAdded && isNotAdded && isAdded2);
        }
        [TestMethod()]
        public void acceptStoreOwner_add2ownersAcceptSecond_returnstrue()
        {
            string storeOwner = "amos";
            string storeOwner2 = "haim";
            StoreOwner add1 = _store.AcceptOwnerAppointment(storeFounder, storeOwner);//success. first owner's appointment.
            StoreOwner add2 = _store.AcceptOwnerAppointment(storeFounder, storeOwner2);// awaiting ownerUsername1 to approve appointment
            bool isAdded = _store.GetOwners().Contains(add1);
            bool isNotAdded = !_store.GetOwners().Contains(add2);
            bool isAdded2 = _store.GetStandbyOwnersInStore().ContainsKey(storeOwner2) && _store.GetStandbyOwnersInStore()[storeOwner2].Contains(storeFounder);
            StoreOwner add3 = _store.AcceptOwnerAppointment(storeOwner, storeOwner2);
            bool isRemovedFromStandby = !_store.GetStandbyOwnersInStore().ContainsKey(storeOwner2);
            bool isAdded3 = _store.GetOwners().Contains(add3);
            Assert.IsTrue(add1 != null && isAdded && isNotAdded && isAdded2 && isRemovedFromStandby && isAdded3);
        }
        [TestMethod()]
        public void rejectStoreOwner_add2ownersRejectSecond_returnstrue()
        {
            string storeOwner = "amos";
            string storeOwner2 = "haim";
            StoreOwner add1 = _store.AcceptOwnerAppointment(storeFounder, storeOwner);//success. first owner's appointment.
            StoreOwner add2 = _store.AcceptOwnerAppointment(storeFounder, storeOwner2);// awaiting ownerUsername1 to approve appointment
            bool isAdded = _store.GetOwners().Contains(add1);
            bool isNotAdded = !_store.GetOwners().Contains(add2);
            bool isAdded2 = _store.GetStandbyOwnersInStore().ContainsKey(storeOwner2) && _store.GetStandbyOwnersInStore()[storeOwner2].Contains(storeFounder);
            _store.RejectOwnerAppointment(storeOwner, storeOwner2);
            bool isRemoved = !_store.GetStandbyOwnersInStore().ContainsKey(storeOwner2);
            bool isNotAdded2 = _store.GetOwners().Find(x => x.Username == storeOwner2) == null;
            Assert.IsTrue(add1 != null && isAdded && isNotAdded && isAdded2 && isRemoved && isNotAdded2);
        }

        [TestMethod]
        public void bidItem_bidAndCheck_success()
        {
            string bidder = "bidder";
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            _store.BidItem(itemId, amount, biddedPrice, bidder);
            List<Bid> bids = _store.GetVisitorBids(bidder);
            Assert.IsTrue(bids != null && bids.Count > 0 && bids[0].ItemID == itemId && bids[0].Amount == amount);
            Assert.IsTrue(bids[0].Acceptors.Count == 0);
        }
        [TestMethod]
        public void acceptBid_acceptBidTwoUsernames_success()
        {
            string bidder = "bidder";
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            string acceptor1 = "acceptor1";
            string acceptor2 = "acceptor2";
            _store.BidItem(itemId, amount, biddedPrice, bidder);
            _store.AcceptBid(acceptor1, itemId, bidder);
            _store.AcceptBid(acceptor2, itemId, bidder);
            ISet<string> acceptors = _store.GetVisitorBids(bidder)[0].Acceptors;
            Assert.IsTrue(acceptors.Contains(acceptor1));
            Assert.IsTrue(acceptors.Contains(acceptor2));
        }
        [TestMethod]
        public void acceptRejectAndCounterOfferBid_acceptBeforeBid_throwsException()
        {
            string bidder = "bidder";
            int itemId = 1;
            string acceptor1 = "acceptor1";
            Assert.ThrowsException<Exception>(() => _store.AcceptBid(acceptor1, itemId, bidder));
            Assert.ThrowsException<Exception>(() => _store.RejectBid(acceptor1, itemId, bidder));
            Assert.ThrowsException<Exception>(() => _store.CounterOfferBid(acceptor1, itemId, bidder, 1.7));
        }
        [TestMethod]
        public void acceptAndReject_acceptRejectAndAccept_throwsException()
        {
            string bidder = "bidder";
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            string acceptor1 = "acceptor1";
            string acceptor2 = "acceptor2";
            _store.BidItem(itemId, amount, biddedPrice, bidder);
            _store.AcceptBid(acceptor1, itemId, bidder);
            ISet<string> acceptors = _store.GetVisitorBids(bidder)[0].Acceptors;
            Assert.IsTrue(acceptors.Contains(acceptor1)); 
            _store.RejectBid(acceptor2, itemId, bidder);
            Assert.IsTrue(_store.GetVisitorBids(bidder).Count == 0);
            Assert.ThrowsException<Exception>(() => _store.AcceptBid("new acceptor", itemId, bidder));
        }
        [TestMethod]
        public void counterOffer_counterOfferBidTwoUsernames_success()
        {
            string bidder = "bidder";
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            _store.BidItem(itemId, amount, biddedPrice, bidder);
            string acceptor1 = "acceptor1";
            string acceptor2 = "acceptor2";
            double counterOffer1 = 1.7;
            double counterOffer2 = 1.9;
            _store.CounterOfferBid(acceptor1, itemId, bidder, counterOffer1);
            _store.CounterOfferBid(acceptor2, itemId, bidder, counterOffer2);
            List<Bid> bids = _store.GetVisitorBids(bidder);
            Assert.IsTrue(bids[0].Acceptors.Contains(acceptor1));
            Assert.IsTrue(bids[0].Acceptors.Contains(acceptor2));
            Assert.IsTrue(bids[0].CounterOffer == counterOffer2);
        }
        [TestMethod]
        public void counterOffer_counterOfferBidTwoUsernames_takesMaxOffer()
        {
            string bidder = "bidder";
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            _store.BidItem(itemId, amount, biddedPrice, bidder);
            string acceptor1 = "acceptor1";
            string acceptor2 = "acceptor2";
            double counterOffer1 = 1.9;
            double counterOffer2 = 1.7;
            _store.CounterOfferBid(acceptor1, itemId, bidder, counterOffer1);
            _store.CounterOfferBid(acceptor2, itemId, bidder, counterOffer2);
            List<Bid> bids = _store.GetVisitorBids(bidder);
            Assert.IsTrue(bids[0].Acceptors.Contains(acceptor1));
            Assert.IsTrue(bids[0].Acceptors.Contains(acceptor2));
            Assert.IsTrue(bids[0].CounterOffer == counterOffer1);
        }
        [TestMethod]
        public void counterOffer_counterOfferBidLessThenBiddedPrice_success()
        {
            string bidder = "bidder";
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            _store.BidItem(itemId, amount, biddedPrice, bidder);
            string acceptor1 = "acceptor1";
            double counterOffer1 = 0.5;
            _store.CounterOfferBid(acceptor1, itemId, bidder, counterOffer1);
            List<Bid> bids = _store.GetVisitorBids(bidder);
            Assert.IsTrue(bids[0].Acceptors.Contains(acceptor1));
            Assert.IsTrue(bids[0].CounterOffer == -1);
        }
        [TestMethod]
        public void rejectBid_bidThenReject_success()
        {
            string bidder = "bidder";
            int itemId = 1;
            int amount = 100;
            double biddedPrice = 1.5;
            _store.BidItem(itemId, amount, biddedPrice, bidder);
            string acceptor1 = "acceptor1";
            double counterOffer1 = 0.5;
            _store.CounterOfferBid(acceptor1, itemId, bidder, counterOffer1);
            List<Bid> bids = _store.GetVisitorBids(bidder);
            Assert.IsTrue(bids[0].Acceptors.Contains(acceptor1));
            Assert.IsTrue(bids[0].CounterOffer == -1);
        }
    }
}
