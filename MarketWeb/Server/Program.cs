using MarketWeb.Server.Domain.PolicyPackage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server
{
    public class Programs
    {
        public static void Main(string[] args)
        {
            //debugfghjk();
            CreateHostBuilder(args).Build().Run();
        }
        private static void debugfghjk()
        {
           
            String itemName1 = "item";
            String category1 = "category";
            DateTime expiration = DateTime.Today.AddDays(10);
            int minAmount = 5;
            int maxAmount = 15;
            int percentageToSubtract = 10;
            int priceToSubtract = 2;
            Condition itemCondition = new SearchItemCondition(itemName1, minAmount, maxAmount, false);
            Condition dayCondition = new DayOnWeekCondition(DateTime.Today.DayOfWeek.ToString(), false);
            List<Condition> condLst = new List<Condition>();
            condLst.Add(itemCondition);
            condLst.Add(dayCondition);
            Condition andCondition = new AndComposition(false, condLst);
            ItemDiscount itemDis = new ItemDiscount(percentageToSubtract, itemName1, andCondition, expiration);
            Condition categoryCond = new SearchCategoryCondition(category1, minAmount, maxAmount, false);
            NumericDiscount numDis = new NumericDiscount(priceToSubtract, categoryCond, expiration);
            ISet<Discount> disLst = new HashSet<Discount>();
            disLst.Add(itemDis);
            disLst.Add(numDis);
            DiscountPolicy pl = new DiscountPolicy();
            pl.AddDiscount(itemDis);
            pl.AddDiscount(numDis);
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };
            String disListJSON = JsonConvert.SerializeObject(pl, settings);
            Console.WriteLine(disListJSON);
            Console.ReadLine();
            DiscountPolicy pl_j = JsonConvert.DeserializeObject<DiscountPolicy>(disListJSON, settings);
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
