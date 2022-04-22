using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketProject.Service;
using MarketProject.Service.DTO;
using System.Collections.Generic;
using System;

namespace AcceptanceTest
{

    [TestClass]
    public class PurchaseTest
    {
        MarketAPI marketAPI = new MarketAPI();
        string storeName_inSystem = "afik's Shop";
        string guest_userToken;
        string registered_userToken;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemId_inRegCart;
        int itemId_inGuestCart;
        int itemAmount_inRegCart = 10;
        int itemAmount_inGuestCart = 10;

        [TestInitialize()]
        public void setup()
        {
            guest_userToken = (marketAPI.EnterSystem()).Value;
            registered_userToken = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(registered_userToken, "afik", "123456789");
            registered_userToken = (marketAPI.Login(registered_userToken, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(registered_userToken, storeName_inSystem);
            itemID_inStock_1 = 1; itemAmount_inSttock_1 = 20;
            itemID_inStock_2 = 2; itemAmount_inSttock_2 = 50;
            marketAPI.AddItemToStoreStock(registered_userToken, storeName_inSystem, itemID_inStock_1,
                "banana", 3.5, "", "fruit", itemAmount_inSttock_1 + 10);
            marketAPI.AddItemToStoreStock(registered_userToken, storeName_inSystem, itemID_inStock_2,
                "banana2", 3.5, "", "fruit", itemAmount_inSttock_2 + 10);
            itemId_inRegCart = itemID_inStock_1;
            itemId_inGuestCart = itemID_inStock_2;
        }

        [TestMethod]
        public void TestPurchaseCart_CartIsEmpty()
        {
            Response guest_res = marketAPI.PurchaseMyCart(guest_userToken, "", "", "", "", "shlomi");
            if (!guest_res.ErrorOccured)
                Assert.Fail("should've faild: can't purchase empty cart");
        }

        [TestMethod]
        public void TestPurchase_CartNonEmptyGuest()
        {
            marketAPI.AddItemToCart(guest_userToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart);
            Response guest_res = marketAPI.PurchaseMyCart(guest_userToken, "", "", "", "", "shlomi");
            if (guest_res.ErrorOccured)
                Assert.Fail("should'nt faild");
            //check recored added in history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_userToken, storeName_inSystem).Value;
            Assert.IsNotNull(store_purchasedBasket);
            Assert.AreEqual(1, store_purchasedBasket.Count);
            foreach (Tuple<DateTime, ShoppingBasketDTO> item in store_purchasedBasket)
            {
                ShoppingBasketDTO shoppingBasketDTO = item.Item2;
                if (shoppingBasketDTO.StoreName== storeName_inSystem)
                {
                    IDictionary<ItemDTO, int> items = shoppingBasketDTO.Items;
                    int item_num = 0;
                    foreach( KeyValuePair<ItemDTO, int> p in items)
                    {
                        if (p.Key.ItemID== itemId_inGuestCart)
                        {
                            Assert.AreEqual(itemAmount_inGuestCart, p.Value);
                            item_num++;
                            break;
                        }
                    }
                    Assert.AreEqual(1, item_num);
                    break;
                }
            }

        }
        [TestMethod]
        public void TestPurchase_CartNonEmptyRegistred()
        {
            marketAPI.AddItemToCart(registered_userToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart);
            Response guest_res = marketAPI.PurchaseMyCart(registered_userToken, "", "", "", "", "shlomi");
            if (guest_res.ErrorOccured)
                Assert.Fail("should'nt faild");
            //check recored added in store history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_userToken, storeName_inSystem).Value;
            Assert.IsNotNull(store_purchasedBasket);
            Assert.AreEqual(1, store_purchasedBasket.Count);
            foreach (Tuple<DateTime, ShoppingBasketDTO> item in store_purchasedBasket)
            {
                ShoppingBasketDTO shoppingBasketDTO = item.Item2;
                if (shoppingBasketDTO.StoreName == storeName_inSystem)
                {
                    IDictionary<ItemDTO, int> items = shoppingBasketDTO.Items;
                    int item_num = 0;
                    foreach (KeyValuePair<ItemDTO, int> p in items)
                    {
                        if (p.Key.ItemID == itemId_inRegCart)
                        {
                            Assert.AreEqual(itemAmount_inRegCart, p.Value);
                            item_num++;
                            break;
                        }
                    }
                    Assert.AreEqual(1, item_num);
                    break;
                }
            }
            //check recored added in user history:
            ICollection<PurchasedCartDTO> user_history = marketAPI.GetMyPurchasesHistory(registered_userToken).Value;
            foreach (PurchasedCartDTO purchasedCartDTO in user_history)
            {
                if (purchasedCartDTO.Date.Date== DateTime.Now.Date)//purchased now
                {
                    Assert.AreEqual(1, purchasedCartDTO.ShoppingCart.Baskets.Count);

                    foreach(ShoppingBasketDTO shoppingBasketDTO in purchasedCartDTO.ShoppingCart.Baskets)
                    {
                        if (shoppingBasketDTO.StoreName == storeName_inSystem)//
                        {
                            IDictionary<ItemDTO, int> items = shoppingBasketDTO.Items;
                            int item_num = 0;
                            foreach (KeyValuePair<ItemDTO, int> p in items)
                            {
                                if (p.Key.ItemID == itemId_inRegCart)
                                {
                                    Assert.AreEqual(itemAmount_inRegCart, p.Value);
                                    item_num++;
                                    break;
                                }
                            }
                            Assert.AreEqual(1, item_num);
                            break;
                        }
                        else
                            Assert.Fail("should br one baskrt in cart and it should be of store afik");
                    }
                }
            }
        }
    }
}
