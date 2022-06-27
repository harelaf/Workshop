using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using System.Collections.Generic;
using System;

namespace AcceptanceTest
{

    [TestClass]
    public class PurchaseTest
    {
        MarketAPI marketAPI = new MarketAPI(null, null);
        string storeName_inSystem = "afik's Shop";
        string guest_VisitorToken;
        string registered_VisitorToken;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemId_inRegCart;
        int itemId_inGuestCart;
        int itemAmount_inRegCart = 10;
        int itemAmount_inGuestCart = 10;
        DateTime dob = new DateTime(2001, 7, 30);
        public static readonly string shippingMethode_mock_false = "mock_false";
        public static readonly string shippingMethode_mock_true = "mock_true";
        public static readonly string paymentMethode_mock_false = "mock_false";
        public static readonly string paymentMethode_mock_true = "mock_true";

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

        [TestInitialize()]
        public void setup()
        {
            guest_VisitorToken = (marketAPI.EnterSystem()).Value;
            registered_VisitorToken = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(registered_VisitorToken, "afik", "123456789", dob);
            registered_VisitorToken = (marketAPI.Login(registered_VisitorToken, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(registered_VisitorToken, storeName_inSystem);
            itemAmount_inSttock_1 = 20;
            itemAmount_inSttock_2 = 50;
            itemID_inStock_1 = marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem,
                "banana", 3.5, "", "fruit", itemAmount_inSttock_1 + 10).Value;
            itemID_inStock_2 = marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem,
                "banana2", 3.5, "", "fruit", itemAmount_inSttock_2 + 10).Value;
            itemId_inRegCart = itemID_inStock_1;
            itemId_inGuestCart = itemID_inStock_2;
        }

        [TestMethod]
        public void TestPurchaseCart_CartIsEmpty()
        {
            Response guest_res = marketAPI.PurchaseMyCart(guest_VisitorToken, "", "", "", "", "shlomi",paymentMethode_mock_true, shippingMethode_mock_true).Result;
            if (!guest_res.ErrorOccured)
                Assert.Fail("should've faild: can't purchase empty cart");
        }

