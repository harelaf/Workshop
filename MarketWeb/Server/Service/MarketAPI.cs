using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using MarketProject.Service.DTO;
using MarketWeb.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace MarketProject.Service
{
    [Route("api/market/")]
    [ApiController]
    public class MarketAPI : ControllerBase
    {
        private Market _market;
        private ILogger<MarketAPI> _logger;
        private int _id;

        public MarketAPI(Market market, ILogger<MarketAPI> logger)
        {
            _market = market;
            _logger = logger;
        }

        private String parseAutherization(String Authorization)
        {
            if (!AuthenticationHeaderValue.TryParse(Authorization, out var headerValue))
            {
                throw new Exception("Invalid token!");
            }
            return headerValue.Parameter;
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
                _logger.LogInformation($"Restart System called with parameters: adminUsername={adminUsername}, adminUsername={adminPassword}, ipShippingService={ipShippingService}, ipPaymentService={ipPaymentService}.");
                _market.RestartSystem(adminUsername, adminPassword, ipShippingService, ipPaymentService);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Restart System.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.1.4. </para>
        /// <para> If credentials are authenticated, log in Visitor.</para>
        /// </summary>
        /// <param name="Authorization"> The token of the guest attempting to log in (to transfer cart).</param>
        /// <param name="Username"> The Username of the Visitor to log in.</param>
        /// <param name="password"> The password to check.</param>
        /// <returns> Response with the authentication token the Visitor should use with the system.</returns>
        [HttpPost("login")]
        public Response<String> Login([FromHeader] String Authorization, String Username, String password)
        {//II.1.4
            Response<String> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Login called with parameters: authToken={authToken}, username={Username}, password={password}.");
                // TODO: Transfer cart? Using authToken
                String loginToken = _market.Login(authToken, Username, password);
                response = new Response<String>(loginToken);
                _logger.LogInformation($"SUCCESSFULY executed Login.");
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.3.1. </para>
        /// <para> Log out Visitor identified by authToken.</para>
        /// <return> new token as a guest</return>
        /// </summary>
        /// <param name="authToken"> The token of the Visitor to log out.</param>
        [HttpGet("Logout")]
        public Response<String> Logout([FromHeader] String Authorization)
        {//II.3.1
            Response<String> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                String guestToken  = _market.Logout(authToken);
                response = new Response<String>(guestToken);
                _logger.LogInformation($"SUCCESSFULY executed Logout.");
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e);_logger.LogError(e.Message);
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
        [HttpPost("Register")]
        public Response Register([FromHeader] String Authorization, String Username, String password, DateTime birthDate)
        {//II.1.3
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Register called with parameters: authToken={authToken}, username={Username}, password={password}.");
                _market.Register(authToken, Username, password, birthDate);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Register.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered Visitor from our system and remove their roles from all relevant stores.</para>
        /// </summary>
        /// <param name="authToken"> The token authenticating the Visitor making the request.</param>
        /// <param name="usr_toremove"> The Visitor to remove and revoke the roles of.</param>
        [HttpGet("RemoveRegisteredVisitor")]
        public Response RemoveRegisteredVisitor([FromHeader] String Authorization, String usr_toremove )
        {//II.6.2
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Remove Registered Visitor called with parameters: authToken={authToken}, username={usr_toremove}.");
                _market.RemoveRegisteredVisitor(authToken, usr_toremove);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Remove Registered Vistor.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response AddItemToCart([FromHeader] String Authorization, int itemID, String storeName, int amount)
        {//II.2.3
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Add Item To Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}, amount={amount}.");
                _market.AddItemToCart(authToken, itemID, storeName, amount);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Add Item To Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response RemoveItemFromCart([FromHeader] String Authorization, int itemID, String storeName)
        {//II.2.4
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Remove Item From Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}.");
                _market.RemoveItemFromCart(authToken, itemID, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Remove Item From Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response UpdateQuantityOfItemInCart([FromHeader] String Authorization, int itemID, String storeName, int newQuantity)
        {//II.2.4
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Update Quantity Of Item In Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}, newQuantity={newQuantity}.");
                _market.UpdateQuantityOfItemInCart(authToken, itemID, storeName, newQuantity);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Update Quantity Of Item In Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        [HttpGet("ViewMyCart")]
        public Response<ShoppingCartDTO> ViewMyCart([FromHeader] String Authorization) /*Add data object of cart*/
        {//II.2.4
            Response<ShoppingCartDTO> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"View My Cart called with parameters: authToken={authToken}.");
                ShoppingCart shoppingCart= _market.ViewMyCart(authToken);
                response = new Response<ShoppingCartDTO>(new ShoppingCartDTO(shoppingCart));
                _logger.LogInformation($"SUCCESSFULY executed View My Cart.");
            }
            catch (Exception e)
            {
                response = new Response<ShoppingCartDTO>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response PurchaseMyCart([FromHeader] String Authorization, String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode)
        {//II.2.5
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Purchase My Cart called with parameters: authToken={authToken}, address={address}, city={city}, country={country}, zip={zip}, purchaserName={purchaserName}.");
                _market.PurchaseMyCart(authToken, address, city, country, zip, purchaserName, paymentMethode, shipmentMethode); 
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Purchase My Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response OpenNewStore([FromHeader] String Authorization, String storeName)
        {//II.3.2
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Open New Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.OpenNewStore(authToken, storeName, new PurchasePolicy(), new DiscountPolicy());
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Open New Store.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response AddStoreManager([FromHeader] String Authorization, String managerUsername, String storeName)
        {//II.4.6
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Add Store Manager called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}.");
                _market.AddStoreManager(authToken, managerUsername, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Add Store Manager.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response AddStoreOwner([FromHeader] String Authorization, String ownerUsername, String storeName)
        {//II.4.4
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Add Store Owner called with parameters: authToken={authToken}, ownerUsername={ownerUsername}, storeName={storeName}.");
                _market.AddStoreOwner(authToken, ownerUsername, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Add Store Owner.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response RemoveStoreOwner([FromHeader] String Authorization, String ownerUsername, String storeName)
        {//II.4.5
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Remove Store Owner called with parameters: authToken={authToken}, ownerUsername={ownerUsername}, storeName={storeName}.");
                _market.RemoveStoreOwner(authToken, ownerUsername, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Remove Store Owner.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response RemoveStoreManager([FromHeader] String Authorization, String managerUsername, String storeName)
        {//II.4.8
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Remove Store Manager called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}.");
                _market.RemoveStoreManager(authToken, managerUsername, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Remove Store Manager.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response AddItemToStoreStock([FromHeader] String Authorization, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {//II.4.1
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Add Item To Stock called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, name={name}, price={price}, description={description}, category={category}, quantity={quantity}.");
                _market.AddItemToStoreStock(authToken, storeName, itemID, name, price, description, category, quantity);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Add Item To Stock.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response RemoveItemFromStore([FromHeader] String Authorization, String storeName, int itemID)
        {//II.4.1
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Remove Item From Stock called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}.");
                _market.RemoveItemFromStore(authToken, storeName, itemID);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Remove Item From Stock.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response UpdateStockQuantityOfItem([FromHeader] String Authorization, String storeName, int itemID, int newQuantity)
        {//II.4.1
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Update Stock Quantity Of Item called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newQuantity={newQuantity}.");
                _market.UpdateStockQuantityOfItem(authToken, storeName, itemID, newQuantity);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Update Stock Quantity Of Item.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response EditItemPrice([FromHeader] String Authorization, String storeName, int itemID, double newPrice)
        {//II.4.1
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Edit Item Price called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newPrice={newPrice}.");
                _market.EditItemPrice(authToken, storeName, itemID, newPrice);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Edit Item Price.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response EditItemName([FromHeader] String Authorization, String storeName, int itemID, String newName)
        {//II.4.1
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Edit Item Name called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newName={newName}.");
                _market.EditItemName(authToken, storeName, itemID, newName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Edit Item Name.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response EditItemDescription([FromHeader] String Authorization, String storeName, int itemID, String newDescription)
        {//II.4.1
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Edit Item Description called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newDescription={newDescription}.");
                _market.EditItemDescription(authToken, storeName, itemID, newDescription);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Edit Item Description.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response RateItem([FromHeader] String Authorization, int itemID, String storeName, int rating, String review)
        {//II.3.3,  II.3.4
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Rate Item called with parameters: authToken={authToken}, itemID={itemID}, storeName={storeName}, rating={rating}, review={review}.");
                _market.RateItem(authToken, itemID, storeName, rating, review);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Rate Item.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response RateStore([FromHeader] String Authorization, String storeName, int rating, String review) // 0 < rating < 10
        {//II.3.4
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Rate Store called with parameters: authToken={authToken}, storeName={storeName}, rating={rating}, review={review}.");
                _market.RateStore(authToken, storeName, rating, review);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Rate Store.");
            } 
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response<StoreDTO> GetStoreInformation([FromHeader] String Authorization, String storeName)
        {//II.2.1
            Response<StoreDTO> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Store Information called with parameters: authToken={authToken}, storeName={storeName}.");
                Store result = _market.GetStoreInformation(authToken, storeName);
                StoreDTO dto = new StoreDTO(result);
                response = new Response<StoreDTO>(dto);
                _logger.LogInformation($"SUCCESSFULY executed Get Store Information.");
            }
            catch (Exception e)
            {
                response = new Response<StoreDTO>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response<List<ItemDTO>> GetItemInformation([FromHeader] String Authorization, String itemName, String itemCategory, String keyWord)
        {//II.2.2
            //filters!!!!!!!!!!!
            Response<List<ItemDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Item Information called with parameters: authToken={authToken}, itemName={itemName}, itemCategory={itemCategory}, keyWord={keyWord}.");
                IDictionary<string, List<Item>> result = _market.GetItemInformation(authToken, itemName, itemCategory, keyWord);
                List<ItemDTO> resultDTO = new List<ItemDTO>();
                foreach(KeyValuePair<string, List<Item>> storeNItems in result)
                {
                    foreach(Item item in storeNItems.Value)
                    {
                        resultDTO.Add(new ItemDTO(item, storeNItems.Key));
                    }   
                }
                response = new Response<List<ItemDTO>>(resultDTO);
                _logger.LogInformation($"SUCCESSFULY executed Get Item Information.");
            }
            catch (Exception e)
            {
                response = new Response<List<ItemDTO>> (null, e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response SendMessageToStore([FromHeader] String Authorization, String storeName, String title, String description, int id)
        {//II.3.5
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Send Message To Store called with parameters: authToken={authToken}, storeName={storeName}, title={title}, description={description}.");
                _market.SendMessageToStore(authToken,storeName, title, description, id);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Send Message To Store.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
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
        public Response FileComplaint([FromHeader] String Authorization, int cartID,  String message)
        {//II.3.6
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Send Message To Store called with parameters: authToken={authToken}, cartID={cartID}, message={message}.");
                _market.FileComplaint(authToken, cartID, message);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed File Complaint.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        [HttpGet("GetMyPurchasesHistory")]
        public Response<ICollection<PurchasedCartDTO>> GetMyPurchasesHistory([FromHeader] String Authorization)
        {//II.3.7
            Response<ICollection<PurchasedCartDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get My Purchases History called with parameters: authToken={authToken}.");
                ICollection<Tuple<DateTime ,ShoppingCart>> purchasedCarts = _market.GetMyPurchases(authToken);
                ICollection<PurchasedCartDTO> purchasedCartsDTO = new List<PurchasedCartDTO>();
                foreach (Tuple<DateTime ,ShoppingCart> purchase in purchasedCarts)
                {
                    purchasedCartsDTO.Add(new PurchasedCartDTO(purchase.Item1, purchase.Item2));
                }

                response = new Response<ICollection<PurchasedCartDTO>>(purchasedCartsDTO);
                _logger.LogInformation($"SUCCESSFULY executed Get My Purchases History.");
            }
            catch (Exception e)
            {
                response = new Response<ICollection<PurchasedCartDTO>>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }

        [HttpGet("GetVisitorInformation")]
        public Response<RegisteredDTO> GetVisitorInformation([FromHeader] String Authorization)
        {//II.3.8
            Response<RegisteredDTO> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Visitor Information called with parameters: authToken={authToken}.");
                Registered registered= _market.GetVisitorInformation(authToken);
                response = new Response<RegisteredDTO>(new RegisteredDTO(registered));
                _logger.LogInformation($"SUCCESSFULY executed Get Visitor Information.");
            }
            catch (Exception e)
            {
                response = new Response<RegisteredDTO>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Boolean EditUsername([FromHeader] String Authorization, String newUsername )
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
        public Response EditVisitorPassword([FromHeader] String Authorization, String oldPassword, String newPassword)
        {//II.3.8
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Edit Visitor Password called with parameters: authToken={authToken}, oldPassword={oldPassword}, newPassword={newPassword}.");
                _market.EditVisitorPassword(authToken, oldPassword, newPassword);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Edit Visitor Password.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response RemoveManagerPermission([FromHeader] String Authorization, String managerUsername, String storeName, string op)//permission param is Enum
        {//II.4.7

            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Remove Manager Permission called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}, op={op}.");
                _market.RemoveManagerPermission(authToken, managerUsername, storeName, op);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Remove Manager Permission.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response AddManagerPermission([FromHeader] String Authorization, String managerUsername, String storeName, String op)//permission param is Enum
        {//II.4.7
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Add Manager Permission called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}, op={op}.");
                _market.AddManagerPermission(authToken, managerUsername, storeName, op);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Add Manager Permission.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response CloseStore([FromHeader] String Authorization, String storeName)
        {//II.4.9
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Close Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.CloseStore(authToken, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Close Store.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response ReopenStore([FromHeader] String Authorization, String storeName)
        {//II.4.10
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Reopen Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.ReopenStore(authToken, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Reopen Store.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response<List<StoreOwnerDTO>> GetStoreOwners([FromHeader] String Authorization, String storeName)
        {//II.4.11
            Response<List<StoreOwnerDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Store Owners called with parameters: authToken={authToken}, storeName={storeName}.");
                List<StoreOwner> ownerList = _market.getStoreOwners(storeName, authToken);
                List<StoreOwnerDTO> ownerDTOlist = new List<StoreOwnerDTO>();
                foreach (StoreOwner owner in ownerList)
                    ownerDTOlist.Add(new StoreOwnerDTO(owner));
                response = new Response<List<StoreOwnerDTO>>(ownerDTOlist);
                _logger.LogInformation($"SUCCESSFULY executed Get Store Owners.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreOwnerDTO>>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response<List<StoreManagerDTO>> GetStoreManagers([FromHeader] String Authorization, String storeName)
        {//II.4.11
            Response<List<StoreManagerDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Store Managers called with parameters: authToken={authToken}, storeName={storeName}.");
                List<StoreManager> managerList = _market.getStoreManagers(storeName, authToken);
                List<StoreManagerDTO> managerDTOlist = new List<StoreManagerDTO>();
                foreach (StoreManager manager in managerList)
                    managerDTOlist.Add(new StoreManagerDTO(manager));
                response = new Response<List<StoreManagerDTO>>(managerDTOlist);
                _logger.LogInformation($"SUCCESSFULY executed Get Store Managers.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreManagerDTO>>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response<StoreFounderDTO> GetStoreFounder([FromHeader] String Authorization, String storeName)
        {//II.4.11
            Response<StoreFounderDTO> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Store Founder called with parameters: authToken={authToken}, storeName={storeName}.");
                StoreFounder founder = _market.getStoreFounder(storeName, authToken);
                response = new Response<StoreFounderDTO>(new StoreFounderDTO(founder));
                _logger.LogInformation($"SUCCESSFULY executed Get Store Founder.");
            }
            catch (Exception e)
            {
                response = new Response<StoreFounderDTO>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response<Queue<MessageToStoreDTO>> GetStoreMessages([FromHeader] String Authorization, String storeName)
        {//II.4.12
            //should return with id
            Response<Queue<MessageToStoreDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                Queue<MessageToStore> messages = _market.GetStoreMessages(authToken, storeName);
                Queue<MessageToStoreDTO> messagesDTOs = new Queue<MessageToStoreDTO>();
                foreach(MessageToStore messageToStore in messages)
                    messagesDTOs.Enqueue(new MessageToStoreDTO(messageToStore));
                response = new Response<Queue<MessageToStoreDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<Queue<MessageToStoreDTO>>(e); _logger.LogError(e.Message);
            }
            return response;
        }

        [HttpGet("GetRegisteredMessagesFromAdmin")]
        public Response<ICollection<AdminMessageToRegisteredDTO>> GetRegisteredMessagesFromAdmin([FromHeader] String Authorization)
        {//II.4.12
            //should return with id
            Response<ICollection<AdminMessageToRegisteredDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                ICollection<AdminMessageToRegistered> messages = _market.GetRegisteredMessagesFromAdmin(authToken);
                ICollection<AdminMessageToRegisteredDTO> messagesDTOs = new List<AdminMessageToRegisteredDTO>();
                foreach (AdminMessageToRegistered message in messages)
                    messagesDTOs.Add(new AdminMessageToRegisteredDTO(message));
                response = new Response<ICollection<AdminMessageToRegisteredDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<AdminMessageToRegisteredDTO>>(e); _logger.LogError(e.Message);
            }
            return response;
        }
        [HttpGet("GetRegisterAnsweredStoreMessages")]
        public Response<ICollection<MessageToStoreDTO>> GetRegisterAnsweredStoreMessages([FromHeader] String Authorization)
        {//II.4.12
            //should return with id
            Response<ICollection<MessageToStoreDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                ICollection<MessageToStore> messages = _market.GetRegisteredAnswerdStoreMessages(authToken);
                ICollection<MessageToStoreDTO> messagesDTOs = new List<MessageToStoreDTO>();
                foreach (MessageToStore message in messages)
                    messagesDTOs.Add(new MessageToStoreDTO(message));
                response = new Response<ICollection<MessageToStoreDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<MessageToStoreDTO>>(e); _logger.LogError(e.Message);
            }
            return response;
        }
        [HttpGet("GetRegisteredMessagesNotofication")]
        public Response<ICollection<NotifyMessageDTO>> GetRegisteredMessagesNotofication([FromHeader] String Authorization)
        {//II.4.12
            //should return with id
            Response<ICollection<NotifyMessageDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                ICollection<NotifyMessage> messages = _market.GetRegisteredMessagesNotofication(authToken);
                ICollection<NotifyMessageDTO> messagesDTOs = new List<NotifyMessageDTO>();
                foreach (NotifyMessage message in messages)
                    messagesDTOs.Add(new NotifyMessageDTO(message));
                response = new Response<ICollection<NotifyMessageDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<NotifyMessageDTO>>(e); _logger.LogError(e.Message);
            }
            return response;
        }

        public Response AnswerStoreMesseage([FromHeader] String Authorization, string receiverUsername, int msgID, string storeName, string reply)
        {//II.4.12
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Answer Store Message called with parameters: authToken={authToken}, msgId={msgID},storeName={storeName} reply={reply}.");
                _market.AnswerStoreMesseage(authToken, receiverUsername, msgID, storeName, reply);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Answer Store Message.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response<List<Tuple<DateTime, ShoppingBasketDTO>>> GetStorePurchasesHistory([FromHeader] String Authorization, String storeName)
        {//II.4.13
            Response<List<Tuple<DateTime, ShoppingBasketDTO>>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Store Purchases History called with parameters: authToken={authToken}, storeName={storeName}.");
                List<Tuple<DateTime, ShoppingBasket>> result = _market.GetStorePurchasesHistory(authToken, storeName);
                List<Tuple<DateTime, ShoppingBasketDTO>> dtos = new List<Tuple<DateTime, ShoppingBasketDTO>>();

                foreach(Tuple<DateTime, ShoppingBasket> tuple in result)
                {
                    ShoppingBasketDTO dto = new ShoppingBasketDTO(tuple.Item2);
                    Tuple<DateTime, ShoppingBasketDTO> toAdd = new Tuple<DateTime, ShoppingBasketDTO>(tuple.Item1, dto);
                    dtos.Add(toAdd);
                }

                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(dtos);
                _logger.LogInformation($"SUCCESSFULY executed Get Store Purchases History.");
            }
            catch (Exception e)
            {
                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response CloseStorePermanently([FromHeader] String Authorization, String storeName)
        {//II.6.1
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Close Store Permanently called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.CloseStorePermanently(authToken, storeName);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Close Store Permanently.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Boolean GetRegisterdComplaints([FromHeader] String Authorization)
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
        public Response ReplyToComplaint([FromHeader] String Authorization, int complaintID, String reply)
        {//II.6.3
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Reply To Complaint called with parameters: authToken={authToken}, complaintID={complaintID}, reply={reply}.");
                _market.ReplyToComplaint(authToken, complaintID, reply);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Reply To Complaint.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response SendMessageToRegisterd([FromHeader] String Authorization, String UsernameReciever, String title, String message)
        {//II.6.3
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Send Message To Registered called with parameters: authToken={authToken},  UsernameReciever={UsernameReciever}, title={title}, message={message}.");
                _market.SendAdminMessageToRegisterd(authToken, UsernameReciever, title, message);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Send Message To Registered.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        [HttpGet("GetStoresOfUser")]
        public Response<List<StoreDTO>> GetStoresOfUser([FromHeader] String Authorization)
        {
            Response<List<StoreDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get Stores Of User called with parameters: authToken={authToken}.");
                List<Store> stores = _market.GetStoresOfUser(authToken);
                List<StoreDTO> storesDTO = new List<StoreDTO>();
                foreach (Store store in stores)
                {
                    storesDTO.Add(new StoreDTO(store));
                }
                response = new Response<List<StoreDTO>>(storesDTO);
                _logger.LogInformation($"SUCCESSFULY executed Get Stores Of User.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreDTO>>(e); _logger.LogError(e.Message);
            }
            return response;
        }
        [HttpGet("GetAllActiveStores")]
        public Response<List<StoreDTO>> GetAllActiveStores([FromHeader] String Authorization)
        {
            Response<List<StoreDTO>> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Get All Active Stores called with parameters: authToken={authToken}.");
                List<Store> stores = _market.GetAllActiveStores(authToken);
                List<StoreDTO> storesDTO = new List<StoreDTO>();
                foreach (Store store in stores)
                {
                    storesDTO.Add(new StoreDTO(store));
                }
                response = new Response<List<StoreDTO>>(storesDTO);
                _logger.LogInformation($"SUCCESSFULY executed Get All Active Stores.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreDTO>>(e); _logger.LogError(e.Message);
            }
            return response;
        }


        [HttpGet("entersystem")]
        public Response<String> EnterSystem() // Generating token and returning it
        { //II.1.1
            Response<String> response;
             try
            {
                
                _logger.LogInformation($"Enter System To Registered called.");
                String token = _market.EnterSystem();   
                response = new Response<String>(token);
                _logger.LogInformation($"SUCCESSFULY executed Enter System.");
            }
            catch (Exception e) 
            { 
                response = new Response<String>(null, e);_logger.LogError(e.Message); 
            }
            return response;
        }
        [HttpGet("ExitSystem")]
        public Response ExitSystem([FromHeader] String Authorization) // Removing cart and token assigned to guest
        { //II.1.2
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Exit System called with parameters: authToken={authToken}.");
                _market.ExitSystem(authToken);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Exit System.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response AppointSystemAdmin([FromHeader] String Authorization, String adminUsername)
        { //II.1.2
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"Appoint System Admin called with parameters: authToken={authToken}, adminUsername={adminUsername}.");
                _market.AppointSystemAdmin(authToken, adminUsername);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Appoint System Admin.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response AddStoreDiscount([FromHeader] String Authorization, String storeName, IDiscountDTO discount_dto)
        {
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                /////////// is log should keep the whole description of the discount??????
                _logger.LogInformation($"Add Store Discount called with parameters: authToken={authToken}, storeName={storeName} and the actual discount.");
                Discount discount = new dtoDiscountConverter().convertDiscount(discount_dto);
                _market.AddStoreDiscount(authToken, storeName, discount);
                response = new Response();
                _logger.LogInformation($"SUCCESSFULY executed Add Store Discount.");
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response<Double> CalcCartActualPrice([FromHeader] String Authorization)
        {
            Response<Double> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"calculate shopping cart actual price - called with parameters: authToken={authToken}.");
                Double price = _market.CalcCartActualPrice(authToken);
                response = new Response<Double>(price);
                _logger.LogInformation($"SUCCESSFULY executed Calculate Cart Actual Price.");
            }
            catch (Exception e)
            {
                response = new Response<Double>(e);
            }
            return response;
        }

        public Response<String> GetCartReceipt([FromHeader] String Authorization)
        {
            Response<String> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _logger.LogInformation($"get current cart info - called with parameters: authToken={authToken}.");
                String receipt = _market.GetCartReceipt(authToken);
                response = new Response<String>(receipt);
                _logger.LogInformation($"SUCCESSFULY executed Get Cart Receipt.");
            }
            catch (Exception e)
            {
                response = new Response<String>(e); _logger.LogError(e.Message);
            }
            return response;
        }

        public Response HasPermission([FromHeader] String Authorization, string storeName, string op)
        {
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _market.HasPermission(storeName, authToken, op);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }
        public Response IsStoreActive([FromHeader] String Authorization, string storeName, string op)
        {
            Response response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                _market.HasPermission(storeName, authToken, op);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e);_logger.LogError(e.Message);
            }
            return response;
        }

        public Response<ItemDTO> GetItem([FromHeader] String Authorization, string storeName, int itemId)
        {
            Response<ItemDTO> response;
             try
            {
                
                String authToken = parseAutherization(Authorization);
                Item item = _market.GetItem(authToken, storeName, itemId);
                response = new Response<ItemDTO>(new ItemDTO(item));
            }
            catch (Exception e)
            {
                response = new Response<ItemDTO>(null, e);_logger.LogError(e.Message);
            }
            return response;
        }
    }
}

