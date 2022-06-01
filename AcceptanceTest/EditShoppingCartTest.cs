using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;
using System;
using MarketWeb.Server.Domain;
using MarketWeb.Service;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;

namespace AcceptanceTest
{
    [TestClass]
    public class EditShoppingCartTest
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

        [TestInitialize()]
        public void setup()
        {
            guest_VisitorToken = (marketAPI.EnterSystem()).Value;
            registered_VisitorToken = (marketAPI.EnterSystem()).Value;// guest
            marketAPI.Register(registered_VisitorToken, "afik", "123456789", dob);
            registered_VisitorToken = (marketAPI.Login(registered_VisitorToken, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(registered_VisitorToken, storeName_inSystem);
            itemID_inStock_1 = 1; itemAmount_inSttock_1 = 20;
            itemID_inStock_2 = 2; itemAmount_inSttock_2 = 50;
            marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem, itemID_inStock_1,
                "banana", 3.5, "", "fruit", itemAmount_inSttock_1 + 10);
            marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem, itemID_inStock_2,
                "banana2", 3.5, "", "fruit", itemAmount_inSttock_2 + 10);
            itemId_inRegCart = itemID_inStock_1;
            itemId_inGuestCart = itemID_inStock_2;
            marketAPI.AddItemToCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart);
            marketAPI.AddItemToCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart);
        }

        [TestMethod]
        public void Test_sad_UpdateQuantotyOfItemInCart_QuantityIsZero()
        {
            int quantity = 0;
            Response response_reg = marketAPI.UpdateQuantityOfItemInCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, quantity);
            Response response_guest = marketAPI.UpdateQuantityOfItemInCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, quantity);
            if (!response_reg.ErrorOccured || !response_guest.ErrorOccured)
                Assert.Fail("should've faild: new q = 0");

        }


        [TestMethod]
        public void Test_happy_RemoveItemFromCart_itemExistsInCart()//UPDATE Fail: remove item in basket
        {
           
            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_before_reg_remove = getAmountOfItemInStock(itemId_inRegCart, store.Stock);
            int amount_of_item_before_guest_remove = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            Response response_reg = marketAPI.RemoveItemFromCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem);
            Response response_guest = marketAPI.RemoveItemFromCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem);

            store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            int amount_of_item_after_reg_remove = getAmountOfItemInStock(itemId_inRegCart, store.Stock);
            int amount_of_item_after_guest_remove = getAmountOfItemInStock(itemId_inGuestCart, store.Stock);

            if (response_reg.ErrorOccured || response_guest.ErrorOccured)
                Assert.Fail(response_reg.ErrorMessage);
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("item removed from the only basket should coused basket to be removed");
            Assert.AreEqual(amount_of_item_after_guest_remove, amount_of_item_before_guest_remove + itemAmount_inGuestCart);
            Assert.AreEqual(amount_of_item_after_reg_remove, amount_of_item_before_reg_remove + itemAmount_inRegCart);

        }
        

        [TestMethod]
        public void Test_sad_RemoveItemFromCart_itemNotExistsInCart()//UPDATE FILE: remove item not in basket
        {
            Response response_reg = marketAPI.RemoveItemFromCart(registered_VisitorToken, itemId_inGuestCart, storeName_inSystem);
            Response response_guest = marketAPI.RemoveItemFromCart(guest_VisitorToken, itemId_inRegCart, storeName_inSystem);
            if (!response_reg.ErrorOccured || !response_guest.ErrorOccured)
                Assert.Fail("should've faild: item not in basket");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            if (cart_guest.Baskets.Count <= 0 || cart_registered.Baskets.Count <= 0)
                Assert.Fail("basket should not change in a faild remove");
        }


        [TestMethod]
        public void Test_sad_UpdateQuantotyOfItemInCart_IncreaseAmountInCart_NotEnoughtInStock()//UPDATE FILE: update amount not in stock
        {
            int toAdd = 100;
            Response response_reg = marketAPI.UpdateQuantityOfItemInCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inSttock_1 + toAdd);
            Response response_guest = marketAPI.UpdateQuantityOfItemInCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inSttock_2 + toAdd);
            if (!response_reg.ErrorOccured || !response_guest.ErrorOccured)
                Assert.Fail("should've faild: amount not in stock");
            //amount in stoer the same
            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            Assert.IsNotNull(store);
            Dictionary<int, Tuple<ItemDTO, int>> stock = store.Stock.Items;
            foreach (int itemid in stock.Keys)
            {

                if(itemid ==itemId_inRegCart)
                    Assert.AreEqual(stock[itemid].Item2, itemAmount_inSttock_1);
                if (itemid == itemAmount_inGuestCart)
                    Assert.AreEqual(stock[itemid].Item2, itemAmount_inSttock_2);
            }
            //anount in cart the same
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            Assert.AreEqual(itemAmount_inGuestCart, cart_guest.getAmountOfItemInCart(storeName_inSystem, itemId_inGuestCart));
            Assert.AreEqual(itemAmount_inRegCart, cart_registered.getAmountOfItemInCart(storeName_inSystem, itemId_inRegCart));
        }

        [TestMethod]
        public void Test_happy_UpdateQuantotyOfItemInCart_IncreaseAmountInCart_EnoughtInStock()//UPDATE FILE: update amount in stock-> increase
        {
            Response response_reg = marketAPI.UpdateQuantityOfItemInCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart+ itemAmount_inSttock_1);
            Response response_guest = marketAPI.UpdateQuantityOfItemInCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inRegCart+ itemAmount_inSttock_2);
            if (response_reg.ErrorOccured || response_guest.ErrorOccured)
                Assert.Fail("should've succseed: amount  in stock");
            //amount in stoer updated to zero
            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            Assert.IsNotNull(store);
            Dictionary<int, Tuple<ItemDTO, int>> stock = store.Stock.Items;
                int itemsInStore = 0;
            foreach (int itemid in stock.Keys)
            {
                if (itemid == itemId_inRegCart)
                {
                    itemsInStore++;
                    Assert.AreEqual(stock[itemid].Item2, 0);
                }
                if (itemid == itemId_inGuestCart)
                {
                    itemsInStore++;
                    Assert.AreEqual(stock[itemid].Item2, 0);
                }
            }
            Assert.AreEqual(itemsInStore, 2);
            //anount in cart changed
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            Assert.AreEqual(itemAmount_inGuestCart + itemAmount_inSttock_2, cart_guest.getAmountOfItemInCart(storeName_inSystem, itemId_inGuestCart));
            Assert.AreEqual(itemAmount_inRegCart + itemAmount_inSttock_1, cart_registered.getAmountOfItemInCart(storeName_inSystem, itemId_inRegCart));
        }

        [TestMethod]
        public void Test_happy_UpdateQuantotyOfItemInCart_DecreaseAmountInCart()//UPDATE FILE: update amoiunt in stock -> decrease
        {
            int to_decrease = 5;
            Response response_reg = marketAPI.UpdateQuantityOfItemInCart(registered_VisitorToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart - to_decrease);
            Response response_guest = marketAPI.UpdateQuantityOfItemInCart(guest_VisitorToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart - to_decrease);
            if (response_reg.ErrorOccured || response_guest.ErrorOccured)
                Assert.Fail("should've succseed ");
            //amount in stoer increased by toDecrease
            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            Assert.IsNotNull(store);
            IDictionary<int, Tuple<ItemDTO, int>> stock = store.Stock.Items;
            int itemsInStore = 0;
            foreach (int itemid in stock.Keys)
            {
                if (itemid == itemId_inRegCart)
                {
                    itemsInStore++;
                    Assert.AreEqual(stock[itemid].Item2, itemAmount_inSttock_1 + to_decrease);
                }
                if (itemid == itemId_inGuestCart)
                {
                    itemsInStore++;
                    Assert.AreEqual(stock[itemid].Item2, itemAmount_inSttock_2 + to_decrease);
                }
            }
            Assert.AreEqual(itemsInStore, 2);
            //anount in cart decrreased nt to_decreased
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_VisitorToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_VisitorToken).Value;
            Assert.AreEqual(itemAmount_inGuestCart - to_decrease, cart_guest.getAmountOfItemInCart(storeName_inSystem, itemId_inGuestCart));
            Assert.AreEqual(itemAmount_inRegCart - to_decrease, cart_registered.getAmountOfItemInCart(storeName_inSystem, itemId_inRegCart));
        }

        [TestMethod]
        public void TestUpdateQuantotyOfItemInCart_2VisitorsIncreaseAmountInCart()//UPDATE FILE: threads- both Visitors try to decrease amount of item-> last in stock
        {
            int iterations = 10000;
            int tot_amountInStock = iterations + 2; 
            int newItenID = 3;
            marketAPI.AddItemToStoreStock(registered_VisitorToken, storeName_inSystem, newItenID, "new", 53.3, "", "", tot_amountInStock);
            marketAPI.AddItemToCart(registered_VisitorToken, newItenID, storeName_inSystem, 1);
            marketAPI.AddItemToCart(guest_VisitorToken, newItenID, storeName_inSystem, 1);
            Thread thread1 = new Thread(() => {
                int cur_amount = 1;
                for (int i = 0; i < iterations; i++)
                {
                    cur_amount++;
                    marketAPI.UpdateQuantityOfItemInCart(registered_VisitorToken, newItenID, storeName_inSystem, cur_amount);
                }
                   
            });
            Thread thread2 = new Thread(() => {
                int cur_amount = 1;
                for (int i = 0; i < iterations; i++)
                {
                    cur_amount++;
                    marketAPI.UpdateQuantityOfItemInCart(guest_VisitorToken, newItenID, storeName_inSystem, cur_amount);
                }
            });

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            int totalAmountInCarts = 0;
            Response<ShoppingCartDTO> r_1 = marketAPI.ViewMyCart(guest_VisitorToken);
            Response<ShoppingCartDTO> r_2 = marketAPI.ViewMyCart(registered_VisitorToken);
            if (r_1.ErrorOccured || r_2.ErrorOccured)
                Assert.Fail("geust err: "+r_1.ErrorMessage+ " ; reg err: "+ r_2.ErrorMessage);
            ShoppingCartDTO Visitor1Cart = r_1.Value;
            ShoppingCartDTO Visitor2Cart = r_2.Value;

            totalAmountInCarts = Visitor1Cart.getAmountOfItemInCart(storeName_inSystem, newItenID) +
                Visitor2Cart.getAmountOfItemInCart(storeName_inSystem, newItenID);
            Assert.AreEqual(tot_amountInStock, totalAmountInCarts);

            StoreDTO store = marketAPI.GetStoreInformation(registered_VisitorToken, storeName_inSystem).Value;
            Assert.IsNotNull(store);
            IDictionary<int, Tuple<ItemDTO, int>> stock = store.Stock.Items;
            foreach (int itemid in stock.Keys)
            {
                if (itemid == newItenID)
                {
                    Assert.AreEqual(stock[itemid].Item2, 0);
                    break;
                }
            }

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
