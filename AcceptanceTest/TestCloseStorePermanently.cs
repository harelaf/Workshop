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
    [TestClass]
    public class TestCloseStorePermanently
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        string storeName_inSystem = "Krusty Krab";
        string storeName_outSystem = "Chum Bucket";
        string username_founder = "SpongeBob SquarePants";
        string guest_token;
        string registered_token_founder;
        string admin_token;

        [TestInitialize]
        public void setup()
        {
            admin_token = (marketAPI.EnterSystem()).Value;
            admin_token = (marketAPI.Login(admin_token, "admin", "admin")).Value;
            guest_token = (marketAPI.EnterSystem()).Value;
            registered_token_founder = (marketAPI.EnterSystem()).Value;
            marketAPI.Register(registered_token_founder, username_founder, "123456789", new DateTime(2001, 7, 30));
            registered_token_founder = (marketAPI.Login(registered_token_founder, username_founder, "123456789")).Value;
            marketAPI.OpenNewStore(registered_token_founder, storeName_inSystem);
            marketAPI.AddItemToStoreStock(registered_token_founder, storeName_inSystem, "Krabby Patty", 5.0, "Yummy", "Food", 100);
        }

        [TestMethod]
        public void sad_StoreDoesntExist()
        {
            Response response = marketAPI.CloseStorePermanently(registered_token_founder, storeName_outSystem);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_UserDoesntHavePermission()
        {
            Response response = marketAPI.CloseStorePermanently(guest_token, storeName_inSystem);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void happy_CloseStorePermanentlySuccess()
        {
            /*
             * NEED TO FIGURE OUT HOW TO USE AN ADMIN TO CLOSE STORE PERMANENTLY.
             * THIS FUNCTION FAILS -- FOR NOW!!!
             */
            Response response = marketAPI.CloseStorePermanently(admin_token, storeName_inSystem);
            Assert.IsFalse(response.ErrorOccured);

            Response<StoreDTO> response1 = marketAPI.GetStoreInformation(registered_token_founder, storeName_inSystem);
            Assert.IsTrue(response1.ErrorOccured);

            Response<RegisteredDTO> response2 = marketAPI.GetVisitorInformation(registered_token_founder);
            Assert.IsFalse(response2.ErrorOccured);
            Assert.AreEqual(response2.Value.NotificationsCount(), 1);

            Response response3 = marketAPI.AddItemToCart(registered_token_founder, 1, storeName_inSystem, 2);
            Assert.IsTrue(response3.ErrorOccured);
        }
    }
}
