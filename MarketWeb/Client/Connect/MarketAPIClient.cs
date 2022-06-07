using MarketWeb.Client.Helpers;
using MarketWeb.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using MarketWeb.Shared.DTO;
using MarketWeb.Client.Models.Account;
using Microsoft.AspNetCore.WebUtilities;

namespace MarketWeb.Client.Connect
{
    public interface IMarketAPIClient
    {
        public bool LoggedIn { get; }
        public bool Admin { get; }
        public bool Guest { get; }
        public Task<Response<string>> EnterSystem();
        public Task<Response> ExitSystem();
        public Task<Response<string>> Login(string username, string password);
        public Task<Response<String>> Login(LoginModel loginModel);
        public Task<Response> Logout();
        public Task<Response<List<StoreDTO>>> GetAllActiveStores();
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
        public Task<Response> AddItemToStoreStock(String storeName, String name, double price, String description, String category, int quantity);
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
        public Task<Response<List<Tuple<DateTime, ShoppingCartDTO>>>> GetMyPurchasesHistory();
        public Task<Response<RegisteredDTO>> GetVisitorInformation();
        public Task<Response> EditVisitorPassword(String oldPassword, String newPassword);
        public Task<Response> RemoveManagerPermission(String managerUsername, String storeName, string op);
        public Task<Response> AddManagerPermission(String managerUsername, String storeName, String op);
        public Task<Response> CloseStore(String storeName);
        public Task<Response> ReopenStore(String storeName);
        public Task<Response<List<StoreOwnerDTO>>> GetStoreOwners(String storeName);
        public Task<Response<List<StoreManagerDTO>>> GetStoreManagers(String storeName);
        public Task<Response<StoreFounderDTO>> GetStoreFounder(String storeName);
        public Task<Response<List<MessageToStoreDTO>>> GetStoreMessages(String storeName);
        public Task<Response> AnswerStoreMesseage(string receiverUsername, int msgID, string storeName, string reply);
        public Task<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>> GetStorePurchasesHistory(String storeName);
        public Task<Response> CloseStorePermanently(String storeName);
        public Task<Response<ICollection<ComplaintDTO>>> GetRegisterdComplaints();
        public Task<Response> ReplyToComplaint(int complaintID, String reply);
        public Task<Response> SendMessageToRegisterd(String storeName, String UsernameReciever, String title, String message);
        public Task<Response<ICollection<AdminMessageToRegisteredDTO>>> GetRegisteredMessagesFromAdmin();
        public Task<Response<ICollection<MessageToStoreDTO>>> GetRegisterAnsweredStoreMessages();
        public Task<Response<ICollection<NotifyMessageDTO>>> GetRegisteredMessagesNotofication();
        public Task<Response> AppointSystemAdmin(String adminUsername);
        public Task<Response> HasPermission(string storeName, string op);
        public Task<Response<ItemDTO>> GetItem(string storeName, int itemId);
        public Task<Response<List<StoreDTO>>> GetStoresOfUser();
        public Task<Response> SendAdminMessage(String receiverUsername, String title, String message);
        public Task<Response> AddStoreDiscount(String StoreName, String ConditionString, String DiscountString);
    }

    public class MarketAPIClient : IMarketAPIClient
    {

        private IHttpService _httpService;
        public bool LoggedIn { get; private set; } = false;
        public bool Admin { get; private set; } = false;
        public bool Guest { get; private set; } = true;

        private void TurnLoggedIn()
        {
            LoggedIn = true;
            Admin = false;
            Guest = false;
        }

        private void TurnAdmin()
        {
            LoggedIn = false;
            Admin = true;
            Guest = false;
        }

        private void TurnGuest()
        {
            LoggedIn = false;
            Admin = false;
            Guest = true;
        }

