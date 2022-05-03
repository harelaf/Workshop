using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using MarketProject.Domain.PurchasePackage.DiscountPackage;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage.Tests
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
			//store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			double expected = 1.0;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			//Assert.AreEqual(basket.GetCategoryPrice(category), 10);
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void GetTotalDiscount_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			ComposedDiscountCondition andCondition = new OrComposition(false);
			DiscountCondition categoryCondition = new SearchCategoryCondition(category, 5, 15, false);
			DiscountCondition hourCondition = new HourCondition(DateTime.Now.Hour - 1, DateTime.Now.Hour + 1, false);
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
			ComposedDiscountCondition andCondition = new AndComposition(false);
			DiscountCondition itemCondition = new SearchCategoryCondition(category, 5, 15, false);
			DiscountCondition hourCondition = new HourCondition(DateTime.Now.Hour - 1, DateTime.Now.Hour + 1, false);
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
			String expected = $"{percentage_to_subtract}% off all '{category}' products prices.\n\nExpired on: {expiration}\n\n";
			String actual = dis.GetDiscountString(basket);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetDiscountString_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			ComposedDiscountCondition andCondition = new AndComposition(false);
			DiscountCondition itemCondition = new SearchItemCondition(category, 5, 15, false);
			DiscountCondition hourCondition = new HourCondition(DateTime.Now.Hour - 1, DateTime.Now.Hour + 1, false);
			andCondition.AddConditionToComposition(itemCondition);
			andCondition.AddConditionToComposition(hourCondition);
			CategoryDiscount dis = new CategoryDiscount(percentage_to_subtract, category, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			String expected = $"{percentage_to_subtract}% off all '{category}' products prices.\n\nExpired on: {expiration}\n\nCondition(s): \n{andCondition.ToString()}";
			String actual = dis.GetDiscountString(basket);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
