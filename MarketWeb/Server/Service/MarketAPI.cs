
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
                    new ConfigurationFileParser().ParseConfigurationFile();
                    // TODO
                    // GET DB CONNECTION? SEND IT TO MARKET?
                }
                catch (Exception e)
                {
                    _logger.Error(e.Message);
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

        public Response ResponseWrapper(Action lambda)
        {
            Response response;
            try
            {
                lambda();
                response = new Response();
            }
            catch (Exception ex)
            {
                response = new Response(ex);
                _logger.Error(ex.Message);
            }
            return response;
        }

        public Response<T> ResponseWrapper<T>(Func<T> lambda)
        {
            Response<T> response;
            try
            {
                T result = lambda();
                response = new Response<T>(result);
            }
            catch (Exception ex)
            {
                response = new Response<T>(ex);
                _logger.Error(ex.Message);
            }
            return response;
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
        /// <para> For Req I.1. </para>
        /// <para> Starts system with the given credentials setting the Visitor as the current admin.</para>
        /// </summary>
        [HttpPost("")]
        public Response RestartSystem(String adminUsername, String adminPassword, String ipShippingService, String ipPaymentService)
        {//I.1
            return ResponseWrapper(()=> _market.RestartSystem(adminUsername, adminPassword, ipShippingService, ipPaymentService));
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
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper<String>(() => _market.Login(authToken, Username, password));
        }

        /// <summary>
        /// <para> For Req II.3.1. </para>
        /// <para> Log out Visitor identified by authToken.</para>
        /// <return> new token as a guest</return>
        /// </summary>
        /// <param name="authToken"> The token of the Visitor to log out.</param>
        [HttpGet("Logout")]
        public Response<String> Logout([FromHeader] String Authorization)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper<String>(() => _market.Logout(authToken));
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
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.Register(authToken, Username, password, birthDate));
        }

        /// <summary>
        /// <para> For Req II.6.2. </para>
        /// <para> Remove a Registered Visitor from our system and remove their roles from all relevant stores.</para>
        /// </summary>
        /// <param name="authToken"> The token authenticating the Visitor making the request.</param>
        /// <param name="usr_toremove"> The Visitor to remove and revoke the roles of.</param>
        [HttpGet("RemoveRegisteredVisitor")]
        public Response RemoveRegisteredVisitor([FromHeader] String Authorization, String usr_toremove)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RemoveRegisteredVisitor(authToken, usr_toremove));
        }
        [HttpPost("AddItemToCart")]
        public Response AddItemToCart([FromHeader] String Authorization, int itemID, String storeName, int amount)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AddItemToCart(authToken, itemID,  storeName,  amount));
        }
        [HttpPost("RemoveItemFromCart")]
        public Response RemoveItemFromCart([FromHeader] String Authorization, int itemID, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RemoveItemFromCart(authToken,  itemID,  storeName));
        }
        [HttpPost("UpdateQuantityOfItemInCart")]
        public Response UpdateQuantityOfItemInCart([FromHeader] String Authorization, int itemID, String storeName, int newQuantity)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.UpdateQuantityOfItemInCart(authToken, itemID, storeName, newQuantity));
        }
        [HttpGet("ViewMyCart")]
        public Response<ShoppingCartDTO> ViewMyCart([FromHeader] String Authorization) /*Add data object of cart*/
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper<ShoppingCartDTO>(() => new DTOtranslator().toDTO(_market.ViewMyCart(authToken)));
        }
        [HttpPost("PurchaseMyCart")]
        public Response PurchaseMyCart([FromHeader] String Authorization, String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.PurchaseMyCart(authToken, address, city, country, zip, purchaserName, paymentMethode, shipmentMethode));
        }
        [HttpPost("OpenNewStore")]
        public Response OpenNewStore([FromHeader] String Authorization, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.OpenNewStore(authToken, storeName, new PurchasePolicy(), new DiscountPolicy()));
        }
        [HttpPost("AddStoreManager")]
        public Response AddStoreManager([FromHeader] String Authorization, String managerUsername, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AddStoreManager(authToken, managerUsername, storeName));
        }
        [HttpPost("AddStoreOwner")]
        public Response AddStoreOwner([FromHeader] String Authorization, String ownerUsername, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AddStoreOwner(authToken, ownerUsername, storeName));
        }
        [HttpPost("RemoveStoreOwner")]
        public Response RemoveStoreOwner([FromHeader] String Authorization, String ownerUsername, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RemoveStoreOwner(authToken, ownerUsername, storeName));
        }
        [HttpPost("RemoveStoreManager")]
        public Response RemoveStoreManager([FromHeader] String Authorization, String managerUsername, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RemoveStoreManager(authToken, managerUsername, storeName));
        }
        [HttpPost("AddItemToStoreStock")]
        public Response AddItemToStoreStock([FromHeader] String Authorization, String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AddItemToStoreStock(authToken, storeName, itemID, name, price, description, category, quantity));
        }
        [HttpPost("RemoveItemFromStore")]
        public Response RemoveItemFromStore([FromHeader] String Authorization, String storeName, int itemID)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RemoveItemFromStore(authToken, storeName, itemID ));
        }
        [HttpPost("UpdateStockQuantityOfItem")]
        public Response UpdateStockQuantityOfItem([FromHeader] String Authorization, String storeName, int itemID, int newQuantity)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.UpdateStockQuantityOfItem(authToken, storeName, itemID, newQuantity ));
        }
        [HttpPost("EditItemPrice")]
        public Response EditItemPrice([FromHeader] String Authorization, String storeName, int itemID, double newPrice)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.EditItemPrice(authToken, storeName, itemID, newPrice));
        }
        [HttpPost("EditItemName")]
        public Response EditItemName([FromHeader] String Authorization, String storeName, int itemID, String newName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.EditItemName(authToken, storeName, itemID, newName));
        }
        [HttpPost("EditItemDescription")]
        public Response EditItemDescription([FromHeader] String Authorization, String storeName, int itemID, String newDescription)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.EditItemDescription(authToken, storeName, itemID, newDescription));
        }
        [HttpPost("RateItem")]
        public Response RateItem([FromHeader] String Authorization, int itemID, String storeName, int rating, String review)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RateItem(authToken, itemID, storeName, rating, review));
        }
        [HttpPost("RateStore")]
        public Response RateStore([FromHeader] String Authorization, String storeName, int rating, String review) // 0 < rating < 10
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RateStore(authToken, storeName, rating, review));
        }
        [HttpGet("GetStoreInformation")]
        public Response<StoreDTO> GetStoreInformation([FromHeader] String Authorization, String storeName)
        {//II.2.1
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => new DTOtranslator().toDTO(_market.GetStoreInformation(authToken, storeName)));
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
        public Response SendMessageToStore([FromHeader] String Authorization, String storeName, String title, String description, int id)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.SendMessageToStore(authToken, storeName, title, description, id));
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
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.FileComplaint(authToken, cartID, message));
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
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => new DTOtranslator().toDTO(_market.GetVisitorInformation(authToken )));
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
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.EditVisitorPassword(authToken, oldPassword, newPassword));
        }

        [HttpPost("RemoveManagerPermission")]
        public Response RemoveManagerPermission([FromHeader] String Authorization, String managerUsername, String storeName, string op)//permission param is Enum
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.RemoveManagerPermission(authToken, managerUsername, storeName, op));
        }

        [HttpPost("AddManagerPermission")]
        public Response AddManagerPermission([FromHeader] String Authorization, String managerUsername, String storeName, String op)//permission param is Enum
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AddManagerPermission(authToken, managerUsername, storeName, op));
        }

        [HttpPost("CloseStore")]
        public Response CloseStore([FromHeader] String Authorization, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.CloseStore(authToken, storeName));
        }

        [HttpPost("ReopenStore")]
        public Response ReopenStore([FromHeader] String Authorization, String storeName)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.ReopenStore(authToken, storeName));
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
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper<StoreFounderDTO>(() => new DTOtranslator().toDTO(_market.getStoreFounder(authToken, storeName)));
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
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AnswerStoreMesseage(authToken, receiverUsername, msgID, storeName, reply));
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
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.CloseStorePermanently(authToken, storeName));
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
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.ReplyToComplaint(authToken, complaintID, reply ));
        }

        [HttpPost("SendMessageToRegisterd")]
        public Response SendMessageToRegisterd([FromHeader] String Authorization, String UsernameReciever, String title, String message)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.SendAdminMessageToRegisterd(authToken, UsernameReciever, title, message));
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
            return ResponseWrapper<String>(() => _market.EnterSystem());
        }
        [HttpGet("ExitSystem")]
        public Response ExitSystem([FromHeader] String Authorization) // Removing cart and token assigned to guest
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.ExitSystem(authToken ));
        }

        [HttpPost("AppointSystemAdmin")]
        public Response AppointSystemAdmin([FromHeader] String Authorization, String adminUsername)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AppointSystemAdmin(authToken, adminUsername ));
        }

        [HttpPost("AddStoreDiscount")]
        public Response AddStoreDiscount([FromHeader] String Authorization, String storeName, String conditionString, String discountString)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AddStoreDiscount(authToken, storeName, conditionString, discountString));
        }

        [HttpPost("AddStorePurchasePolicy")]
        public Response AddStorePurchasePolicy([FromHeader] String Authorization, String storeName, String conditionString)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.AddStorePurchasePolicy(authToken, storeName, conditionString ));
        }

        [HttpPost("CalcCartActualPrice")]
        public Response<Double> CalcCartActualPrice([FromHeader] String Authorization)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper<Double>(() => _market.CalcCartActualPrice(authToken));
        }

        [HttpPost("GetCartReceipt")]
        public Response<String> GetCartReceipt([FromHeader] String Authorization)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper<String>(() => _market.GetCartReceipt(authToken));
        }

        [HttpPost("HasPermission")]
        public Response HasPermission([FromHeader] String Authorization, string storeName, string op)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.HasPermission(authToken, storeName , op));
        }


        [HttpPost("IsStoreActive")]
        public Response IsStoreActive([FromHeader] String Authorization, string storeName, string op)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper(() => _market.HasPermission(storeName, authToken, op ));
        }

        [HttpGet("GetItem")]
        public Response<ItemDTO> GetItem([FromHeader] String Authorization, string storeName, int itemId)
        {
            String authToken = parseAutherization(Authorization);
            return ResponseWrapper<ItemDTO>(() => new DTOtranslator().toDTO(_market.GetItem(authToken, storeName, itemId), storeName));
        }
        public void LoadData()
        {
            String username1 = "username1";
            String password1 = "password1";
            String storeName1 = "storeName1";
            String auth1 = "Bearer " + EnterSystem().Value;

            String username2 = "username2";
            String password2 = "password2";
            String storeName2 = "storeName2";
            String auth2 = "Bearer " + EnterSystem().Value;

            if (Register(auth1, username1, password1, new DateTime(1992, 8, 4)).ErrorOccured)
                return;
            auth1 = "Bearer " + Login(auth1, username1, password1).Value;
            OpenNewStore(auth1, storeName1);

            Register(auth2, username2, password2, new DateTime(1992, 8, 4));
            auth2 = "Bearer " + Login(auth2, username2, password2).Value;
            OpenNewStore(auth2, storeName2);

            int itemID1 = 1;
            int price1 = 1;
            String itemName1 = "itemName1";
            String category1 = "category1";
            String desc1 = "some item description goes here.";
            int quantity1 = 100;

            int itemID2 = 2;
            int price2 = 2;
            String itemName2 = "itemName2";
            String category2 = "dairy";
            String desc2 = "some other item description goes here.";
            int quantity2 = 200;

            int itemID3 = 3;
            int price3 = 3;
            String itemName3 = "itemName3";
            String category3 = "category3";
            String desc3 = "some other other item description goes here.";
            int quantity3 = 300;
            
            AddItemToStoreStock(auth1, storeName1, itemID1, itemName1, price1, desc1, category1, quantity1);

            AddItemToStoreStock(auth1, storeName1, itemID2, itemName2, price2, desc2, category2, quantity2);
            AddItemToStoreStock(auth1, storeName1, itemID3, itemName3, price3, desc3, category3, quantity3);

            AddItemToStoreStock(auth2, storeName2, itemID1, itemName1, price1, desc1, category1, quantity1);
            AddItemToStoreStock(auth2, storeName2, itemID2, itemName2, price2, desc2, category2, quantity2);
            AddItemToStoreStock(auth2, storeName2, itemID3, itemName3, price3, desc3, category3, quantity3);

            DateTime expiration = DateTime.Today.AddDays(10);
            int minAmount = 5;
            int maxAmount = 15;
            int percentageToSubtract = 10;
            int priceToSubtract = 2;

            IConditionDTO itemCondition = new SearchItemConditionDTO(itemName1, minAmount, maxAmount, false);
            IConditionDTO dayCondition = new DayOnWeekConditionDTO(DateTime.Today.DayOfWeek.ToString(), false);
            List<IConditionDTO> condLst = new List<IConditionDTO>();
            condLst.Add(itemCondition);
            condLst.Add(dayCondition);
            IConditionDTO andCondition = new AndCompositionDTO(false, condLst);
            ItemDiscountDTO itemDis = new ItemDiscountDTO(percentageToSubtract, itemName1, andCondition, expiration);
            IConditionDTO categoryCond = new SearchCategoryConditionDTO(category1, minAmount, maxAmount, false);
            NumericDiscountDTO numDis = new NumericDiscountDTO(priceToSubtract, categoryCond, expiration);
            List<IDiscountDTO> disLst = new List<IDiscountDTO>();
            disLst.Add(itemDis);
            disLst.Add(numDis);
            MaxDiscountDTO max = new MaxDiscountDTO(disLst, null);

            //AddStoreDiscount(auth1, storeName1, max);


            String dairyCategory = category2;
            PriceableConditionDTO pricable = new PriceableConditionDTO(null, 100, -1, false);
            SearchItemConditionDTO itemCond = new SearchItemConditionDTO(itemName2, 3, -1, false);
            List<IConditionDTO> condLst2 = new List<IConditionDTO>();
            condLst2.Add(pricable);
            condLst2.Add(itemCond);
            OrCompositionDTO orCond = new OrCompositionDTO(false, condLst2);
            CategoryDiscountDTO categoryDis = new CategoryDiscountDTO(percentageToSubtract, dairyCategory, orCond, expiration);
            //AddStoreDiscount(auth2, storeName2, categoryDis);
        }
    }
}