        public MarketAPIClient(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<Response<string>> EnterSystem()
        {
            Response<String> token = await _httpService.Get<Response<String>>("api/market/entersystem");
            if (!token.ErrorOccured && !LoggedIn)
            {
                _httpService.Token = token.Value;
            }
            return token;
        }

        public async Task<Response> ExitSystem()
        {
            Response res = await _httpService.Get<Response>("api/market/ExitSystem");
            return res;
        }
        public async Task<Response<String>> Login(string username, string password)
        {
            const string url = "api/market/login";
            var param = new Dictionary<string, string>() { { "username", username }, { "password", password } };

            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response<String> token = await _httpService.Post<Response<String>>(newUrl, null);
            if (!token.ErrorOccured)
            {
                _httpService.Token = token.Value;
                try
                {
                    Response HasAccess = await HasPermission("", "PERMENENT_CLOSE_STORE");
                    if (HasAccess.ErrorOccured)
                    {
                        TurnLoggedIn();
                    }
                    else
                    {
                        TurnAdmin();
                    }
                }
                catch (Exception) { }
            }
            return token;
        }

        public async Task<Response<String>> Login(LoginModel loginModel)
        {
            return await Login(loginModel.Username, loginModel.Password);
        }

        public async Task<Response> Register(string username, string password, DateTime dob)
        {
            const string url = "api/market/Register";
            var param = new Dictionary<string, string>() { { "username", username }, { "password", password }, { "dob", dob.ToString() } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response<String>>(newUrl, null);
            return res;
        }

        public async Task<Response> Logout()
        {
            Response res = await _httpService.Get<Response>("api/market/Logout");
            if (!res.ErrorOccured)
            {
                TurnGuest();
            }
            return res;
        }

        public async Task<Response> RemoveRegisteredVisitorAsync(string usr_toremove)
        {
            const string url = "api/market/RemoveRegisteredVisitor";
            var param = new Dictionary<string, string>() { { "usr_toremove", usr_toremove } };
            var newUrl = QueryHelpers.AddQueryString(url, param);
            Response res = await _httpService.Get<Response>(newUrl);
            return res;
        }

        public async Task<Response> AddItemToCart(int itemID, string storeName, int amount)
        {
            const string url = "api/market/AddItemToCart";
            var param = new Dictionary<string, string>() { { "itemID", itemID.ToString() }, { "storeName", storeName }, { "amount", amount.ToString() } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> RemoveItemFromCart(int itemID, string storeName)
        {
            const string url = "api/market/RemoveItemFromCart";
            var param = new Dictionary<string, string>() { { "itemID", itemID.ToString() }, { "storeName", storeName } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> UpdateQuantityOfItemInCart(int itemID, string storeName, int newQuantity)
        {
            const string url = "api/market/UpdateQuantityOfItemInCart";
            var param = new Dictionary<string, string>() { { "itemID", itemID.ToString() }, { "storeName", storeName }, { "newQuantity", newQuantity.ToString() } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<ShoppingCartDTO>> ViewMyCart()
        {
            Response<ShoppingCartDTO> res = await _httpService.Get<Response<ShoppingCartDTO>>("api/market/ViewMyCart");
            return res;
        }

        public async Task<Response> PurchaseMyCart(string address, string city, string country, string zip, string purchaserName, string paymentMethode, string shipmentMethode)
        {
            const string url = "api/market/PurchaseMyCart";
            var param = new Dictionary<string, string>() {
                { "address", address},
                { "city" , city},
                {"country",  country},
                { "zip", zip},
                { "purchaserName", purchaserName},
                { "paymentMethode",  paymentMethode},
                { "shipmentMethode",  shipmentMethode}};
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> OpenNewStore(string storeName)
        {
            const string url = "api/market/OpenNewStore";
            var param = new Dictionary<string, string>() { { "storeName", storeName } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> AddStoreManager(string managerUsername, string storeName)
        {
            const string url = "api/market/AddStoreManager";
            var param = new Dictionary<string, string>() { { "storeName", storeName }, { "managerUsername", managerUsername } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> AddStoreOwner(string ownerUsername, string storeName)
        {
            const string url = "api/market/AddStoreOwner";
            var param = new Dictionary<string, string>() { { "storeName", storeName }, { "ownerUsername", ownerUsername } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> RemoveStoreOwner(string ownerUsername, string storeName)
        {
            const string url = "api/market/RemoveStoreOwner";
            var param = new Dictionary<string, string>() { { "storeName", storeName }, { "ownerUsername", ownerUsername } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> RemoveStoreManager(string managerUsername, string storeName)
        {
            const string url = "api/market/RemoveStoreManager";
            var param = new Dictionary<string, string>() { { "storeName", storeName }, { "managerUsername", managerUsername } };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> AddItemToStoreStock(string storeName, string name, double price, string description, string category, int quantity)
        {
            const string url = "api/market/AddItemToStoreStock";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "name" , name},
                { "price", price.ToString()},
                { "description" , description},
                { "category", category},
                { "quantity", quantity.ToString()}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;

        }

        public async Task<Response> RemoveItemFromStore(string storeName, int itemID)
        {
            const string url = "api/market/RemoveItemFromStore";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "itemID" , itemID.ToString()}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> UpdateStockQuantityOfItem(string storeName, int itemID, int newQuantity)
        {
            const string url = "api/market/UpdateStockQuantityOfItem";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "itemID" , itemID.ToString()},
                { "newQuantity", newQuantity.ToString()}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;

        }
        public async Task<Response> EditItemPrice(string storeName, int itemID, double newPrice)
        {
            const string url = "api/market/EditItemPrice";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "itemID" , itemID.ToString()},
                { "newPrice", newPrice.ToString()}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> EditItemName(string storeName, int itemID, string newName)
        {
            const string url = "api/market/EditItemName";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "itemID" , itemID.ToString()},
                { "newName", newName}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> EditItemDescription(string storeName, int itemID, string newDescription)
        {
            const string url = "api/market/EditItemDescription";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "itemID" , itemID.ToString()},
                { "newDescription", newDescription}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> RateItem(int itemID, string storeName, int rating, string review)
        {
            const string url = "api/market/RateItem";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "itemID" , itemID.ToString()},
                { "rating" , rating.ToString()},
                { "review", review}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> RateStore(string storeName, int rating, string review)
        {
            const string url = "api/market/RateStore";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
                { "rating" , rating.ToString()},
                { "review", review}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<StoreDTO>> GetStoreInformation(string storeName)
        {
            const string url = "api/market/GetStoreInformation";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response<StoreDTO> res = await _httpService.Get<Response<StoreDTO>>(newUrl);
            return res;
        }

        public async Task<Response<List<ItemDTO>>> GetItemInformation(string itemName, string itemCategory, string keyWord)
        {
            const string url = "api/market/GetItemInformation";
            var param = new Dictionary<string, string>() {
            { "itemName" , itemName },
            { "itemCategory" , itemCategory },
            { "keyWord" , keyWord }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response<List<ItemDTO>> res = await _httpService.Get<Response<List<ItemDTO>>>(newUrl);
            return res;
        }

        public async Task<Response> SendMessageToStore(string storeName, string title, string description)
        {
            const string url = "api/market/SendMessageToStore";
            var param = new Dictionary<string, string>() {
            { "storeName" , storeName },
            { "title" , title },
            { "description" , description }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl,null);
            return res;
        }

        public async Task<Response> FileComplaint(int cartID, string message)
        {
            const string url = "api/market/FileComplaint";
            var param = new Dictionary<string, string>() {
            { "cartID" , cartID.ToString() },
            { "message" , message }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<List<Tuple<DateTime, ShoppingCartDTO>>>> GetMyPurchasesHistory()
        {
            Response<List<Tuple<DateTime, ShoppingCartDTO>>> res = await _httpService.Get<Response<List<Tuple<DateTime, ShoppingCartDTO>>>>("api/market/GetMyPurchasesHistory");
            return res;
        }

        public async Task<Response<RegisteredDTO>> GetVisitorInformation()
        {
            Response<RegisteredDTO> res = await _httpService.Get<Response<RegisteredDTO>>("api/market/GetVisitorInformation");
            return res;
        }

        public async Task<Response> EditVisitorPassword(string oldPassword, string newPassword)
        {
            const string url = "api/market/EditVisitorPassword";
            var param = new Dictionary<string, string>() {
            { "oldPassword" , oldPassword },
            { "newPassword" , newPassword }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> RemoveManagerPermission(string managerUsername, string storeName, string op)
        {
            const string url = "api/market/RemoveManagerPermission";
            var param = new Dictionary<string, string>() {
                { "managerUsername" , managerUsername },
                {"op", op },
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> AddManagerPermission(string managerUsername, string storeName, string op)
        {
            const string url = "api/market/AddManagerPermission";
            var param = new Dictionary<string, string>() {
                { "managerUsername" , managerUsername },
                {"op", op },
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> CloseStore(string storeName)
        {
            const string url = "api/market/CloseStore";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> ReopenStore(string storeName)
        {
            const string url = "api/market/ReopenStore";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<List<StoreOwnerDTO>>> GetStoreOwners(string storeName)
        {
            const string url = "api/market/GetStoreOwners";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);
            Response<List<StoreOwnerDTO>> res = await _httpService.Post<Response<List<StoreOwnerDTO>>>(newUrl, null);
            return res;
        }

        public async Task<Response<List<StoreManagerDTO>>> GetStoreManagers(string storeName)
        {
            const string url = "api/market/GetStoreManagers";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response<List<StoreManagerDTO>> res = await _httpService.Post<Response<List<StoreManagerDTO>>>(newUrl, null);
            return res;
        }

        public async Task<Response<StoreFounderDTO>> GetStoreFounder(string storeName)
        {
            //throw new NotImplementedException("in client get founder of store: "+storeName);
            const string url = "api/market/GetStoreFounder";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response<StoreFounderDTO> res = await _httpService.Post<Response<StoreFounderDTO>>(newUrl, null);
			
            return res;
        }

        public async Task<Response<List<MessageToStoreDTO>>> GetStoreMessages(string storeName)
        {

            const string url = "api/market/GetStoreMessages";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response<List<MessageToStoreDTO>> res = await _httpService.Get<Response<List<MessageToStoreDTO>>>(newUrl);
            return res;
        }

        public async Task<Response> AnswerStoreMesseage(string receiverUsername, int msgID, string storeName, string reply)
        {
            
            const string url = "api/market/AnswerStoreMesseage";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName },
                {"receiverUsername", receiverUsername },
                { "msgID", msgID.ToString()},
                { "reply", reply}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
			if (res.ErrorOccured)
			{
                throw new NotImplementedException($"answer msg: reciver= {receiverUsername}, msgID = {msgID},  store={storeName}, reply = {reply}");
            }
            return res;
        }

        public async Task<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>> GetStorePurchasesHistory(string storeName)
        {
            const string url = "api/market/GetStorePurchasesHistory";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response<List<Tuple<DateTime, ShoppingBasketDTO>>> res = await _httpService.Post<Response<List<Tuple<DateTime, ShoppingBasketDTO>>>>(newUrl, null);
            return res;
        }

        public async Task<Response> CloseStorePermanently(string storeName)
        {
            const string url = "api/market/CloseStorePermanently";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<ICollection<ComplaintDTO>>> GetRegisterdComplaints()
        {
            Response<ICollection<ComplaintDTO>> res = await _httpService.Post<Response<ICollection<ComplaintDTO>>>("api/market/GetRegisterdComplaints", null);
            return res;
        }

        public async Task<Response> ReplyToComplaint(int complaintID, string reply)
        {
            const string url = "api/market/ReplyToComplaint";
            var param = new Dictionary<string, string>() {
                { "reply" , reply },
                {"complaintID", complaintID.ToString() }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> SendMessageToRegisterd(string storeName, string UsernameReciever, string title, string message)
        {
            const string url = "api/market/SendMessageToRegisterd";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName },
                { "title", title},
                { "message", message},
                {"UsernameReciever", UsernameReciever}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<ICollection<AdminMessageToRegisteredDTO>>> GetRegisteredMessagesFromAdmin()
        {
            Response<ICollection<AdminMessageToRegisteredDTO>> res = await _httpService.Get<Response<ICollection<AdminMessageToRegisteredDTO>>>("api/market/GetRegisteredMessagesFromAdmin");
            return res;
        }

        public async Task<Response<ICollection<MessageToStoreDTO>>> GetRegisterAnsweredStoreMessages()
        {
            Response<ICollection<MessageToStoreDTO>> res = await _httpService.Get<Response<ICollection<MessageToStoreDTO>>>("api/market/GetRegisterAnsweredStoreMessages");
            return res;
        }

        public async Task<Response<ICollection<NotifyMessageDTO>>> GetRegisteredMessagesNotofication()
        {
            Response<ICollection<NotifyMessageDTO>> res = await _httpService.Get<Response<ICollection<NotifyMessageDTO>>>("api/market/GetRegisteredMessagesNotofication");
            return res;
        }

        public async Task<Response> AppointSystemAdmin(string adminUsername)
        {
            const string url = "api/market/AppointSystemAdmin";
            var param = new Dictionary<string, string>() {
                { "adminUsername" , adminUsername }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<List<StoreDTO>>> GetAllActiveStores()
        {
            Response<List<StoreDTO>> res = await _httpService.Get<Response<List<StoreDTO>>>("api/market/GetAllActiveStores");
            return res;
        }

        public async Task<Response> HasPermission(string storeName, string op)
        {
            const string url = "api/market/HasPermission";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName },{ "op",op}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);

            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response<ItemDTO>> GetItem(string storeName, int itemId)
        {
            const string url = "api/market/GetItem";
            var param = new Dictionary<string, string>() {
                { "storeName" , storeName },{ "itemId",itemId.ToString()}
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);
            Response<ItemDTO> res = await _httpService.Get<Response<ItemDTO>>(newUrl);
            return res;
        }

        public async Task<Response<List<StoreDTO>>> GetStoresOfUser()
        {
            Response<List<StoreDTO>> res = await _httpService.Get<Response<List<StoreDTO>>>("api/market/GetStoresOfUser");
            return res;
        }

        public async Task<Response> SendAdminMessage(String receiverUsername, String title, String message)
        {
            const string url = "api/market/SendMessageToRegisterd";
            var param = new Dictionary<string, string>() {
                { "UsernameReciever", receiverUsername },
                { "title", title },
                { "message", message }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);
            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }

        public async Task<Response> AddStoreDiscount(string StoreName, string ConditionString, string DiscountString)
        {
            const string url = "api/market/AddStoreDiscount";
            var param = new Dictionary<string, string>() {
                { "storeName", StoreName },
                { "conditionString", ConditionString },
                { "discountString", DiscountString }
            };
            var newUrl = QueryHelpers.AddQueryString(url, param);
            Response res = await _httpService.Post<Response>(newUrl, null);
            return res;
        }
    }
}
