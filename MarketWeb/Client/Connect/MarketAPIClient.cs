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
        public Task<Response> RemoveRegisteredVisitorAsync(String usr_toremove);
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
            Response<string> token = await _httpService.Get<Response<string>>("api/market/enter");
            if (!token.ErrorOccured)
            {
                _httpService.Token = token.Value;
            }
            return token;
        }

        public async Task<Response> ExitSystem()
        {
            Response res = await _httpService.Get<Response>("api/market/exit");
            return res;
        }
        public async Task<Response<String>> Login(string username, string password)
        {
            Response<String> token = await _httpService.Post<Response<String>>("api/market/login", new { username = username, password = password });
            if (!token.ErrorOccured)
            {
                _httpService.Token = token.Value;
                LoggedIn = true;
            }
            return token;
        }

        public async Task<Response> Register(string username, string password, DateTime dob)
        {
            Response res = await _httpService.Post<Response<String>>("api/market/register", new { username = username, password = password, dob = dob });
            return res;
        }

        public async Task<Response> Logout()
        {
            Response res = await _httpService.Get<Response>("api/market/logout");
            return res;
        }

        public async Task<Response> RemoveRegisteredVisitorAsync(string usr_toremove)
        {
            Response res = await _httpService.Get<Response>("api/market/RemoveRegisteredVisitor");
            return res;
        }

        public async Task<Response> AddItemToCart(int itemID, string storeName, int amount)
        {
            Response res = await _httpService.Post<Response>("api/market/AddItemToCart", new { itemID = itemID, storeName = storeName, amount = amount });
            return res;
        }

        public async Task<Response> RemoveItemFromCart(int itemID, string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/RemoveItemFromCart", new { itemID = itemID, storeName = storeName});
            return res;
        }

        public async Task<Response> UpdateQuantityOfItemInCart(int itemID, string storeName, int newQuantity)
        {
            Response res = await _httpService.Post<Response>("api/market/UpdateQuantityOfItemInCart", new { itemID = itemID, storeName = storeName, newQuantity = newQuantity });
            return res;
        }

        public async Task<Response<ShoppingCartDTO>> ViewMyCart()
        {
            Response <ShoppingCartDTO> res = await _httpService.Get<Response<ShoppingCartDTO>>("api/market/ViewMyCart");
            return res;
        }

        public async Task<Response> PurchaseMyCart(string address, string city, string country, string zip, string purchaserName, string paymentMethode, string shipmentMethode)
        {
            Response res = await _httpService.Post<Response>("api/market/PurchaseMyCart", new {
                address = address,
                city = city,
                country = country,
                zip = zip,
                purchaserName = purchaserName,
                paymentMethode = paymentMethode,
                shipmentMethod = shipmentMethode
            });
            return res;
        }

        public async Task<Response> OpenNewStore(string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new {storeName = storeName});
            return res;
        }

        public async Task<Response> AddStoreManager(string managerUsername, string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/AddStoreManager", new { managerUsername = managerUsername, storeName = storeName });
            return res;
        }

        public async Task<Response> AddStoreOwner(string ownerUsername, string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/AddStoreOwner", new { ownerUsername= ownerUsername, storeName = storeName });
            return res;
        }

        public async Task<Response> RemoveStoreOwner(string ownerUsername, string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/RemoveStoreOwner", new { ownerUsername= ownerUsername, storeName = storeName });
            return res;
        }

        public async Task<Response> RemoveStoreManager(string managerUsername, string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/RemoveStoreManager", new { managerUsername= managerUsername, storeName = storeName });
            return res;
        }

        public async Task<Response> AddItemToStoreStock(string storeName, int itemID, string name, double price, string description, string category, int quantity)
        {
            Response res = await _httpService.Post<Response>("api/market/AddItemToStoreStock", new { storeName = storeName,
                itemID = itemID,
                name = name,
                price = price,
                description = description,
                category = category,
                quantit = quantity
            });

            return res;
        }

        public async Task<Response> RemoveItemFromStore(string storeName, int itemID)
        {
            Response res = await _httpService.Post<Response>("api/market/RemoveItemFromStore", new { storeName = storeName, itemID = itemID });
            return res;
        }

        public async Task<Response> UpdateStockQuantityOfItem(string storeName, int itemID, int newQuantity)
        {
            Response res = await _httpService.Post<Response>("api/market/UpdateStockQuantityOfItem", new { storeName = storeName, itemID = itemID, newQuantity = newQuantity});
            return res;
        }

        public async Task<Response> EditItemPrice(string storeName, int itemID, double newPrice)
        {
            Response res = await _httpService.Post<Response>("api/market/EditItemPrice", new { storeName = storeName, itemID= itemID, newPrice = newPrice });
            return res;
        }

        public Task<Response> EditItemName(string storeName, int itemID, string newName)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
        }

        public Task<Response> EditItemDescription(string storeName, int itemID, string newDescription)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
        }

        public Task<Response> RateItem(int itemID, string storeName, int rating, string review)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
        }

        public Task<Response> RateStore(string storeName, int rating, string review)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
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
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
        }

        public Task<Response> FileComplaint(int cartID, string message)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
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
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
        }

        public Task<Response> RemoveManagerPermission(string managerUsername, string storeName, string op)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
        }

        public Task<Response> AddManagerPermission(string managerUsername, string storeName, string op)
        {
            Response res = await _httpService.Post<Response>("api/market/OpenNewStore", new { storeName = storeName });
            return res;
        }

        public Task<Response> CloseStore(string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/RemoveItemFromStore", new { storeName = storeName, itemID = itemID });
            return res;
        }

        public async Task<Response> ReopenStore(string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/ReopenStore", new { storeName = storeName });
            return res;
        }

        public async Task<Response<List<StoreOwnerDTO>>> GetStoreOwners(string storeName)
        {
            Response<List<StoreOwnerDTO>> res = await _httpService.Post<Response<List<StoreOwnerDTO>>>("api/market/GetStoreOwners", new { storeName = storeName });
            return res;
        }

        public async Task<Response<List<StoreManagerDTO>>> GetStoreManagers(string storeName)
        {
            Response<List<StoreManagerDTO>> res = await _httpService.Post<Response<List<StoreManagerDTO>>>("api/market/GetStoreManagers", new { storeName = storeName });
            return res;
        }

        public async Task<Response<StoreFounderDTO>> GetStoreFounder(string storeName)
        {
            Response<StoreFounderDTO> res = await _httpService.Post<Response<StoreFounderDTO>>("api/market/GetStoreFounder",
                new { storeName = storeName });
            return res;
        }

        public async Task<Response<Queue<MessageToStoreDTO>>> GetStoreMessages(string storeName)
        {
            Response<Queue<MessageToStoreDTO>> res = await _httpService.Post<Response<Queue<MessageToStoreDTO>>>("api/market/GetStoreMessages",
                new { storeName = storeName } );
            return res;
        }

        public async Task<Response> AnswerStoreMesseage(string storeName, string recieverUsername, string title, string reply)
        {
            Response res = await _httpService.Post<Response>("api/market/AnswerStoreMesseage", 
                new { storeName = storeName, recieverUsername = recieverUsername, title = title, reply= reply });
            return res;
        }

        public async Task<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>> GetStorePurchasesHistory(string storeName)
        {
            Response<List<Tuple<DateTime, ShoppingBasketDTO>>> res = await _httpService.Post<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>>("api/market/GetStorePurchasesHistory", new { storeName = storeName });
            return res;
        }

        public async Task<Response> CloseStorePermanently(string storeName)
        {
            Response res = await _httpService.Post<Response>("api/market/CloseStorePermanently", new { storeName = storeName });
            return res;
        }

        public async Task<Response<ComplaintDTO>> GetRegisterdComplaints()
        {
            Response<ComplaintDTO> res = await _httpService.Get<Response<ComplaintDTO>>("api/market/GetRegisterdComplaints");
            return res;
        }

        public async Task<Response> ReplyToComplaint(int complaintID, string reply)
        {
            Response res = await _httpService.Post<Response>("api/market/ReplyToComplaint", new { complaintID = complaintID, reply = reply });
            return res;
        }

        public async Task<Response> SendMessageToRegisterd(string storeName, string UsernameReciever, string title, string message)
        {
            Response res = await _httpService.Post<Response>("api/market/SendMessageToRegisterd",
                new { storeName = storeName, UsernameReciever = UsernameReciever, title=title, message=message});
            return res;
        }

        public async Task<Response<IAsyncEnumerable<AdminMessageToRegisteredDTO>>> GetRegisteredMessagesFromAdmin()
        {
            Response<IAsyncEnumerable<AdminMessageToRegisteredDTO>> res = await _httpService.Get<Response<IAsyncEnumerable<AdminMessageToRegisteredDTO>>>("api/market/GetRegisteredMessagesFromAdmin");
            return res;
        }

        public async Task<Response<IAsyncEnumerable<MessageToStoreDTO>>> GetRegisterAnsweredStoreMessages()
        {
            Response<IAsyncEnumerable<MessageToStoreDTO>> res = await _httpService.Get<Response<IAsyncEnumerable<MessageToStoreDTO>>>("api/market/GetRegisterAnsweredStoreMessages");
            return res;
        }

        public async Task<Response<IAsyncEnumerable<NotifyMessageDTO>>> GetRegisteredMessagesNotofication()
        {
            Response<IAsyncEnumerable<NotifyMessageDTO>> res = await _httpService.Get<Response<IAsyncEnumerable<NotifyMessageDTO>>>("api/market/GetRegisteredMessagesNotofication");
            return res;
        }

        public async Task<Response> AppointSystemAdmin(string adminUsername)
        {
            Response res = await _httpService.Post<Response>("api/market/AppointSystemAdmin", new { adminUsername = adminUsername});
            return res;
        }
    }
}
