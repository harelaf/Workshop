using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using MarketWeb.Service;
using MarketWeb.Shared;

namespace AcceptanceTest
{
    [TestClass]
    public class DiscountAndConditionsTests
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        String storeName = "test's Shop";
        //String storeName_outSystem = "bla";
        String? guest_VisitorToken;
        String? store_founder_token;
        String? store_founder_name;
        DateTime expiration = DateTime.Now.AddDays(1);
        int itemID = 1;
        String itemName = "item";
        String category = "category";
        DateTime bDay = new DateTime(1992, 8, 4);

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

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

            String dis = $"ItemPercentage_{itemName}_{percentage_to_subtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            
            String cond = "";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, cond, dis);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 9.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

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
            String item_condition = $"ItemTotalAmountInBasketRange_{itemName}_{5}_{15}";
            String hour_condition = $"Hour_{(DateTime.Now.Hour + 23) % 24}_{(DateTime.Now.Hour + 1) % 24}";
            String and_condition = $"(AND {item_condition} {hour_condition})";

            String discount = $"ItemPercentage_{itemName}_{percentage_to_subtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, and_condition, discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);
           
            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 9.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

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

            String item_condition = $"(NOT ItemTotalAmountInBasketRange_{itemName}_{5}_{15})";
            String hour_condition = $"Hour_{(DateTime.Now.Hour + 23) % 24}_{(DateTime.Now.Hour + 1) % 24}";
            String and_condition = $"(AND {item_condition} {hour_condition})";

            String discount = $"ItemPercentage_{itemName}_{percentage_to_subtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, and_condition, discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 10.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalDiscount_withMaxDiscount_success()
        {
            double percentageToSubtract = 10;
            double priceToSubtract = 2;
            int amount = 17;
            double price = 1;
            int quantity = 100;
            String desc = "an item";

            String item_condition = $"(NOT ItemTotalAmountInBasketRange_{itemName}_{5}_{15})";
            String hour_condition = $"Hour_{(DateTime.Now.Hour + 23) % 24}_{(DateTime.Now.Hour + 1) % 24}";
            String and_condition = $"(AND {item_condition} {hour_condition})";

            String item_discount = $"ItemPercentage_{itemName}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String category_discount = $"CategoryPercentage_{category}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String numeric_discount = $"BasketAbsolute_{priceToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String max_discount = $"(MAX {item_discount} {category_discount} {numeric_discount})";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, and_condition, max_discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 15.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalDiscount_ComplicatedCondition_success()
        {
            double percentageToSubtract = 10;
            double priceToSubtract = 2;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";

            String item_condition = $"(NOT ItemTotalAmountInBasketFrom_{itemName}_{100})";
            String day_condition = $"DayOfWeek_{(int)DateTime.Now.DayOfWeek + 1}";
            String basket_price_condition = $"TotalBasketPriceFrom_{5}";
            String item_amount_condition = $"ItemTotalAmountInBasketRange_{itemName}_{1}_{2}"; // False
            String and_condition = $"(AND {item_condition} {day_condition})";
            String or_condition = $"(OR {and_condition} {basket_price_condition})";
            String xor_condition = $"(XOR {or_condition} {item_amount_condition})";

            String item_discount = $"ItemPercentage_{itemName}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, xor_condition, item_discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 9.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalDiscount_ComplicatedCondition_failure()
        {
            double percentageToSubtract = 10;
            double priceToSubtract = 2;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";

            String item_condition = $"(NOT ItemTotalAmountInBasketFrom_{itemName}_{100})";
            String day_condition = $"DayOfWeek_{((int)DateTime.Now.DayOfWeek + 1) % 7}";
            String basket_price_condition = $"TotalBasketPriceFrom_{5}";
            String item_amount_condition = $"ItemTotalAmountInBasketRange_{itemName}_{5}_{10}"; // True
            String and_condition = $"(AND {item_condition} {day_condition})";
            String or_condition = $"(OR {and_condition} {basket_price_condition})";
            String xor_condition = $"(XOR {or_condition} {item_amount_condition})";

            String item_discount = $"ItemPercentage_{itemName}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, xor_condition, item_discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 10.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalDiscount_ComplicatedDiscount_success()
        {
            double percentageToSubtract = 10;
            double bigpercentageToSubtract = 50;
            double priceToSubtract = 2;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";

            String item_condition = $"(NOT ItemTotalAmountInBasketFrom_{itemName}_{100})"; //True

            String category_percentage1 = $"CategoryPercentage_{category}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String category_percentage2 = $"CategoryPercentage_{category}_{percentageToSubtract + 10}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String absolute_discount = $"BasketAbsolute_{priceToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String big_item_discount = $"ItemPercentage_{itemName}_{bigpercentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String plus_discount = $"(PLUS {category_percentage1} {category_percentage2} {absolute_discount})";
            String max_discount = $"(MAX {plus_discount} {big_item_discount})";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, item_condition, max_discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 5.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalDiscount_ComplicatedDiscount_failure()
        {
            double percentageToSubtract = 10;
            double bigpercentageToSubtract = 50;
            double priceToSubtract = 2;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";

            String item_condition = $"(NOT ItemTotalAmountInBasketFrom_{itemName}_{10})"; //False

            String category_percentage1 = $"CategoryPercentage_{category}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String category_percentage2 = $"CategoryPercentage_{category}_{percentageToSubtract + 10}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String absolute_discount = $"BasketAbsolute_{priceToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String big_item_discount = $"ItemPercentage_{itemName}_{bigpercentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String plus_discount = $"(PLUS {category_percentage1} {category_percentage2} {absolute_discount})";
            String max_discount = $"(MAX {plus_discount} {big_item_discount})";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, item_condition, max_discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 10.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalDiscount_ComplicatedDiscountAndCondition_success()
        {
            double percentageToSubtract = 10;
            double bigpercentageToSubtract = 50;
            double priceToSubtract = 2;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";

            String item_condition = $"(NOT ItemTotalAmountInBasketFrom_{itemName}_{100})";
            String day_condition = $"DayOfWeek_{((int)DateTime.Now.DayOfWeek + 1) % 7}";
            String basket_price_condition = $"TotalBasketPriceFrom_{5}";
            String item_amount_condition = $"ItemTotalAmountInBasketRange_{itemName}_{1}_{2}"; // False
            String and_condition = $"(AND {item_condition} {day_condition})";
            String or_condition = $"(OR {and_condition} {basket_price_condition})";
            String xor_condition = $"(XOR {or_condition} {item_amount_condition})";

            String category_percentage1 = $"CategoryPercentage_{category}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String category_percentage2 = $"CategoryPercentage_{category}_{percentageToSubtract + 10}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String absolute_discount = $"BasketAbsolute_{priceToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String big_item_discount = $"ItemPercentage_{itemName}_{bigpercentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String plus_discount = $"(PLUS {category_percentage1} {category_percentage2} {absolute_discount})";
            String max_discount = $"(MAX {plus_discount} {big_item_discount})";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, xor_condition, max_discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 5.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetTotalDiscount_ComplicatedDiscountAndCondition_failure()
        {
            double percentageToSubtract = 10;
            double bigpercentageToSubtract = 50;
            double priceToSubtract = 2;
            int amount = 10;
            double price = 1;
            int quantity = 100;
            String desc = "an item";

            String item_condition = $"(NOT ItemTotalAmountInBasketFrom_{itemName}_{100})";
            String day_condition = $"DayOfWeek_{((int)DateTime.Now.DayOfWeek + 1) % 7}";
            String basket_price_condition = $"TotalBasketPriceFrom_{5}";
            String item_amount_condition = $"ItemTotalAmountInBasketRange_{itemName}_{5}_{10}"; // True
            String and_condition = $"(AND {item_condition} {day_condition})";
            String or_condition = $"(OR {and_condition} {basket_price_condition})";
            String xor_condition = $"(XOR {or_condition} {item_amount_condition})";

            String category_percentage1 = $"CategoryPercentage_{category}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String category_percentage2 = $"CategoryPercentage_{category}_{percentageToSubtract + 10}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String absolute_discount = $"BasketAbsolute_{priceToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String big_item_discount = $"ItemPercentage_{itemName}_{bigpercentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            String plus_discount = $"(PLUS {category_percentage1} {category_percentage2} {absolute_discount})";
            String max_discount = $"(MAX {plus_discount} {big_item_discount})";

            Response res1 = marketAPI.AddStoreDiscount(store_founder_token, storeName, xor_condition, max_discount);
            Response res2 = marketAPI.AddItemToStoreStock(store_founder_token, storeName, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(guest_VisitorToken, itemID, storeName, amount);

            Assert.IsFalse(res1.ErrorOccured, "res1 " + res1.ErrorMessage);
            Assert.IsFalse(res2.ErrorOccured, "res2 " + res2.ErrorMessage);
            Assert.IsFalse(res3.ErrorOccured, "res3 " + res3.ErrorMessage);

            //act
            double expected = 10.0;
            Response<double> response = marketAPI.CalcCartActualPrice(guest_VisitorToken);
            Assert.IsFalse(response.ErrorOccured, "response " + response.ErrorMessage);
            double actual = response.Value;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
