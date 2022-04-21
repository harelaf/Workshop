using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using MarketProject.Service;
using MarketProject.Service.DTO;
namespace AcceptanceTest
{
    [TestClass]
    public class MultiThreading
    {
        MarketAPI marketAPI = new MarketAPI();

        [TestMethod]
        public void TestAddItemToCart_2UsersAddingLastItemInStock()
        {
            string userToken1 = (marketAPI.EnterSystem()).Value;
            string userToken2 = (marketAPI.EnterSystem()).Value;// guest
            string storeName = "afik's Shop";
            marketAPI.Register(userToken1, "afik", "123456789");
            userToken1 = (marketAPI.Login(userToken1, "afik", "123456789")).Value;// reg
            marketAPI.OpenNewStore(userToken1, storeName);
            int iterations = 10000;
            marketAPI.AddItemToStoreStock(userToken1, storeName, 1, "banana", 3.5, "", "fruit", iterations);
           
                Thread thread1 = new Thread(() => {
                    for (int i = 0; i < iterations; i++)
                        marketAPI.AddItemToCart(userToken1, 1, storeName, 1);
                        });
                Thread thread2 = new Thread(() => {
                    for (int i = 0; i < iterations; i++)
                        marketAPI.AddItemToCart(userToken2, 1, storeName, 1);
                });
               
                thread1.Start();
                thread2.Start();
                thread1.Join();
                thread2.Join();

            int totalAmountInCarts = 0;
            Response<ShoppingCartDTO> r_1 = marketAPI.ViewMyCart(userToken1);
            Response<ShoppingCartDTO> r_2 = marketAPI.ViewMyCart(userToken2);
            if (r_1.ErrorOccured)
            {
                Assert.Fail(r_1.ErrorMessage);
            }
            if (r_2.ErrorOccured)
            {
                Assert.Fail(r_2.ErrorMessage);
            }
            ShoppingCartDTO user1Cart = r_1.Value;
            ShoppingCartDTO user2Cart = r_2.Value;

            foreach(ShoppingBasketDTO shoppingBasketDTO in user1Cart.Baskets)
            {
                if (shoppingBasketDTO.StoreName == storeName)
                {
                    foreach(ItemDTO item in shoppingBasketDTO.Items.Keys)
                    {
                        if (item.ItemID == 1)
                        {
                            totalAmountInCarts += shoppingBasketDTO.Items[item];
                            break;
                        }
                            
                    }
                    break;
                }
                    
            }
            foreach (ShoppingBasketDTO shoppingBasketDTO in user2Cart.Baskets)
            {
                if (shoppingBasketDTO.StoreName == storeName)
                {
                    foreach (ItemDTO item in shoppingBasketDTO.Items.Keys)
                    {
                        if (item.ItemID == 1)
                        {
                            totalAmountInCarts += shoppingBasketDTO.Items[item];
                            break;
                        }

                    }
                    break;
                }

            }
            Assert.AreEqual(iterations, totalAmountInCarts);

        }
    }
}