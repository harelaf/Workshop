using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service
{
    internal class MarketAPI
    {
        public Boolean Login(String username, String password)
        {
            throw new NotImplementedException();
        }
        public Boolean Logout()
        {
            throw new NotImplementedException();
        }
        public Boolean Register(String username, String password)
        {
            throw new NotImplementedException();
        }
        public Boolean DeleteUser(String username)
        {
            throw new NotImplementedException();
        }


        public Boolean AddItemToCart(int itemID)
        {
            throw new NotImplementedException();
        }
        public String ViewMyCart() /*Add data object of cart*/
        {
            throw new NotImplementedException();
        }
        public Boolean Checkout()
        {
            throw new NotImplementedException();
        }
        public Boolean OpenNewStore(String storeName)
        {
            throw new NotImplementedException();
        }
        public Boolean AddStoreRole(int storeID)
        {
            throw new NotImplementedException();
        }
        public Boolean RemoveStoreRole(int storeID)
        {
            throw new NotImplementedException();
        }
        public Boolean AddItemToStore(int storeID, int itemID)
        {
            throw new NotImplementedException();
        }
        public Boolean RemoveItemFromStore(int storeID, int itemID)
        {
            throw new NotImplementedException();
        }
        public Boolean RateItem(int itemID /*, ReviewData rd*/)
        {
            throw new NotImplementedException();
        }
    }
}
