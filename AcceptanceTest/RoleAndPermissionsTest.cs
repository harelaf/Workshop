using MarketWeb.Server.DataLayer;
using MarketWeb.Server.Domain;
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
    public class RoleAndPermissionsTest
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        String storeName = "test's Shop";
        //String storeName_outSystem = "bla";
        String? guest_VisitorToken;
        String? store_founder_token;
        String? store_founder_name;
        DateTime dob = new DateTime(2001, 7, 30);

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

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
        [TestMethod]
        public void TestAddManager_2VisitorsAddingDiffRolesSamePerson_oneIsFailed()
        {
            String managerUsername = "new manager";
            String ownerUsername = "new owner";
            String store_owner_token = (marketAPI.EnterSystem()).Value;// owner
            marketAPI.Register(store_owner_token, ownerUsername, "123456789", dob); 
            marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername, storeName);//success. first owner's appointment.
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
            marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            marketAPI.AcceptOwnerAppointment(store_owner_token1, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment
            marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);//success. everyone approved owner's appointment.

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
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;
            marketAPI.AcceptOwnerAppointment(store_owner_token1, ownerUsername2, storeName);//success. first owner's appointment.
            List<StoreManagerDTO> lst_m;
            List<StoreOwnerDTO> lst_o;
            //act
            Response response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            Response response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment
            Response response6 = marketAPI.AcceptOwnerAppointment(store_owner_token1, ownerUsername2, storeName);//success. everyone approved owner's appointment.
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

        [TestMethod]
        public void TestDenyStoreManagerPermission_DenyManagerOwnedPermissionByAppointer_success()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            Response response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);//success. first owner's appointment.
            Response response3 = marketAPI.AddStoreManager(store_owner_token2, managerUsername3, storeName);

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.RemoveManagerPermission(store_owner_token2, managerUsername3, storeName, Operation.STORE_HISTORY_INFO.ToString());
            Response response5 = marketAPI.HasPermission(store_manager_token3, storeName, Operation.STORE_HISTORY_INFO.ToString());

            Assert.IsFalse(response4.ErrorOccured, response4.ErrorMessage);
            Assert.IsTrue(response5.ErrorOccured);
        }

        [TestMethod]
        public void TestDenyStoreManagerPermission_DenyManagerOwnedPermissionNotByAppointer_throwsException()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob); 
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            Response response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            Response response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment
            Response response6 = marketAPI.AcceptOwnerAppointment(store_owner_token1, ownerUsername2, storeName);//success. everyone approved owner's appointment.
            Response response3 = marketAPI.AddStoreManager(store_owner_token2, managerUsername3, storeName);

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.RemoveManagerPermission(store_owner_token1, managerUsername3, storeName, Operation.STORE_HISTORY_INFO.ToString());
            Response response5 = marketAPI.HasPermission(store_manager_token3, storeName, Operation.STORE_HISTORY_INFO.ToString());
            
            Assert.IsTrue(response4.ErrorOccured);
            Assert.IsFalse(response5.ErrorOccured, response5.ErrorMessage);
        }

        [TestMethod]
        public void TestDenyStoreManagerPermission_DenyManagerMissingPermission_failure()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            Response response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            Response response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment
            Response response6 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);//success. everyone approved owner's appointment.
            Response response3 = marketAPI.AddStoreManager(store_owner_token2, managerUsername3, storeName);

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.RemoveManagerPermission(store_owner_token2, managerUsername3, storeName, Operation.STORE_WORKERS_INFO.ToString());
            Response response5 = marketAPI.HasPermission(store_manager_token3, storeName, Operation.STORE_WORKERS_INFO.ToString());

            Assert.IsTrue(response4.ErrorOccured);
            Assert.IsTrue(response5.ErrorOccured);
        }

        [TestMethod]
        public void TestDenyStoreOwnerPermission_DenyOwnerPermission_throwsException()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            Response response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.RemoveManagerPermission(store_founder_token, ownerUsername1, storeName, Operation.STORE_WORKERS_INFO.ToString());
            Response response5 = marketAPI.HasPermission(store_owner_token1, storeName, Operation.STORE_WORKERS_INFO.ToString());

            Assert.IsTrue(response4.ErrorOccured);
            Assert.IsFalse(response5.ErrorOccured, response5.ErrorMessage);
        }
        [TestMethod]
        public void TestDenySystemAdminPermission_DenyAdminPermission_throwsException()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String adminUsername1 = "admin";
            String adminUsername2 = "admin2";
            String password = "admin";
            String admin_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(admin_token1, adminUsername1, password, dob);
            String admin_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(admin_token2, adminUsername2, password, dob);

            admin_token1 = marketAPI.Login(admin_token1, adminUsername1, password).Value;
            admin_token2 = marketAPI.Login(admin_token2, adminUsername2, password).Value;
            Response response1 = marketAPI.AppointSystemAdmin(admin_token1, adminUsername2);

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.RemoveManagerPermission(admin_token1, adminUsername2, storeName, Operation.STORE_WORKERS_INFO.ToString());

            Assert.IsTrue(response4.ErrorOccured);
        }

        [TestMethod]
        public void TestGrantStoreManagerPermission_GrantManagerPermissionByAppointer_success()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            Response response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);//success. first owner's appointment.
            Response response3 = marketAPI.AddStoreManager(store_owner_token2, managerUsername3, storeName);

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.AddManagerPermission(store_owner_token2, managerUsername3, storeName, Operation.STOCK_EDITOR.ToString());
            Response response5 = marketAPI.HasPermission(store_manager_token3, storeName, Operation.STOCK_EDITOR.ToString());

            Assert.IsFalse(response4.ErrorOccured, response4.ErrorMessage);
            Assert.IsFalse(response5.ErrorOccured, response4.ErrorMessage);
        }

        [TestMethod]
        public void TestGrantStoreManagerPermission_GrantManagerPermissionNotByAppointer_fail()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            Response response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            Response response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment.
            Response response6 = marketAPI.AcceptOwnerAppointment(store_owner_token1, ownerUsername2, storeName);//success. everyone approved owner's appointment.
            Response response3 = marketAPI.AddStoreManager(store_owner_token2, managerUsername3, storeName);

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.AddManagerPermission(store_owner_token1, managerUsername3, storeName, Operation.STOCK_EDITOR.ToString());
            Response response5 = marketAPI.HasPermission(store_manager_token3, storeName, Operation.STOCK_EDITOR.ToString());

            Assert.IsTrue(response4.ErrorOccured);
            Assert.IsTrue(response5.ErrorOccured);
        }

        [TestMethod]
        public void TestGrantStoreOwnerPermission_GrantOwnerPermission_throwsException()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername2, password).Value;
            Response response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.

            //act
            //Response response3_fire = marketAPI.RemoveStoreManager(store_owner_token2, managerUsername3, storeName);
            Response response4 = marketAPI.AddManagerPermission(store_founder_token, ownerUsername1, storeName, Operation.PERMENENT_CLOSE_STORE.ToString());
            Response response5 = marketAPI.HasPermission(managerUsername3, storeName, Operation.PERMENENT_CLOSE_STORE.ToString());

            Assert.IsTrue(response4.ErrorOccured);
            Assert.IsTrue(response5.ErrorOccured, response5.ErrorMessage);
        }
        [TestMethod]
        public void TestAddOwner_Add2ownersAcceptSecond_Success()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            //act
            Response<bool> response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            Response<bool> response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment
            Response<bool> response3 = marketAPI.AcceptOwnerAppointment(store_owner_token1, ownerUsername2, storeName);//success. everyone approved owner's appointment.
            
            Assert.IsTrue(!response1.ErrorOccured && response1.Value, response1.ErrorMessage);
            Assert.IsTrue(!response2.ErrorOccured && !response2.Value, response2.ErrorMessage);
            Assert.IsTrue(!response2.ErrorOccured && response3.Value, response3.ErrorMessage);
        }
        [TestMethod]
        public void TestAddOwner_Add2ownersAcceptThird_failure()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            //act
            Response<bool> response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            Response<bool> response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment
            Response<bool> response3 = marketAPI.AcceptOwnerAppointment(store_owner_token1, store_founder_name, storeName);//success. everyone approved owner's appointment.

            Assert.IsTrue(!response1.ErrorOccured && response1.Value, response1.ErrorMessage);
            Assert.IsTrue(!response2.ErrorOccured && !response2.Value, response2.ErrorMessage);
            Assert.IsTrue(response3.ErrorOccured);
        }
        [TestMethod]
        public void TestAddOwner_Add2ownersRejectSecond_Success()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";
            String ownerUsername2 = "new owner2";
            String managerUsername3 = "new manager3";
            String password = "123456789";
            String store_owner_token1 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token1, ownerUsername1, password, dob);
            String store_owner_token2 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_owner_token2, ownerUsername2, password, dob);
            String store_manager_token3 = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(store_manager_token3, managerUsername3, password, dob);

            store_owner_token1 = marketAPI.Login(store_owner_token1, ownerUsername1, password).Value;
            store_owner_token2 = marketAPI.Login(store_owner_token2, ownerUsername2, password).Value;
            store_manager_token3 = marketAPI.Login(store_manager_token3, managerUsername3, password).Value;

            //act
            Response<bool> response1 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername1, storeName);//success. first owner's appointment.
            Response<bool> response2 = marketAPI.AcceptOwnerAppointment(store_founder_token, ownerUsername2, storeName);// awaiting ownerUsername1 to approve appointment
            Response response3 = marketAPI.RejectOwnerAppointment(store_owner_token1, storeName, ownerUsername2);//success. everyone approved owner's appointment.
            bool notAdded = !marketAPI.GetUsernamesWithOwnerAppointmentPermissionInStore(store_owner_token1, storeName).Value.Contains(ownerUsername2);

            Assert.IsTrue(!response1.ErrorOccured && response1.Value, response1.ErrorMessage);
            Assert.IsTrue(!response2.ErrorOccured && !response2.Value, response2.ErrorMessage);
            Assert.IsFalse(response3.ErrorOccured);
            Assert.IsTrue(notAdded);
        }
        [TestMethod]
        public void TestRejectOwner_RejectStranger_Failure()
        {
            RegisteredDTO user = marketAPI.GetVisitorInformation(store_founder_token).Value;
            String ownerUsername1 = "new owner1";

            //act
            Response response = marketAPI.RejectOwnerAppointment(store_founder_token, storeName, ownerUsername1);

            Assert.IsTrue(response.ErrorOccured);
        }
    }
}
