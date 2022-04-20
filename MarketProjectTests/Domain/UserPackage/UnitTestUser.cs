using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;


namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class UnitTestUser
    {

        Guest guest;
        Store store;
        Item item1;

        [TestInitialize]
        public void setup()
        {
            guest = new Guest("sys_id_user_1");
            store = new Store("STORE1", new StoreFounder("founder", "STORE1"), new PurchasePolicy(), new DiscountPolicy());
            item1 = new Item(1, "name1", 1.0, "description1", "category1");

        }
        [TestMethod]
        public void TestAddItemToCart1()
        {
            //there is no basket with that store in cart:
            // Arrange
            int amount = 8;
            //action
            guest.AddItemToCart(store, item1, amount);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store.StoreName);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item1), amount);
        }

        [TestMethod]
        public void TestAddItemToCart2()
        {
            //there is allready basket with that store in cart:
            // Arrange
            Item item2 = new Item(2, "name2", 2.0, "description2", "category2");
            int amount1 = 8;
            int amount2 = 10;
            //action
            guest.AddItemToCart(store, item1, amount1);
            guest.AddItemToCart(store, item2, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store.StoreName);
            Assert.IsNotNull(actual);
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item1), amount1);
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item2), amount2);
        }

        [TestMethod]
        public void TestAddItemToCart3()
        {
            //there is allready basket with that store in cart:
            // Arrange
            int amount1 = 8;
            int amount2 = 10;
            int expected_amount = amount1 + amount2;
            //action
            guest.AddItemToCart(store, item1, amount1);
            guest.AddItemToCart(store, item1, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store.StoreName);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item1), expected_amount);
        }
        [TestMethod]
        public void TestRemoveItemToCart1()
        {
            //there is basket with that store and item in cart:
            // Arrange
            int amount = 8;
            guest.AddItemToCart(store, item1, amount);
            //action
            int removred_amount = guest.RemoveItemFromCart(item1, store);
            bool itemStillInBasket = guest.ShoppingCart.GetShoppingBasket(store.StoreName).isItemInBasket(item1);
            // Assert
            Assert.AreEqual(amount, removred_amount);
            Assert.AreEqual(itemStillInBasket, false);
        }

        [TestMethod]
        public void TestRemoveItemToCart2()
        {
            //there is no basket with that store in cart:
            // Arrange
            //action&Assert
            Assert.ThrowsException<Exception>(() =>
                   guest.RemoveItemFromCart(item1, store));
        }

        [TestMethod]
        public void TestRemoveItemToCart3()
        {
            //there is a basket with that store in cart, but there is no such item in it:
            // Arrange
            Item item2 = new Item(2, "name2", 1.0, "description", "category");
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
            int amount = 8;
            guest.AddItemToCart(store, item1, amount);
            int amount_toUpdate = 4;
            //Action
            guest.UpdateItemInCart(store, item1, amount_toUpdate);
            //Assert
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item1), amount_toUpdate);
        }
        [TestMethod]
        public void TestUpdateItemInCart2()
        {
            // there is basket of that store in cart and the basket contains that item. amount to update<0
            // Arrange
            int amount = 8;
            guest.AddItemToCart(store, item1, amount);
            int amount_toUpdate_1 = 0;
            int amount_toUpdate_2 = -2;
            //Action & Assert
            Assert.ThrowsException<Exception>(() => guest.UpdateItemInCart(store, item1, amount_toUpdate_1));
            Assert.ThrowsException<Exception>(() => guest.UpdateItemInCart(store, item1, amount_toUpdate_2));
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item1), amount);
        }
        [TestMethod]
        public void TestUpdateItemInCart3()
        {
            // there is basket of that store in cart but the basket doesn't contain that item.
            // Arrange
            Item itemInBasket = item1;
            int amountInBasket = 8;
            guest.AddItemToCart(store, itemInBasket, amountInBasket);
            Item itemToUpdate = new Item(2, "name2", 2.0, "description2", "category2");
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
            int amount1 = 8;
            guest.AddItemToCart(store, item1, amount1);
            Store store2 = new Store("STORE2", new StoreFounder("founder", "STORE2"), new PurchasePolicy(), new DiscountPolicy());
            Item itemToUpdate = new Item(2, "name2", 2.0, "description2", "category2");
            int amountToUpdate = 20;
            //Action &Aaaert
            Assert.ThrowsException<Exception>(() => guest.UpdateItemInCart(store2, itemToUpdate, amountToUpdate));
            Assert.AreEqual(guest.GetQuantityOfItemInCart(store, item1), amount1);
        }
        
    }
}