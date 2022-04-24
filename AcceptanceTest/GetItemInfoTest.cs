﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketProject.Service;
using MarketProject.Service.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTest
{
    [TestClass]
    public class GetItemInfoTest
    {
        MarketAPI marketAPI = new MarketAPI();
        string storeName_inSystem = "Shefa Issachar";
        string storeName_outSystem = "bla";
        string guest_userToken;
        string registered_userToken;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemID_outStock = 1111111;

        [TestInitialize()]
        public void setup()
        {
            guest_userToken = (marketAPI.EnterSystem()).Value;
            registered_userToken = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(registered_userToken, "Issachar Shkedi", "123456");
            registered_userToken = (marketAPI.Login(registered_userToken, "Issachar Shkedi", "123456")).Value;// reg
            marketAPI.OpenNewStore(registered_userToken, storeName_inSystem);
            itemID_inStock_1 = 111; itemAmount_inSttock_1 = 150;
            itemID_inStock_2 = 222; itemAmount_inSttock_2 = 30;
            marketAPI.AddItemToStoreStock(registered_userToken, storeName_inSystem, itemID_inStock_1,
                "Leben", 1.6, "Basic product", "Diary", itemAmount_inSttock_1);
            marketAPI.AddItemToStoreStock(registered_userToken, storeName_inSystem, itemID_inStock_2,
                "Tomatoes Juice", 4.2, "500ml bottle", "Drinks", itemAmount_inSttock_2);
        }

        [TestMethod]
        public void TestEditItemDescription_Happy()
        {
            bool flag1 = false, flag2 = false; ;
            List<ItemDTO> items = marketAPI.GetItemInformation(registered_userToken, "Leben", "Diary", "").Value;
            if (items != null)
            {
                foreach (ItemDTO item in items)
                {
                    if (item.Name == "Leben")
                    {
                        flag1 = true;
                    }
                }
                items = marketAPI.GetItemInformation(registered_userToken, "i", "", "bottle").Value;
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
        public void TestEditItemDescription_Sad_StoreNotExist()
        {
            bool flag1 = false, flag2 = false; ;
            Response<List<ItemDTO>> items = marketAPI.GetItemInformation(registered_userToken, "aaaaaa", "", "");
            if (!items.ErrorOccured)
            {
                Assert.Fail();  
            }
        }
    }
}
