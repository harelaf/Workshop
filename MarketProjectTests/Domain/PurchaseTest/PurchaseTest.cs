using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System;
using MarketProject.Domain.PurchasePackage.PolicyPackage;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class UnitTestPurchase
    {
        private Mock<PaymentHandlerProxy> paymentProxyMoq;
        private Mock<ShippingHandlerProxy> shippingProxyMoq;
        private PurchaseProcess purchase;
        private Mock<ShoppingCart> cartMoq;
        private Mock<ShoppingBasket> basketMoq1;
        private Mock<ShoppingBasket> basketMoq2;
        private Mock<DiscountPolicy> discountMoq;
        private Mock<PurchasePolicy> purchaseMoq;
        private Mock<Store> storeMoq1;
        private Mock<Store> storeMoq2;
        private ICollection<ShoppingBasket> shoppingBaskets;
        private ICollection<Item> items1;
        private ICollection<Item> items2;
        private double cartPrice;

        [TestInitialize]
        public void setUp()
        {
            paymentProxyMoq = new Mock<PaymentHandlerProxy>();
            shippingProxyMoq = new Mock<ShippingHandlerProxy>();
            purchase = PurchaseProcess.GetInstance();
            cartMoq = new Mock<ShoppingCart>();
            discountMoq = new Mock<DiscountPolicy>();
            purchaseMoq = new Mock<PurchasePolicy>();
            storeMoq1 = new Mock<Store>("1", null, purchaseMoq.Object, discountMoq.Object);
            storeMoq2 = new Mock<Store>("2", null, purchaseMoq.Object, discountMoq.Object);
            storeMoq1.Setup(x => x.GetDiscountPolicy()).Returns(discountMoq.Object);
            storeMoq1.Setup(x => x.GetPurchasePolicy()).Returns(purchaseMoq.Object);
            storeMoq2.Setup(x => x.GetDiscountPolicy()).Returns(discountMoq.Object);
            storeMoq2.Setup(x => x.GetPurchasePolicy()).Returns(purchaseMoq.Object);
            basketMoq1 = new Mock<ShoppingBasket>(storeMoq1.Object);
            basketMoq2 = new Mock<ShoppingBasket>(storeMoq2.Object);
            basketMoq1.Setup(x => x.Store()).Returns(storeMoq1.Object);
            basketMoq2.Setup(x => x.Store()).Returns(storeMoq2.Object);

            shoppingBaskets = new List<ShoppingBasket>();
            items1 = new List<Item>();
            items2 = new List<Item>();
            purchase.SetInstance(paymentProxyMoq.Object, shippingProxyMoq.Object);
            shoppingBaskets.Add(basketMoq1.Object);
            shoppingBaskets.Add(basketMoq2.Object);
            cartMoq.Setup(x => x._shoppingBaskets).Returns(shoppingBaskets);
            double price1 = 0;
            for (int i = 1; i < 10; i++)
            {
                Mock<Item> itemMoq = new Mock<Item>(i, "item_" + i, 1.5, "b", "b");
                items1.Add(itemMoq.Object);
                itemMoq.Setup(x => x._price).Returns(i + 0.5);
                price1 += i + 0.5;
            }
            basketMoq1.Setup(x => x.GetItems()).Returns(items1);
            basketMoq1.Setup(x => x.GetAmountOfItem(It.IsAny<Item>())).Returns(5);
            price1 *= 5;// each item amount = 5
            double price2 = 0;
            for (int i = 10; i < 20; i++)
            {
                Mock<Item> itemMoq = new Mock<Item>(i, "item_" + i, 1.5, "b", "b");
                items2.Add(itemMoq.Object);
                itemMoq.Setup(x => x._price).Returns(i + 0.5);
                price2 += i + 0.5;
            }
            basketMoq2.Setup(x => x.GetItems()).Returns(items2);
            basketMoq1.Setup(x => x.GetAmountOfItem(It.IsAny<Item>())).Returns(10);
            price2 *= 5;// each item amount = 10
            cartPrice = price1 + price2;
            cartMoq.Setup(x => x.RelaseItemsOfCart());
        }

        [TestMethod]
        public void TestPurchase_PaymentNShippingAproved()
        {
            //arrange
            paymentProxyMoq.Setup(x => x.Pay(It.IsAny<double>(), It.IsAny<string>())).Returns(true);
            shippingProxyMoq.Setup(x => x.ShippingApproval(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            discountMoq.Setup(x => x.calculateDiscounts(It.IsAny<ShoppingBasket>())).Returns(0);
            //action
            //calculateDiscounts
            try
            {
                purchase.Purchase("adr","", "", "","", cartMoq.Object, "", "");
            }
            catch (Exception ex) { Assert.Fail(ex.Message); }
        }

        [TestMethod]
        public void TestPurchase_PaymentAproved_ShippingNot()
        {
            //arrange
            setUp();
            paymentProxyMoq.Setup(x => x.Pay(It.IsAny<double>(), It.IsAny<string>())).Returns(true);
            shippingProxyMoq.Setup(x => x.ShippingApproval(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            //action
            try
            {
                purchase.Purchase("adr","", "", "", " ", cartMoq.Object,"","");
                Assert.Fail();
            }
            catch (Exception ex) { }

        }
        [TestMethod]
        public void TestPurchase_ShippingAproved_PaymentNot()
        {
            //arrange
            paymentProxyMoq.Setup(x => x.Pay(It.IsAny<double>(), It.IsAny<string>())).Returns(false);
            shippingProxyMoq.Setup(x => x.ShippingApproval(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            //action
            try
            {
                purchase.Purchase("adr", "", "", "", "", cartMoq.Object,"", "");
                Assert.Fail();
            }
            catch (Exception ex) { }

        }
        [TestMethod]
        public void TestPurchase_ShippingNPaymentNotAproved()
        {
            //arrange
            paymentProxyMoq.Setup(x => x.Pay(It.IsAny<double>(), It.IsAny<string>())).Returns(false);
            shippingProxyMoq.Setup(x => x.ShippingApproval(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            //action
            try
            {
                purchase.Purchase("adr","", "", "", "", cartMoq.Object,"","");
                Assert.Fail();
            }
            catch (Exception ex) { }

        }
    }
}