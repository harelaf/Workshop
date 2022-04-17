using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;

namespace TestMarket
{

    [TestClass]
    public class UnitTestStore
    {
        [TestMethod]
        public void TestReserveItem1()
        {
            //item exists in stock and there is more than amount in stock
            // Arrange
            Store store = new Store("STORE1");
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID);
            store.Stock.AddItem(item, inStock);
            int amountToReserve = 10;
            //action
            store.ReserveItem(itemID, amountToReserve);
            int expectedAmountInStock = inStock - amountToReserve;
            // Assert
            Assert.AreEqual(store.Stock.GetItemAmount(item), expectedAmountInStock);
        }
        [TestMethod]
        public void TestReserveItem2()
        {
            //item exists in stock but there is not enought in stock
            // Arrange
            Store store = new Store("STORE1");
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID);
            store.Stock.AddItem(item, inStock);
            int amountToReserve = inStock+ 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
            Assert.AreEqual(store.Stock.GetItemAmount(item), inStock);
        }
        [TestMethod]
        public void TestReserveItem3()
        {
            //item does'nt exists in stock.
            // Arrange
            Store store = new Store("STORE1");
            int itemID = 1;
            Item item = new Item(itemID);
            int amountToReserve = 10;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
        }
        [TestMethod]
        public void TestReserveItem4()
        {
            //trying reserve amount<=0
            // Arrange
            Store store = new Store("STORE1");
            int itemID = 1;
            int inStock = 30;
            Item item = new Item(itemID);
            store.Stock.AddItem(item, inStock);
            int amountToReserve = 0;
            //action+ assert
            Assert.ThrowsException<Exception>(() => store.ReserveItem(itemID, amountToReserve));
        }
    }
}