        [TestMethod]
        public void Test_happy_Purchase_Guest()
        {
            marketAPI.AddItemToCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart);
            Response guest_res = marketAPI.PurchaseMyCart(guest_VisitorToken, "", "", "", "", "shlomi",paymentMethode_mock_true, shippingMethode_mock_true).Result;
            if (guest_res.ErrorOccured)
                Assert.Fail("should'nt faild");
            //check recored added in store history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            Assert.IsNotNull(store_purchasedBasket);
            Assert.AreEqual(1, store_purchasedBasket.Count);
            foreach (Tuple<DateTime, ShoppingBasketDTO> item in store_purchasedBasket)
            {
                ShoppingBasketDTO shoppingBasketDTO = item.Item2;
                if (shoppingBasketDTO.StoreName == storeName_inSystem)
                {
                    IDictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> items = shoppingBasketDTO.Items;
                    int item_num = 0;
                    foreach (int itemId in items.Keys)
                    {
                        if (itemId == itemId_inGuestCart)
                        {
                            Assert.AreEqual(itemAmount_inGuestCart, items[itemId].Item2.Amount);
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
        public void Test_sad_Purchase_Guest_shippmentFaild()
        {
            marketAPI.AddItemToCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart);

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_guest_faild_purchase = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            Response guest_res = marketAPI.PurchaseMyCart(guest_VisitorToken, "", "", "", "", "shlomi", paymentMethode_mock_true, shippingMethode_mock_false).Result;
            if (!guest_res.ErrorOccured)
                Assert.Fail("should faild");

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_guest_faild_purchase = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            //check there was no  recored added in store  history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            if (store_purchasedBasket != null)
                Assert.AreEqual(0, store_purchasedBasket.Count);
           
            //check if stock was refilled:
            Assert.AreEqual(amount_of_item_after_guest_faild_purchase, amount_of_item_before_guest_faild_purchase+ itemAmount_inGuestCart);

        }

        [TestMethod]
        public void Test_sad_Purchase_Guest_paymentFaild()
        {
            marketAPI.AddItemToCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart);

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_guest_faild_purchase = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            Response guest_res = marketAPI.PurchaseMyCart(guest_VisitorToken, "", "", "", "", "shlomi", paymentMethode_mock_false, shippingMethode_mock_true).Result;
            if (!guest_res.ErrorOccured)
                Assert.Fail("should faild");

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_guest_faild_purchase = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            //check there was no  recored added in store  history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            if (store_purchasedBasket != null)
                Assert.AreEqual(0, store_purchasedBasket.Count);

            //check if stock was refilled:
            Assert.AreEqual(amount_of_item_after_guest_faild_purchase, amount_of_item_before_guest_faild_purchase + itemAmount_inGuestCart);

        }

        [TestMethod]
        public void Test_sad_Purchase_Guest_bothPaymentNShippmentFaild()
        {
            marketAPI.AddItemToCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart);

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_guest_faild_purchase = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            Response guest_res = marketAPI.PurchaseMyCart(guest_VisitorToken, "", "", "", "", "shlomi", paymentMethode_mock_false, shippingMethode_mock_false).Result;
            if (!guest_res.ErrorOccured)
                Assert.Fail("should faild");

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_guest_faild_purchase = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            //check there was no  recored added in store  history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            if (store_purchasedBasket != null)
                Assert.AreEqual(0, store_purchasedBasket.Count);

            //check if stock was refilled:
            Assert.AreEqual(amount_of_item_after_guest_faild_purchase, amount_of_item_before_guest_faild_purchase + itemAmount_inGuestCart);

        }


        [TestMethod]
        public void Test_happy_Purchase_Registred()
        {
            marketAPI.AddItemToCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart);
            Response guest_res = marketAPI.PurchaseMyCart(registered_VisitorToken, "", "", "", "", "shlomi",paymentMethode_mock_true, shippingMethode_mock_true).Result;
            if (guest_res.ErrorOccured)
                Assert.Fail("should'nt faild");
            //check recored added in store history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            Assert.IsNotNull(store_purchasedBasket);
            Assert.AreEqual(1, store_purchasedBasket.Count);
            foreach (Tuple<DateTime, ShoppingBasketDTO> item in store_purchasedBasket)
            {
                ShoppingBasketDTO shoppingBasketDTO = item.Item2;
                if (shoppingBasketDTO.StoreName == storeName_inSystem)
                {
                    IDictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> items = shoppingBasketDTO.Items;
                    int item_num = 0;
                    foreach (int itemId in items.Keys)
                    {
                        if (itemId == itemId_inRegCart)
                        {
                            Assert.AreEqual(itemAmount_inRegCart, items[itemId].Item2.Amount);
                            item_num++;
                            break;
                        }
                    }
                    Assert.AreEqual(1, item_num);
                    break;
                }
            }
            //check recored added in Visitor history:
            List<Tuple<DateTime,ShoppingCartDTO>> Visitor_history = marketAPI.GetMyPurchasesHistory(registered_VisitorToken).Value;
            foreach (Tuple<DateTime, ShoppingCartDTO> purchasedCartDTO in Visitor_history)
            {
                if (purchasedCartDTO.Item1.Date== DateTime.Now.Date)//purchased now
                {
                    Assert.AreEqual(1, purchasedCartDTO.Item2.Baskets.Count);

                    foreach(ShoppingBasketDTO shoppingBasketDTO in purchasedCartDTO.Item2.Baskets)
                    {
                        if (shoppingBasketDTO.StoreName == storeName_inSystem)
                        {
                            IDictionary<int, Tuple<ItemDTO, DiscountDetailsDTO>> items = shoppingBasketDTO.Items;
                            int item_num = 0;
                            foreach (int itemId in items.Keys)
                            {
                                if (itemId == itemId_inRegCart)
                                {
                                    Assert.AreEqual(itemAmount_inRegCart, items[itemId].Item2.Amount);
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

        [TestMethod]
        public void Test_sad_Purchase_Register_shippmentFaild()
        {
            marketAPI.AddItemToCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart);

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_reg_faild_purchase = getAmountOfItemInStock(itemId_inRegCart, store.Stock);

            Response reg_res = marketAPI.PurchaseMyCart(registered_VisitorToken, "", "", "", "", "shlomi", paymentMethode_mock_true, shippingMethode_mock_false).Result;
            if (!reg_res.ErrorOccured)
                Assert.Fail("should faild");

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_reg_faild_purchase = getAmountOfItemInStock(itemId_inRegCart, store.Stock);

            //check there was no  recored added in store  history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            if (store_purchasedBasket != null)
                Assert.AreEqual(0, store_purchasedBasket.Count);

            //check there was recored added in Visitor history:
            List<Tuple<DateTime, ShoppingCartDTO>> Visitor_history = marketAPI.GetMyPurchasesHistory(registered_VisitorToken).Value;
            if(Visitor_history != null)
                Assert.AreEqual(0, Visitor_history.Count);
            //check if stock was refilled:
            Assert.AreEqual(amount_of_item_after_reg_faild_purchase, amount_of_item_before_reg_faild_purchase + itemAmount_inRegCart);

        }

        [TestMethod]
        public void Test_sad_Purchase_Register_paymentFaild()
        {
            marketAPI.AddItemToCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart);

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_reg_faild_purchase = getAmountOfItemInStock(itemId_inRegCart, store.Stock);

            Response reg_res = marketAPI.PurchaseMyCart(registered_VisitorToken, "", "", "", "", "shlomi", paymentMethode_mock_false, shippingMethode_mock_true).Result;
            if (!reg_res.ErrorOccured)
                Assert.Fail("should faild");

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_reg_faild_purchase = getAmountOfItemInStock(itemId_inRegCart, store.Stock);

            //check there was no  recored added in store  history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            if (store_purchasedBasket != null)
                Assert.AreEqual(0, store_purchasedBasket.Count);

            //check there was recored added in Visitor history:
            List<Tuple<DateTime, ShoppingCartDTO>> Visitor_history = marketAPI.GetMyPurchasesHistory(registered_VisitorToken).Value;
            if(Visitor_history != null)
                Assert.AreEqual(0, Visitor_history.Count);
            //check if stock was refilled:
            Assert.AreEqual(amount_of_item_after_reg_faild_purchase, amount_of_item_before_reg_faild_purchase + itemAmount_inRegCart);

        }

        [TestMethod]
        public void Test_sad_Purchase_Register_bothPaymentNShippmentFaild()
        {
            marketAPI.AddItemToCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart);

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_reg_faild_purchase = getAmountOfItemInStock(itemId_inRegCart, store.Stock);

            Response reg_res = marketAPI.PurchaseMyCart(registered_VisitorToken, "", "", "", "", "shlomi", paymentMethode_mock_false, shippingMethode_mock_false).Result;
            if (!reg_res.ErrorOccured)
                Assert.Fail("should faild");

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_reg_faild_purchase = getAmountOfItemInStock(itemId_inRegCart, store.Stock);

            //check there was no  recored added in store  history:
            ICollection<Tuple<DateTime, ShoppingBasketDTO>> store_purchasedBasket = marketAPI.GetStorePurchasesHistory(registered_VisitorToken, storeName_inSystem).Value;
            if (store_purchasedBasket != null)
                Assert.AreEqual(0, store_purchasedBasket.Count);

            //check there was recored added in Visitor history:
            List<Tuple<DateTime, ShoppingCartDTO>> Visitor_history = marketAPI.GetMyPurchasesHistory(registered_VisitorToken).Value;
            if(Visitor_history != null)
                Assert.AreEqual(0, Visitor_history.Count);
            //check if stock was refilled:
            Assert.AreEqual(amount_of_item_after_reg_faild_purchase, amount_of_item_before_reg_faild_purchase + itemAmount_inRegCart);

        }

        private int getAmountOfItemInStock(int itemID, StockDTO stock)
        {
            Dictionary<int, Tuple<ItemDTO, int>> dic_stock = stock.Items;
            foreach (int itemid in dic_stock.Keys)
            {
                ItemDTO item = dic_stock[itemid].Item1;
                if (itemID == item.ItemID)
                    return stock.Items[itemid].Item2;
            }
            return 0;
        }
    }
}
