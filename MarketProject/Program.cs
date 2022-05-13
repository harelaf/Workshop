using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;

namespace MarketProject
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		static void Main(string[] args)
		{
			log.Info("Hello logging world!");
			Console.WriteLine("Hit enter");
			Console.ReadLine();

			String itemName = "cookie";
			String storeName = "capCake";
			String founderName = "george washington";
			DateTime expiration = DateTime.Now.AddDays(1);
			double percentage_to_subtract = 10;
			StoreFounder founder;
			Store store;
			ShoppingBasket basket;


			founder = new StoreFounder(founderName, storeName);
			store = new Store(storeName, founder, new PurchasePolicy(), new DiscountPolicy());
			basket = new ShoppingBasket(store);

			DiscountCondition itemCon = new SearchItemCondition(itemName, 5, 15, false);
			DiscountCondition hour = new HourCondition(20, 22, false);
			List<DiscountCondition> lst = new List<DiscountCondition>();
			lst.Add(itemCon);
			lst.Add(hour);
			DiscountCondition and = new AndComposition(false, lst);
			DiscountCondition priceCon = new PriceableCondition("", 0, 100, false);
			List<DiscountCondition> lst2 = new List<DiscountCondition>();
			lst2.Add(priceCon);
			lst2.Add(and);
			DiscountCondition or = new OrComposition(false, lst2);
			Discount dis = new CategoryDiscount(percentage_to_subtract, "category", or, expiration);
			ItemDiscount dis2 = new ItemDiscount(percentage_to_subtract, itemName, expiration);
			List<Discount> lst3 = new List<Discount>();
			lst3.Add(dis);
			lst3.Add(dis2);
			Discount max = new MaxDiscount(lst3);
			store.AddDiscount(max);
			//store.AddDiscount(dis2);
			basket.AddItem(new Item(1, itemName, 10, "desc", "category"), 1);

            //Assert
            //Console.WriteLine(dis.GetTotalDiscount(basket));
            //Console.WriteLine(dis.GetDiscountString(0));
            //Console.WriteLine("------------------------------------------------------------");
            //Console.WriteLine(dis2.GetTotalDiscount(basket));
            //Console.WriteLine(dis2.GetDiscountString(0));
            Console.WriteLine(max.GetTotalDiscount(basket));
            Console.WriteLine(max.GetActualDiscountString(basket, 0));
            Console.ReadLine();
		}
    }
}
