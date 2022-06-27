using MarketWeb.Server.Domain;
using MarketWeb.Server.Service;
using MarketWeb.Server.Domain.PolicyPackage;
using MarketWeb.Shared;
using MarketWeb.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MarketWeb.Server.DataLayer;

namespace MarketWeb.Service
{
    [Route("api/market/")]
    [ApiController]
    public class MarketAPI : ControllerBase
    {
        private Market _market;
        private static readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //private ILogger<MarketAPI> _logger;
        private int _id;
        private bool testMode = false;
        private static bool useInitializationFile = true;
        private static bool useConfigurationFile = true;
        public MarketAPI(Market market, ILogger<MarketAPI> logger)
        {
            if (market == null)
            {
                _market = new Market();
                testMode = true;
                Server.DataLayer.DalController.GetInstance(true);
                // V This line causes the acceptance tests to be super slow! V
                useConfigurationFile = true;
            }
            else
            {
                _market = market;
            }

            if (useConfigurationFile)
            {
                useConfigurationFile = false;
                try
                {
                    Dictionary<String, String> configurations = new ConfigurationFileParser().ParseConfigurationFile();
                    _market.RestartSystem(configurations["admin_username"], configurations["admin_password"], configurations["external_shipping"], configurations["external_payment"], configurations["db_ip"], configurations["db_initial_catalog"], configurations["db_username"], configurations["db_password"]).Wait();
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                    _logger.Error("Unable to load configuration properly, exiting system.");
                    Environment.Exit(-1);
                }
            }

            if (useInitializationFile)
            {
                useInitializationFile = false;
                bool restore = testMode;
                testMode = true;
                try
                {
                    new InitializationFileParser(this).ParseInitializationFile();
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
                }
                testMode = restore;
            }
        }

