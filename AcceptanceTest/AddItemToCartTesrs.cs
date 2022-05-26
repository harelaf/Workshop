using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketProject.Service;
using MarketProject.Service.DTO;
using System.Collections.Generic;
using System;

namespace AcceptanceTest
{
    [TestClass]
    public class AddItemToCartTesrs
    {
        MarketAPI marketAPI = new MarketAPI();
        string storeName_inSystem = "afik's Shop";
        string storeName_outSystem = "bla";
        string guest_VisitorToken;
        string registered_VisitorToken;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemID_outStock = 1111111;

        [TestInitialize()]
        public void setup()
        {
            DateTime dob = new DateTime(2001, 7, 30);
            guest_VisitorToken = (marketAPI.EnterSystem()).Value;
            registered_VisitorToken = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(registered_VisitorToken, "afik", "123456789", dob);
            registered_VisitorToken = (marketAPI.Login(registered_VisitorToken, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(registered_VisitorToken, storeName_inSystem);
            itemID_inStock_1 = 1; itemAmount_inSttock_1 = 20;
            itemID_inStock_2 = 2; itemAmount_inSttock_2 = 50;
            marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem, itemID_inStock_1,
                "banana", 3.5, "", "fruit", itemAmount_inSttock_1);
            marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem, itemID_inStock_2,
                "banana2", 3.5, "", "fruit", itemAmount_inSttock_2);

        }

        [TestMethod]
        public void TestAddItemToCart_2VisitorsAddingLastItemInStock()
        {
            int iterations = 10000;
            marketAPI.UpdateStockQuantityOfItem(registered_VisitorToken, storeName_inSystem, itemID_inStock_1, iterations);
            Thread thread1 = new Thread(() => {
                for (int i = 0; i < iterations; i++)
                    marketAPI.AddItemToCart(registered_VisitorToken, itemID_inStock_1, storeName_inSystem, 1);
            });
            Thread thread2 = new Thread(() => {
                for (int i = 0; i < iterations; i++)
                    marketAPI.AddItemToCart(guest_VisitorToken, itemID_inStock_1, storeName_inSystem, 1);
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            int totalAmountInCarts = 0;
            Response<ShoppingCartDTO> r_1 = marketAPI.ViewMyCart(guest_VisitorToken);
            Response<ShoppingCartDTO> r_2 = marketAPI.ViewMyCart(registered_VisitorToken);
            if (r_1.ErrorOccured)
            {
                Assert.Fail(r_1.ErrorMessage);
            }
            if (r_2.ErrorOccured)
            {
                Assert.Fail(r_2.ErrorMessage);
            }
            ShoppingCartDTO Visitor1Cart = r_1.Value;
            ShoppingCartDTO Visitor2Cart = r_2.Value;

            totalAmountInCarts = Visitor1Cart.getAmountOfItemInCart(storeName_inSystem, 1) +
                Visitor2Cart.getAmountOfItemInCart(storeName_inSystem, 1);

            Assert.AreEqual(iterations, totalAmountInCarts);
            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            Assert.IsNotNull(store);
            IDictionary<ItemDTO, int> stock = store.Stock.Items;
            foreach (ItemDTO item in stock.Keys)
            {
                if (item.ItemID == itemID_inStock_1)
                {
                    Assert.AreEqual(stock[item], 0);
                    break;
                }
            }

        }

        [TestMethod]
        public void TestAddItemToCart_storeNotExists()
        {
            Response response_reg = marketAPI.AddItemToCart(registered_VisitorToken, itemID_inStock_1, storeName_outSystem, 10);
            Response response_guest = marketAPI.AddItemToCart(guest_VisitorToken, itemID_inStock_2, storeName_outSystem, 10);
            if (!response_guest.ErrorOccured || !response_reg.ErrorOccured)
                Assert.Fail("should've faild since there is no such store");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("no item should have been added to cart : since there is no such store");
        }

        [TestMethod]
        public void TestAddItemToCart_itemNotExists()
        {
            Response response_reg = marketAPI.AddItemToCart(registered_VisitorToken, itemID_outStock, storeName_inSystem, 10);
            Response response_guest = marketAPI.AddItemToCart(guest_VisitorToken, itemID_outStock, storeName_inSystem, 10);
            if (!response_guest.ErrorOccured || !response_reg.ErrorOccured)
                Assert.Fail("should've faild since there is no such item");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("no item should have been added to cart : since there is no such item");
        }

        [TestMethod]
        public void TestAddItemToCart_itemAmountNotInStock()
        {
            Response response_reg = marketAPI.AddItemToCart(registered_VisitorToken, itemID_inStock_1, storeName_inSystem, itemAmount_inSttock_1 + 10);
            Response response_guest = marketAPI.AddItemToCart(guest_VisitorToken, itemID_inStock_2, storeName_inSystem, itemAmount_inSttock_2 + 10);
            if (!response_guest.ErrorOccured || !response_reg.ErrorOccured)
                Assert.Fail("should've faild since there is not enought items in stock");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("no item should have been added to cart : since there is not enought items in stock");
        }

        [TestMethod]
        public void TestAddItemToCart_addingSuccses()
        {
            int reg_amount_to_add = itemAmount_inSttock_1 - 2;
            int guest_amount_to_add = itemAmount_inSttock_2 - 2;

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_reg_add = getAmountOfItemInStock(itemID_inStock_1, store.Stock);
            int amount_of_item_before_guest_add = getAmountOfItemInStock(itemID_inStock_2, store.Stock);
            
            Response response_reg = marketAPI.AddItemToCart(registered_VisitorToken, itemID_inStock_1, storeName_inSystem,reg_amount_to_add );
            Response response_guest = marketAPI.AddItemToCart(guest_VisitorToken, itemID_inStock_2, storeName_inSystem, guest_amount_to_add);

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_reg_add = getAmountOfItemInStock(itemID_inStock_1, store.Stock);
            int amount_of_item_agter_guest_add = getAmountOfItemInStock(itemID_inStock_2, store.Stock);

            if (response_guest.ErrorOccured || response_reg.ErrorOccured)
                Assert.Fail("reg_err: " + response_reg.ErrorMessage + " ;  guest_errL " + response_guest.ErrorMessage);
            
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            if (cart_guest.Baskets.Count < 0 || cart_registered.Baskets.Count < 0)
                Assert.Fail(" item should have been added to cart ");
            
            Assert.AreEqual(reg_amount_to_add, cart_registered.getAmountOfItemInCart(storeName_inSystem, itemID_inStock_1));
            Assert.AreEqual(guest_amount_to_add, cart_guest.getAmountOfItemInCart(storeName_inSystem, itemID_inStock_2));
            Assert.AreEqual(amount_of_item_after_reg_add, amount_of_item_before_reg_add - reg_amount_to_add);
            Assert.AreEqual(amount_of_item_agter_guest_add, amount_of_item_before_guest_add - guest_amount_to_add);
        }
        
        private int getAmountOfItemInStock(int itemID, StockDTO stock)
        {
            foreach (ItemDTO item in stock.Items.Keys)
            {
                if (itemID == item.ItemID)
                    return stock.Items[item];
            }
            return 0;
        }
        
    }
}
