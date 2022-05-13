using MarketWeb.Client.Helpers;
using MarketWeb.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using MarketWeb.Shared.DTO;

namespace MarketWeb.Client.Connect
{
    public interface IMarketAPIClient
    {

        public Task<Response<string>> EnterSystem();

        public Task<Response> ExitSystem();
        public Task<Response<string>> Login(string username, string password);

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
        public Task<Response> RemoveManagerPermission(String managerUsername, String storeName, string op);
        public Task<Response> AddManagerPermission(String managerUsername, String storeName, String op);
        public Task<Response> CloseStore(String storeName);
        public Task<Response> ReopenStore(String storeName);
        public Task<Response<List<StoreOwnerDTO>>> GetStoreOwners(String storeName);
        public Task<Response<List<StoreManagerDTO>>> GetStoreManagers(String storeName);
        public Task<Response<StoreFounderDTO>> GetStoreFounder(String storeName);
        public Task<Response<Queue<MessageToStoreDTO>>> GetStoreMessages(String storeName);
        public Task<Response> AnswerStoreMesseage(String storeName, string recieverUsername, String title, String reply);
        public Task<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>> GetStorePurchasesHistory(String storeName);
        public Task<Response> CloseStorePermanently(String storeName);
        public Task<Response<ComplaintDTO>> GetRegisterdComplaints();
        public Task<Response> ReplyToComplaint(int complaintID, String reply);
        public Task<Response> SendMessageToRegisterd(String storeName, String UsernameReciever, String title, String message);
        public Task<Response<ICollection<AdminMessageToRegisteredDTO>>> GetRegisteredMessagesFromAdmin();
        public Task<Response<ICollection<MessageToStoreDTO>>> GetRegisterAnsweredStoreMessages();
        public Task<Response<ICollection<NotifyMessageDTO>>> GetRegisteredMessagesNotofication();
        public Task<Response> AppointSystemAdmin(String adminUsername);
    }
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

        public Response RemoveRegisteredVisitor(string usr_toremove)
        {
            throw new NotImplementedException();
        }

        public Task<Response> AddItemToCart(int itemID, string storeName, int amount)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RemoveItemFromCart(int itemID, string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateQuantityOfItemInCart(int itemID, string storeName, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ShoppingCartDTO>> ViewMyCart()
        {
            throw new NotImplementedException();
        }

        public Task<Response> PurchaseMyCart(string address, string city, string country, string zip, string purchaserName, string paymentMethode, string shipmentMethode)
        {
            throw new NotImplementedException();
        }

        public Task<Response> OpenNewStore(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> AddStoreManager(string managerUsername, string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> AddStoreOwner(string ownerUsername, string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RemoveStoreOwner(string ownerUsername, string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RemoveStoreManager(string managerUsername, string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> AddItemToStoreStock(string storeName, int itemID, string name, double price, string description, string category, int quantity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RemoveItemFromStore(string storeName, int itemID)
        {
            throw new NotImplementedException();
        }

        public Task<Response> UpdateStockQuantityOfItem(string storeName, int itemID, int newQuantity)
        {
            throw new NotImplementedException();
        }

        public Task<Response> EditItemPrice(string storeName, int itemID, double newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<Response> EditItemName(string storeName, int itemID, string newName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> EditItemDescription(string storeName, int itemID, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RateItem(int itemID, string storeName, int rating, string review)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RateStore(string storeName, int rating, string review)
        {
            throw new NotImplementedException();
        }

        public Task<Response<StoreDTO>> GetStoreInformation(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<ItemDTO>>> GetItemInformation(string itemName, string itemCategory, string keyWord)
        {
            throw new NotImplementedException();
        }

        public Task<Response> SendMessageToStore(string storeName, string title, string description)
        {
            throw new NotImplementedException();
        }

        public Task<Response> FileComplaint(int cartID, string message)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ICollection<PurchasedCartDTO>>> GetMyPurchasesHistory()
        {
            throw new NotImplementedException();
        }

        public Task<Response<RegisteredDTO>> GetVisitorInformation()
        {
            throw new NotImplementedException();
        }

        public Task<Response> EditVisitorPassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<Response> RemoveManagerPermission(string managerUsername, string storeName, string op)
        {
            throw new NotImplementedException();
        }

        public Task<Response> AddManagerPermission(string managerUsername, string storeName, string op)
        {
            throw new NotImplementedException();
        }

        public Task<Response> CloseStore(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> ReopenStore(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<StoreOwnerDTO>>> GetStoreOwners(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<StoreManagerDTO>>> GetStoreManagers(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<StoreFounderDTO>> GetStoreFounder(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Queue<MessageToStoreDTO>>> GetStoreMessages(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> AnswerStoreMesseage(string storeName, string recieverUsername, string title, string reply)
        {
            throw new NotImplementedException();
        }

        public Task<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>> GetStorePurchasesHistory(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response> CloseStorePermanently(string storeName)
        {
            throw new NotImplementedException();
        }

        public Task<Response<ComplaintDTO>> GetRegisterdComplaints()
        {
            throw new NotImplementedException();
        }

        public Task<Response> ReplyToComplaint(int complaintID, string reply)
        {
            throw new NotImplementedException();
        }

        public Task<Response> SendMessageToRegisterd(string storeName, string UsernameReciever, string title, string message)
        {
            throw new NotImplementedException();
        }
        public Task<Response<ICollection<AdminMessageToRegisteredDTO>>> GetRegisteredMessagesFromAdmin()
        {
            throw new NotImplementedException();
        }

        public Task<Response<ICollection<MessageToStoreDTO>>> GetRegisterAnsweredStoreMessages()
        {
            throw new NotImplementedException();
        }

        public Task<Response<ICollection<NotifyMessageDTO>>> GetRegisteredMessagesNotofication()
        {
            throw new NotImplementedException();
        }

        public Task<Response> AppointSystemAdmin(string adminUsername)
        {
            throw new NotImplementedException();
        }
    }
}
