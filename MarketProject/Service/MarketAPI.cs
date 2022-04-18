using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service
{
    internal class MarketAPI
    {
        private Market _market;

        public MarketAPI()
        {
            _market = new Market();
        }

        public Boolean RestartSystem(String sysManegerUsername, String ipShippingService, String ipPaymentService)
        {//I.1
            throw new NotImplementedException();
        }
        public Boolean Login(String username, String password)
        {//II.1.4
            throw new NotImplementedException();
        }
        public Boolean Logout(String username)
        {//II.3.1
            throw new NotImplementedException();
        }
        public Boolean Register(String username, String password)
        {//II.1.3
            throw new NotImplementedException();
        }
        public Boolean RemoveRegisteredUser(String username_actor,String usr_toremove )
        {//II.6.2
            //remmeber to fire him for all its roles
            throw new NotImplementedException();
        }
        public Boolean AddItemToCart(String username, int itemID, int storeID, int amount)
        {//II.2.3
            throw new NotImplementedException();
        }
        public Boolean RemoveItemFromCart(String username, int itemID, int storeID)
        {//II.2.4
            throw new NotImplementedException();
        }
        public Boolean UpdateQuantityOfItemInCart(String username, int itemID, int storeID, int newQuantity)
        {//II.2.4
            throw new NotImplementedException();
        }
        public String ViewMyCart(String username) /*Add data object of cart*/
        {//II.2.4
            throw new NotImplementedException();
        }
        public Boolean PurchaseMyCart(String username)
        {//II.2.5
            throw new NotImplementedException();
        }
        // TODO: WHEN WE KNOW MORE ABOUT DISCOUNT/PURCHASE POLICIES, ADD PARAMETERS HERE:
        // TODO: HOW DO I GET A STOREFOUNDER?
        public bool OpenNewStore(String username, String storeName)
        {//II.3.2
            if (storeName.Equals(""))
                return false;
            return _market.OpenNewStore(null, storeName, new PurchasePolicy(), new DiscountPolicy());
        }
        public Boolean AddStoreManager(String appointerUsername, String ownerUsername, int storeID)
        {//II.4.6
            throw new NotImplementedException();
        }
        public Boolean AddStoreOwner(String appointerUsername, String ownerUsername, int storeID)
        {//II.4.4
            throw new NotImplementedException();
        }
        public Boolean RemoveStoreOwner(String appointerUsername, String ownerUsername, int storeID)
        {//II.4.5
            throw new NotImplementedException();
        }
        public Boolean RemoveStoreManager(String appointerUsername, String ownerUsername, int storeID)
        {//II.4.8
            throw new NotImplementedException();
        }
        public Boolean AddItemToStoreStock(String username, String storeName, int itemID, int quantity)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean RemoveItemFromStore(String username, String storeName, int itemID)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean UpdateStockQuantityOfItem(String username, String storeName, int itemID, int newQuantity)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemPrice(String username, String storeName, int itemID, int new_price, String newPrice)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemName(String username, String storeName, int itemID, int new_price, String newName)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemDescription(String username, String storeName, int itemID, String newDescription)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean RateItem(String username, int itemID, String storeName, int rating, String review)
        {//II.3.3,  II.3.4
            //should check that this user bought this item by his purches History
            throw new NotImplementedException();
        }
        public bool RateStore(String username, String storeName, int rating, String review) // 0 < rating < 10
        {//II.3.4
            throw new NotImplementedException();
        }
        public String GetStoreInformation(String storeName)
        {//II.2.1
         //should return data of store + the items it owns
            throw new NotImplementedException();
        }
        public Boolean GetItemInformation(String itemName, String itemCategory, String keyWord)
        {//II.2.2
            //filters!!!!!!!!!!!
            throw new NotImplementedException();
        }
        public Boolean SendMessageToStore(String username, int storeID, String title, String description)
        {//II.3.5
            throw new NotImplementedException();
        }
        public Boolean FileComplaint(String username, int cartID,  String description)
        {//II.3.6
            //to system admin!! should define some queue of messages for admin
            throw new NotImplementedException();
        }
        public Boolean GetMyPurchases(String username)
        {//II.3.7
            //we may add func that get the purchase by date
            throw new NotImplementedException();
        }
        public Boolean GetUserInformation(String username)
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean EditUsername(String username,String newUsrname )
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean EditUserPassword(String username, String newPassword)
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean RemoveManagerPermission(String username, String managerUsername,String perrmision)//permission param is Enum
        {//II.4.7

            throw new NotImplementedException();
        }
        public Boolean AddManagerPermission(String username, String managerUsername, String perrmision)//permission param is Enum
        {//II.4.7
            throw new NotImplementedException();
        }
        public Boolean CloseStore(String username, int storeID)
        {//II.4.9
            //state of store is INACTIVE-> which means its data still available
            throw new NotImplementedException();
        }
        public Boolean ReopenStore(String username, int storeID)
        {//II.4.10
            //SHOULD VALIDATE THAT store state is INACTIVE
            throw new NotImplementedException();
        }
        public Boolean GetStoreRoleInformation(String username, int storeID)
        {//II.4.11
            throw new NotImplementedException();
        }
        public Boolean GetStoreMesseage(String username, String storeName)
        {//II.4.12
            //should return with id
            throw new NotImplementedException();
        }
        public Boolean AnswerStoreMesseage(String username, String storeName, int messageID, String reply)
        {//II.4.12
            throw new NotImplementedException();
        }
        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchasesHistory(String username, String storeName)
        {//II.4.13
            if (storeName.Equals(""))
                return null;
            return _market.GetStorePurchaseHistory(username, storeName);
        }
        public Boolean CloseStorePermanently(String username, String storeName)
        {//II.6.1
            //send message to all roles in that store
            throw new NotImplementedException();
        }
        public Boolean GetRegisterdComplaints(String username)
        {//II.6.3
            //return each complaint id in addition to its information
            throw new NotImplementedException();
        }
        public Boolean ReplyToComplaint(String username, int complaintID)
        {//II.6.3
            throw new NotImplementedException();
        }
        public Boolean SendMessageToRegiaterd(String usernameSender, String usernameReciver, String message)
        {//II.6.3
            throw new NotImplementedException();
        }

    }
}
