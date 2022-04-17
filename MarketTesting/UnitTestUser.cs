using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;

namespace MarketTesting
{
    [TestClass]
    public class UnitTestUser
    {
        [TestMethod]
        public void TestAddItemToCart1()
        {
            //there is no basket with that store in cart:
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item = new Item(1);
            int amount = 8;
            //action
            guest.addItemToCart(store, item, amount);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item), amount);
        }

        [TestMethod]
        public void TestAddItemToCart2()
        {
            //there is allready basket with that store in cart:
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item1 = new Item(1);
            Item item2 = new Item(2);
            int amount1 = 8;
            int amount2 = 10;
            //action
            guest.addItemToCart(store, item1, amount1);
            guest.addItemToCart(store, item2, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item1), amount1);
            Assert.AreEqual(actual.GetAmountOfItem(item2), amount2);
        }

        [TestMethod]
        public void TestAddItemToCart3()
        {
            //there is allready basket with that store in cart:
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item = new Item(1);
            int amount1 = 8;
            int amount2 = 10;
            int expected_amount = amount1 + amount2;
            //action
            guest.addItemToCart(store, item, amount1);
            guest.addItemToCart(store, item, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item), expected_amount);
        }
        [TestMethod]
        public void TestRemoveItemToCart1()
        {
            //there is basket with that store and item in cart:
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item = new Item(1);
            int amount = 8;
            guest.addItemToCart(store, item, amount);
            //action
            int removred_amount = guest.RemoveItemFromCart(item, store);
            bool itemStillInBasket = guest.ShoppingCart.GetShoppingBasket(store).isItemInBasket(item);
            // Assert
            Assert.AreEqual(amount, removred_amount);
            Assert.AreEqual(itemStillInBasket, false);
        }

        [TestMethod]
        public void TestRemoveItemToCart2()
        {
            //there is no basket with that store in cart:
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item = new Item(1);
            //action&Assert
            Assert.ThrowsException<Exception>(() =>
                   guest.RemoveItemFromCart(item, store));
        }

        [TestMethod]
        public void TestRemoveItemToCart3()
        {
            //there is a basket with that store in cart, but there is no such item in it:
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item1 = new Item(1);
            Item item2 = new Item(2);
            int amount1 = 8;
            guest.addItemToCart(store, item1, amount1);
            //Action& Assert
            Assert.ThrowsException<Exception>(()=> guest.RemoveItemFromCart(item2, store));
        }
    }
}