        private String parseAutherization(String Authorization)
        {
            if (testMode)
            {
                return Authorization;
            }
            if (!AuthenticationHeaderValue.TryParse(Authorization, out var headerValue))
            {
                throw new Exception("Invalid token!");
            }
            return headerValue.Parameter;
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

                _logger.Info($"Login called with parameters: authToken={authToken}, username={Username}, password={password}.");
                // TODO: Transfer cart? Using authToken
                String loginToken = _market.Login(authToken, Username, password);
                ConnectedUser.ChangeToken(authToken, loginToken);
                response = new Response<String>(loginToken);
                _logger.Info($"SUCCESSFULY executed Login.");
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e); _logger.Error(e.Message);
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
                String guestToken = _market.Logout(authToken);
                response = new Response<String>(guestToken);
                _logger.Info($"SUCCESSFULY executed Logout.");
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e); _logger.Error(e.Message);
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
                _logger.Info($"Register called with parameters: authToken={authToken}, username={Username}, password={password}.");
                _market.Register(authToken, Username, password, birthDate);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Register.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
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
        public Response RemoveRegisteredVisitor([FromHeader] String Authorization, String usr_toremove)
        {//II.6.2
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Remove Registered Visitor called with parameters: authToken={authToken}, username={usr_toremove}.");
                _market.RemoveRegisteredVisitor(authToken, usr_toremove);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Remove Registered Vistor.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("AddItemToCart")]
        public Response AddItemToCart([FromHeader] String Authorization, int itemID, String storeName, int amount)
        {//II.2.3
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Item To Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}, amount={amount}.");
                _market.AddItemToCart(authToken, itemID, storeName, amount);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Add Item To Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RemoveItemFromCart")]
        public Response RemoveItemFromCart([FromHeader] String Authorization, int itemID, String storeName)
        {//II.2.4
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Remove Item From Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}.");
                _market.RemoveItemFromCart(authToken, itemID, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Remove Item From Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RemoveAcceptedBidFromCart")]
        public Response RemoveAcceptedBidFromCart([FromHeader] String Authorization, int itemID, String storeName)
        {//II.2.4
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Remove Accepted bid From Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}.");
                _market.RemoveAcceptedBidFromCart(authToken, itemID, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Remove Accepted Bid From Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("UpdateQuantityOfItemInCart")]
        public Response UpdateQuantityOfItemInCart([FromHeader] String Authorization, int itemID, String storeName, int newQuantity)
        {//II.2.4
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Update Quantity Of Item In Cart called with parameters: authToken={authToken}, itemId={itemID}, storeName={storeName}, newQuantity={newQuantity}.");
                _market.UpdateQuantityOfItemInCart(authToken, itemID, storeName, newQuantity);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Update Quantity Of Item In Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
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
                _logger.Info($"View My Cart called with parameters: authToken={authToken}.");
                ShoppingCart shoppingCart = _market.ViewMyCart(authToken);
                response = new Response<ShoppingCartDTO>(new DTOtranslator().toDTO(shoppingCart));
                _logger.Info($"SUCCESSFULY executed View My Cart.");
            }
            catch (Exception e)
            {
                response = new Response<ShoppingCartDTO>(null, e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("PurchaseMyCart")]
        public async Task<Response> PurchaseMyCart([FromHeader] String Authorization, String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode,  string cardNumber = null, string month = null, string year = null, string holder = null, string ccv = null, string id = null)
        {//II.2.5
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Purchase My Cart called with parameters: authToken={authToken}, address={address}, city={city}, country={country}, zip={zip}, purchaserName={purchaserName}.");
                await _market.PurchaseMyCartAsync(authToken, address, city, country, zip, purchaserName, paymentMethode, shipmentMethode, cardNumber, month, year, holder, ccv, id);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Purchase My Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("OpenNewStore")]
        public Response OpenNewStore([FromHeader] String Authorization, String storeName)
        {//II.3.2
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Open New Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.OpenNewStore(authToken, storeName, new PurchasePolicy(), new DiscountPolicy());
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Open New Store.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("AddStoreManager")]
        public Response AddStoreManager([FromHeader] String Authorization, String managerUsername, String storeName)
        {//II.4.6
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Store Manager called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}.");
                _market.AddStoreManager(authToken, managerUsername, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Add Store Manager.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RemoveStoreOwner")]
        public Response RemoveStoreOwner([FromHeader] String Authorization, String ownerUsername, String storeName)
        {//II.4.5
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Remove Store Owner called with parameters: authToken={authToken}, ownerUsername={ownerUsername}, storeName={storeName}.");
                _market.RemoveStoreOwner(authToken, ownerUsername, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Remove Store Owner.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RemoveStoreManager")]
        public Response RemoveStoreManager([FromHeader] String Authorization, String managerUsername, String storeName)
        {//II.4.8
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Remove Store Manager called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}.");
                _market.RemoveStoreManager(authToken, managerUsername, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Remove Store Manager.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("AddItemToStoreStock")]
        public Response<int> AddItemToStoreStock([FromHeader] String Authorization, String storeName, String name, double price, String description, String category, int quantity)
        {//II.4.1
            Response<int> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Item To Stock called with parameters: authToken={authToken}, storeName={storeName}, name={name}, price={price}, description={description}, category={category}, quantity={quantity}.");
                int id = _market.AddItemToStoreStock(authToken, storeName, name, price, description, category, quantity);
                response = new Response<int>(id);
                _logger.Info($"SUCCESSFULY executed Add Item To Stock.");
            }
            catch (Exception e)
            {
                response = new Response<int>(e); _logger.Error(e.Message);
            }
            return response;
        }        
        [HttpPost("RemoveItemFromStore")]
        public Response RemoveItemFromStore([FromHeader] String Authorization, String storeName, int itemID)
        {//II.4.1
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Remove Item From Stock called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}.");
                _market.RemoveItemFromStore(authToken, storeName, itemID);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Remove Item From Stock.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("UpdateStockQuantityOfItem")]
        public Response UpdateStockQuantityOfItem([FromHeader] String Authorization, String storeName, int itemID, int newQuantity)
        {//II.4.1
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Update Stock Quantity Of Item called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newQuantity={newQuantity}.");
                _market.UpdateStockQuantityOfItem(authToken, storeName, itemID, newQuantity);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Update Stock Quantity Of Item.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("EditItemPrice")]
        public Response EditItemPrice([FromHeader] String Authorization, String storeName, int itemID, double newPrice)
        {//II.4.1
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Edit Item Price called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newPrice={newPrice}.");
                _market.EditItemPrice(authToken, storeName, itemID, newPrice);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Edit Item Price.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("EditItemName")]
        public Response EditItemName([FromHeader] String Authorization, String storeName, int itemID, String newName)
        {//II.4.1
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Edit Item Name called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newName={newName}.");
                _market.EditItemName(authToken, storeName, itemID, newName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Edit Item Name.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("EditItemDescription")]
        public Response EditItemDescription([FromHeader] String Authorization, String storeName, int itemID, String newDescription)
        {//II.4.1
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Edit Item Description called with parameters: authToken={authToken}, storeName={storeName}, itemID={itemID}, newDescription={newDescription}.");
                _market.EditItemDescription(authToken, storeName, itemID, newDescription);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Edit Item Description.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RateItem")]
        public Response RateItem([FromHeader] String Authorization, int itemID, String storeName, int rating, String review)
        {//II.3.3,  II.3.4
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Rate Item called with parameters: authToken={authToken}, itemID={itemID}, storeName={storeName}, rating={rating}, review={review}.");
                _market.RateItem(authToken, itemID, storeName, rating, review);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Rate Item.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RateStore")]
        public Response RateStore([FromHeader] String Authorization, String storeName, int rating, String review) // 0 < rating < 10
        {//II.3.4
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Rate Store called with parameters: authToken={authToken}, storeName={storeName}, rating={rating}, review={review}.");
                _market.RateStore(authToken, storeName, rating, review);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Rate Store.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpGet("GetStoreInformation")]
        public Response<StoreDTO> GetStoreInformation([FromHeader] String Authorization, String storeName)
        {//II.2.1
            Response<StoreDTO> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Store Information called with parameters: authToken={authToken}, storeName={storeName}.");
                Store result = _market.GetStoreInformation(authToken, storeName);
                StoreDTO dto = new DTOtranslator().toDTO(result);
                response = new Response<StoreDTO>(dto);
                _logger.Info($"SUCCESSFULY executed Get Store Information.");
            }
            catch (Exception e)
            {
                response = new Response<StoreDTO>(null, e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpGet("GetItemInformation")]
        public Response<List<ItemDTO>> GetItemInformation([FromHeader] String Authorization, String itemName, String itemCategory, String keyWord)
        {//II.2.2
            //filters!!!!!!!!!!!
            Response<List<ItemDTO>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Item Information called with parameters: authToken={authToken}, itemName={itemName}, itemCategory={itemCategory}, keyWord={keyWord}.");
                IDictionary<string, List<Item>> result = _market.GetItemInformation(authToken, itemName, itemCategory, keyWord);
                List<ItemDTO> resultDTO = new List<ItemDTO>();
                foreach (KeyValuePair<string, List<Item>> storeNItems in result)
                {
                    foreach (Item item in storeNItems.Value)
                    {
                        resultDTO.Add(new DTOtranslator().toDTO(item, storeNItems.Key));
                    }
                }
                response = new Response<List<ItemDTO>>(resultDTO);
                _logger.Info($"SUCCESSFULY executed Get Item Information.");
            }
            catch (Exception e)
            {
                response = new Response<List<ItemDTO>>(null, e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("SendMessageToStore")]
        public Response SendMessageToStore([FromHeader] String Authorization, String storeName, String title, String description)
        {//II.3.5
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Send Message To Store called with parameters: authToken={authToken}, storeName={storeName}, title={title}, description={description}.");
                _market.SendMessageToStore(authToken, storeName, title, description);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Send Message To Store.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
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
        [HttpPost("FileComplaint")]
        public Response FileComplaint([FromHeader] String Authorization, int cartID, String message)
        {//II.3.6
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Send Message To Store called with parameters: authToken={authToken}, cartID={cartID}, message={message}.");
                _market.FileComplaint(authToken, cartID, message);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed File Complaint.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpGet("GetMyPurchasesHistory")]
        public Response<List<Tuple<DateTime, ShoppingCartDTO>>> GetMyPurchasesHistory([FromHeader] String Authorization)
        {//II.3.7
            Response<List<Tuple<DateTime, ShoppingCartDTO>>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get My Purchases History called with parameters: authToken={authToken}.");
                ICollection<Tuple<DateTime, ShoppingCart>> purchasedCarts = _market.GetMyPurchases(authToken);
                List<PurchasedCartDTO> purchasedCartsDTO = new List<PurchasedCartDTO>();
                foreach (Tuple<DateTime, ShoppingCart> purchase in purchasedCarts)
                {
                    purchasedCartsDTO.Add(new DTOtranslator().toDTO(purchase.Item1, purchase.Item2));
                }
                List<Tuple<DateTime, ShoppingCartDTO>> something = new List<Tuple<DateTime, ShoppingCartDTO>>();
                foreach (PurchasedCartDTO p in purchasedCartsDTO)
                {
                    something.Add(new Tuple<DateTime, ShoppingCartDTO>(p.Date, p.ShoppingCart));
                }
                response = new Response<List<Tuple<DateTime, ShoppingCartDTO>>>(something);
                _logger.Info($"SUCCESSFULY executed Get My Purchases History.");
            }
            catch (Exception e)
            {
                response = new Response<List<Tuple<DateTime, ShoppingCartDTO>>>(null, e); _logger.Error(e.Message);
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
                _logger.Info($"Get Visitor Information called with parameters: authToken={authToken}.");
                Registered registered = _market.GetVisitorInformation(authToken);
                response = new Response<RegisteredDTO>(new DTOtranslator().toDTO(registered));
                _logger.Info($"SUCCESSFULY executed Get Visitor Information.");
            }
            catch (Exception e)
            {
                response = new Response<RegisteredDTO>(null, e); _logger.Error(e.Message);
            }
            return response;
        }

        public Boolean EditUsername([FromHeader] String Authorization, String newUsername)
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
        [HttpPost("EditVisitorPassword")]
        public Response EditVisitorPassword([FromHeader] String Authorization, String oldPassword, String newPassword)
        {//II.3.8
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Edit Visitor Password called with parameters: authToken={authToken}, oldPassword={oldPassword}, newPassword={newPassword}.");
                _market.EditVisitorPassword(authToken, oldPassword, newPassword);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Edit Visitor Password.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("RemoveManagerPermission")]
        public Response RemoveManagerPermission([FromHeader] String Authorization, String managerUsername, String storeName, string op)//permission param is Enum
        {//II.4.7

            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Remove Manager Permission called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}, op={op}.");
                _market.RemoveManagerPermission(authToken, managerUsername, storeName, op);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Remove Manager Permission.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("AddManagerPermission")]
        public Response AddManagerPermission([FromHeader] String Authorization, String managerUsername, String storeName, String op)//permission param is Enum
        {//II.4.7
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Manager Permission called with parameters: authToken={authToken}, managerUsername={managerUsername}, storeName={storeName}, op={op}.");
                _market.AddManagerPermission(authToken, managerUsername, storeName, op);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Add Manager Permission.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("CloseStore")]
        public Response CloseStore([FromHeader] String Authorization, String storeName)
        {//II.4.9
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Close Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.CloseStore(authToken, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Close Store.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("ReopenStore")]
        public Response ReopenStore([FromHeader] String Authorization, String storeName)
        {//II.4.10
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Reopen Store called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.ReopenStore(authToken, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Reopen Store.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("GetStoreOwners")]
        public Response<List<StoreOwnerDTO>> GetStoreOwners([FromHeader] String Authorization, String storeName)
        {//II.4.11
            Response<List<StoreOwnerDTO>> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Store Owners called with parameters: authToken={authToken}, storeName={storeName}.");
                List<StoreOwner> ownerList = _market.getStoreOwners(storeName, authToken);
                List<StoreOwnerDTO> ownerDTOlist = new List<StoreOwnerDTO>();
                foreach (StoreOwner owner in ownerList)
                    ownerDTOlist.Add(new DTOtranslator().toDTO(owner));
                response = new Response<List<StoreOwnerDTO>>(ownerDTOlist);
                _logger.Info($"SUCCESSFULY executed Get Store Owners.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreOwnerDTO>>(null, e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("GetStoreManagers")]
        public Response<List<StoreManagerDTO>> GetStoreManagers([FromHeader] String Authorization, String storeName)
        {//II.4.11
            Response<List<StoreManagerDTO>> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Store Managers called with parameters: authToken={authToken}, storeName={storeName}.");
                List<StoreManager> managerList = _market.getStoreManagers(storeName, authToken);
                List<StoreManagerDTO> managerDTOlist = new List<StoreManagerDTO>();
                foreach (StoreManager manager in managerList)
                    managerDTOlist.Add(new DTOtranslator().toDTO(manager));
                response = new Response<List<StoreManagerDTO>>(managerDTOlist);
                _logger.Info($"SUCCESSFULY executed Get Store Managers.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreManagerDTO>>(null, e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("GetStoreFounder")]
        public Response<StoreFounderDTO> GetStoreFounder([FromHeader] String Authorization, String storeName)
        {//II.4.11
            Response<StoreFounderDTO> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Store Founder called with parameters: authToken={authToken}, storeName={storeName}.");
                StoreFounder founder = _market.getStoreFounder(storeName, authToken);
                response = new Response<StoreFounderDTO>(new DTOtranslator().toDTO(founder));
                _logger.Info($"SUCCESSFULY executed Get Store Founder.");
            }
            catch (Exception e)
            {
                response = new Response<StoreFounderDTO>(null, e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpGet("GetStoreMessages")]
        public Response<List<MessageToStoreDTO>> GetStoreMessages([FromHeader] String Authorization, String storeName)
        {//II.4.12
            //should return with id
            Response<List<MessageToStoreDTO>> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                List<MessageToStore> messages = _market.GetStoreMessages(authToken, storeName);
                List<MessageToStoreDTO> messagesDTOs = new List<MessageToStoreDTO>();
                foreach (MessageToStore messageToStore in messages)
                    messagesDTOs.Add(new DTOtranslator().toDTO(messageToStore));
                response = new Response<List<MessageToStoreDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<List<MessageToStoreDTO>>(e); _logger.Error(e.Message);
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
                    messagesDTOs.Add(new DTOtranslator().toDTO(message));
                response = new Response<ICollection<AdminMessageToRegisteredDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<AdminMessageToRegisteredDTO>>(e); _logger.Error(e.Message);
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
                    messagesDTOs.Add(new DTOtranslator().toDTO(message));
                response = new Response<ICollection<MessageToStoreDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<MessageToStoreDTO>>(e); _logger.Error(e.Message);
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
                    messagesDTOs.Add(new DTOtranslator().toDTO(message));
                response = new Response<ICollection<NotifyMessageDTO>>(messagesDTOs);
            }
            catch (Exception e)
            {
                response = new Response<ICollection<NotifyMessageDTO>>(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("AnswerStoreMesseage")]
        public Response AnswerStoreMesseage([FromHeader] String Authorization, string receiverUsername, int msgID, string storeName, string reply)
        {//II.4.12
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Answer Store Message called with parameters: authToken={authToken}, msgId={msgID},storeName={storeName} reply={reply}.");
                _market.AnswerStoreMesseage(authToken, receiverUsername, msgID, storeName, reply);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Answer Store Message.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("GetStorePurchasesHistory")]
        public Response<List<Tuple<DateTime, ShoppingBasketDTO>>> GetStorePurchasesHistory([FromHeader] String Authorization, String storeName)
        {//II.4.13
            Response<List<Tuple<DateTime, ShoppingBasketDTO>>> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Store Purchases History called with parameters: authToken={authToken}, storeName={storeName}.");
                List<Tuple<DateTime, ShoppingBasket>> result = _market.GetStorePurchasesHistory(authToken, storeName);
                List<Tuple<DateTime, ShoppingBasketDTO>> dtos = new List<Tuple<DateTime, ShoppingBasketDTO>>();

                foreach (Tuple<DateTime, ShoppingBasket> tuple in result)
                {
                    ShoppingBasketDTO dto = new DTOtranslator().toDTO(tuple.Item2);
                    Tuple<DateTime, ShoppingBasketDTO> toAdd = new Tuple<DateTime, ShoppingBasketDTO>(tuple.Item1, dto);
                    dtos.Add(toAdd);
                }

                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(dtos);
                _logger.Info($"SUCCESSFULY executed Get Store Purchases History.");
            }
            catch (Exception e)
            {
                response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(null, e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("CloseStorePermanently")]
        public Response CloseStorePermanently([FromHeader] String Authorization, String storeName)
        {//II.6.1
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Close Store Permanently called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.CloseStorePermanently(authToken, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Close Store Permanently.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("GetRegisterdComplaints")]
        public Response<ICollection<ComplaintDTO>> GetRegisterdComplaints([FromHeader] String Authorization)
        {//II.6.3
            Response<ICollection<ComplaintDTO>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Registered Complaints called with parameters: authToken={authToken}.");
                IDictionary<int, Complaint> complaints = _market.GetRegisterdComplaints(authToken);
                ICollection<ComplaintDTO> result = new List<ComplaintDTO>();
                foreach (KeyValuePair<int, Complaint> c in complaints)
                    result.Add(new DTOtranslator().toDTO(c.Value));
                response = new Response<ICollection<ComplaintDTO>>(result);
                _logger.Info($"SUCCESSFULY executed Get Registered Complaints.");
            }
            catch (Exception e)
            {
                response = new Response<ICollection<ComplaintDTO>>(e); _logger.Error(e.Message);
            }
            return response;
        }

        /// <summary>
        /// <para> For Req II.6.3. </para>
        /// <para> System admin replies to a complaint he received.</para>
        /// </summary>
        /// <param name="authToken"> The authorisation token of the system admin.</param>
        /// <param name="complaintID"> The ID of the complaint. </param>
        /// <param name="reply"> The response to the complaint. </param>
        [HttpPost("ReplyToComplaint")]
        public Response ReplyToComplaint([FromHeader] String Authorization, int complaintID, String reply)
        {//II.6.3
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Reply To Complaint called with parameters: authToken={authToken}, complaintID={complaintID}, reply={reply}.");
                _market.ReplyToComplaint(authToken, complaintID, reply);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Reply To Complaint.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("SendMessageToRegisterd")]
        public Response SendMessageToRegisterd([FromHeader] String Authorization, String UsernameReciever, String title, String message)
        {//II.6.3
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Send Message To Registered called with parameters: authToken={authToken},  UsernameReciever={UsernameReciever}, title={title}, message={message}.");
                _market.SendAdminMessageToRegisterd(authToken, UsernameReciever, title, message);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Send Message To Registered.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
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
                _logger.Info($"Get Stores Of User called with parameters: authToken={authToken}.");
                List<Store> stores = _market.GetStoresOfUser(authToken);
                List<StoreDTO> storesDTO = new List<StoreDTO>();
                foreach (Store store in stores)
                {
                    storesDTO.Add(new DTOtranslator().toDTO(store));
                }
                response = new Response<List<StoreDTO>>(storesDTO);
                _logger.Info($"SUCCESSFULY executed Get Stores Of User.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreDTO>>(e); _logger.Error(e.Message);
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
                _logger.Info($"Get All Active Stores called with parameters: authToken={authToken}.");
                List<Store> stores = _market.GetAllActiveStores(authToken);
                List<StoreDTO> storesDTO = new List<StoreDTO>();
                foreach (Store store in stores)
                {
                    storesDTO.Add(new DTOtranslator().toDTO(store));
                }
                response = new Response<List<StoreDTO>>(storesDTO);
                _logger.Info($"SUCCESSFULY executed Get All Active Stores.");
            }
            catch (Exception e)
            {
                response = new Response<List<StoreDTO>>(e); _logger.Error(e.Message);
            }
            return response;
        }


        [HttpGet("entersystem")]
        public Response<String> EnterSystem() // Generating token and returning it
        { //II.1.1
            Response<String> response;
            try
            {

                _logger.Info($"Enter System To Registered called.");
                String token = _market.EnterSystem();
                response = new Response<String>(token);
                _logger.Info($"SUCCESSFULY executed Enter System.");
            }
            catch (Exception e)
            {
                response = new Response<String>(null, e); _logger.Error(e.Message);
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
                _logger.Info($"Exit System called with parameters: authToken={authToken}.");
                _market.ExitSystem(authToken);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Exit System.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("AppointSystemAdmin")]
        public Response AppointSystemAdmin([FromHeader] String Authorization, String adminUsername)
        { //II.1.2
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"Appoint System Admin called with parameters: authToken={authToken}, adminUsername={adminUsername}.");
                _market.AppointSystemAdmin(authToken, adminUsername);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Appoint System Admin.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("AddStoreDiscount")]
        public Response AddStoreDiscount([FromHeader] String Authorization, String storeName, String conditionString, String discountString)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Store Discount called with parameters: authToken={authToken}, storeName={storeName}, conditionString={conditionString}, discountString={discountString}.");
                _market.AddStoreDiscount(authToken, storeName, conditionString, discountString);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Add Store Discount.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("ResetStoreDiscountPolicy")]
        public Response ResetStoreDiscountPolicy([FromHeader] String Authorization, String storeName)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Reset Store Discount Policy called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.ResetStoreDiscountPolicy(authToken, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Reset Store Discount Policy.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("ResetStorePurchasePolicy")]
        public Response ResetStorePurchasePolicy([FromHeader] String Authorization, String storeName)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Reset Store Purchase Policy called with parameters: authToken={authToken}, storeName={storeName}.");
                _market.ResetStorePurchasePolicy(authToken, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Reset Store Purchase Policy.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpGet("GetDiscountPolicyStrings")]
        public Response<List<String>> GetDiscountPolicyStrings([FromHeader] String Authorization, String storeName)
        {
            Response<List<String>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Store Discount Policy called with parameters: authToken={authToken}, storeName={storeName}.");
                response = new Response<List<String>>(_market.GetDiscountPolicyStrings(authToken, storeName));
                _logger.Info($"SUCCESSFULY executed Get Store Discount Policy.");
            }
            catch (Exception e)
            {
                response = new Response<List<String>>(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpGet("GetPurchasePolicyStrings")]
        public Response<List<String>> GetPurchasePolicyStrings([FromHeader] String Authorization, String storeName)
        {
            Response<List<String>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Store Purchase Policy called with parameters: authToken={authToken}, storeName={storeName}.");
                response = new Response<List<String>>(_market.GetPurchasePolicyStrings(authToken, storeName));
                _logger.Info($"SUCCESSFULY executed Get Store Purchase Policy.");
            }
            catch (Exception e)
            {
                response = new Response<List<String>>(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("AddStorePurchasePolicy")]
        public Response AddStorePurchasePolicy([FromHeader] String Authorization, String storeName, String conditionString)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Store Purchase Policy called with parameters: authToken={authToken}, storeName={storeName}, conditionString={conditionString}.");
                _market.AddStorePurchasePolicy(authToken, storeName, conditionString);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Add Store Purchase Policy.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("CalcCartActualPrice")]
        public Response<Double> CalcCartActualPrice([FromHeader] String Authorization)
        {
            Response<Double> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"calculate shopping cart actual price - called with parameters: authToken={authToken}.");
                Double price = _market.CalcCartActualPrice(authToken);
                response = new Response<Double>(price);
                _logger.Info($"SUCCESSFULY executed Calculate Cart Actual Price.");
            }
            catch (Exception e)
            {
                response = new Response<Double>(e);
            }
            return response;
        }

        [HttpPost("GetCartReceipt")]
        public Response<String> GetCartReceipt([FromHeader] String Authorization)
        {
            Response<String> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _logger.Info($"get current cart info - called with parameters: authToken={authToken}.");
                String receipt = _market.GetCartReceipt(authToken);
                response = new Response<String>(receipt);
                _logger.Info($"SUCCESSFULY executed Get Cart Receipt.");
            }
            catch (Exception e)
            {
                response = new Response<String>(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpPost("HasPermission")]
        public Response HasPermission([FromHeader] String Authorization, string storeName, string op)
        {
            Response response;
            try
            {

                String authToken = parseAutherization(Authorization);
                _market.HasPermission(authToken, storeName, op);
                response = new Response();
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }


        [HttpPost("IsStoreActive")]
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
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }

        [HttpGet("GetItem")]
        public Response<ItemDTO> GetItem([FromHeader] String Authorization, string storeName, int itemId)
        {
            Response<ItemDTO> response;
            try
            {

                String authToken = parseAutherization(Authorization);
                Item item = _market.GetItem(authToken, storeName, itemId);
                response = new Response<ItemDTO>(new DTOtranslator().toDTO(item, storeName));
            }
            catch (Exception e)
            {
                response = new Response<ItemDTO>(null, e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpGet("GetPaymentMethods")]
        public Response<List<String>> GetPaymentMethods()
        {
            Response<List<String>> response;
            try
            {

                List<String> paymentMethods = _market.GetPaymentMethods();
                response = new Response<List<String>>(paymentMethods);
            }
            catch (Exception e)
            {
                response = new Response<List<String>>(null, e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpGet("GetShipmentMethods")]
        public Response<List<String>> GetShipmentMethods()
        {
            Response<List<String>> response;
            try
            {
                List<String> paymentMethods = _market.GetShipmentMethods();
                    response = new Response<List<String>>(paymentMethods);
                }
                catch (Exception e)
                {
                    response = new Response<List<String>>(null, e); _logger.Error(e.Message);
                }
            return response;
        }

        [HttpPost("BidItemInStore")]
        public Response BidItemInStore([FromHeader] String Authorization, string storeName, int itemId, int amount, double newPrice)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Bid Item In Store called with parameters: authToken={authToken}, storeName={storeName}, itemId={itemId}, amount={amount}, newPrice={newPrice}.");
                _market.BidItemInStore(authToken, storeName, itemId, amount, newPrice);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Bid Item In Store.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("AcceptBid")]
        public Response AcceptBid([FromHeader] String Authorization, string storeName, int itemId, string bidder)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Accept Bid called with parameters: authToken={authToken}, storeName={storeName}, itemId={itemId}, bidder={bidder}.");
                _market.AcceptBid(authToken, storeName, itemId, bidder);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Accept Bid.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("CounterOfferBid")]
        public Response CounterOfferBid([FromHeader] String Authorization, string storeName, int itemId, string bidder, double counterOffer)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Counter-Offer Bid called with parameters: authToken={authToken}, storeName={storeName}, itemId={itemId}, bidder={bidder}, counterOffer={counterOffer}.");
                _market.CounterOfferBid(authToken, storeName, itemId, bidder, counterOffer);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed CounterOffer Bid.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RejectBid")]
        public Response RejectBid([FromHeader] String Authorization, string storeName, int itemId, string bidder)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Reject Bid called with parameters: authToken={authToken}, storeName={storeName}, itemId={itemId}, bidder={bidder}.");
                _market.RejectBid(authToken, storeName, itemId, bidder);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Reject Bid.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("GetUsernamesWithInventoryPermissionInStore")]
        public Response<List<String>> GetUsernamesWithInventoryPermissionInStore([FromHeader] String Authorization, string storeName)
        {
            Response<List<String>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                response = new Response<List<String>>(_market.GetUsernamesWithPermissionInStore(authToken, storeName, Operation.STOCK_EDITOR));
            }
            catch (Exception e)
            {
                response = new Response<List<String>>(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("GetBidsForStore")]
        public Response<List<BidDTO>> GetBidsForStore([FromHeader] String Authorization, string storeName)
        {
            Response<List<BidDTO>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Bids For Store called with parameters: authToken={authToken}, storeName={storeName}.");
                List<Bid> bids = _market.GetBidsForStore(authToken, storeName);
                List<BidDTO> dtoBids = new List<BidDTO>();
                DTOtranslator translator = new DTOtranslator();
                foreach (Bid bid in bids)
                {
                    dtoBids.Add(translator.toDTO(bid, _market.GetItem(authToken, storeName, bid.ItemID)._price));
                }
                response = new Response<List<BidDTO>>(dtoBids);
                _logger.Info($"SUCCESSFULY executed Get Bids For Store.");
            }
            catch (Exception e)
            {
                response = new Response<List<BidDTO>>(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("AddAcceptedBidToCart")]
        public Response AddAcceptedBidToCart([FromHeader] String Authorization, int itemId, string storeName, int amount)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Acceted Bid To Cart called with parameters: authToken={authToken}, storeName={storeName}, amount={amount}.");
                _market.AddAcceptedBidToCart(authToken, itemId, storeName, amount);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Add Accepted Bid To Cart.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("GetVisitorBidsAtStore")]
        public Response<List<BidDTO>> GetVisitorBidsAtStore([FromHeader] String Authorization, string storeName)
        {
            Response<List<BidDTO>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Visitor Bids At Store called with parameters: authToken={authToken}, storeName={storeName}.");
                List<Bid> bids = _market.GetVisitorBidsAtStore(authToken, storeName);
                List<BidDTO> dtoBids = new List<BidDTO>();
                DTOtranslator translator = new DTOtranslator();
                foreach (Bid bid in bids)
                    dtoBids.Add(translator.toDTO(bid, _market.GetItem(authToken, storeName, bid.ItemID)._price));
                response = new Response<List<BidDTO>>(dtoBids);
                _logger.Info($"SUCCESSFULY executed Get Visitor Bids At Store.");
            }
            catch (Exception e)
            {
                response = new Response<List<BidDTO>>(e); _logger.Error(e.Message);
            }
            return response;
        }
        public Response AddStoreOwnerForTestPurposes(String Authorization, String ownerUsername, String storeName)
        {//II.4.4
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Add Store Owner For Test Purposes Only called with parameters: authToken={authToken}, ownerUsername={ownerUsername}, storeName={storeName}.");
                _market.AddStoreOwnerForTestPurposes(authToken, ownerUsername, storeName);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Add Store Owner.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("AcceptOwnerAppointment")]
        public Response<bool> AcceptOwnerAppointment([FromHeader] String Authorization, String newOwner, String storeName)
        {//II.4.4
            Response<bool> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Accept Store Owner called with parameters: authToken={authToken}, storeName={storeName}, ownerUsername={newOwner}.");
                bool res = _market.AcceptOwnerAppointment(authToken, storeName, newOwner);
                response = new Response<bool>(res);
                _logger.Info($"SUCCESSFULY executed Accept Store Owner.");
            }
            catch (Exception e)
            {
                response = new Response<bool>(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("RejectOwnerAppointment")]
        public Response RejectOwnerAppointment([FromHeader] String Authorization, string storeName, string newOwner)
        {
            Response response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Reject Owner Appointment called with parameters: authToken={authToken}, storeName={storeName}, ownerUsername={newOwner}.");
                _market.RejectOwnerAppointment(authToken, storeName, newOwner);
                response = new Response();
                _logger.Info($"SUCCESSFULY executed Reject Owner Appointment.");
            }
            catch (Exception e)
            {
                response = new Response(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("GetUsernamesWithOwnerAppointmentPermissionInStore")]
        public Response<List<String>> GetUsernamesWithOwnerAppointmentPermissionInStore([FromHeader] String Authorization, string storeName)
        {
            Response<List<String>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                response = new Response<List<String>>(_market.GetUsernamesWithPermissionInStore(authToken, storeName, Operation.APPOINT_OWNER));
            }
            catch (Exception e)
            {
                response = new Response<List<String>>(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("GetStandbyOwnersInStore")]
        public Response<Dictionary<string, List<string>>> GetStandbyOwnersInStore([FromHeader] String Authorization, string storeName)
        {
            Response<Dictionary<string, List<string>>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Standby Owners In Store called with parameters: authToken={authToken}, storeName={storeName}.");
                Dictionary<string, List<string>> standbyOwners = _market.GetStandbyOwnersInStore(authToken, storeName);
                response = new Response<Dictionary<string, List<string>>>(standbyOwners);
                _logger.Info($"SUCCESSFULY executed Get Standby Owners In Store.");
            }
            catch (Exception e)
            {
                response = new Response<Dictionary<string, List<string>>>(e); _logger.Error(e.Message);
            }
            return response;
        }
        [HttpPost("GetDailyPopulationStatistics")]
        public Response<ICollection<PopulationStatisticsDTO>> GetDailyPopulationStatistics([FromHeader] String Authorization, int day, int month, int year)
        {
            Response<ICollection<PopulationStatisticsDTO>> response;
            try
            {
                String authToken = parseAutherization(Authorization);
                _logger.Info($"Get Daily Population Statistics called with parameters: authToken={authToken}, dateTime={new DateTime(year, month, day)}.");
                ICollection<PopulationStatistics> domainStatiscs = _market.GetDailyPopulationStatistics(authToken, new DateTime(year, month, day));
                DTOtranslator translator = new DTOtranslator();
                ICollection<PopulationStatisticsDTO> statisticsDTOs = new List<PopulationStatisticsDTO>();
                foreach(PopulationStatistics populationStatistics in domainStatiscs)
                {
                    statisticsDTOs.Add(translator.toDTO(populationStatistics));
                }
                response = new Response<ICollection<PopulationStatisticsDTO>>(statisticsDTOs);
                _logger.Info($"SUCCESSFULY executed Get Daily Population Statistics.");
            }
            catch (Exception e)
            {
                response = new Response<ICollection<PopulationStatisticsDTO>>(e); 
                _logger.Error(e.Message);
            }
            return response;
        }       
    }
}

