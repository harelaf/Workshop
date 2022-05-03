using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using MarketProject.Domain.PurchasePackage.DiscountPackage;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage.Tests
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
			Assert.AreEqual(actual, expected);
		}
		[TestMethod]
		public void GetTotalDiscount_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			double price = 1;
			ComposedDiscountCondition andCondition = new AndComposition(false);
			DiscountCondition itemCondition = new SearchItemCondition(itemName, 5, 15, false);
			DiscountCondition hourCondition = new HourCondition(DateTime.Now.Hour - 1, DateTime.Now.Hour + 1, false);
			andCondition.AddConditionToComposition(itemCondition);
			andCondition.AddConditionToComposition(hourCondition);
			AllProductsDiscount dis = new AllProductsDiscount(percentage_to_subtract, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(new Item(1, itemName, price, "desc", "category"), amount);

			//act
			double expected = 1.0;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(actual, expected);
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
			String expected = $"{percentage_to_subtract}% off all products.\n\nExpired on: {expiration}\n\n";
			String actual = dis.GetDiscountString(basket);

			//Assert
			Assert.AreEqual(actual, expected);
		}

		[TestMethod]
		public void GetDiscountString_withCondition_success()
		{
			double percentage_to_subtract = 10;
			int amount = 10;
			double price = 1;
			ComposedDiscountCondition andCondition = new AndComposition(false);
			DiscountCondition itemCondition = new SearchItemCondition(itemName, 5, 15, false);
			DiscountCondition hourCondition = new HourCondition(DateTime.Now.Hour - 1, DateTime.Now.Hour + 1, false);
			andCondition.AddConditionToComposition(itemCondition);
			andCondition.AddConditionToComposition(hourCondition);
			AllProductsDiscount dis = new AllProductsDiscount(percentage_to_subtract, andCondition, expiration);
			//store.AddDiscount(dis);
			basket.AddItem(new Item(1, itemName, price, "desc", "category"), amount);

			//act
			String expected = $"{percentage_to_subtract}% off all products.\n\nExpired on: {expiration}\n\nCondition(s): \n{andCondition.ToString()}";
			String actual = dis.GetDiscountString(basket);

			//Assert
			Assert.AreEqual(actual, expected);
		}
	}
}
