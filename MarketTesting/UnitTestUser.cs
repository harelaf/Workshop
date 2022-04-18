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
            guest.AddItemToCart(store, item, amount);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item), amount);
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
            guest.AddItemToCart(store, item1, amount1);
            guest.AddItemToCart(store, item2, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item1), amount1);
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item2), amount2);
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
            guest.AddItemToCart(store, item, amount1);
            guest.AddItemToCart(store, item, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store , item), expected_amount);
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
            guest.AddItemToCart(store, item, amount);
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
            guest.AddItemToCart(store, item1, amount1);
            //Action& Assert
            Assert.ThrowsException<Exception>(()=> guest.RemoveItemFromCart(item2, store));
        }

        [TestMethod]
        public void TestUpdateItemInCart1()
        {
            // there is basket of that store in cart and the basket contains that item.
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item = new Item(1);
            int amount = 8;
            guest.AddItemToCart(store, item, amount);
            int amount_toUpdate = 4;
            //Action
            guest.UpdateItemInCart(store, item, amount_toUpdate);
            //Assert
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item), amount_toUpdate);
        }
        [TestMethod]
        public void TestUpdateItemInCart2()
        {
            // there is basket of that store in cart and the basket contains that item. amount to update<0
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item item = new Item(1);
            int amount = 8;
            guest.AddItemToCart(store, item, amount);
            int amount_toUpdate_1 = 0;
            int amount_toUpdate_2 = -2;
            //Action & Assert
            Assert.ThrowsException<Exception>(() => guest.UpdateItemInCart(store, item, amount_toUpdate_1));
            Assert.ThrowsException<Exception>(() => guest.UpdateItemInCart(store, item, amount_toUpdate_2));
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item), amount);
        }
        [TestMethod]
        public void TestUpdateItemInCart3()
        {
            // there is basket of that store in cart but the basket doesn't contain that item.
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store = new Store("STORE1");
            Item itemInBasket = new Item(1);
            int amountInBasket = 8;
            guest.AddItemToCart(store, itemInBasket, amountInBasket);
            Item itemToUpdate = new Item(2);
            int amountToUpdate = 20;
            //Action &Aaaert
            Assert.ThrowsException<Exception>(()=> guest.UpdateItemInCart(store, itemToUpdate, amountToUpdate));
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, itemInBasket), amountInBasket);
        }
        [TestMethod]
        public void TestUpdateItemInCart4()
        {
            // there is no basket of that store in cart
            // Arrange
            Guest guest = new Guest("sys_id_user_1");
            Store store1 = new Store("STORE1");
            Item item1 = new Item(1);
            int amount1 = 8;
            guest.AddItemToCart(store1, item1, amount1);
            Store store2 = new Store("STORE2");
            Item itemToUpdate = new Item(2);
            int amountToUpdate = 20;
            //Action &Aaaert
            Assert.ThrowsException<Exception>(() => guest.UpdateItemInCart(store2, itemToUpdate, amountToUpdate));
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store1, item1), amount1);
        }
        
    }
}