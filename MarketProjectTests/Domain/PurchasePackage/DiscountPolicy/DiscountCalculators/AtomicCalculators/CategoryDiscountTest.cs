using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain;
using MarketWeb.Server.Domain.PolicyPackage;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage.Tests
{
	[TestClass]
	public class CategoryDiscountTest
	{
		String category = "cakes";
		String itemName = "cookie";
		double price = 1;
		String storeName = "capCakeFactory";
		String founderName = "george washington";
		DateTime expiration = DateTime.Now.AddDays(1);
		StoreFounder founder;
		Store store;
		ShoppingBasket basket;
		Item item;

		DalController dc = DalController.GetInstance(true);
		[TestCleanup()]
		public void cleanup()
		{
			dc.Cleanup();
		}
		[TestInitialize]
		public void setup()
		{
			founder = new StoreFounder(founderName, storeName);
			store = new Store(storeName, founder, new PurchasePolicy(), new DiscountPolicy());
			basket = new ShoppingBasket(store);
			item = new Item(1, itemName, price, "desc", category);
		}

		[TestMethod]
		public void GetTotalDiscount_noCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			CategoryDiscount dis = new CategoryDiscount(percentage_to_subtract, category, expiration);
			store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			double expected = 1.0;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(basket.GetCategoryPrice(category), 10);
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void GetTotalDiscount_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			ComposedCondition andCondition = new OrComposition(false);
			Condition categoryCondition = new SearchCategoryCondition(category, 5, 15, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(categoryCondition);
			andCondition.AddConditionToComposition(hourCondition);
			CategoryDiscount dis = new CategoryDiscount(percentage_to_subtract, category, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			double expected = 1.0;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void GetTotalDiscount_withCondition_fail()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			ComposedCondition andCondition = new AndComposition(false);
			Condition itemCondition = new SearchCategoryCondition(category, 5, 15, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(itemCondition);
			andCondition.AddConditionToComposition(hourCondition);
			CategoryDiscount dis = new CategoryDiscount(percentage_to_subtract, category, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(new Item(2, itemName, price, "desc", "otherCategory"), amount);

			//act
			double expected = 0;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetDiscountString_noCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			CategoryDiscount dis = new CategoryDiscount(percentage_to_subtract, category, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			String expected = $"{percentage_to_subtract}% off all '{category}' products prices.\nExpired on: {expiration}";
			String actual = dis.GetDiscountString(0);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetDiscountString_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			ComposedCondition andCondition = new AndComposition(false);
			Condition itemCondition = new SearchItemCondition(category, 5, 15, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(itemCondition);
			andCondition.AddConditionToComposition(hourCondition);
			CategoryDiscount dis = new CategoryDiscount(percentage_to_subtract, category, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			String expected = $"{percentage_to_subtract}% off all '{category}' products prices.\nExpired on: {expiration}\nCondition(s): {andCondition.GetConditionString(0)}";
			String actual = dis.GetDiscountString(0);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
