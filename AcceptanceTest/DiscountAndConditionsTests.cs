using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using MarketWeb.Service;
using MarketWeb.Shared.DTO;

namespace AcceptanceTest
{
    [TestClass]
    public class DiscountAndConditionsTests
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
        [TestMethod]
        public void GetTotalDiscount_noCondition_success()
        {
            double percentage_to_subtract = 10;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";
            ItemDiscountDTO dis = new ItemDiscountDTO(percentage_to_subtract, itemName, null, expiration);
            marketAPI.AddStoreDiscount(store_founder_token, storeName, dis);
            bool ans2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemID, itemName, price, desc, category, quantity).ErrorOccured;
            bool ans = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount).ErrorOccured;

            //act
            double expected = 9.0;
            double actual = marketAPI.CalcCartActualPrice(guest_VisitorToken).Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetTotalDiscount_withCondition_success()
        {
            double percentage_to_subtract = 10;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";
            IConditionDTO itemCondition = new SearchItemConditionDTO(itemName, 5, 15, false);
            IConditionDTO hourCondition = new HourConditionDTO((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
            List<IConditionDTO> condLst = new List<IConditionDTO>();
            condLst.Add(itemCondition);
            condLst.Add(hourCondition);
            IConditionDTO andCondition = new AndCompositionDTO(false, condLst);
            ItemDiscountDTO dis = new ItemDiscountDTO(percentage_to_subtract, itemName, andCondition, expiration);
            marketAPI.AddStoreDiscount(store_founder_token, storeName, dis);
            bool ans2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemID, itemName, price, desc, category, quantity).ErrorOccured;
            bool ans = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount).ErrorOccured;

            //act
            double expected = 9.0;
            double actual = marketAPI.CalcCartActualPrice(guest_VisitorToken).Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetTotalDiscount_withConditionFalse_fail()
        {
            double percentage_to_subtract = 10;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";
            IConditionDTO itemCondition = new SearchItemConditionDTO(itemName, 5, 15, true);
            IConditionDTO hourCondition = new HourConditionDTO((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
            List<IConditionDTO> condLst = new List<IConditionDTO>();
            condLst.Add(itemCondition);
            condLst.Add(hourCondition);
            IConditionDTO andCondition = new AndCompositionDTO(false, condLst);
            ItemDiscountDTO dis = new ItemDiscountDTO(percentage_to_subtract, itemName, andCondition, expiration);
            marketAPI.AddStoreDiscount(store_founder_token, storeName, dis);
            bool ans2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemID, itemName, price, desc, category, quantity).ErrorOccured;
            bool ans = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount).ErrorOccured;

            //act
            double expected = 10.0;
            double actual = marketAPI.CalcCartActualPrice(guest_VisitorToken).Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetTotalDiscount_withMaxDiscount_success()
        {
            double percentageToSubtract = 10;
            double priceToSubtract = 2;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";
            IConditionDTO itemCondition = new SearchItemConditionDTO(itemName, 5, 15, false);
            IConditionDTO hourCondition = new HourConditionDTO((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
            List<IConditionDTO> condLst = new List<IConditionDTO>();
            condLst.Add(itemCondition);
            condLst.Add(hourCondition);
            IConditionDTO andCondition = new AndCompositionDTO(false, condLst);
            ItemDiscountDTO itemDis = new ItemDiscountDTO(percentageToSubtract, itemName, andCondition, expiration);
            IConditionDTO categoryCond = new SearchCategoryConditionDTO(category, 5, 15, false);
            NumericDiscountDTO numDis = new NumericDiscountDTO(priceToSubtract, categoryCond, expiration);
            List<IDiscountDTO> disLst = new List<IDiscountDTO>();
            disLst.Add(itemDis);
            disLst.Add(numDis);
            MaxDiscountDTO max = new MaxDiscountDTO(disLst, null);
            marketAPI.AddStoreDiscount(store_founder_token, storeName, max);
            bool ans2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemID, itemName, price, desc, category, quantity).ErrorOccured;
            bool ans = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount).ErrorOccured;

            //act
            double expected = 8.0;
            double actual = marketAPI.CalcCartActualPrice(guest_VisitorToken).Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
