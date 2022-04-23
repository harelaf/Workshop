using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketProject.Service;
using MarketProject.Service.DTO;
using System.Collections.Generic;
using System;
using System.Linq;
using MarketProject.Domain;

namespace AcceptanceTest
{
    [TestClass]
    public class EditItemTest
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
                "Leben", 1.6, "", "Diary", itemAmount_inSttock_1);
            //marketAPI.AddItemToStoreStock(guest_userToken, storeName_inSystem, itemID_inStock_2,
                //"Tomatoes Juice", 4.2, "", "Drinks", itemAmount_inSttock_2);
        }

        [TestMethod]
        public void TestEditItemName_Happy()
        {
            Response response_reg = marketAPI.EditItemName(registered_userToken, storeName_inSystem, itemID_inStock_1, "Eshel");
            if (response_reg.ErrorOccured)
                Assert.Fail(response_reg.ErrorMessage);
            List<ItemDTO> res = marketAPI.GetItemInformation(registered_userToken, "Eshel", "", "").Value;
            if(res == null || res.Count == 0)
            {
                Assert.Fail("Item Eshel supposed to be found");
            }
            marketAPI.EditItemName(registered_userToken, storeName_inSystem, itemID_inStock_1, "Leben");
            res = marketAPI.GetItemInformation(registered_userToken, "Leben", "", "").Value;
            if (res == null || res.Count == 0)
            {
                Assert.Fail("Item Leben supposed to be found");
            }
        }

        [TestMethod]
        public void TestEditItemNmae_Sad_ItemOutOfStock()
        {
            Response response_reg = marketAPI.EditItemName(registered_userToken, storeName_inSystem, itemID_inStock_2, "apple juice");
            if (!response_reg.ErrorOccured)
                Assert.Fail("should've faild: item isn't in stock");
        }

        [TestMethod]
        public void TestEditItemName_Sad_UserWithoutPermition()
        {
            Response response_guest = marketAPI.EditItemName(guest_userToken, storeName_inSystem, itemID_inStock_1, "Eshel");
            if (!response_guest.ErrorOccured)
                Assert.Fail("should've faild: User doesn't have a permition to edit item");
        }


        [TestMethod]
        public void TestEditItemPrice_Happy()
        {
            Response response_reg = marketAPI.EditItemPrice(registered_userToken, storeName_inSystem, itemID_inStock_1, 12.5);
            if (response_reg.ErrorOccured)
                Assert.Fail("shouldn't have faild: item is in stock");
            List<ItemDTO> items = marketAPI.GetItemInformation(registered_userToken, "Leben", "Diary", "").Value;
            if (items != null)
            {
                foreach (ItemDTO item in items)
                {
                    if (item.Name == "Leben")
                    {
                        if (item.Price != 12.5)
                        {
                            Assert.Fail("Item's Description haven't changed");
                        }
                    }
                }
            }
            else
            {
                Assert.Fail("Item Leben supposed to be found");
            }
        }

        [TestMethod]
        public void TestEditItemPrice_Sad_ItemOutOfStock()
        {
            Response response_reg = marketAPI.EditItemPrice(registered_userToken, storeName_inSystem, itemID_inStock_2, 12.5);
            if (!response_reg.ErrorOccured)
                Assert.Fail("should've faild: item isn't in stock");
        }

        [TestMethod]
        public void TestEditItemPrice_Sad_UserWithoutPermition()
        {
            Response response_guest = marketAPI.EditItemPrice(guest_userToken, storeName_inSystem, itemID_inStock_1, 12.5);
            if (!response_guest.ErrorOccured)
                Assert.Fail("should've faild: User doesn't have a permition to edit item");
        }


        [TestMethod]
        public void TestEditItemDescription_Happy()
        {
            Response response_reg = marketAPI.EditItemDescription(registered_userToken, storeName_inSystem, itemID_inStock_1, RandomString(52));
            if (response_reg.ErrorOccured)
                Assert.Fail("shouldn't have faild: item is in stock");
            List<ItemDTO> items = marketAPI.GetItemInformation(registered_userToken, "Leben", "Diary", "").Value;
            if (items != null)
            {
                foreach (ItemDTO item in items)
                {
                    if (item.Name == "Leben")
                    {
                        if (item.Description.Length != 52)
                        {
                            Assert.Fail("Item's Description haven't changed");
                        }
                    }
                }
            }
            else
            {
                Assert.Fail("Item Leben supposed to be found");
            }
        }

        [TestMethod]
        public void TestEditItemDescription_Sad_ItemOutOfStock()
        {
            Response response_reg = marketAPI.EditItemDescription(registered_userToken, storeName_inSystem, itemID_inStock_2, RandomString(50));
            if (!response_reg.ErrorOccured)
                Assert.Fail("should've faild: item isn't in stock");
        }

        [TestMethod]
        public void TestEditItemDescription_Sad_UserWithoutPermition()
        {
            Response response_guest = marketAPI.EditItemDescription(registered_userToken, storeName_inSystem, itemID_inStock_2, RandomString(50));
            if (!response_guest.ErrorOccured)
                Assert.Fail("should've faild: User doesn't have a permition to edit item");
        }

        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
