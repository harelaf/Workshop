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
        private static void debugfghjk()
        {
            MarketAPI marketAPI = new MarketAPI(new Domain.Market(), null);
            String username1 = "username1";
            String password1 = "password1";
            String storeName1 = "storeName1";
            String auth1 = "Bearer " + marketAPI.EnterSystem().Value;

            String username2 = "username2";
            String password2 = "password2";
            String storeName2 = "storeName2";
            String auth2 = "Bearer " + marketAPI.EnterSystem().Value;

            if (marketAPI.Register(auth1, username1, password1, new DateTime(1992, 8, 4)).ErrorOccured)
                return;
            auth1 = "Bearer " + marketAPI.Login(auth1, username1, password1).Value;
            marketAPI.OpenNewStore(auth1, storeName1);

            marketAPI.Register(auth2, username2, password2, new DateTime(1992, 8, 4));
            auth2 = "Bearer " + marketAPI.Login(auth2, username2, password2).Value;
            marketAPI.OpenNewStore(auth2, storeName2);

            DateTime expiration = DateTime.Now.AddDays(1);
            int itemID = 1;
            String itemName = "item";
            String category = "category";
            DateTime bDay = new DateTime(1992, 8, 4);

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

            Response res1 = marketAPI.AddStoreDiscount(auth1, storeName1, xor_condition, max_discount);
            Response res2 = marketAPI.AddItemToStoreStock(auth1, storeName1, itemID, itemName, price, desc, category, quantity);
            Response res3 = marketAPI.AddItemToCart(auth2, itemID, storeName1, amount);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            ShoppingCartDTO cart = marketAPI.ViewMyCart(auth2).Value;
            ShoppingBasketDTO basket = cart.GetBasket(storeName1);
            String disListJSON = JsonConvert.SerializeObject(basket.Items[itemID].Item2.DiscountList, settings);
            Console.WriteLine(disListJSON);
            Console.ReadLine();
            List<IDiscountDTO> dislst = JsonConvert.DeserializeObject<List<IDiscountDTO>>(disListJSON);
            Console.WriteLine("finished");
            Console.ReadLine();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
