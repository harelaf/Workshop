using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain;
using MarketWeb.Server.Domain.PolicyPackage;

namespace MarketProjectTests.Domain.PurchasePackage.DiscountPolicy.DiscountCalculators.AtomicCalculators
{
    [TestClass]
    public class NumericDiscountTest
    {
		String category = "cakes";
		String itemName = "cookie";
		double price = 100;
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
			store = new Store(storeName, founder, new PurchasePolicy(), new MarketWeb.Server.Domain.PolicyPackage.DiscountPolicy());
			basket = new ShoppingBasket(store);
			item = new Item(1, itemName, price, "desc", category);
		}

		[TestMethod]
		public void GetTotalDiscount_noCondition_success()
		{
			double amount_to_subtract = 10;
			int amount = 2;
			NumericDiscount dis = new NumericDiscount(amount_to_subtract, null, expiration);
			store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			double expected = amount_to_subtract;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(amount * price - amount_to_subtract, basket.GetTotalPrice() - basket.GetAdditionalDiscountsPrice());
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetTotalDiscount_withCondition_success()
		{
			double amount_to_subtract = 10;
			int amount = 2;
			ComposedCondition andCondition = new OrComposition(false);
			Condition categoryCondition = new SearchCategoryCondition(category, 0, 10, false);
			Condition hourCondition = new HourCondition((DateTime.Now.Hour + 23) % 24, (DateTime.Now.Hour + 1) % 24, false);
			andCondition.AddConditionToComposition(categoryCondition);
			andCondition.AddConditionToComposition(hourCondition);
			NumericDiscount dis = new NumericDiscount(amount_to_subtract, andCondition, expiration);
			store.AddDiscount(dis);
			basket.AddItem(item, amount);

			//act
			double expected = amount_to_subtract;
			double actual = dis.GetTotalDiscount(basket);

			//Assert
			Assert.AreEqual(amount * price - amount_to_subtract, basket.GetTotalPrice() - basket.GetAdditionalDiscountsPrice());
			Assert.AreEqual(expected, actual);
		}
	}
}
