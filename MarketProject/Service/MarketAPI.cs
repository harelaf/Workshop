using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using MarketProject.Service.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service
{
    public class MarketAPI
    {
        private Market _market;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MarketAPI()
        {
            _market = new Market();
        }

        /// <summary>
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the Visitor as the current admin.</para>
        /// </summary>
        public Response RestartSystem(String adminUsername, String adminPassword, String ipShippingService, String ipPaymentService)
        {//I.1
            Response response;
            try
            {
                log.Info($"Restart System called with parameters: adminUsername={adminUsername}, adminUsername={adminPassword}, ipShippingService={ipShippingService}, ipPaymentService={ipPaymentService}.");
                _market.RestartSystem(adminUsername, adminPassword, ipShippingService, ipPaymentService);
                response = new Response();
                log.Info($"SUCCESSFULY executed Restart System.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in Visitor.</para>
        /// </summary>
        /// <param name="authToken"> The token of the guest attempting to log in (to transfer cart).</param>
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> Response with the authentication token the Visitor should use with the system.</returns>
        public Response<String> Login(String authToken, String Username, String password)
        {//II.1.4
            Response<String> response;
            try
            {
                log.Info($"Login called with parameters: authToken={authToken}, username={Username}, password={password}.");
                // TODO: Transfer cart? Using authToken
                String loginToken = _market.Login(authToken, Username, password);
                response = new Response<String>(loginToken);
                log.Info($"SUCCESSFULY executed Login.");
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.3.1. </para>
        /// <para> Log out Visitor identified by authToken.</para>
        /// <return> new token as a guest</return>
        /// </summary>
        /// <param name="authToken"> The token of the Visitor to log out.</param>
        public Response<String> Logout(String authToken)
        {//II.3.1
            Response<String> response;
            try
            {
                log.Info($"Logout called with parameters: authToken={authToken}.");
                String guestToken  = _market.Logout(authToken);
                response = new Response<String>(guestToken);
                log.Info($"SUCCESSFULY executed Logout.");
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.1.3. </para>
        /// <para> If credentials are valid, register new Visitor.</para>
        /// </summary>
        /// <param name="authToken"> The token of the guest currently registering.</param>
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        public Response Register(String authToken, String Username, String password)
        {//II.1.3
            Response response;
            try
            {
                log.Info($"Logout called with parameters: authToken={authToken}, username={Username}, password={password}.");
                _market.Register(authToken, Username, password);
                response = new Response();
                log.Info($"SUCCESSFULY executed Register.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered Visitor from our system and remove their roles from all relevant stores.</para>
        /// </summary>
        /// <param name="authToken"> The token authenticating the Visitor making the request.</param>
        /// <param name="usr_toremove"> The Visitor to remove and revoke the roles of.</param>
        public Response RemoveRegisteredVisitor(String authToken, String usr_toremove )
        {//II.6.2
            Response response;
            try
            {
                log.Info($"Remove Registered Visitor called with parameters: authToken={authToken}, username={usr_toremove}.");
                _market.RemoveRegisteredVisitor(authToken, usr_toremove);
                response = new Response();
                log.Info($"SUCCESSFULY executed Remove Registered Vistor.");
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
                log.Info($"Add Item To Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}, amount={amount}.");
                _market.AddItemToCart(authToken, itemID, storeName, amount);
                response = new Response();
                log.Info($"SUCCESSFULY executed Add Item To Cart.");
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
                log.Info($"Remove Item From Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}.");
                _market.RemoveItemFromCart(authToken, itemID, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Remove Item From Cart.");
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
                log.Info($"Update Quantity Of Item In Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}, newQuantity={newQuantity}.");
                _market.UpdateQuantityOfItemInCart(authToken, itemID, storeName, newQuantity);
                response = new Response();
                log.Info($"SUCCESSFULY executed Update Quantity Of Item In Cart.");
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
                log.Info($"View My Cart called with parameters: authToken={authToken}.");
                ShoppingCart shoppingCart= _market.ViewMyCart(authToken);
                response = new Response<ShoppingCartDTO>(new ShoppingCartDTO(shoppingCart));
                log.Info($"SUCCESSFULY executed View My Cart.");
            }
            catch (Exception e)
            {
                response = new Response<ShoppingCartDTO>(null, e);
            }
            return response;
        }
        public Response PurchaseMyCart(String authToken, String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode)
        {//II.2.5
            Response response;
            try
            {
                log.Info($"Purchase My Cart called with parameters: authToken={authToken}, address={address}, city={city}, country={country}, zip={zip}, purchaserName={purchaserName}.");
                _market.PurchaseMyCart(authToken, address, city, country, zip, purchaserName, paymentMethode, shipmentMethode); 
                response = new Response();
                log.Info($"SUCCESSFULY executed Purchase My Cart.");
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
                log.Info($"Open New Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.OpenNewStore(authToken, storeName, new PurchasePolicy(), new DiscountPolicy());
                response = new Response();
                log.Info($"SUCCESSFULY executed Open New Store.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response AddStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.6
            Response response;
            try
            {
                log.Info($"Add Store Manager called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}.");
                _market.AddStoreManager(authToken, managerUsername, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Add Store Manager.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response AddStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.4
            Response response;
            try
            {
                log.Info($"Add Store Owner called with parameters: authToken={authToken}, ownerUsername={ownerUsername}, storeName={storeName}.");
                _market.AddStoreOwner(authToken, ownerUsername, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Add Store Owner.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RemoveStoreOwner(String authToken, String ownerUsername, String storeName)
        {//II.4.5
            Response response;
            try
            {
                log.Info($"Remove Store Owner called with parameters: authToken={authToken}, ownerUsername={ownerUsername}, storeName={storeName}.");
                _market.RemoveStoreOwner(authToken, ownerUsername, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Remove Store Owner.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response RemoveStoreManager(String authToken, String managerUsername, String storeName)
        {//II.4.8
            Response response;
            try
            {
                log.Info($"Remove Store Manager called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}.");
                _market.RemoveStoreManager(authToken, managerUsername, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Remove Store Manager.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response AddItemToStoreStock(String authToken, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {//II.4.1
            Response response;
            try
            {
                log.Info($"Add Item To Stock called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, name={name}, price={price}, description={description}, category={category}, quantity={quantity}.");
                _market.AddItemToStoreStock(authToken, storeName, itemID, name, price, description, category, quantity);
                response = new Response();
                log.Info($"SUCCESSFULY executed Add Item To Stock.");
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
                log.Info($"Remove Item From Stock called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}.");
                _market.RemoveItemFromStore(authToken, storeName, itemID);
                response = new Response();
                log.Info($"SUCCESSFULY executed Remove Item From Stock.");
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
                log.Info($"Update Stock Quantity Of Item called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newQuantity={newQuantity}.");
                _market.UpdateStockQuantityOfItem(authToken, storeName, itemID, newQuantity);
                response = new Response();
                log.Info($"SUCCESSFULY executed Update Stock Quantity Of Item.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
        public Response EditItemPrice(String authToken, String storeName, int itemID, double newPrice)
        {//II.4.1
            Response response;
            try
            {
                log.Info($"Edit Item Price called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newPrice={newPrice}.");
                _market.EditItemPrice(authToken, storeName, itemID, newPrice);
                response = new Response();
                log.Info($"SUCCESSFULY executed Edit Item Price.");
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
                log.Info($"Edit Item Name called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newName={newName}.");
                _market.EditItemName(authToken, storeName, itemID, newName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Edit Item Name.");
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
                log.Info($"Edit Item Description called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newDescription={newDescription}.");
                _market.EditItemDescription(authToken, storeName, itemID, newDescription);
                response = new Response();
                log.Info($"SUCCESSFULY executed Edit Item Description.");
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
                log.Info($"Rate Item called with parameters: authToken={authToken}, itemID={itemID}, storeName={storeName}, rating={rating}, review={review}.");
                _market.RateItem(authToken, itemID, storeName, rating, review);
                response = new Response();
                log.Info($"SUCCESSFULY executed Rate Item.");
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
                log.Info($"Rate Store called with parameters: authToken={authToken}, storeName={storeName}, rating={rating}, review={review}.");
                _market.RateStore(authToken, storeName, rating, review);
                response = new Response();
                log.Info($"SUCCESSFULY executed Rate Store.");
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
                log.Info($"Get Store Information called with parameters: authToken={authToken}, storeName={storeName}.");
                Store result = _market.GetStoreInformation(authToken, storeName);
                StoreDTO dto = new StoreDTO(result);
                response = new Response<StoreDTO>(dto);
                log.Info($"SUCCESSFULY executed Get Store Information.");
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
                log.Info($"Get Item Information called with parameters: authToken={authToken}, itemName={itemName}, itemCategory={itemCategory}, keyWord={keyWord}.");
                List<Item> result = _market.GetItemInformation(authToken, itemName, itemCategory, keyWord);
                List<ItemDTO> resultDTO = new List<ItemDTO>();
                foreach(Item item in result)
                {
                    resultDTO.Add(new ItemDTO(item));
                }
                response = new Response<List<ItemDTO>>(resultDTO);
                log.Info($"SUCCESSFULY executed Get Item Information.");
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
                log.Info($"Send Message To Store called with parameters: authToken={authToken}, storeName={storeName}, title={title}, description={description}.");
                _market.SendMessageToStore(authToken,storeName, title, description);
                response = new Response();
                log.Info($"SUCCESSFULY executed Send Message To Store.");
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
        /// <param name="authToken"> The token of the Visitor filing the complaint. </param>
        /// <param name="cartID"> The cart ID relevant to the complaint. </param>
        /// <param name="message"> The message detailing the complaint. </param>
        public Response FileComplaint(String authToken, int cartID,  String message)
        {//II.3.6
            Response response;
            try
            {
                log.Info($"Send Message To Store called with parameters: authToken={authToken}, cartID={cartID}, message={message}.");
                _market.FileComplaint(authToken, cartID, message);
                response = new Response();
                log.Info($"SUCCESSFULY executed File Complaint.");
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
                log.Info($"Get My Purchases History called with parameters: authToken={authToken}.");
                ICollection<Tuple<DateTime ,ShoppingCart>> purchasedCarts = _market.GetMyPurchases(authToken);
                ICollection<PurchasedCartDTO> purchasedCartsDTO = new List<PurchasedCartDTO>();
                foreach (Tuple<DateTime ,ShoppingCart> purchase in purchasedCarts)
                {
                    purchasedCartsDTO.Add(new PurchasedCartDTO(purchase.Item1, purchase.Item2));
                }

                response = new Response<ICollection<PurchasedCartDTO>>(purchasedCartsDTO);
                log.Info($"SUCCESSFULY executed Get My Purchases History.");
            }
            catch (Exception e)
            {
                response = new Response<ICollection<PurchasedCartDTO>>(null, e);
            }
            return response;
        }

        public Response<RegisteredDTO> GetVisitorInformation(String authToken)
        {//II.3.8
            Response<RegisteredDTO> response;
            try
            {
                log.Info($"Get Visitor Information called with parameters: authToken={authToken}.");
                Registered registered= _market.GetVisitorInformation(authToken);
                response = new Response<RegisteredDTO>(new RegisteredDTO(registered));
                log.Info($"SUCCESSFULY executed Get Visitor Information.");
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
        /// <para> Updates a Visitor's password if given the correct previous password.</para>
        /// </summary>
        /// <param name="authToken"> The authenticating token of the Visitor changing the password.</param>
        /// <param name="oldPassword"> The Visitor's current password. </param>
        /// <param name="newPassword"> The new updated password. </param>
        public Response EditVisitorPassword(String authToken, String oldPassword, String newPassword)
        {//II.3.8
            Response response;
            try
            {
                log.Info($"Edit Visitor Password called with parameters: authToken={authToken}, oldPassword={oldPassword}, newPassword={newPassword}.");
                _market.EditVisitorPassword(authToken, oldPassword, newPassword);
                response = new Response();
                log.Info($"SUCCESSFULY executed Edit Visitor Password.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response RemoveManagerPermission(String authToken, String managerUsername, String storeName, Operation op)//permission param is Enum
        {//II.4.7

            Response response;
            try
            {
                log.Info($"Remove Manager Permission called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}, op={op}.");
                _market.RemoveManagerPermission(authToken, managerUsername, storeName, op);
                response = new Response();
                log.Info($"SUCCESSFULY executed Remove Manager Permission.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response AddManagerPermission(String authToken, String managerUsername, String storeName, Operation op)//permission param is Enum
        {//II.4.7
            Response response;
            try
            {
                log.Info($"Add Manager Permission called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}, op={op}.");
                _market.AddManagerPermission(authToken, managerUsername, storeName, op);
                response = new Response();
                log.Info($"SUCCESSFULY executed Add Manager Permission.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response CloseStore(String authToken, String storeName)
        {//II.4.9
            Response response;
            try
            {
                log.Info($"Close Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.CloseStore(authToken, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Close Store.");
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
                log.Info($"Reopen Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.ReopenStore(authToken, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Reopen Store.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response<List<StoreOwnerDTO>> GetStoreOwners(String authToken, String storeName)
        {//II.4.11
            Response<List<StoreOwnerDTO>> response;
            try
            {
                log.Info($"Get Store Owners called with parameters: authToken={authToken}, storeName={storeName}.");
                List<StoreOwner> ownerList = _market.getStoreOwners(storeName, authToken);
                List<StoreOwnerDTO> ownerDTOlist = new List<StoreOwnerDTO>();
                foreach (StoreOwner owner in ownerList)
                    ownerDTOlist.Add(new StoreOwnerDTO(owner));
                response = new Response<List<StoreOwnerDTO>>(ownerDTOlist);
                log.Info($"SUCCESSFULY executed Get Store Owners.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreOwnerDTO>>(null, e);
            }
            return response;
        }

        public Response<List<StoreManagerDTO>> GetStoreManagers(String authToken, String storeName)
        {//II.4.11
            Response<List<StoreManagerDTO>> response;
            try
            {
                log.Info($"Get Store Managers called with parameters: authToken={authToken}, storeName={storeName}.");
                List<StoreManager> managerList = _market.getStoreManagers(storeName, authToken);
                List<StoreManagerDTO> managerDTOlist = new List<StoreManagerDTO>();
                foreach (StoreManager manager in managerList)
                    managerDTOlist.Add(new StoreManagerDTO(manager));
                response = new Response<List<StoreManagerDTO>>(managerDTOlist);
                log.Info($"SUCCESSFULY executed Get Store Managers.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreManagerDTO>>(null, e);
            }
            return response;
        }

        public Response<StoreFounderDTO> GetStoreFounder(String authToken, String storeName)
        {//II.4.11
            Response<StoreFounderDTO> response;
            try
            {
                log.Info($"Get Store Founder called with parameters: authToken={authToken}, storeName={storeName}.");
                StoreFounder founder = _market.getStoreFounder(storeName, authToken);
                response = new Response<StoreFounderDTO>(new StoreFounderDTO(founder));
                log.Info($"SUCCESSFULY executed Get Store Founder.");
            }
            catch (Exception e)
            {
                response = new Response<StoreFounderDTO>(null, e);
            }
            return response;
        }

        public Boolean GetStoreMessage(String authToken, String storeName)
        {//II.4.12
            //should return with id
            throw new NotImplementedException();
        }

        public Response AnswerStoreMesseage(String authToken, String storeName, string recieverUsername, String title, String reply)
        {//II.4.12
            Response response;
            try
            {
                log.Info($"Answer Store Message called with parameters: authToken={authToken}, storeName={storeName}, recieverUsername={recieverUsername}, title={title}, reply={reply}.");
                _market.AnswerStoreMesseage(authToken, storeName, recieverUsername, title, reply);
                response = new Response();
                log.Info($"SUCCESSFULY executed Answer Store Message.");
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
                log.Info($"Get Store Purchases History called with parameters: authToken={authToken}, storeName={storeName}.");
                List<Tuple<DateTime, ShoppingBasket>> result = _market.GetStorePurchasesHistory(authToken, storeName);
                List<Tuple<DateTime, ShoppingBasketDTO>> dtos = new List<Tuple<DateTime, ShoppingBasketDTO>>();

                foreach(Tuple<DateTime, ShoppingBasket> tuple in result)
                {
                    ShoppingBasketDTO dto = new ShoppingBasketDTO(tuple.Item2);
                    Tuple<DateTime, ShoppingBasketDTO> toAdd = new Tuple<DateTime, ShoppingBasketDTO>(tuple.Item1, dto);
                    dtos.Add(toAdd);
                }

                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(dtos);
                log.Info($"SUCCESSFULY executed Get Store Purchases History.");
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
                log.Info($"Close Store Permanently called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.CloseStorePermanently(authToken, storeName);
                response = new Response();
                log.Info($"SUCCESSFULY executed Close Store Permanently.");
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
                log.Info($"Reply To Complaint called with parameters: authToken={authToken}, complaintID={complaintID}, reply={reply}.");
                _market.ReplyToComplaint(authToken, complaintID, reply);
                response = new Response();
                log.Info($"SUCCESSFULY executed Reply To Complaint.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response SendMessageToRegisterd(String authToken, String storeName, String UsernameReciever, String title, String message)
        {//II.6.3
            Response response;
            try
            {
                log.Info($"Send Message To Registered called with parameters: authToken={authToken}, storeName={storeName}, UsernameReciever={UsernameReciever}, title={title}, message={message}.");
                _market.SendMessageToRegisterd(storeName, UsernameReciever, title, message);
                response = new Response();
                log.Info($"SUCCESSFULY executed Send Message To Registered.");
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
                log.Info($"Enter System To Registered called.");
                String token = _market.EnterSystem();   
                response = new Response<String>(token);
                log.Info($"SUCCESSFULY executed Enter System.");
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
                log.Info($"Exit System called with parameters: authToken={authToken}.");
                _market.ExitSystem(authToken);
                response = new Response();
                log.Info($"SUCCESSFULY executed Exit System.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response AppointSystemAdmin(String authToken, String adminUsername)
        { //II.1.2
            Response response;
            try
            {
                log.Info($"Appoint System Admin called with parameters: authToken={authToken}, adminUsername={adminUsername}.");
                _market.AppointSystemAdmin(authToken, adminUsername);
                response = new Response();
                log.Info($"SUCCESSFULY executed Appoint System Admin.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }

        public Response AddStoreDiscount(String authToken, String storeName, IDiscountDTO discount_dto)
        {
            Response response;
            try
            {
                /////////// is log should keep the whole description of the discount??????
                log.Info($"Add Store Discount called with parameters: authToken={authToken}, storeName={storeName} and the actual discount.");
                Discount discount = new dtoDiscountConverter().convertDiscount(discount_dto);
                _market.AddStoreDiscount(authToken, storeName, discount);
                response = new Response();
                log.Info($"SUCCESSFULY executed Add Store Discount.");
            }
            catch (Exception e)
            {
                response = new Response(e);
            }
            return response;
        }
    }
}
