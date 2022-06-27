using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using System.Threading;

namespace AcceptanceTest
{
    // Acceptance tests for requirement: II.4.1
    [TestClass]
    public class TestRemoveItemFromStock
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        DateTime dob = new DateTime(2001, 7, 30);
        string storeName_inSystem = "Krusty Krab";
        string storeName_outSystem = "Chum Bucket";
        string username_founder = "SpongeBob SquarePants";
        string guest_token = "";
        string registered_token_founder = "";
        int itemId;
        string itemName = "";
        double itemPrice;
        string itemDescription = "";
        string itemCategory = "";
        int itemQuantity;

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

        [TestInitialize]
        public void setup()
        {
            guest_token = (marketAPI.EnterSystem()).Value;
            registered_token_founder = (marketAPI.EnterSystem()).Value;
            marketAPI.Register(registered_token_founder, username_founder, "123456789", dob);
            registered_token_founder = (marketAPI.Login(registered_token_founder, username_founder, "123456789")).Value;
            marketAPI.OpenNewStore(registered_token_founder, storeName_inSystem);
            itemName = "Krabby Patty";
            itemPrice = 5.0;
            itemDescription = "Yummy";
            itemCategory = "Hamburger";
            itemQuantity = 1000;
            itemId = marketAPI.AddItemToStoreStock(registered_token_founder, storeName_inSystem, itemName, itemPrice, itemDescription, itemCategory, itemQuantity).Value;
        }

        [TestMethod]
        public void sad_UserDoesntHavePermission()
        {
            Response response = marketAPI.RemoveItemFromStore(guest_token, storeName_inSystem, itemId);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_StoreDoesntExist()
        {
            Response response = marketAPI.RemoveItemFromStore(guest_token, storeName_outSystem, itemId);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_ItemIdDoesntExist()
        {
            int fakeItemId = 101;
            Response response = marketAPI.RemoveItemFromStore(registered_token_founder, storeName_inSystem, fakeItemId);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod] 
        public void concurrentScenario_UserIsAddingItemButItemIsBeingDeleted()
        {
            int iterations = itemQuantity;
            int i = 0;
            int thread2_stop = -1;
            bool flag = true;
            Thread thread1 = new Thread(() => {
                for (i = 0; i < iterations; i++)
                {
                    while (!flag) { }
                    marketAPI.AddItemToCart(guest_token, itemId, storeName_inSystem, 1);
                }
            });
            Thread thread2 = new Thread(() => {
                while (i < iterations / 2) { }
                flag = false;
                marketAPI.RemoveItemFromStore(registered_token_founder, storeName_inSystem, itemId);
                thread2_stop = i;
                flag = true;
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            Response<ShoppingCartDTO> r_1 = marketAPI.ViewMyCart(guest_token);
            if (r_1.ErrorOccured)
            {
                Assert.Fail(r_1.ErrorMessage);
            }
            ShoppingCartDTO user1Cart = r_1.Value;

            int totalAmountInCart = user1Cart.getAmountOfItemInCart(storeName_inSystem, itemId);

            Assert.AreEqual(thread2_stop, totalAmountInCart);
        }

        [TestMethod]
        public void happy_RemoveItemFromStockSuccess()
        {
            Response response = marketAPI.RemoveItemFromStore(registered_token_founder, storeName_inSystem, itemId);
            Assert.IsFalse(response.ErrorOccured);
        }
    }
}
