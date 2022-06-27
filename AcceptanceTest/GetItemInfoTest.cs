using MarketWeb.Server.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;

namespace AcceptanceTest
{
    [TestClass]
    public class GetItemInfoTest
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        string storeName_inSystem = "Shefa Issachar";
        string storeName_outSystem = "bla";
        string guest_VisitorToken;
        string registered_VisitorToken;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemID_outStock = 1111111;

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
            registered_VisitorToken = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(registered_VisitorToken, "Issachar Shkedi", "123456", new DateTime(1963, 3, 12));
            registered_VisitorToken = (marketAPI.Login(registered_VisitorToken, "Issachar Shkedi", "123456")).Value;// reg
            marketAPI.OpenNewStore(registered_VisitorToken, storeName_inSystem);
            itemAmount_inSttock_1 = 150;
            itemAmount_inSttock_2 = 30;
            itemID_inStock_1 = marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem,
                "Leben", 1.6, "Basic product", "Diary", itemAmount_inSttock_1).Value;
            itemID_inStock_2 = marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem,
                "Tomatoes Juice", 4.2, "500ml bottle", "Drinks", itemAmount_inSttock_2).Value;
        }

        [TestMethod]
        public void GetItemInformation_Happy()
        {
            bool flag1 = false, flag2 = false; ;
            List<ItemDTO> items = marketAPI.GetItemInformation(registered_VisitorToken, "Leben", "Diary", "").Value;
            if (items != null)
            {
                foreach (ItemDTO item in items)
                {
                    if (item.Name == "Leben")
                    {
                        flag1 = true;
                    }
                }
                items = marketAPI.GetItemInformation(registered_VisitorToken, "i", "", "bottle").Value;
                if (items != null)
                {
                    foreach (ItemDTO item in items)
                    {
                        if (item.Name == "Tomatoes Juice")
                        {
                            flag2 = true;
                        }
                    }
                }
                else
                {
                    Assert.Fail("Item Leben supposed to be found");
                }
                Assert.IsTrue(flag1 && flag2);
            }
        }

        [TestMethod]
        public void GetItemInformation_Sad_StoreNotExist()
        {
            bool flag1 = false, flag2 = false; ;
            Response<List<ItemDTO>> items = marketAPI.GetItemInformation(registered_VisitorToken, "aaaaaa", "", "");
            if (items.Value != null && items.Value.Count != 0)
            {
                Assert.Fail();  
            }
        }
    }
}
