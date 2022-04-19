using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
namespace MarketProjectTests
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
            guest.AddItemToCart(store, item1, amount1);
            guest.AddItemToCart(store, item2, amount2);
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
            guest.AddItemToCart(store, item, amount1);
            guest.AddItemToCart(store, item, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item), expected_amount);
        }
    }
}