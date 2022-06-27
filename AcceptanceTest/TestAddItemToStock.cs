using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;

namespace AcceptanceTest
{
    // Acceptance tests for requirement: II.4.1
    [TestClass]
    public class TestAddItemToStock
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
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
            marketAPI.Register(registered_token_founder, username_founder, "123456789", new DateTime(1992, 8, 4));
            registered_token_founder = (marketAPI.Login(registered_token_founder, username_founder, "123456789")).Value;
            marketAPI.OpenNewStore(registered_token_founder, storeName_inSystem);
            itemId = 100;
            itemName = "Krabby Patty";
            itemPrice = 5.0;
            itemDescription = "Yummy";
            itemCategory = "Hamburger";
            itemQuantity = 10;
        }

        [TestMethod]
        public void sad_UserDoesntHavePermission()
        {
            Response<int> response = marketAPI.AddItemToStoreStock(guest_token, storeName_inSystem, itemName, itemPrice, itemDescription, itemCategory, itemQuantity);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_StoreDoesntExist()
        {
            Response<int> response = marketAPI.AddItemToStoreStock(guest_token, storeName_outSystem, itemName, itemPrice, itemDescription, itemCategory, itemQuantity);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_PriceOutOfRange()
        {
            double priceOutOfRange = -100;
            Response<int> response = marketAPI.AddItemToStoreStock(guest_token, storeName_inSystem, itemName, priceOutOfRange, itemDescription, itemCategory, itemQuantity);
            Assert.IsTrue(response.ErrorOccured);
        }

        // This is a redudant test now, ids are assigned in the DAL.
        /*
        [TestMethod]
        public void sad_ItemIdAlreadyTaken()
        {
            Response<int> response = marketAPI.AddItemToStoreStock(registered_token_founder, storeName_inSystem, itemName, itemPrice, itemDescription, itemCategory, itemQuantity);
            Assert.IsFalse(response.ErrorOccured);

            Response<int> response1 = marketAPI.AddItemToStoreStock(registered_token_founder, storeName_inSystem, itemName, itemPrice, itemDescription, itemCategory, itemQuantity);
            Assert.IsTrue(response1.ErrorOccured);
        }
        */

        [TestMethod]
        public void happy_AddItemToStockSuccess()
        {
            Response<int> response = marketAPI.AddItemToStoreStock(registered_token_founder, storeName_inSystem, itemName, itemPrice, itemDescription, itemCategory, itemQuantity);
            Assert.IsFalse(response.ErrorOccured);
        }
    }
}
