using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;
using System;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using MarketWeb.Server.Service;
using System.Linq;
using MarketWeb.Server.DataLayer;

namespace AcceptanceTest
{
    [TestClass]
    public class StatisticsTest
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        DateTime now;
        List<string> stores = new List<string>();
        List<string> registeredUserNames = new List<string>();
        List<string> managersOnlyUsernames = new List<string>();
        List<string> ownersOnlyUsernames = new List<string>();
        List<string> adminUsernames =  new List<string>();
        string pass = "1";
        DateTime dob = new DateTime(2001, 7, 30);
        int iterations = 20;
        int populationsKind = 5;
        int prep_loginOwnerCount = 0;
        int prep_loginAdminCount = 0;

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }
        [TestInitialize()]
        public void setup()
        {
            now = DateTime.Now;
            string token = marketAPI.EnterSystem().Value;
            string username = "username_";
            string manager = "manager_";
            string owner = "owner_";
            string admin = "admin_";
            for (int i = 0; i < iterations; i++)
            {
                marketAPI.Register(token, username + i, pass, dob);
                registeredUserNames.Add(username + i);

                marketAPI.Register(token, owner + i, pass, dob);
                ownersOnlyUsernames.Add(owner + i);

               marketAPI.Register(token, manager + i, pass, dob);
               managersOnlyUsernames.Add(manager + i);

                marketAPI.Register(token, admin + i, pass, dob);
                adminUsernames.Add(admin + i);

            }
            string store = "store_";  
            for ( int i =0; i < iterations; i++)
            {
                string ownerName = ownersOnlyUsernames[i];
                string managerName = managersOnlyUsernames[i];
                string storeName = store + i;
                string ownertoken = marketAPI.Login(token, ownerName, pass).Value;
                prep_loginOwnerCount++;
                marketAPI.OpenNewStore(ownertoken, storeName);
                stores.Add(storeName);
                marketAPI.AddStoreManager(ownertoken, managerName, storeName);
                token = marketAPI.Logout(ownertoken).Value;
            }
            Dictionary<String, String> configurations = new ConfigurationFileParser().ParseConfigurationFile();
            string adminToken = marketAPI.Login(token, configurations["admin_username"], configurations["admin_password"]).Value;
            prep_loginAdminCount++;
            for (int i = 0; i < iterations; i++)
            {
                marketAPI.AppointSystemAdmin(adminToken, adminUsernames[i]);
            }
            
        }

        [TestCleanup]


        [TestMethod]
        public void TestStaticticsSucses()
        {
            int expectedGuestCount = 100;
            int expectedRegisteredOnlyCount = iterations;
            int expectedManagersOnlyCount = iterations;
            int expectedOwnersCount = iterations;
            int expectedAdminsCount = iterations;
            string adminToken ="";
            foreach(string regUsername in registeredUserNames)
            {
                string token  = marketAPI.EnterSystem().Value;
                marketAPI.Login(token, regUsername, pass);
            }
            foreach (string managerUsername in managersOnlyUsernames)
            {
                string token = marketAPI.EnterSystem().Value;
                marketAPI.Login(token, managerUsername, pass);
            }
            foreach (string ownerUsername in ownersOnlyUsernames)
            {
                string token = marketAPI.EnterSystem().Value;
                marketAPI.Login(token, ownerUsername, pass);
            }
            foreach (string adminUsername in adminUsernames)
            {
                string token = marketAPI.EnterSystem().Value;
                adminToken =  marketAPI.Login(token, adminUsername, pass).Value;
            }
            for (int i = 0; i < expectedGuestCount; i++)
            {
                marketAPI.EnterSystem();
            }

            Response<ICollection<PopulationStatisticsDTO>> res = marketAPI.GetDailyPopulationStatistics(adminToken, now.Day, now.Month, now.Year);
            if (res.ErrorOccured)
                Assert.Fail("shouldn't fail");

            ICollection<PopulationStatisticsDTO> statisticsDTOs = res.Value;
            if(statisticsDTOs.Count != populationsKind)
                Assert.Fail($"there are only 5 kinds of population: {PopulationSection.GUESTS}, {PopulationSection.REGISTERED_NO_ROLES}, {PopulationSection.STORE_MANAGERS_ONLY}, {PopulationSection.STORE_OWNERS_NOT_ADMIN}, {PopulationSection.ADMIN}.\nStatistics collection should hold the count of each one. ");
            
            PopulationStatisticsDTO guestStatistics = statisticsDTOs.Where(x => x._section.Equals(PopulationSection.GUESTS)).FirstOrDefault();
            if (guestStatistics != null)
                Assert.AreEqual(expectedGuestCount, guestStatistics._count);
            
            PopulationStatisticsDTO RegStatistics = statisticsDTOs.Where(x => x._section.Equals(PopulationSection.REGISTERED_NO_ROLES)).FirstOrDefault();
            if (RegStatistics != null)
                Assert.AreEqual(expectedRegisteredOnlyCount, RegStatistics._count);
            
            PopulationStatisticsDTO managerStatistics = statisticsDTOs.Where(x => x._section.Equals(PopulationSection.STORE_MANAGERS_ONLY)).FirstOrDefault();
            if (managerStatistics != null)
                Assert.AreEqual(expectedManagersOnlyCount, managerStatistics._count);
           
            PopulationStatisticsDTO ownersStatistics = statisticsDTOs.Where(x => x._section.Equals(PopulationSection.STORE_OWNERS_NOT_ADMIN)).FirstOrDefault();
            if (ownersStatistics != null)
                Assert.AreEqual(expectedOwnersCount, ownersStatistics._count - prep_loginOwnerCount);
            
            PopulationStatisticsDTO adminStatistics = statisticsDTOs.Where(x => x._section.Equals(PopulationSection.ADMIN)).FirstOrDefault();
            if (adminStatistics != null)
                Assert.AreEqual(expectedAdminsCount, adminStatistics._count - prep_loginAdminCount);

            if (guestStatistics == null || RegStatistics == null || managerStatistics == null || ownersStatistics == null || adminStatistics == null)
                Assert.Fail("we should be able to find each one of population section in the statistic collection");
        }
    }
}
