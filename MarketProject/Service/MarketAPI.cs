using MarketProject.Domain;
using MarketProject.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service
{
    public class MarketAPI
    {
        private Market _market;

        public MarketAPI()
        {
            _market = new Market();

        }

        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the user as the current admin.</para>
        /// </summary>
        public Response RestartSystem(String adminUsername, String adminPassword, String ipShippingService, String ipPaymentService)
        {//I.1
            Response response;
            try
            {
                _market.RestartSystem(adminUsername, adminPassword, ipShippingService, ipPaymentService);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in user.</para>
        /// </summary>
        /// <param name="authToken"> The token of the guest attempting to log in (to transfer cart).</param>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> Response with the authentication token the user should use with the system.</returns>
        public Response<String> Login(String authToken, String username, String password)
        {//II.1.4
            Response<String> response;
            try
            {
                // TODO: Transfer cart? Using authToken
                String loginToken = _market.Login(authToken, username, password);
                response = new Response<String>(loginToken);
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.3.1. </para>
        /// <para> Log out user identified by authToken.</para>
        /// <return> new token as a guest</return>
        /// </summary>
        /// <param name="authToken"> The token of the user to log out.</param>
        public Response<String> Logout(String authToken)
        {//II.3.1
            Response<String> response;
            try
            {
                String guestToken  = _market.Logout(authToken);
                response = new Response<String>(guestToken);
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> If credentials are valid, register new user.</para>
        /// </summary>
        /// <param name="authToken"> The token of the guest currently registering.</param>
        /// <param name="username"> The username of the user to log in.</param>
        /// <param name="password"> The password to check.</param>
        public Response Register(String authToken, String username, String password)
        {//II.1.3
            Response response;
            try
            {
                _market.Register(authToken, username, password);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered user from our system and remove their roles from all relevant stores.</para>
        /// </summary>
        /// <param name="authToken"> The token authenticating the user making the request.</param>
        /// <param name="usr_toremove"> The user to remove and revoke the roles of.</param>
        public Response RemoveRegisteredUser(String authToken, String usr_toremove )
        {//II.6.2
            Response response;
            try
            {
                _market.RemoveRegisteredUser(authToken, usr_toremove);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response AddItemToCart(String authToken, int itemID, String storeName, int amount)
        {//II.2.3
            Response response;
            try
            {
                _market.AddItemToCart(authToken, itemID, storeName, amount);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RemoveItemFromCart(String authToken, int itemID, String storeName)
        {//II.2.4
            Response response;
            try
            {
                _market.RemoveItemFromCart(authToken, itemID, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response UpdateQuantityOfItemInCart(String authToken, int itemID, String storeName, int newQuantity)
        {//II.2.4
            Response response;
            try
            {
                _market.UpdateQuantityOfItemInCart(authToken, itemID, storeName, newQuantity);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response<ShoppingCartDTO> ViewMyCart(String authToken) /*Add data object of cart*/
        {//II.2.4
            Response<ShoppingCartDTO> response;
            try
            {
                ShoppingCart shoppingCart= _market.ViewMyCart(authToken);
                response = new Response<ShoppingCartDTO>(new ShoppingCartDTO(shoppingCart));
            }
            catch (Exception e)
            {
                response = new Response<ShoppingCartDTO>(null, e);
            }
            return response;
        }
        public Response PurchaseMyCart(String authToken, String address, String city, String country, String zip, String purchaserName)
        {//II.2.5
            Response response;
            try
            {
                _market.PurchaseMyCart(authToken, address, city, country, zip, purchaserName); 
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response OpenNewStore(String authToken, String storeName)
        {//II.3.2
            Response response;
            try
            {
                _market.OpenNewStore(authToken, storeName, new PurchasePolicy(), new DiscountPolicy());
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
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
        public Response AddItemToStoreStock(String authToken, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {//II.4.1
            Response response;
            try
            {
                _market.AddItemToStoreStock(authToken, storeName, itemID, name, price, description, category, quantity);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RemoveItemFromStore(String authToken, String storeName, int itemID)
        {//II.4.1
            Response response;
            try
            {
                _market.RemoveItemFromStore(authToken, storeName, itemID);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response UpdateStockQuantityOfItem(String authToken, String storeName, int itemID, int newQuantity)
        {//II.4.1
            Response response;
            try
            {
                _market.UpdateStockQuantityOfItem(authToken, storeName, itemID, newQuantity);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response EditItemPrice(String authToken, String storeName, int itemID, float new_price)
        {//II.4.1
            Response response;
            try
            {
                _market.EditItemPrice(authToken, storeName, itemID, new_price);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response EditItemName(String authToken, String storeName, int itemID, String newName)
        {//II.4.1
            Response response;
            try
            {
                _market.EditItemName(authToken, storeName, itemID, newName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response EditItemDescription(String authToken, String storeName, int itemID, String newDescription)
        {//II.4.1
            Response response;
            try
            {
                _market.EditItemDescription(authToken, storeName, itemID, newDescription);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RateItem(String authToken, int itemID, String storeName, int rating, String review)
        {//II.3.3,  II.3.4
            Response response;
            try
            {
                _market.RateItem(authToken, itemID, storeName, rating, review);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RateStore(String authToken, String storeName, int rating, String review) // 0 < rating < 10
        {//II.3.4
            Response response;
            try
            {
                _market.RateStore(authToken, storeName, rating, review);
                response = new Response();
            } 
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response<StoreDTO> GetStoreInformation(String authToken, String storeName)
        {//II.2.1
            Response<StoreDTO> response;
            try
            {
                Store result = _market.GetStoreInformation(authToken, storeName);
                StoreDTO dto = new StoreDTO(result);
                response = new Response<StoreDTO>(dto);
            }
            catch (Exception e)
            {
                response = new Response<StoreDTO>(null, e);
            }
            return response;
        }
        public Response<List<ItemDTO>> GetItemInformation(String authToken, String itemName, String itemCategory, String keyWord)
        {//II.2.2
            //filters!!!!!!!!!!!
            Response<List<ItemDTO>> response;
            try
            {
                List<Item> result = _market.GetItemInformation(authToken, itemName, itemCategory, keyWord);
                List<ItemDTO> resultDTO = new List<ItemDTO>();
                foreach(Item item in result)
                {
                    resultDTO.Add(new ItemDTO(item));
                }
                response = new Response<List<ItemDTO>>(resultDTO);
            }
            catch (Exception e)
            {
                response = new Response<List<ItemDTO>> (null, e);
            }
            return response;
        }
        public Response SendMessageToStore(String authToken, String storeName, String title, String description)
        {//II.3.5
            Response response;
            try
            {
                _market.SendMessageToStore(authToken,storeName, title, description);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.3.6. </para>
        /// <para> Files a complaint to the current system admin.</para>
        /// </summary>
        /// <param name="authToken"> The token of the user filing the complaint. </param>
        /// <param name="cartID"> The cart ID relevant to the complaint. </param>
        /// <param name="message"> The message detailing the complaint. </param>
        public Response FileComplaint(String authToken, int cartID,  String message)
        {//II.3.6
            Response response;
            try
            {
                _market.FileComplaint(authToken, cartID, message);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response<ICollection<PurchasedCartDTO>> GetMyPurchasesHistory(String authToken)
        {//II.3.7
            Response<ICollection<PurchasedCartDTO>> response;
            try
            {
                ICollection<Tuple<DateTime ,ShoppingCart>> purchasedCarts = _market.GetMyPurchases(authToken);
                ICollection<PurchasedCartDTO> purchasedCartsDTO = new List<PurchasedCartDTO>();
                foreach (Tuple<DateTime ,ShoppingCart> purchase in purchasedCarts)
                {
                    purchasedCartsDTO.Add(new PurchasedCartDTO(purchase.Item1, purchase.Item2));
                }

                response = new Response<ICollection<PurchasedCartDTO>>(purchasedCartsDTO);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<PurchasedCartDTO>>(null, e);
            }
            return response;
        }
        public Response<RegisteredDTO> GetUserInformation(String authToken)
        {//II.3.8
            Response<RegisteredDTO> response;
            try
            {
                Registered registered= _market.GetUserInformation(authToken);
                response = new Response<RegisteredDTO>(new RegisteredDTO(registered));
            }
            catch (Exception e)
            {
                response = new Response<RegisteredDTO>(null, e);
            }
            return response;
        }
        public Boolean EditUsername(String authToken, String newUsername )
        {//II.3.8
            throw new NotImplementedException();
        }

        /// <summary>
        /// <para> For Req II.3.8. </para>
        /// <para> Updates a user's password if given the correct previous password.</para>
        /// </summary>
        /// <param name="authToken"> The authenticating token of the user changing the password.</param>
        /// <param name="oldPassword"> The user's current password. </param>
        /// <param name="newPassword"> The new updated password. </param>
        public Response EditUserPassword(String authToken, String oldPassword, String newPassword)
        {//II.3.8
            Response response;
            try
            {
                _market.EditUserPassword(authToken, oldPassword, newPassword);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Boolean RemoveManagerPermission(String authToken, String managerUsername)//permission param is Enum
        {//II.4.7

            throw new NotImplementedException();
        }
        public Boolean AddManagerPermission(String authToken, String managerUsername)//permission param is Enum
        {//II.4.7
            throw new NotImplementedException();
        }
        public Response CloseStore(String authToken, String storeName)
        {//II.4.9
            Response response;
            try
            {
                _market.CloseStore(authToken, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response ReopenStore(String authToken, String storeName)
        {//II.4.10
            Response response;
            try
            {
                _market.ReopenStore(authToken, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
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
        public Response AnswerStoreMesseage(String authToken, String storeName, string recieverUsername, String title, String reply)
        {//II.4.12
            Response response;
            try
            {
                _market.AnswerStoreMesseage(authToken, storeName, recieverUsername, title, reply);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response<List<Tuple<DateTime, ShoppingBasketDTO>>> GetStorePurchasesHistory(String authToken, String storeName)
        {//II.4.13
            Response<List<Tuple<DateTime, ShoppingBasketDTO>>> response;
            try
            {
                List<Tuple<DateTime, ShoppingBasket>> result = _market.GetStorePurchasesHistory(authToken, storeName);
                List<Tuple<DateTime, ShoppingBasketDTO>> dtos = new List<Tuple<DateTime, ShoppingBasketDTO>>();

                foreach(Tuple<DateTime, ShoppingBasket> tuple in result)
                {
                    ShoppingBasketDTO dto = new ShoppingBasketDTO(tuple.Item2);
                    Tuple<DateTime, ShoppingBasketDTO> toAdd = new Tuple<DateTime, ShoppingBasketDTO>(tuple.Item1, dto);
                    dtos.Add(toAdd);
                }

                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(dtos);
            }
            catch (Exception e)
            {
                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(null, e);
            }
            return response;
        }
        public Response CloseStorePermanently(String authToken, String storeName)
        {//II.6.1
            Response response;
            try
            {
                _market.CloseStorePermanently(authToken, storeName);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Boolean GetRegisterdComplaints(String authToken)
        {//II.6.3
            //return each complaint id in addition to its information
            throw new NotImplementedException();
        }

        /// <summary>
        /// <para> For Req II.6.3. </para>
        /// <para> System admin replies to a complaint he received.</para>
        /// </summary>
        /// <param name="authToken"> The authorisation token of the system admin.</param>
        /// <param name="complaintID"> The ID of the complaint. </param>
        /// <param name="reply"> The response to the complaint. </param>
        public Response ReplyToComplaint(String authToken, int complaintID, String reply)
        {//II.6.3
            Response response;
            try
            {
                _market.ReplyToComplaint(authToken, complaintID, reply);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response SendMessageToRegisterd(String authToken, String storeName, String usernameReciever, String title, String message)
        {//II.6.3
            Response response;
            try
            {
                _market.SendMessageToRegisterd(storeName, usernameReciever, title, message);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response<String> EnterSystem() // Generating token and returning it
        { //II.1.1
            Response<String> response;
            try
            {
                String token = _market.EnterSystem();   
                response = new Response<String>(token);
            }
            catch (Exception e) 
            { 
                response = new Response<String>(null, e); 
            }
            return response;
        }

        public Response ExitSystem(String authToken) // Removing cart and token assigned to guest
        { //II.1.2
            Response response;
            try
            {
                _market.ExitSystem(authToken);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
    }
}
