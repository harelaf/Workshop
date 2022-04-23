using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketProject.Service;
using MarketProject.Service.DTO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace AcceptanceTest
{
    internal class EditItemTest
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
            marketAPI.AddItemToStoreStock(guest_userToken, storeName_inSystem, itemID_inStock_1,
                "Leben", 1.6, "", "Diary", itemAmount_inSttock_1);
            marketAPI.AddItemToStoreStock(guest_userToken, storeName_inSystem, itemID_inStock_2,
                "Tomatoes Juice", 4.2, "", "Drinks", itemAmount_inSttock_2);
        }

        [TestMethod]
        public void TestEditItem_happy()
        {
            Response response_reg = marketAPI.EditItemName(registered_userToken, storeName_inSystem, itemID_inStock_1, "Eshel");

        }

        public static string RandomString(int length)
        {
            Random random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
