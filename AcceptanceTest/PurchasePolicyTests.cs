using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MarketWeb.Service;
using MarketWeb.Shared;

namespace AcceptanceTest
{
    [TestClass]
    public class PurchasePolicyTests
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        String storeName = "test's Shop";
        //String storeName_outSystem = "bla";
        String guest_VisitorToken;
        String store_founder_token;
        String store_founder_name;
        DateTime expiration = DateTime.Now.AddDays(1);
        int itemID = 1;
        String itemName = "item";
        String category = "category";
        DateTime bDay = new DateTime(1992, 8, 4);


        [TestInitialize]
        public void setup()
        {
            guest_VisitorToken = (marketAPI.EnterSystem()).Value;
            store_founder_token = (marketAPI.EnterSystem()).Value;// guest
            store_founder_name = "afik";
            marketAPI.Register(store_founder_token, store_founder_name, "123456789", bDay);
            store_founder_token = (marketAPI.Login(store_founder_token, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(store_founder_token, storeName);
        }
    }
}
