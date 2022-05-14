using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketProject.Service;
using System.Threading;
using MarketProject.Service.DTO;

namespace AcceptanceTest
{
    [TestClass]
    public class TestCloseStore
    {
        MarketAPI marketAPI = new MarketAPI();
        string storeName_inSystem = "Krusty Krab";
        string storeName_outSystem = "Chum Bucket";
        string username_founder = "SpongeBob SquarePants";
        string guest_token;
        string registered_token_founder;


        [TestInitialize]
        public void setup()
        {
            guest_token = (marketAPI.EnterSystem()).Value;
            registered_token_founder = (marketAPI.EnterSystem()).Value;
            marketAPI.Register(registered_token_founder, username_founder, "123456789");
            registered_token_founder = (marketAPI.Login(registered_token_founder, username_founder, "123456789")).Value;
            marketAPI.OpenNewStore(registered_token_founder, storeName_inSystem);
            marketAPI.AddItemToStoreStock(registered_token_founder, storeName_inSystem, 1, "Krabby Patty", 5.0, "Yummy", "Food", 100);
        }

        [TestMethod]
        public void sad_UserDoesntHavePermission()
        {
            Response response = marketAPI.CloseStore(guest_token, storeName_inSystem);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void sad_StoreDoesntExist()
        {
            Response response = marketAPI.CloseStore(registered_token_founder, storeName_outSystem);
            Assert.IsTrue(response.ErrorOccured);
        }

        [TestMethod]
        public void happy_CloseStoreSuccess()
        {
            Response response = marketAPI.CloseStore(registered_token_founder, storeName_inSystem);
            Assert.IsFalse(response.ErrorOccured);

            Response<StoreDTO> response1 = marketAPI.GetStoreInformation(registered_token_founder, storeName_inSystem);
            Assert.IsFalse(response1.ErrorOccured);

            StoreDTO dto = response1.Value;
            Assert.AreEqual(dto.State, MarketProject.Domain.StoreState.Inactive);

            Response<RegisteredDTO> response2 = marketAPI.GetVisitorInformation(registered_token_founder);
            Assert.IsFalse(response2.ErrorOccured);
            Assert.AreEqual(response2.Value.NotificationsCount(), 1);

            Response response3 = marketAPI.AddItemToCart(registered_token_founder, 1, storeName_inSystem, 2);
            Assert.IsTrue(response3.ErrorOccured);
        }
    }
}
