using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;

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
			StoreFounder founder;
			Store store;
			ShoppingBasket basket;


			founder = new StoreFounder(founderName, storeName);
			store = new Store(storeName, founder, new PurchasePolicy(), new DiscountPolicy());
			basket = new ShoppingBasket(store);



			double percentage_to_subtract = 10;
			ItemDiscount dis = new ItemDiscount(percentage_to_subtract, itemName, expiration);
			store.AddDiscount(dis);
			basket.AddItem(new Item(1, itemName, 10, "desc", "category"), 1);

			//Assert
			Console.WriteLine(dis.GetTotalDiscount(basket));
			Console.WriteLine(dis.GetDiscountString(basket));
			Console.ReadLine();
		}
    }
}
