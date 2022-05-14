using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MarketProject.Service;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using MarketProject.Service.DTO;

namespace AcceptanceTest
{
    [TestClass]
    public class DiscountAndConditionsTests
    {
        MarketAPI marketAPI = new MarketAPI();
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
            ItemDiscountDTO dis = new ItemDiscountDTO(percentage_to_subtract, itemName, null, expiration);
            marketAPI.AddStoreDiscount(store_founder_token, storeName, dis);
            marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);
            double price = marketAPI.CalcCartActualPrice(guest_VisitorToken).Value;

            //act
            double expected = 9.0;
            double actual = marketAPI.CalcCartActualPrice(guest_VisitorToken).Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        //[TestMethod]
        //public void GetTotalDiscount_withCondition_success()
        //{
        //    double percentage_to_subtract = 10;
        //    int amount = 10;
        //    double price = 1;
        //    ComposedDiscountCondition andCondition = new AndComposition(false);
        //    DiscountCondition itemCondition = new SearchItemCondition(itemName, 5, 15, false);
        //    DiscountCondition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
        //    andCondition.AddConditionToComposition(itemCondition);
        //    andCondition.AddConditionToComposition(hourCondition);
        //    ItemDiscount dis = new ItemDiscount(percentage_to_subtract, itemName, andCondition, expiration);
        //    //store.AddDiscount(dis);
        //    basket.AddItem(new Item(1, itemName, price, "desc", "category"), amount);

        //    //act
        //    double expected = 1.0;
        //    double actual = dis.GetTotalDiscount(basket);

        //    //Assert
        //    Assert.AreEqual(expected, actual);
        //    //Assert.IsTrue(itemCondition.Check(basket));
        //    //Assert.AreEqual(basket.GetItemPrice(itemName), 10);
        //}
    }
}
