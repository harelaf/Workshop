using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AcceptanceTest
{
    [TestClass]
    public class TestItemSearch
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        String username = "SpongeBob SquarePants";
        String password = "ILoveKrabbyPatties123";
        String username2 = "Patrick Star";
        String password2 = "IAmVerySmart";
        DateTime dob = DateTime.Now;
        String storename = "Krusty Krab";

        String itemname1 = "Krabby Patty";
        String category1 = "Patties";
        String description1 = "Tasty";
        String itemname2 = "Small Plankton";
        String category2 = "Green";
        String description2 = "Nasty";
        String itemname3 = "Garry The Snail";
        String category3 = "Slimy";
        String description3 = "Meow";
        double price = 15.0;
        int quantity = 100;

        int itemId1;
        int itemId2;
        int itemId3;
        String authToken;

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

        [TestInitialize]
        public void setup()
        {
            authToken = (marketAPI.EnterSystem()).Value;
            marketAPI.Register(authToken, username, password, dob);
            authToken = (marketAPI.Login(authToken, username, password)).Value;
            marketAPI.OpenNewStore(authToken, storename);
            itemId1 = marketAPI.AddItemToStoreStock(authToken, storename, itemname1, price, description1, category1, quantity).Value;
            itemId2 = marketAPI.AddItemToStoreStock(authToken, storename, itemname2, price, description2, category2, quantity).Value;
            itemId3 = marketAPI.AddItemToStoreStock(authToken, storename, itemname3, price, description3, category3, quantity).Value;
        }

        [TestMethod]
        public void SearchForItem1_AllParameters_Success()
        {
            Response<List<ItemDTO>> res = marketAPI.GetItemInformation(authToken, itemname1, category1, description1);
            Assert.IsFalse(res.ErrorOccured);

            Assert.IsTrue(res.Value.Count == 1);
            Assert.AreEqual(itemname1, res.Value[0].Name);
            Assert.AreEqual(description1, res.Value[0].Description);
            Assert.AreEqual(category1, res.Value[0].Category);            
        }

        [TestMethod]
        public void SearchForItems_NameContains_a_Success()
        {
            Response<List<ItemDTO>> res = marketAPI.GetItemInformation(authToken, "a", null, null);
            Assert.IsFalse(res.ErrorOccured);

            Assert.IsTrue(res.Value.Count == 3);
            bool found1 = false;
            bool found2 = false;
            bool found3 = false;
            foreach (ItemDTO item in res.Value)
            {
                if (item.ItemID == itemId1)
                {
                    Assert.AreEqual(itemname1, item.Name);
                    Assert.AreEqual(description1, item.Description);
                    Assert.AreEqual(category1, item.Category);
                    found1 = true;
                }
                if (item.ItemID == itemId2)
                {
                    Assert.AreEqual(itemname2, item.Name);
                    Assert.AreEqual(description2, item.Description);
                    Assert.AreEqual(category2, item.Category);
                    found2 = true;
                }
                if (item.ItemID == itemId3)
                {
                    Assert.AreEqual(itemname3, item.Name);
                    Assert.AreEqual(description3, item.Description);
                    Assert.AreEqual(category3, item.Category);
                    found3 = true;
                }
            }
            Assert.IsTrue(found1 && found2 && found3);
        }

        [TestMethod]
        public void SearchForItems1and2_SearchUsingName1AndDesciprtion2_Success()
        {
            Response<List<ItemDTO>> res = marketAPI.GetItemInformation(authToken, itemname1, null, description2);
            Assert.IsFalse(res.ErrorOccured);

            Assert.IsTrue(res.Value.Count == 2);
            bool found1 = false;
            bool found2 = false;
            bool found3 = false;
            foreach (ItemDTO item in res.Value)
            {
                if (item.ItemID == itemId1)
                {
                    Assert.AreEqual(itemname1, item.Name);
                    Assert.AreEqual(description1, item.Description);
                    Assert.AreEqual(category1, item.Category);
                    found1 = true;
                }
                if (item.ItemID == itemId2)
                {
                    Assert.AreEqual(itemname2, item.Name);
                    Assert.AreEqual(description2, item.Description);
                    Assert.AreEqual(category2, item.Category);
                    found2 = true;
                }
            }
            Assert.IsTrue(found1 && found2 && !found3);
        }

        [TestMethod]
        public void SearchForItems_CategoryDoesntContain_z_Failure()
        {
            Response<List<ItemDTO>> res = marketAPI.GetItemInformation(authToken, null, "z", null);
            Assert.IsFalse(res.ErrorOccured);

            Assert.IsTrue(res.Value.Count == 0);
        }

        [TestMethod]
        public void SearchForItems_SearchUsingItemsValues_Success()
        {
            Response<List<ItemDTO>> res = marketAPI.GetItemInformation(authToken, itemname1, category2, description3);
            Assert.IsFalse(res.ErrorOccured);

            Assert.IsTrue(res.Value.Count == 3);
            bool found1 = false;
            bool found2 = false;
            bool found3 = false;
            foreach (ItemDTO item in res.Value)
            {
                if (item.ItemID == itemId1)
                {
                    Assert.AreEqual(itemname1, item.Name);
                    Assert.AreEqual(description1, item.Description);
                    Assert.AreEqual(category1, item.Category);
                    found1 = true;
                }
                if (item.ItemID == itemId2)
                {
                    Assert.AreEqual(itemname2, item.Name);
                    Assert.AreEqual(description2, item.Description);
                    Assert.AreEqual(category2, item.Category);
                    found2 = true;
                }
                if (item.ItemID == itemId3)
                {
                    Assert.AreEqual(itemname3, item.Name);
                    Assert.AreEqual(description3, item.Description);
                    Assert.AreEqual(category3, item.Category);
                    found3 = true;
                }
            }
            Assert.IsTrue(found1 && found2 && found3);
        }
    }
}
