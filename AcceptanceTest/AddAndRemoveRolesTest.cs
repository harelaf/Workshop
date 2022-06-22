﻿using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
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
    public class AddAndRemoveRolesTest
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        String storeName = "test's Shop";
        //String storeName_outSystem = "bla";
        String guest_VisitorToken;
        String store_founder_token;
        String store_founder_name;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemID_outStock = 1111111;
        DateTime dob = new DateTime(2001, 7, 30);

        [TestInitialize()]
        public void setup()
        {
            guest_VisitorToken = (marketAPI.EnterSystem()).Value;
            store_founder_token = (marketAPI.EnterSystem()).Value;// guest
            store_founder_name = "afik";
            marketAPI.Register(store_founder_token, store_founder_name, "123456789", new DateTime(2001, 7, 30));
            store_founder_token = (marketAPI.Login(store_founder_token, store_founder_name, "123456789")).Value;// reg
            marketAPI.OpenNewStore(store_founder_token, storeName);
        }

        [TestCleanup]


        [TestMethod]
        public void TestAddManager_2VisitorsAddingDiffRolesSamePerson_oneIsFailed()
        {
            String managerUsername = "new manager";
            String ownerUsername = "new owner";
            String store_owner_token = (marketAPI.EnterSystem()).Value;// owner
            marketAPI.Register(store_owner_token, ownerUsername, "123456789", dob); 
            marketAPI.AddStoreOwnerForTestPurposes(store_founder_token, ownerUsername, storeName);
            String store_manager_token = (marketAPI.EnterSystem()).Value;// manager
            marketAPI.Register(store_manager_token, managerUsername, "123456789", dob);
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

            List<StoreManagerDTO> lst = marketAPI.GetStoreManagers(store_founder_token, storeName).Value;
            Assert.IsFalse(lst.Count == 0, "Managers list is empty");
            bool found = false;
            foreach(StoreManagerDTO s in lst)
                if(s.Username == managerUsername)
                    found = true;
            
            Assert.IsTrue(found, "found is false");
            Assert.IsTrue(res1 || res2, "both res1 and res2 are false");
            Assert.IsFalse(res1 && res2, "both res1 and res2 are true");
        }

        [TestMethod]
        public void TestAddManager_circularAppointing_unsuccessful()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            bool doubleCheck = false;
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;
            marketAPI.AddStoreOwnerForTestPurposes(store_founder_token, ownerUsername1, storeName);
            marketAPI.AddStoreOwnerForTestPurposes(store_owner_token1, ownerUsername2, storeName);

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

        [TestMethod]
        public void TestRemoveRole()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            bool doubleCheck = false;
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;
            //marketAPI.AddStoreOwner(store_owner_token1, ownerUsername2, storeName);
            List<StoreManagerDTO> lst_m;
            List<StoreOwnerDTO> lst_o;
            //act
            Response response1 = marketAPI.AddStoreOwnerForTestPurposes(store_founder_token, ownerUsername1, storeName);
            Response response2 = marketAPI.AddStoreOwnerForTestPurposes(store_founder_token, ownerUsername2, storeName);
            Response response3 = marketAPI.AddStoreManager(store_owner_token2, managerUsername3, storeName);
            
            lst_m = marketAPI.GetStoreManagers(store_founder_token, storeName).Value;
            Assert.IsNotNull(lst_m);
            Assert.AreEqual(1, lst_m.Count);
            lst_o = marketAPI.GetStoreOwners(store_founder_token, storeName).Value;
            Assert.IsNotNull(lst_o);
            Assert.AreEqual(2, lst_o.Count);
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response1_fire = marketAPI.RemoveStoreOwner(store_founder_token, ownerUsername1, storeName);
            Response response2_fire = marketAPI.RemoveStoreOwner(store_founder_token, ownerUsername2, storeName);
            

            lst_m = marketAPI.GetStoreManagers(store_founder_token, storeName).Value;
            Assert.IsNotNull(lst_m);
            Assert.AreEqual(0, lst_m.Count);
            lst_o = marketAPI.GetStoreOwners(store_founder_token, storeName).Value;
            Assert.IsNotNull(lst_o);
            Assert.AreEqual(0, lst_o.Count);
           
        }

    }
}
