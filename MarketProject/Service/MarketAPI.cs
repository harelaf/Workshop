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
        public Boolean Login(String authToken, String username, String password)
        {//II.1.4
            throw new NotImplementedException();
        }
        public Boolean Logout(String authToken)
        {//II.3.1
            throw new NotImplementedException();
        }
        public Boolean Register(String authToken, String username, String password)
        {//II.1.3
            throw new NotImplementedException();
        }
        public Boolean RemoveRegisteredUser(String authToken, String usr_toremove )
        {//II.6.2
            //remmeber to fire him for all its roles
            throw new NotImplementedException();
        }
        public Boolean AddItemToCart(String authToken, int itemID, String storeName, int amount)
        {//II.2.3
            throw new NotImplementedException();
        }
        public Boolean RemoveItemFromCart(String authToken, int itemID, String storeName)
        {//II.2.4
            throw new NotImplementedException();
        }
        public Boolean UpdateQuantityOfItemInCart(String authToken, int itemID, String storeName, int newQuantity)
        {//II.2.4
            throw new NotImplementedException();
        }
        public String ViewMyCart(String authToken) /*Add data object of cart*/
        {//II.2.4
            throw new NotImplementedException();
        }
        public Boolean PurchaseMyCart(String authToken)
        {//II.2.5
            throw new NotImplementedException();
        }

        // TODO: WHEN WE KNOW MORE ABOUT DISCOUNT/PURCHASE POLICIES, ADD PARAMETERS HERE:
        // TODO: HOW DO I GET A STOREFOUNDER?
        public bool OpenNewStore(String authToken, String storeName)

        {//II.3.2
            throw new NotImplementedException();
        }
        public Boolean AddStoreManager(String authToken, String ownerUsername, String storeName)
        {//II.4.6
            throw new NotImplementedException();
        }
        public Boolean AddStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.4
            throw new NotImplementedException();
        }
        public Boolean RemoveStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.5
            throw new NotImplementedException();
        }
        public Boolean RemoveStoreManager(String authToken, String ownerUsername, String storeName)
        {//II.4.8
            throw new NotImplementedException();
        }
        public void AddItemToStoreStock(String username, String storeName, int itemID, String name, double price, String description, int quantity)
        {//II.4.1
            throw new NotImplementedException();
        }
        public void RemoveItemFromStore(String authToken, String storeName, int itemID)
        {//II.4.1
            throw new NotImplementedException();
        }
        public void UpdateStockQuantityOfItem(String authToken, String storeName, int itemID, int newQuantity)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemPrice(String username, int storeID, int itemID, float new_price)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemName(String username, String storeName, int itemID, int new_price, String newName)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean EditItemDescription(String authToken, String storeName, int itemID, String newDescription)
        {//II.4.1
            throw new NotImplementedException();
        }
        public Boolean RateItem(String authToken, int itemID, String storeName, int rating, String review)
        {//II.3.3,  II.3.4
            //should check that this user bought this item by his purches History
            throw new NotImplementedException();
        }
        public bool RateStore(String authToken, String storeName, int rating, String review) // 0 < rating < 10
        {//II.3.4
            throw new NotImplementedException();
        }
        public String GetStoreInformation(String username, String storeName)
        {//II.2.1
         //should return data of store + the items it owns
            throw new NotImplementedException();
        }
        public Boolean GetItemInformation(String authToken, String itemName, String itemCategory, String keyWord)
        {//II.2.2
            //filters!!!!!!!!!!!
            throw new NotImplementedException();
        }
        public Boolean SendMessageToStore(String authToken, String storeName, String title, String description)
        {//II.3.5
            throw new NotImplementedException();
        }
        public Boolean FileComplaint(String authToken, int cartID,  String message)
        {//II.3.6
            //to system admin!! should define some queue of messages for admin
            throw new NotImplementedException();
        }
        public Boolean GetMyPurchases(String authToken)
        {//II.3.7
            //we may add func that get the purchase by date
            throw new NotImplementedException();
        }
        public Boolean GetUserInformation(String authToken)
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean EditUsername(String authToken, String newUsername )
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean EditUserPassword(String authToken, String newPassword)
        {//II.3.8
            throw new NotImplementedException();
        }
        public Boolean RemoveManagerPermission(String authToken, String managerUsername)//permission param is Enum
        {//II.4.7

            throw new NotImplementedException();
        }
        public Boolean AddManagerPermission(String authToken, String managerUsername)//permission param is Enum
        {//II.4.7
            throw new NotImplementedException();
        }
        public void CloseStore(String authToken, String storeName)
        {//II.4.9
            //state of store is INACTIVE-> which means its data still available
            throw new NotImplementedException();
        }
        public void ReopenStore(String authToken, String storeName)
        {//II.4.10
            //SHOULD VALIDATE THAT store state is INACTIVE
            throw new NotImplementedException();
        }
        public Boolean GetStoreRoleInformation(String authToken, String storeName)
        {//II.4.11
            throw new NotImplementedException();
        }
        public Boolean GetStoreMesseage(String authToken, String storeName)
        {//II.4.12
            //should return with id
            throw new NotImplementedException();
        }
        public Boolean AnswerStoreMesseage(String authToken, String storeName, int messageID, String reply)
        {//II.4.12
            throw new NotImplementedException();
        }
        public List<Tuple<DateTime, ShoppingBasket>> GetStorePurchasesHistory(String authToken, String storeName)
        {//II.4.13
            throw new NotImplementedException();
        }
        public void CloseStorePermanently(String authToken, String storeName)
        {//II.6.1
            //send message to all roles in that store
            throw new NotImplementedException();
        }
        public Boolean GetRegisterdComplaints(String authToken)
        {//II.6.3
            //return each complaint id in addition to its information
            throw new NotImplementedException();
        }
        public Boolean ReplyToComplaint(String authToken, int complaintID)
        {//II.6.3
            throw new NotImplementedException();
        }
        public Boolean SendMessageToRegisterd(String authToken, String usernameReciever, String message)
        {//II.6.3
            throw new NotImplementedException();
        }

        public String EnterSystem() // Generating token and returning it
        { //II.1.1
            throw new NotImplementedException();
        }

        public void ExitSystem(String authToken) // Removing cart and token assigned to guest
        { //II.1.2
            throw new NotImplementedException();
        }
    }
}
