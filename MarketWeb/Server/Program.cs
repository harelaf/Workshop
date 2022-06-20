using MarketWeb.Service;
using MarketWeb.Shared.DTO;
using MarketWeb.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketWeb.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //MarketAPI marketAPI = new MarketAPI(new Domain.Market(), null);
            //String username1 = "username1";
            //String password1 = "password1";
            //String storeName1 = "storeName1";
            //String auth1 = "Bearer " + marketAPI.EnterSystem().Value;

            //String username2 = "username2";
            //String password2 = "password2";
            //String storeName2 = "storeName2";
            //String auth2 = "Bearer " + marketAPI.EnterSystem().Value;

            //if (marketAPI.Register(auth1, username1, password1, new DateTime(1992, 8, 4)).ErrorOccured)
            //    return;
            //auth1 = "Bearer " + marketAPI.Login(auth1, username1, password1).Value;
            //marketAPI.OpenNewStore(auth1, storeName1);

            //marketAPI.Register(auth2, username2, password2, new DateTime(1992, 8, 4));
            //auth2 = "Bearer " + marketAPI.Login(auth2, username2, password2).Value;
            //marketAPI.OpenNewStore(auth2, storeName2);

            ////int itemID1 = 1;
            ////int price1 = 1;
            ////String itemName1 = "itemName1";
            ////String category1 = "category1";
            ////String desc1 = "some item description goes here.";
            ////int quantity1 = 100;

            ////int itemID2 = 2;
            ////int price2 = 2;
            ////String itemName2 = "itemName2";
            ////String category2 = "dairy";
            ////String desc2 = "some other item description goes here.";
            ////int quantity2 = 200;

            ////int itemID3 = 3;
            ////int price3 = 3;
            ////String itemName3 = "itemName3";
            ////String category3 = "category3";
            ////String desc3 = "some other other item description goes here.";
            ////int quantity3 = 300;

            ////market.AddItemToStoreStock(auth1, storeName1, itemID1, itemName1, price1, desc1, category1, quantity1);

            ////market.AddItemToStoreStock(auth1, storeName1, itemID2, itemName2, price2, desc2, category2, quantity2);
            ////market.AddItemToStoreStock(auth1, storeName1, itemID3, itemName3, price3, desc3, category3, quantity3);

            ////market.AddItemToStoreStock(auth2, storeName2, itemID1, itemName1, price1, desc1, category1, quantity1);
            ////market.AddItemToStoreStock(auth2, storeName2, itemID2, itemName2, price2, desc2, category2, quantity2);
            ////market.AddItemToStoreStock(auth2, storeName2, itemID3, itemName3, price3, desc3, category3, quantity3);

            ////DateTime expiration = DateTime.Today.AddDays(10);
            ////int minAmount = 5;
            ////int maxAmount = 15;
            ////int percentageToSubtract = 10;
            ////int priceToSubtract = 2;

            ////IConditionDTO itemCondition = new SearchItemConditionDTO(itemName1, minAmount, maxAmount, false);
            ////IConditionDTO dayCondition = new DayOnWeekConditionDTO(DateTime.Today.DayOfWeek.ToString(), false);
            ////List<IConditionDTO> condLst = new List<IConditionDTO>();
            ////condLst.Add(itemCondition);
            ////condLst.Add(dayCondition);
            ////IConditionDTO andCondition = new AndCompositionDTO(false, condLst);
            ////ItemDiscountDTO itemDis = new ItemDiscountDTO(percentageToSubtract, itemName1, andCondition, expiration);
            ////IConditionDTO categoryCond = new SearchCategoryConditionDTO(category1, minAmount, maxAmount, false);
            ////NumericDiscountDTO numDis = new NumericDiscountDTO(priceToSubtract, categoryCond, expiration);
            ////List<IDiscountDTO> disLst = new List<IDiscountDTO>();
            ////disLst.Add(itemDis);
            ////disLst.Add(numDis);
            ////MaxDiscountDTO max = new MaxDiscountDTO(disLst, null);

            ////market.AddStoreDiscount(auth1, storeName1, max);

            //double percentageToSubtract = 10;
            //double bigpercentageToSubtract = 50;
            //double priceToSubtract = 2;
            //int amount = 10;
            //double price = 1;
            //int quantity = 100;
            //String desc = "an item";
            //DateTime expiration = DateTime.Now.AddDays(1);
            //int itemID = 1;
            //String itemName = "item";
            //String category = "category";
            //DateTime bDay = new DateTime(1992, 8, 4);

            //String item_condition = $"(NOT ItemTotalAmountInBasketFrom_{itemName}_{100})";
            //String day_condition = $"DayOfWeek_{((int)DateTime.Now.DayOfWeek + 1) % 7}";
            //String basket_price_condition = $"TotalBasketPriceFrom_{5}";
            //String item_amount_condition = $"ItemTotalAmountInBasketRange_{itemName}_{5}_{10}"; // True
            //String and_condition = $"(AND {item_condition} {day_condition})";
            //String or_condition = $"(OR {and_condition} {basket_price_condition})";
            //String xor_condition = $"(XOR {or_condition} {item_amount_condition})";

            //String category_percentage1 = $"CategoryPercentage_{category}_{percentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            //String category_percentage2 = $"CategoryPercentage_{category}_{percentageToSubtract + 10}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            //String absolute_discount = $"BasketAbsolute_{priceToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            //String big_item_discount = $"ItemPercentage_{itemName}_{bigpercentageToSubtract}_{expiration.Year}_{expiration.Month}_{expiration.Day}";
            //String plus_discount = $"(PLUS {category_percentage1} {category_percentage2} {absolute_discount})";
            //String max_discount = $"(MAX {plus_discount} {big_item_discount})";

            //Response res1 = marketAPI.AddStoreDiscount(auth1, storeName1, xor_condition, max_discount);
            //Response res2 = marketAPI.AddItemToStoreStock(auth1, storeName1, itemID, itemName, price, desc, category, quantity);


            //DiscountPolicyDTO disPol = marketAPI.GetAllActiveStores(auth1).Value.Find(x => x.StoreName == storeName1).DiscountPolicy;

            //JsonSerializerSettings settings = new JsonSerializerSettings
            //{
            //    TypeNameHandling = TypeNameHandling.Auto,
            //    Formatting = Formatting.Indented
            //};

            //string json = JsonConvert.SerializeObject(disPol, settings);
            //Console.WriteLine(json);
            //Console.ReadLine();
            //DiscountPolicyDTO dto = JsonConvert.DeserializeObject<DiscountPolicyDTO>(json, settings);
            //Console.WriteLine("finished");
            //Console.ReadLine();


            //String dairyCategory = category2;
            //PriceableConditionDTO pricable = new PriceableConditionDTO(null, 100, -1, false);
            //SearchItemConditionDTO itemCond = new SearchItemConditionDTO(itemName2, 3, -1, false);
            //List<IConditionDTO> condLst2 = new List<IConditionDTO>();
            //condLst2.Add(pricable);
            //condLst2.Add(itemCond);
            //OrCompositionDTO orCond = new OrCompositionDTO(false, condLst2);
            //CategoryDiscountDTO categoryDis = new CategoryDiscountDTO(percentageToSubtract, dairyCategory, orCond, expiration);
            //AddStoreDiscount(auth2, storeName2, categoryDis);
            CreateHostBuilder(args).Build().Run();
        }


        interface animal
        {

        }



        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
