using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage.Tests
{
    [TestClass]
    public class AllProductsDiscountTest
    {
		String itemName = "cookie";
		String storeName = "capCake";
		String founderName = "george washington";
		DateTime expiration = DateTime.Now.AddDays(1);
		StoreFounder founder;
		Store store;
		ShoppingBasket basket;

		[TestInitialize]
		public void setup()
		{
			founder = new StoreFounder(founderName, storeName);
			store = new Store(storeName, founder, new PurchasePolicy(), new DiscountPolicy());
			basket = new ShoppingBasket(store);
		}

		[TestMethod]
		public void GetTotalDiscount_noCondition_success()
		{
			double percentage_to_subtract = 10;
			AllProductsDiscount dis = new AllProductsDiscount(percentage_to_subtract, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(new Item(1, itemName, 10, "desc", "category"), 1);
			
			//act
			double expected = 1.0;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void GetTotalDiscount_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			double price = 1;
			ComposedCondition andCondition = new AndComposition(false);
			Condition itemCondition = new SearchItemCondition(itemName, 5, 15, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(itemCondition);
			andCondition.AddConditionToComposition(hourCondition);
			AllProductsDiscount dis = new AllProductsDiscount(percentage_to_subtract, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(new Item(1, itemName, price, "desc", "category"), amount);

			//act
			double expected = 1.0;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetDiscountString_noCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			double price = 1;
			AllProductsDiscount dis = new AllProductsDiscount(percentage_to_subtract, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(new Item(1, itemName, price, "desc", "category"), amount);

			//act
			String expected = $"{percentage_to_subtract}% off all products.\nExpired on: {expiration}";
			String actual = dis.GetDiscountString(0);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetDiscountString_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			double price = 1;
			ComposedCondition andCondition = new AndComposition(false);
			Condition itemCondition = new SearchItemCondition(itemName, 5, 15, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(itemCondition);
			andCondition.AddConditionToComposition(hourCondition);
			AllProductsDiscount dis = new AllProductsDiscount(percentage_to_subtract, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(new Item(1, itemName, price, "desc", "category"), amount);

			//act
			String expected = $"{percentage_to_subtract}% off all products.\nExpired on: {expiration}\nCondition(s): {andCondition.GetConditionString(0)}";
			String actual = dis.GetDiscountString(0);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
