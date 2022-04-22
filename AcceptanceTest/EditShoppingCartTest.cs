using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketProject.Service;
using MarketProject.Service.DTO;
namespace AcceptanceTest
{
    [TestClass]
    public class EditShoppingCartTest
    {
        MarketAPI marketAPI = new MarketAPI();
        string storeName_inSystem = "afik's Shop";
        string storeName_outSystem = "bla";
        string guest_userToken;
        string registered_userToken;
        int itemID_inStock_1;
        int itemAmount_inSttock_1;
        int itemID_inStock_2;
        int itemAmount_inSttock_2;
        int itemID_outStock = 1111111;
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
            marketAPI.AddItemToCart(registered_userToken, itemId_inRegCart, storeName_inSystem, itemAmount_inRegCart);
            marketAPI.AddItemToCart(guest_userToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inGuestCart);
        }

        [TestMethod]
        public void TestUpdateQuantotyOfItemInCart_QuantityIsZero()
        {
            int quantity = 0;
            Response response_reg = marketAPI.UpdateQuantityOfItemInCart(registered_userToken, itemId_inRegCart, storeName_inSystem, quantity);
            Response response_guest = marketAPI.UpdateQuantityOfItemInCart(guest_userToken, itemId_inGuestCart, storeName_inSystem, quantity);
            if (!response_reg.ErrorOccured || !response_guest.ErrorOccured)
                Assert.Fail("should've faild: new q = 0");

        }


        [TestMethod]
        public void TestRemoveItemFromCart_itemExistsInCart()//UPDATE FILE: remove item in basket
        {
            Response response_reg = marketAPI.RemoveItemFromCart(registered_userToken, itemId_inRegCart, storeName_inSystem);
            Response response_guest = marketAPI.RemoveItemFromCart(guest_userToken, itemId_inGuestCart, storeName_inSystem);
            if (response_reg.ErrorOccured || response_guest.ErrorOccured)
                Assert.Fail(response_reg.ErrorMessage);
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_userToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_userToken).Value;
            if (cart_guest.Baskets.Count > 0 || cart_registered.Baskets.Count > 0)
                Assert.Fail("item removed from the only basket should coused basket to be removed");
        }
        //update amount not in stock
        //update amount in stock-> increase
        // update amoiunt in stock -> decrease
        //threads: both users try to decrease amount of item-> last in stock

        [TestMethod]
        public void TestRemoveItemFromCart_itemNotExistsInCart()//UPDATE FILE: remove item not in basket
        {
            Response response_reg = marketAPI.RemoveItemFromCart(registered_userToken, itemId_inGuestCart, storeName_inSystem);
            Response response_guest = marketAPI.RemoveItemFromCart(guest_userToken, itemId_inRegCart, storeName_inSystem);
            if (!response_reg.ErrorOccured || !response_guest.ErrorOccured)
                Assert.Fail("should've faild: item not in basket");
            ShoppingCartDTO cart_guest = marketAPI.ViewMyCart(guest_userToken).Value;
            ShoppingCartDTO cart_registered = marketAPI.ViewMyCart(registered_userToken).Value;
            if (cart_guest.Baskets.Count <= 0 || cart_registered.Baskets.Count <= 0)
                Assert.Fail("basket should not change in a faild remove");
        }


        [TestMethod]
        public void TestRemoveItemFromCart_IncreaseAmountNotInStock()//UPDATE FILE: update amount not in stock
        {
            Response response_reg = marketAPI.UpdateQuantityOfItemInCart(registered_userToken, itemId_inRegCart, storeName_inSystem, itemAmount_inSttock_1+100);
            Response response_guest = marketAPI.UpdateQuantityOfItemInCart(guest_userToken, itemId_inGuestCart, storeName_inSystem, itemAmount_inSttock_2 + 100);
            if (!response_reg.ErrorOccured || !response_guest.ErrorOccured)
                Assert.Fail("should've faild: amount not in stock");
            //amount in stoer the same
            //anount in cart the same
            //------marketAPI.GetStoreInformation
        }


    }
}
