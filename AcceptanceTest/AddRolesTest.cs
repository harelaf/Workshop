using MarketProject.Service;
using MarketProject.Service.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcceptanceTest
{
    [TestClass]
    public class AddRolesTest
    {
        MarketAPI marketAPI = new MarketAPI();
        string storeName = "test's Shop";
        //string storeName_outSystem = "bla";
        string guest_VisitorToken;
        string store_founder_token;
        string store_founder_name;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemID_outStock = 1111111;

        [TestInitialize()]
        public void setup()
        {
            guest_VisitorToken = (marketAPI.EnterSystem()).Value;
            store_founder_token = (marketAPI.EnterSystem()).Value;// guest
            store_founder_name = "afik";
            marketAPI.Register(store_founder_token, store_founder_name, "123456789");
            store_founder_token = (marketAPI.Login(store_founder_token, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(store_founder_token, storeName);
        }

        [TestMethod]
        public void TestAddManager_2VisitorsAddingDiffRolesSamePerson_oneIsFailed()
        {
            string managerUsername = "new manager";
            string ownerUsername = "new owner";
            string store_owner_token = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token, ownerUsername, "123456789"); 
            marketAPI.AddStoreOwner(store_founder_token, ownerUsername, storeName);
            Boolean res1 = false, res2 = false;
            Thread thread1 = new Thread(() => {
                res1 = marketAPI.AddStoreManager(store_founder_token, managerUsername, storeName).ErrorOccured;
            });
            Thread thread2 = new Thread(() => {
                res2 = marketAPI.AddStoreManager(store_founder_token, managerUsername, storeName).ErrorOccured;
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            Assert.IsTrue(res1 || res2);

        }

        [TestMethod]
        public void TestAddManager_circularAppointing_unsuccessful()
        {
            string ownerUsername1 = "new owner1";
            string ownerUsername2 = "new owner2";
            string managerUsername3 = "new manager3";
            string password = "123456789";
            bool doubleCheck = false;
            string store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password);
            string store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password);
            string store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;
            marketAPI.AddStoreOwner(store_founder_token, ownerUsername1, storeName);
            marketAPI.AddStoreOwner(store_owner_token1, ownerUsername2, storeName);

            //act
            Response response1 = marketAPI.AddStoreManager(store_owner_token2, store_founder_name, storeName);
            Response response2 = marketAPI.AddStoreManager(store_owner_token2, ownerUsername1, storeName);
            Response response3 = marketAPI.AddStoreManager(store_owner_token2, managerUsername3, storeName);
            List<StoreManagerDTO> lst = marketAPI.GetStoreManagers(store_founder_token, storeName).Value;
            foreach (StoreManagerDTO s in lst)
                if (s.Username == managerUsername3)
                    doubleCheck = s.Appointer == ownerUsername2;

            Assert.IsTrue(response1.ErrorOccured);
            Assert.IsTrue(response2.ErrorOccured);
            Assert.IsFalse(response3.ErrorOccured);
            Assert.IsTrue(doubleCheck);
        }
    }
}
