using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
namespace MarketProjectTests
{
    [TestClass]
    public class UnitTestUser
    {

        Guest guest;
        Store store;
        Item item1;

        [TestInitialize]
        public void setup()
        {
            guest = new Guest("sys_id_user_1");
            store = new Store("STORE1", "founder");
            item1 = new Item(1);
        }

        [TestMethod]
        public void TestAddItemToCart1()
        {
            //there is no basket with that store in cart:
            // Arrange
            int amount = 8;
            //action
            guest.addItemToCart(store, item1, amount);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item1), amount);
        }

        [TestMethod]
        public void TestAddItemToCart2()
        {
            //there is allready basket with that store in cart:
            // Arrange
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
            int amount1 = 8;
            int amount2 = 10;
            int expected_amount = amount1 + amount2;
            //action
            guest.addItemToCart(store, item1, amount1);
            guest.addItemToCart(store, item1, amount2);
            // Assert
            ShoppingBasket actual = guest.ShoppingCart.GetShoppingBasket(store);
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.GetAmountOfItem(item1), expected_amount);
        }
    }
}