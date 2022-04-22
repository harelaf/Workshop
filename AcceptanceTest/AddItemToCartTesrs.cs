using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketProject.Service;
using MarketProject.Service.DTO;
using System.Collections.Generic;

namespace AcceptanceTest
{
    [TestClass]
    public class AddItemToCartTesrs
    {
        MarketAPI marketAPI = new MarketAPI();
        string storeName_inSystem = "afik's Shop";
        string storeName_outSystem = "bla";
        string guest_userToken;
        string registered_userToken;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemID_outStock = 1111111;

        [TestInitialize()]
        public void setup()
        {
            guest_userToken = (marketAPI.EnterSystem()).Value;
            registered_userToken = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(registered_userToken, "afik", "123456789");
            registered_userToken = (marketAPI.Login(registered_userToken, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(registered_userToken, storeName_inSystem);
            itemID_inStock_1 = 1; itemAmount_inSttock_1 = 20;
            itemID_inStock_2 = 2; itemAmount_inSttock_2 = 50;
            marketAPI.AddItemToStoreStock(registered_userToken, storeName_inSystem, itemID_inStock_1,
                "banana", 3.5, "", "fruit", itemAmount_inSttock_1);
            marketAPI.AddItemToStoreStock(registered_userToken, storeName_inSystem, itemID_inStock_2,
                "banana2", 3.5, "", "fruit", itemAmount_inSttock_2);

        }

        [TestMethod]
        public void TestAddItemToCart_2UsersAddingLastItemInStock()
        {
            int iterations = 10000;
            marketAPI.UpdateStockQuantityOfItem(registered_userToken, storeName_inSystem, itemID_inStock_1, iterations);
            Thread thread1 = new Thread(() => {
                for (int i = 0; i < iterations; i++)
                    marketAPI.AddItemToCart(registered_userToken, itemID_inStock_1, storeName_inSystem, 1);
            });
            Thread thread2 = new Thread(() => {
                for (int i = 0; i < iterations; i++)
                    marketAPI.AddItemToCart(guest_userToken, itemID_inStock_1, storeName_inSystem, 1);
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            int totalAmountInCarts = 0;
            Response<ShoppingCartDTO> r_1 = marketAPI.ViewMyCart(guest_userToken);
            Response<ShoppingCartDTO> r_2 = marketAPI.ViewMyCart(registered_userToken);
            if (r_1.ErrorOccured)
            {
                Assert.Fail(r_1.ErrorMessage);
            }
            if (r_2.ErrorOccured)
            {
                Assert.Fail(r_2.ErrorMessage);
            }
            ShoppingCartDTO user1Cart = r_1.Value;
            ShoppingCartDTO user2Cart = r_2.Value;

            totalAmountInCarts = user1Cart.getAmountOfItemInCart(storeName_inSystem, 1) +
                user2Cart.getAmountOfItemInCart(storeName_inSystem, 1);

            Assert.AreEqual(iterations, totalAmountInCarts);
            StoreDTO store = marketAPI.GetStoreInformation(registered_userToken, storeName_inSystem).Value;
            Assert.IsNotNull(store);
            IDictionary<ItemDTO, int> stock = store.Stock.Items;
            foreach (ItemDTO item in stock.Keys)
            {
                if (item.ItemID == itemID_inStock_1)
                {
                    Assert.AreEqual(stock[item], 0);
                    break;
                }
            }

        }

        [TestMethod]
        public void TestAddItemToCart_storeNotExists()
        {
            Response response_reg = marketAPI.AddItemToCart(registered_userToken, itemID_inStock_1, storeName_outSystem, 10);
            Response response_guest = marketAPI.AddItemToCart(guest_userToken, itemID_inStock_2, storeName_outSystem, 10);
            if (!response_guest.ErrorOccured || !response_reg.ErrorOccured)
                Assert.Fail("should've faild since there is no such store");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_userToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_userToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("no item should have been added to cart : since there is no such store");
        }

        [TestMethod]
        public void TestAddItemToCart_itemNotExists()
        {
            Response response_reg = marketAPI.AddItemToCart(registered_userToken, itemID_outStock, storeName_inSystem, 10);
            Response response_guest = marketAPI.AddItemToCart(guest_userToken, itemID_outStock, storeName_inSystem, 10);
            if (!response_guest.ErrorOccured || !response_reg.ErrorOccured)
                Assert.Fail("should've faild since there is no such item");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_userToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_userToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("no item should have been added to cart : since there is no such item");
        }

        [TestMethod]
        public void TestAddItemToCart_itemAmountNotInStock()
        {
            Response response_reg = marketAPI.AddItemToCart(registered_userToken, itemID_inStock_1, storeName_inSystem, itemAmount_inSttock_1 + 10);
            Response response_guest = marketAPI.AddItemToCart(guest_userToken, itemID_inStock_2, storeName_inSystem, itemAmount_inSttock_2 + 10);
            if (!response_guest.ErrorOccured || !response_reg.ErrorOccured)
                Assert.Fail("should've faild since there is not enought items in stock");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_userToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_userToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("no item should have been added to cart : since there is not enought items in stock");
        }

        [TestMethod]
        public void TestAddItemToCart_addingSuccses()
        {
            Response response_reg = marketAPI.AddItemToCart(registered_userToken, itemID_inStock_1, storeName_inSystem, itemAmount_inSttock_1);
            Response response_guest = marketAPI.AddItemToCart(guest_userToken, itemID_inStock_2, storeName_inSystem, itemAmount_inSttock_2);
            if (response_guest.ErrorOccured || response_reg.ErrorOccured)
                Assert.Fail("reg_err: " + response_reg.ErrorMessage + " ;  guest_errL " + response_guest.ErrorMessage);
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_userToken).Value;
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_userToken).Value;
            if (cart_guest.Baskets.Count < 0 || cart_registered.Baskets.Count < 0)
                Assert.Fail(" item should have been added to cart ");
            Assert.AreEqual(itemAmount_inSttock_1, cart_registered.getAmountOfItemInCart(storeName_inSystem, itemID_inStock_1));
            Assert.AreEqual(itemAmount_inSttock_2, cart_guest.getAmountOfItemInCart(storeName_inSystem, itemID_inStock_2));
        }
        
    }
}
