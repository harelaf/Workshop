using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using System.IO;
using System.Collections.Generic;

namespace AcceptanceTest
{
    [TestClass]
    public class TestInitializationFIle
    {
        String filename = "Initialization-File.txt";
        MarketAPI marketAPI;
        String username = "SpongeBob SquarePants";
        String password = "ILoveKrabbyPatties123";
        String username2 = "Patrick Star";
        String password2 = "IAmVerySmart";
        String storename = "Krusty Krab";
        String itemname = "Krabby Patty";
        String category = "Patties";
        String description = "Tasty";
        double price = 15.0;
        int amount = 100;
        DateTime dob = DateTime.Now;

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
            StreamWriter writer = new StreamWriter(filename);
            writer.Write("ignore_file");
            writer.Close();
        }

        [TestInitialize]
        public void setup()
        {
            MarketAPI.ResetInits();
            StreamWriter writer = new StreamWriter(filename);
            writer.Write("ignore_file");
            writer.Close();
        }

        [TestMethod]
        public void InitializationFileEmpty_Success()
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.Write("");
            writer.Close();

            // Create Market using initialization file.
            marketAPI = new MarketAPI(null, null);
            String authToken = marketAPI.EnterSystem().Value;
            marketAPI.Register(authToken, username, password, dob);
            authToken = marketAPI.Login(authToken, username, password).Value;

            // Assert Market is empty.
            Response<Dictionary<string, string>> res = marketAPI.GetAllActiveStores(authToken);
            Assert.IsFalse(res.ErrorOccured);
            Assert.IsTrue(res.Value.Count == 0);
        }

        [TestMethod]
        public void InitializationFileHasInstructions_Success()
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.Write("");
            writer.WriteLine($"EnterSystem()");
            writer.WriteLine($"EnterSystem()");
            writer.WriteLine($"Register(1, {username}, {password}, 1/2/2000 12:00:00 AM)");
            writer.WriteLine($"Register(2, {username2}, {password2}, 1/2/2000 12:00:00 AM)");
            writer.WriteLine($"Login(1, {username}, {password})");
            writer.WriteLine($"Login(2, {username2}, {password2})");
            writer.WriteLine($"OpenNewStore(1, {storename})");
            writer.WriteLine($"AddStoreManager(1, {username2}, {storename})");
            writer.WriteLine($"AddItemToStoreStock(1, {storename}, {itemname}, {price}, {description}, {category}, {amount})");
            writer.WriteLine($"Logout(1)");
            writer.WriteLine($"Logout(2)");
            writer.Close();

            // Create Market using initialization file.
            marketAPI = new MarketAPI(null, null);
            String authToken = marketAPI.EnterSystem().Value;
            marketAPI.Register(authToken, username, password, dob);
            authToken = marketAPI.Login(authToken, username, password).Value;

            // Assert Market is correct.
            Response<Dictionary<string, string>> res = marketAPI.GetAllActiveStores(authToken);
            Assert.IsFalse(res.ErrorOccured);
            Assert.IsTrue(res.Value.Count == 1);
            Assert.IsTrue(res.Value.ContainsKey(storename));

            Response<StoreDTO> res1 = marketAPI.GetStoreInformation(authToken, storename);
            Assert.IsFalse(res1.ErrorOccured);
            Assert.AreEqual(username, res1.Value.Founder.Username);
            Assert.IsTrue(res1.Value.Managers.Count == 1);
            Assert.AreEqual(username2, res1.Value.Managers[0].Username);
            Assert.IsTrue(res1.Value.Stock.Items.Count == 1);
            int id = 0;
            foreach (int item in res1.Value.Stock.Items.Keys)
            {
                id = item;
                break;
            }
            Assert.AreEqual(itemname, res1.Value.Stock.Items[id].Item1.Name);
            Assert.AreEqual(price, res1.Value.Stock.Items[id].Item1.Price);
            Assert.AreEqual(amount, res1.Value.Stock.Items[id].Item2);
        }

        [TestMethod]
        public void InitializationFileHasInstructionsFailsInTheMiddle_Failure()
        {
            StreamWriter writer = new StreamWriter(filename);
            writer.Write("");
            writer.WriteLine($"EnterSystem()");
            writer.WriteLine($"EnterSystem()");
            writer.WriteLine($"Register(1, {username}, {password}, 1/2/2000 12:00:00 AM)");
            writer.WriteLine($"Register(2, {username2}, {password2}, 1/2/2000 12:00:00 AM)");
            writer.WriteLine($"Login(1, {username}, {password})");
            writer.WriteLine($"Login(2, {username2}, {password2})");
            writer.WriteLine($"OpenNewStore(1, {storename})");
            writer.WriteLine($"AddStoreManager(1, {username2}, {storename})");
            writer.WriteLine($"AddStoreManager(1, {username2}, {storename})"); //Duplicate
            writer.WriteLine($"AddItemToStoreStock(1, {storename}, {itemname}, {price}, {description}, {category}, {amount})");
            writer.WriteLine($"Logout(1)");
            writer.WriteLine($"Logout(2)");
            writer.Close();

            // Create Market using initialization file.
            marketAPI = new MarketAPI(null, null);
            String authToken = marketAPI.EnterSystem().Value;
            String newuser = "Mr. Krabs";
            String newpass = "ILoveMoney";
            marketAPI.Register(authToken, newuser, newpass, dob);
            authToken = marketAPI.Login(authToken, newuser, newpass).Value;

            // Assert Market is correct.
            Response<Dictionary<string, string>> res = marketAPI.GetAllActiveStores(authToken);
            Assert.IsFalse(res.ErrorOccured);
            Assert.IsTrue(res.Value.Count == 1);
            Assert.IsTrue(res.Value.ContainsKey(storename));

            Response<StoreDTO> res1 = marketAPI.GetStoreInformation(authToken, storename);
            Assert.IsFalse(res1.ErrorOccured);
            Assert.AreEqual(username, res1.Value.Founder.Username);
            Assert.IsTrue(res1.Value.Managers.Count == 1);
            Assert.AreEqual(username2, res1.Value.Managers[0].Username);
            Assert.IsTrue(res1.Value.Stock.Items.Count == 0);
        }
    }
}
