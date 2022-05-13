using MarketWeb.Client.Helpers;
using MarketWeb.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MarketWeb.Client.Connect
{
    public interface IMarketAPIClient 
    {

        public Task<Response<string>> EnterSystem();

        public Task<Response> ExitSystem();
        public  Task<Response<string>> Login(string username, string password);

        public Task<Response> Logout();

        public Task<Response> Register(string Username, string password, DateTime dob);
        public Response RemoveRegisteredVisitor(String usr_toremove);
        public Task<Response> AddItemToCart(int itemID, String storeName, int amount);
        public Task<Response> RemoveItemFromCart(int itemID, String storeName);
        public Task<Response> UpdateQuantityOfItemInCart(int itemID, String storeName, int newQuantity);
        public Task<Response<ShoppingCartDTO>> ViewMyCart();
        public Task<Response> PurchaseMyCart(String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode);
        public Task<Response> OpenNewStore(String storeName);
        public Task<Response> AddStoreManager(String managerUsername, String storeName);
        public Task<Response> AddStoreOwner(String ownerUsername, String storeName);
        public Task<Response> RemoveStoreOwner(String ownerUsername, String storeName);
        public Task<Response> RemoveStoreManager(String managerUsername, String storeName);
        public Task<Response> AddItemToStoreStock(String storeName, int itemID, String name, double price, String description, String category, int quantity);
        public Task<Response> RemoveItemFromStore(String storeName, int itemID);
        public Task<Response> UpdateStockQuantityOfItem(String storeName, int itemID, int newQuantity);
        public Task<Response> EditItemPrice(String storeName, int itemID, double newPrice);
        public Task<Response> EditItemName(String storeName, int itemID, String newName);
        public Task<Response> EditItemDescription(String storeName, int itemID, String newDescription);
        public Task<Response> RateItem(int itemID, String storeName, int rating, String review);
        public Task<Response> RateStore(String storeName, int rating, String review);
        public Task<Response<StoreDTO>> GetStoreInformation(String storeName);
        public Task<Response<List<ItemDTO>>> GetItemInformation(String itemName, String itemCategory, String keyWord);
        public Task<Response> SendMessageToStore(String storeName, String title, String description);
        public Task<Response> FileComplaint(int cartID, String message);
        public Task<Response<ICollection<PurchasedCartDTO>>> GetMyPurchasesHistory();
        public Task<Response<RegisteredDTO>> GetVisitorInformation();
        public Task<Response> EditVisitorPassword(String oldPassword, String newPassword);
        public Task<Response> RemoveManagerPermission(String managerUsername, String storeName, Operation op);
        public Task<Response> AddManagerPermission( String managerUsername, String storeName, String op);
        public Task<Response> CloseStore(String storeName);
        public Task<Response> ReopenStore( String storeName);
        public Task<Response<List<StoreOwnerDTO>>> GetStoreOwners(String storeName);
        public Task<Response<List<StoreManagerDTO>>> GetStoreManagers(String storeName);
        public Task<Response<StoreFounderDTO>> GetStoreFounder(String storeName);
        public Task<Response<Queue<MessageToStoreDTO>>> GetStoreMessages(String storeName);
        public Task<Response<ICollection<MessageToRegisteredDTO>>> GetRegisteredMessages();
        public Task<Response> AnswerStoreMesseage(String storeName, string recieverUsername, String title, String reply);
        public Task<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>> GetStorePurchasesHistory(String storeName);
        public Task<Response> CloseStorePermanently(String storeName);
        public Task<Response<ComplaintDTO>> GetRegisterdComplaints();
        public Task<Response> ReplyToComplaint(int complaintID, String reply);
        public Task<Response> SendMessageToRegisterd(String storeName, String UsernameReciever, String title, String message);

        public Response AppointSystemAdmin(String adminUsername);
    public class MarketAPIClient : IMarketAPIClient
    {
        private IHttpService _httpService;
        public bool LoggedIn;

        public MarketAPIClient(IHttpService httpService)
        {
            _httpService = httpService;
            LoggedIn = false;
        }

        public async Task<Response<string>> EnterSystem()
        {
            Response<string> token = await _httpService.Get<Response<string>>("api/users/enter");
            if (!token.ErrorOccured)
            {
                _httpService.Token = token.Value;
            }
            return token;
        }

        public async Task<Response> ExitSystem()
        {
            throw new NotImplementedException();
        }
        public async Task<Response<String>> Login(string username, string password)
        {
            Response<String> token = await _httpService.Post<Response<String>>("api/users/login", new { username = username, password = password });
            if (!token.ErrorOccured)
            {
                _httpService.Token = token.Value;
                LoggedIn = true;
            }
            return token;
        }

        public async Task<Response> Register(string Username, string password, DateTime dob)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Logout()
        {
            throw new NotImplementedException();
        }
    }
}
