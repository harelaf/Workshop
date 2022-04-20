//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using MarketProject.Domain;
//using System;

//namespace MarketTesting
//{

//    [TestClass]
//    public class UnitTestStore
//    {

//        Store store;
//        int itemID;

//        [TestInitialize]
//        public void setup()
//        {
//            store = new Store("STORE1", "founder");
//            itemID = 1;

//        }
//        [TestMethod]
//        public void TestReserveItem1()
//        {
//            //item exists in stock and there is more than amount in stock
//            // Arrange
//            int inStock = 30;
//            Item item = new Item(itemID);
//            store.Stock.AddItem(item, inStock);
//            int amountToReserve = 10;
//            //action
//            store.ReserveItem(itemID, amountToReserve);
//            int expectedAmountInStock = inStock - amountToReserve;
//            // Assert
//            Assert.AreEqual(store.Stock.GetItemAmount(item), expectedAmountInStock);
//        }
//        [TestMethod]
//        public void TestReserveItem2()
//        {
//            //item exists in stock but there is not enought in stock
//            // Arrange
//            int inStock = 30;
//            Item item = new Item(itemID);
//            store.Stock.AddItem(item, inStock);
//            int amountToReserve = inStock + 10;
//            //action+ assert
//            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
//            Assert.AreEqual(store.Stock.GetItemAmount(item), inStock);
//        }
//        [TestMethod]
//        public void TestReserveItem3()
//        {
//            //item does'nt exists in stock.
//            // Arrange
//            Item item = new Item(itemID);
//            int amountToReserve = 10;
//            //action+ assert
//            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
//        }
//        [TestMethod]
//        public void TestReserveItem4()
//        {
//            //trying reserve amount<=0
//            // Arrange
//            int inStock = 30;
//            Item item = new Item(itemID);
//            store.Stock.AddItem(item, inStock);
//            int amountToReserve = 0;
//            //action+ assert
//            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
//        }
//        [TestMethod]
//        public void TestUnreserveItem1()
//        {
//            //item exists in stock and the given amount>0
//            // Arrange
//            int inStock = 0;
//            Item item = new Item(itemID);
//            store.Stock.AddItem(item, inStock);
//            int amountToUneserve = 10;
//            //action
//            store.UnReserveItem(item, amountToUneserve);
//            int expectedAmountInStock = inStock + amountToUneserve;
//            // Assert
//            Assert.AreEqual(store.Stock.GetItemAmount(item), expectedAmountInStock);
//        }
//        [TestMethod]
//        public void TestUnreserveItem2()
//        {
//            //item does'nt exists in stock.
//            // Arrange
//            Item item = new Item(itemID);
//            int amountToUnreserve = 10;
//            //action+ assert
//            Assert.ThrowsException<Exception>(() => store.UnReserveItem(item, amountToUnreserve));
//        }
//        [TestMethod]
//        public void TestUnreserveItem3()
//        {
//            //trying unureserve amount_to_add<=0
//            // Arrange
//            int inStock = 30;
//            Item item = new Item(itemID);
//            store.Stock.AddItem(item, inStock);
//            int amountToUnreserve = 0;
//            //action+ assert
//            Assert.ThrowsException<Exception>(() => store.UnReserveItem(item,amountToUnreserve ));
//        }
//    }
//}
