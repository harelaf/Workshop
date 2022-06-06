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
            //debugfghjk();
            CreateHostBuilder(args).Build().Run();
        }
        //private static void debugfghjk()
        //{
        //    MarketAPI market = new MarketAPI(new Domain.Market(), null);
        //    String username1 = "username1";
        //    String password1 = "password1";
        //    String storeName1 = "storeName1";
        //    String auth1 = "Bearer " + market.EnterSystem().Value;

        //    String username2 = "username2";
        //    String password2 = "password2";
        //    String storeName2 = "storeName2";
        //    String auth2 = "Bearer " + market.EnterSystem().Value;

        //    if (market.Register(auth1, username1, password1, new DateTime(1992, 8, 4)).ErrorOccured)
        //        return;
        //    auth1 = "Bearer " + market.Login(auth1, username1, password1).Value;
        //    market.OpenNewStore(auth1, storeName1);

        //    market.Register(auth2, username2, password2, new DateTime(1992, 8, 4));
        //    auth2 = "Bearer " + market.Login(auth2, username2, password2).Value;
        //    market.OpenNewStore(auth2, storeName2);

        //    int itemID1 = 1;
        //    int price1 = 1;
        //    String itemName1 = "itemName1";
        //    String category1 = "category1";
        //    String desc1 = "some item description goes here.";
        //    int quantity1 = 100;

        //    int itemID2 = 2;
        //    int price2 = 2;
        //    String itemName2 = "itemName2";
        //    String category2 = "dairy";
        //    String desc2 = "some other item description goes here.";
        //    int quantity2 = 200;

        //    int itemID3 = 3;
        //    int price3 = 3;
        //    String itemName3 = "itemName3";
        //    String category3 = "category3";
        //    String desc3 = "some other other item description goes here.";
        //    int quantity3 = 300;

        //    market.AddItemToStoreStock(auth1, storeName1, itemID1, itemName1, price1, desc1, category1, quantity1);

        //    market.AddItemToStoreStock(auth1, storeName1, itemID2, itemName2, price2, desc2, category2, quantity2);
        //    market.AddItemToStoreStock(auth1, storeName1, itemID3, itemName3, price3, desc3, category3, quantity3);

        //    market.AddItemToStoreStock(auth2, storeName2, itemID1, itemName1, price1, desc1, category1, quantity1);
        //    market.AddItemToStoreStock(auth2, storeName2, itemID2, itemName2, price2, desc2, category2, quantity2);
        //    market.AddItemToStoreStock(auth2, storeName2, itemID3, itemName3, price3, desc3, category3, quantity3);

        //    DateTime expiration = DateTime.Today.AddDays(10);
        //    int minAmount = 5;
        //    int maxAmount = 15;
        //    int percentageToSubtract = 10;
        //    int priceToSubtract = 2;

        //    IConditionDTO itemCondition = new SearchItemConditionDTO(itemName1, minAmount, maxAmount, false);
        //    IConditionDTO dayCondition = new DayOnWeekConditionDTO(DateTime.Today.DayOfWeek.ToString(), false);
        //    List<IConditionDTO> condLst = new List<IConditionDTO>();
        //    condLst.Add(itemCondition);
        //    condLst.Add(dayCondition);
        //    IConditionDTO andCondition = new AndCompositionDTO(false, condLst);
        //    AtomicDiscountDTO itemDis = new ItemDiscountDTO(percentageToSubtract, itemName1, null, expiration);
        //    IConditionDTO categoryCond = new SearchCategoryConditionDTO(category1, minAmount, maxAmount, false);
        //    NumericDiscountDTO numDis = new NumericDiscountDTO(priceToSubtract, categoryCond, expiration);
        //    List<IDiscountDTO> disLst = new List<IDiscountDTO>();
        //    disLst.Add(itemDis);
        //    disLst.Add(numDis);
        //    MaxDiscountDTO max = new MaxDiscountDTO(disLst, null);

        //    market.AddStoreDiscount(auth1, storeName1, max);


        //    String dairyCategory = category2;
        //    PriceableConditionDTO pricable = new PriceableConditionDTO(null, 100, -1, false);
        //    SearchItemConditionDTO itemCond = new SearchItemConditionDTO(itemName2, 3, -1, false);
        //    List<IConditionDTO> condLst2 = new List<IConditionDTO>();
        //    condLst2.Add(pricable);
        //    condLst2.Add(itemCond);
        //    OrCompositionDTO orCond = new OrCompositionDTO(false, condLst2);
        //    IDiscountDTO categoryDis = new CategoryDiscountDTO(percentageToSubtract, dairyCategory, orCond, expiration);
        //    market.AddStoreDiscount(auth2, storeName2, categoryDis);



        //    JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
        //    serializerSettings.Formatting = Formatting.Indented;
        //    serializerSettings.TypeNameHandling = TypeNameHandling.Auto;
        //    serializerSettings.Converters.Add(new DiscountConverter());
        //    serializerSettings.Converters.Add(new ConditionConverter());
        //    serializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        //    String json = JsonConvert.SerializeObject(max, serializerSettings);
        //    Console.WriteLine(json);
        //    Console.ReadLine();
        //    AtomicDiscountDTO restored = JsonConvert.DeserializeObject<AtomicDiscountDTO>(json, serializerSettings);
        //}

        //class CategoryConverter : JsonConverter
        //{
        //    public override bool CanConvert(Type objectType)
        //    {
        //        return (objectType == typeof(AtomicDiscountDTO));
        //    }
        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        return serializer.Deserialize(reader, typeof(CategoryDiscountDTO));
        //    }
        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        serializer.Serialize(writer, value, typeof(CategoryDiscountDTO));
        //    }
        //}

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
