using MarketWeb.Server.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketWeb.Server.Domain;
using MarketWeb.Server.Domain.PolicyPackage;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MarketProjectTests.Domain.PurchasePackage.DiscountPolicy.DiscountCalculators
{
	[TestClass]
	public class PlusDiscountTest
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
			store = new Store(storeName, founder, new PurchasePolicy(), new MarketWeb.Server.Domain.PolicyPackage.DiscountPolicy());
			basket = new ShoppingBasket(store);
			item = new Item(1, itemName, price, "desc", category);
		}

		[TestMethod]
		public void GetTotalDiscount_noCondition_success()
		{
			double amount_to_subtract = 2;
			double percents_to_subtract = 30;
			int amount = 10;
			NumericDiscount dis1 = new NumericDiscount(amount_to_subtract, expiration);
			ItemDiscount dis2 = new ItemDiscount(percents_to_subtract, itemName, expiration);
			List<Discount> disList = new List<Discount>();
			disList.Add(dis1);
			disList.Add(dis2);
			PlusDiscount plusDis = new PlusDiscount(disList);
			store.AddDiscount(plusDis);
			basket.AddItem(item, amount);

			//act
			double currPrice = basket.GetTotalPrice();
			double expected = currPrice - plusDis.calcPriceFromCurrPrice(basket, currPrice); 
			double actual = plusDis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(amount_to_subtract + (amount * price - amount_to_subtract) * percents_to_subtract / 100, expected);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetTotalDiscount_withCondition_success()
		{
			double amount_to_subtract = 2;
			double percents_to_subtract = 30;
			int amount = 10;
			Item item2 = new Item(2, "2", price, "desc", "other");
			ComposedCondition andCondition = new AndComposition(false);
			Condition categoryCondition = new SearchCategoryCondition(category, 0, 10, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(categoryCondition);
			andCondition.AddConditionToComposition(hourCondition);
			NumericDiscount dis1 = new NumericDiscount(amount_to_subtract, andCondition, expiration);
			ComposedCondition orCondition = new OrComposition(false);
			Condition dayOnWeekCond = new DayOnWeekCondition(DateTime.Today.AddDays(1).DayOfWeek.ToString(), false);
			Condition priceCond = new PriceableCondition(null, 10, -1, false);
			orCondition.AddConditionToComposition(dayOnWeekCond);
			orCondition.AddConditionToComposition(priceCond);
			AllProductsDiscount dis2 = new AllProductsDiscount(percents_to_subtract, orCondition, expiration);
			List<Discount> disList = new List<Discount>();
			disList.Add(dis1);
			disList.Add(dis2);
			PlusDiscount plusDis = new PlusDiscount(disList);
			store.AddDiscount(plusDis);
			basket.AddItem(item, amount);
			basket.AddItem(item2, 20);

			//act
			double currPrice = basket.GetTotalPrice();
			double expected = currPrice - plusDis.calcPriceFromCurrPrice(basket, currPrice);
			double actual = plusDis.GetTotalDiscount(basket);

			//Assert
			Assert.IsTrue(Math.Abs(amount_to_subtract + (basket.GetTotalPrice() - amount_to_subtract) * percents_to_subtract / 100 - expected) < 0.0000001);
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void GetTotalDiscount2_withTrueConditionAndFalse_success()
		{
			double amount_to_subtract = 2;
			double percents_to_subtract = 30;
			int amount = 10;
			Item item2 = new Item(2, "2", price, "desc", "other");
			ComposedCondition andCondition = new AndComposition(false);
			Condition categoryCondition = new SearchCategoryCondition(category, 0, 10, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(categoryCondition);
			andCondition.AddConditionToComposition(hourCondition);
			NumericDiscount dis1 = new NumericDiscount(amount_to_subtract, andCondition, expiration);
			ComposedCondition orCondition = new OrComposition(false);
			Condition dayOnWeekCond = new DayOnWeekCondition(DateTime.Today.AddDays(1).DayOfWeek.ToString(), false);
			Condition priceCond = new PriceableCondition(null, 50, -1, false);
			orCondition.AddConditionToComposition(dayOnWeekCond);
			orCondition.AddConditionToComposition(priceCond);
			AllProductsDiscount dis2 = new AllProductsDiscount(percents_to_subtract, orCondition, expiration);
			List<Discount> disList = new List<Discount>();
			disList.Add(dis1);
			disList.Add(dis2);
			PlusDiscount plusDis = new PlusDiscount(disList);
			store.AddDiscount(plusDis);
			basket.AddItem(item, amount);
			basket.AddItem(item2, 20);

			//act
			double expected = dis1.GetTotalDiscount(basket);
			double actual = plusDis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(amount_to_subtract, expected);
			Assert.AreEqual(expected, actual);
		}
		[TestMethod]
		public void GetTotalDiscount_withCondition_fail()
		{
			double amount_to_subtract = 2;
			double percents_to_subtract = 30;
			int amount = 10;
			Item item2 = new Item(2, "2", price, "desc", "other");
			ComposedCondition andCondition = new AndComposition(false);
			Condition categoryCondition = new SearchCategoryCondition(category, 0, 10, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			ComposedCondition orCondition = new OrComposition(false);
			Condition dayOnWeekCond = new DayOnWeekCondition(DateTime.Today.AddDays(1).DayOfWeek.ToString(), false);
			Condition priceCond = new PriceableCondition(null, 50, -1, false);
			orCondition.AddConditionToComposition(dayOnWeekCond);
			orCondition.AddConditionToComposition(priceCond);
			andCondition.AddConditionToComposition(categoryCondition);
			andCondition.AddConditionToComposition(hourCondition);
			andCondition.AddConditionToComposition(orCondition);
			NumericDiscount dis1 = new NumericDiscount(amount_to_subtract, expiration);
			AllProductsDiscount dis2 = new AllProductsDiscount(percents_to_subtract, expiration);
			List<Discount> disList = new List<Discount>();
			disList.Add(dis1);
			disList.Add(dis2);
			PlusDiscount plusDis = new PlusDiscount(disList, andCondition);
			store.AddDiscount(plusDis);
			basket.AddItem(item, amount);
			basket.AddItem(item2, 20);

			//act
			double expected = 0;
			double actual = plusDis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
