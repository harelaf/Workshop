using MarketWeb.Shared.DTO;
using MarketWeb.Shared;
using MarketWebProject.Models;
using MarketWebProject.Views.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MarketWeb.Client.Connect;
namespace MarketWebProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IMarketAPIClient _marketAPIClient;
        //private Service service

        private void TurnGuest()
        {
            LayoutConfig.IsGuest = true;
            LayoutConfig.IsLoggedIn = false;
            LayoutConfig.IsAdmin = false;
        }

        private void TurnLoggedIn()
        {
            LayoutConfig.IsGuest = false;
            LayoutConfig.IsLoggedIn = true;
            LayoutConfig.IsAdmin = false;
        }

        private void TurnAdmin()
        {
            LayoutConfig.IsGuest = false;
            LayoutConfig.IsLoggedIn = false;
            LayoutConfig.IsAdmin = true;
        }

        public HomeController(ILogger<HomeController> logger, IMarketAPIClient marketAPIClient)
        {
            _logger = logger;
            _marketAPIClient = marketAPIClient;
            marketAPIClient.EnterSystem();
        }

        public IActionResult Index(MainModel modelcs)
        {
            /*if (modelcs == null)
                modelcs = new MainModel();
            //Task<Response<List<StoreDTO>>> task = _marketAPIClient.GetAllActiveStores();

             StoreDTO store1 = new StoreDTO("Store1", new StoreFounderDTO("admin", "Store1"), null, null, new StockDTO(new Dictionary<ItemDTO, int>()), 
                 new Queue<MessageToStoreDTO>(), new RatingDTO(new Dictionary<string, Tuple<int, string>>()), new List<StoreManagerDTO>(), new List<StoreOwnerDTO>(), StoreState.Active);
            StoreDTO store2 = new StoreDTO("Store2", new StoreFounderDTO("admin", "Store2"), null, null, new StockDTO(new Dictionary<ItemDTO, int>()),
                 new Queue<MessageToStoreDTO>(), new RatingDTO(new Dictionary<string, Tuple<int, string>>()), new List<StoreManagerDTO>(), new List<StoreOwnerDTO>(), StoreState.Active);
            List<StoreDTO> lst = new List<StoreDTO>();
            lst.Add(store1);
            lst.Add(store2);
            Response<List<StoreDTO>> response = new Response<List<StoreDTO>>(lst);

            //Response<List<StoreDTO>> response = task.Result;
            ViewResult view;
            if (response.ErrorOccured)
            {
                view = View(modelcs);
                view.ViewData["activeStores"] = new List<StoreDTO>();
                modelcs.ErrorOccurred = true;
                modelcs.Message = response.ErrorMessage;
            }
            else
            {
                view = View(modelcs);
                view.ViewData["activeStores"] = response.Value;
            }
            return view;*/
        }

        public IActionResult AdministrationPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            Task<Response> task = _marketAPIClient.HasPermission(null, "PERMENENT_CLOSE_STORE");
            Response isAdmin = task.Result;
            if (isAdmin.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = isAdmin.ErrorMessage });
            }
            else
            {
                return View(modelcs);
            }
        }

        public IActionResult OpenNewStorePage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            return View(modelcs);
        }

        public IActionResult OpenNewStoreForm(MainModel modelcs, String storename)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            Task<Response> task = _marketAPIClient.OpenNewStore(storename);
            Response response = task.Result;
            if (response.ErrorOccured)
            {
                return RedirectToAction("OpenNewStorePage", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = false, Message = "Store opened successfully!" });
            }
        }

        public IActionResult CartPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            Task<Response<ShoppingCartDTO>> task = _marketAPIClient.ViewMyCart();
            Response<ShoppingCartDTO> cart = task.Result; //new Response<ShoppingCartDTO>(new ShoppingCartDTO());
            if (cart.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = cart.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["cart"] = cart.Value;
                return viewResult;
            }
        }

        public IActionResult ItemSearchPage(MainModel modelcs, string category, string itemname,string keyword)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            if (category == null && itemname == null && keyword == null)
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = "must enter at least one option of search" });
            Task<Response<List<ItemDTO>>> task = _marketAPIClient.GetItemInformation(itemname, category, keyword);
            /*List<ItemDTO> items = new List<ItemDTO>();
            items.Add(new ItemDTO("item1", 10.5, "store1"));
            items.Add(new ItemDTO("item2", 9.5, "store1"));
            items.Add(new ItemDTO("item3", 15.5, "store2"));*/
            Response<List<ItemDTO>> response = task.Result; //new Response<List<ItemDTO>>(items);
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["searchedItems"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult StoreRolePage(string storeName)
        {
            MainModel modelcs = new MainModel();
            Task<Response<StoreFounderDTO>> taskFounder = _marketAPIClient.GetStoreFounder(storeName);
            Response<StoreFounderDTO> founder_res = taskFounder.Result;//new Response<StoreFounderDTO>(new StoreFounderDTO(storeName, "Yafa"));//service.GetStoreFounder(storeName);
            /*List<StoreOwnerDTO> owners_list = new List<StoreOwnerDTO>();
            owners_list.Add(new StoreOwnerDTO(storeName, "Afik", "Shlomi"));
            owners_list.Add(new StoreOwnerDTO(storeName, "Shlomi", "Beni"));
            owners_list.Add(new StoreOwnerDTO(storeName, "Beni", "Yafa"));*/
            Task<Response<List<StoreOwnerDTO>>> taskOwners = _marketAPIClient.GetStoreOwners(storeName);
            Response<List<StoreOwnerDTO>> owners_res = taskOwners.Result;//new Response<List<StoreOwnerDTO>>(owners_list); //_service.GetStoreOwners(storeName);
            /*List<StoreManagerDTO> managers_list = new List<StoreManagerDTO>();
            managers_list.Add(new StoreManagerDTO(storeName, "Afik", "Shlomi"));
            managers_list.Add(new StoreManagerDTO(storeName, "Shlomi", "Beni"));
            managers_list.Add(new StoreManagerDTO(storeName, "Beni", "Yafa"));*/
            Task<Response<List<StoreManagerDTO>>> taskManagers = _marketAPIClient.GetStoreManagers(storeName);
            Response<List<StoreManagerDTO>> managers_res = taskManagers.Result; //new Response<List<StoreManagerDTO>>(managers_list); //_service.GetStoreManagers(storeName);
            if (owners_res.ErrorOccured || managers_res.ErrorOccured || founder_res.ErrorOccured)
            {
                string message = "owners: " + owners_res.ErrorMessage + "\nmanagers: " + managers_res.ErrorMessage + "\nfounder: "+founder_res.ErrorMessage;
                return RedirectToAction("StorePage", "Home", new {ErrorOccurred = true, Message = message });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["owners"] = owners_res.Value;
                viewResult.ViewData["managers"] = managers_res.Value;
                viewResult.ViewData["founder"] = founder_res.Value;
                viewResult.ViewData["storename"] = storeName;
                return viewResult;
            }
        }
        public IActionResult ItemPage(string storeName, int itemId)
        {
            MainModel modelcs = new MainModel();
            Task<Response<ItemDTO>> task = _marketAPIClient.GetItem(storeName, itemId);
            Response<ItemDTO> response = task.Result; //new Response<ItemDTO>(new ItemDTO("banana", 20.5, "store1"));
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["item"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult MyProfile(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            Task<Response<RegisteredDTO>> task1 = _marketAPIClient.GetVisitorInformation();
            Response<RegisteredDTO> response1 = task1.Result;
            Task<Response<List<StoreDTO>>> task2 = _marketAPIClient.GetStoresOfUser();
            Response<List<StoreDTO>> response2 = task2.Result;
            /*Response<RegisteredDTO> response1 = new Response<RegisteredDTO>(new RegisteredDTO());
            List<StoreDTO> storesDTO = new List<StoreDTO>();
            storesDTO.Add(new StoreDTO());
            Response<List<StoreDTO>> response2 = new Response<List<StoreDTO>>(storesDTO);*/
            if (response1.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response1.ErrorMessage });
            }
            else if (response2.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response2.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["visitor"] = response1.Value;
                viewResult.ViewData["stores"] = response2.Value;
                return viewResult;
            }
        }

        public IActionResult MyPurchaseHistory(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            Task<Response<ICollection<PurchasedCartDTO>>> task1 = _marketAPIClient.GetMyPurchasesHistory();
            Response<ICollection<PurchasedCartDTO>> response1 = task1.Result;
            Task<Response<RegisteredDTO>> task2 = _marketAPIClient.GetVisitorInformation();
            Response<RegisteredDTO> response2 = task2.Result;
            /*List<PurchasedCartDTO> history = new List<PurchasedCartDTO>();
            history.Add(new PurchasedCartDTO());
            history.Add(new PurchasedCartDTO());
            Response<ICollection<PurchasedCartDTO>> response1 = new Response<ICollection<PurchasedCartDTO>>(history);
            Response<RegisteredDTO> response2 = new Response<RegisteredDTO>(new RegisteredDTO());*/
            if (response1.ErrorOccured || response2.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response1.ErrorMessage });
            }
            else if (response2.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response2.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["history"] = response1.Value;
                viewResult.ViewData["username"] = response2.Value.Username;
                return viewResult;
            }
        }

        public IActionResult MyMessages(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            Task<Response<RegisteredDTO>> task = _marketAPIClient.GetVisitorInformation();
            Response<RegisteredDTO> response = task.Result;
            //Response<RegisteredDTO> response = new Response<RegisteredDTO>(new RegisteredDTO());
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["visitor"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult ChangePassword(MainModel modelcs, string newPassword, string oldPassword)
        {
            Task<Response> task = _marketAPIClient.EditVisitorPassword(oldPassword, newPassword);
            Response response = task.Result;
            //Response response = new Response();
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = false, Message = "Operation Successful!" });
            }
        }

        public IActionResult ItemPageEditable(string storeName, int itemId)
        {

            MainModel modelcs = new MainModel();
            Task<Response<ItemDTO>> task = _marketAPIClient.GetItem(storeName, itemId);
            Response<ItemDTO> response = task.Result;
            //Response<ItemDTO> response = new Response<ItemDTO>(new ItemDTO("banana", 20.5, "store1"));
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new {ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["item"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult StorePage(MainModel modelcs, string storename)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            string viewName = "StorePage";

            Task<Response<StoreDTO>> task = _marketAPIClient.GetStoreInformation(storename);
            Response<StoreDTO> response = task.Result;//new Response<StoreDTO>(new StoreDTO(new StoreFounderDTO("afik's store", "afik"),"afik's store", "Active"));// get store frome service
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                Response stockPermissions = _marketAPIClient.HasPermission(storename, "MANAGE_STOCK").Result;
                Response rolesPermission = _marketAPIClient.HasPermission(storename, "APPOINT_OWNER").Result; // if has accsess to appoint owner' he has all roles permissions.
                Response closePermission = _marketAPIClient.HasPermission(storename, "CLOSE_STORE").Result; 
                Response reopenPermission = _marketAPIClient.HasPermission(storename, "REOPEN_STORE").Result; 
                Response closePermenantlyPermission =_marketAPIClient.HasPermission(storename, "PERMENENT_CLOSE_STORE").Result; 
                Response purchaseHistoryPermission = _marketAPIClient.HasPermission(storename, "STORE_HISTORY_INFO").Result; 
                Response storeMsgPermission = _marketAPIClient.HasPermission(storename, "RECEIVE_AND_REPLY_STORE_MESSAGE").Result; 

                ViewResult viewResult = View(viewName, modelcs);
                viewResult.ViewData["store"] = response.Value;
                viewResult.ViewData["manageRoles"] = !rolesPermission.ErrorOccured;
                viewResult.ViewData["manageStock"] = !stockPermissions.ErrorOccured;
                viewResult.ViewData["closeStore"] = !closePermission.ErrorOccured;
                viewResult.ViewData["reopenStore"] = !reopenPermission.ErrorOccured;
                viewResult.ViewData["closeStorePermenantly"] = !closePermenantlyPermission.ErrorOccured;
                viewResult.ViewData["purchaseHistory"] = !purchaseHistoryPermission.ErrorOccured;
                viewResult.ViewData["storeMsg"] = !storeMsgPermission.ErrorOccured;
                viewResult.ViewData["ActiveStore"] = response.Value.State== StoreState.Active;
                return viewResult;
            }
        }

        public IActionResult RegistrationPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            return View(modelcs);
        }
        public IActionResult PurchasePage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            return View(modelcs);
        }

        public IActionResult LoginPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            return View(modelcs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RegisterForm(string name, string password, DateTime dob)
        {// register
            Task<Response> task = _marketAPIClient.Register(name, password, dob);
            Response response = task.Result;//new Response(new Exception(err_msg));//call register(name, password, dob)
            String msg = "Successfully registered! You can now log in.";
            if (response.ErrorOccured)
            {
                msg = response.ErrorMessage;
            }
            return RedirectToAction("RegistrationPage", "Home", new { ErrorOccurred = response.ErrorOccured, Message = msg });
        }

        public IActionResult LoginForm(string name, string password)
        {//login
            Task<Response<string>> task = _marketAPIClient.Login(name, password);
            Response response = task.Result;
            if (response.ErrorOccured)
            {
                TurnGuest();
                return RedirectToAction("LoginPage", "Home", new { ErrorOccurred = response.ErrorOccured, Message = response.ErrorMessage });
            }
            else
            {
                //TODO: CHECK IF IS ADMIN THEN TurnAdmin();
                TurnLoggedIn();
                Task<Response> task_admin = _marketAPIClient.HasPermission(null, "PERMENENT_CLOSE_STORE");
                Response isAdmin = task_admin.Result;
                if (!isAdmin.ErrorOccured)
                {
                    TurnAdmin();
                }
                return RedirectToAction("Index", "Home", new { });
            }
        }

        public IActionResult Logout()
        {//logout
            Response response = _marketAPIClient.Logout().Result;//new Response(new Exception(err_msg));//call logout()
            if (response.ErrorOccured)
            {
                TurnGuest();
                return RedirectToAction("Index", "Home", new { ErrorOccurred = response.ErrorOccured, Message = response.ErrorMessage });
            }
            else
            {
                TurnGuest();
                return RedirectToAction("Index", "Home", new { });
            }
        }

        public IActionResult RemoveItemFromCart(String storeName, int itemID)
        {
            //I_User_ServiceLayer SL = validateConnection();
            Task<Response> task = _marketAPIClient.RemoveItemFromCart(itemID, storeName);
            Response res = task.Result;//new Response(new Exception("could'nt remove item: " + itemID + " from store: " + storeName));//=service.removeItemFromCart(token,itemID, storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("CartPage", "Home", new { ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("CartPage", "Home");
            }
        }
        public IActionResult UpdateItemQuantityInCart(String storeName, int itemID, int newQuantity)
        {
            //I_User_ServiceLayer SL = validateConnection();
            Task<Response> task = _marketAPIClient.UpdateQuantityOfItemInCart(itemID, storeName, newQuantity);
            Response res = task.Result;//new Response(new Exception("could'nt update item: "+itemID+" from store: "+storeName+" to quantity: "+newQuantity));//=service.removeItemFromCart(token,itemID, storeName, newQuantity)
            if (res.ErrorOccured)
            {
                return RedirectToAction("CartPage", "Home", new { ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("CartPage", "Home");
            }
        }
        
        public IActionResult AddItemToCart(int amount, string storename, int itemid)
        {
            Task<Response> task = _marketAPIClient.AddItemToCart(itemid, storename, amount);
            Response res = task.Result;
            if (!res.ErrorOccured)
            {
                return RedirectToAction("CartPage", "Home", ModelState);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = res.ErrorMessage }); ;
            }
        }

        public IActionResult PurchaseUserCart(String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode)
        {
            String purchaseDet = "address: "+address+", city: "+ city+", country: "+country+", zip: "+zip+", purchaserName: "+purchaserName;
            Response response = new Response();// service.PurchaseMyCart(address, city, country, zip, purchaserName, paymentMethode, shipmentMethode)
            if (!response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = false, Message = "congratulations on your purchase!!\nyour purchase details: \n"+purchaseDet });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage }); ;
            }
        }

        public IActionResult StorePurchaseHistoryPage(MainModel modelcs, string storeName)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            List<Tuple<DateTime, ShoppingBasketDTO>> tuples = new List<Tuple<DateTime, ShoppingBasketDTO>>();
            tuples.Add(new Tuple<DateTime, ShoppingBasketDTO>(new DateTime(2022, 5, 30, 14, 30, 0), new ShoppingBasketDTO("store1",new Dictionary<ItemDTO, int>())));
            tuples.Add(new Tuple<DateTime, ShoppingBasketDTO>(new DateTime(2022, 5, 18, 10, 14, 0), new ShoppingBasketDTO("store2",new Dictionary<ItemDTO, int>())));
            tuples.Add(new Tuple<DateTime, ShoppingBasketDTO>(new DateTime(2022, 5, 1, 22, 55, 0),  new ShoppingBasketDTO("store3",new Dictionary<ItemDTO, int>())));
            Response<List<Tuple<DateTime, ShoppingBasketDTO>>> response = new Response<List<Tuple<DateTime, ShoppingBasketDTO>>>(tuples);// service.GetStorePurchaseHistory(token storeName);
            if (!response.ErrorOccured)
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["history"] = response.Value;
                viewResult.ViewData["storeName"] = storeName;
                return viewResult;
            }
            else
            {
                return RedirectToAction("StorePage", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage }); 
            }
        }

        public IActionResult StoreMessagesPage(MainModel modelcs, string storeName)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            Queue<MessageToStoreDTO> messages = new Queue<MessageToStoreDTO>();
            messages.Enqueue(new MessageToStoreDTO("Store1", "afik", "goog store", "good!!","happy 4 u", "admin", 1));
            messages.Enqueue(new MessageToStoreDTO("Store1", "afik", "goog store", "good!!","happy 4 u", "admin", 2));
            messages.Enqueue(new MessageToStoreDTO("Store1", "afik", "goog store", "good!!","happy 4 u", "admin", 3));
            Response<Queue<MessageToStoreDTO>> response = new Response<Queue<MessageToStoreDTO>>(messages);// service.GetStoreMessages(token, storeName);
            if (!response.ErrorOccured)
            {
                ViewResult viewResult = View("StoreMessagesPage",modelcs);
                viewResult.ViewData["messages"] = response.Value;
                viewResult.ViewData["storeName"] = storeName;
                return viewResult;
            }
            else
            {
                return RedirectToAction("StorePage", "Home", new {storeName=storeName, ErrorOccurred = true, Message = response.ErrorMessage }); ;
            }
        }

        public IActionResult ReplyMessage(string storename, string receiverUsername, int msgId, string replyMessage)
        {
            Response response = new Response();// service.AnswerStoreMesseage(authToken, receiverUsername, msgId, storeName, replyMessage);
            if (!response.ErrorOccured)
            {
                return RedirectToAction("StoreMessagesPage", "Home", new {storeName=storename,  ErrorOccurred = false, Message = "reply sent: "+replyMessage });
            }
            else
            {
                return RedirectToAction("StoreMessagesPage", "Home", new { storeName = storename, ErrorOccurred = true, Message = response.ErrorMessage });
            }
        }

        public IActionResult SendAdminMessage(String receiverUsername, String title, String message)
        {
            Response response = new Response();// service.SendAdminMessage(authToken, receiverUsername, title, message);
            if (!response.ErrorOccured)
            {
                return RedirectToAction("AdministrationPage", "Home", new { ErrorOccurred = false, Message = "Message sent." });
            }
            else
            {
                return RedirectToAction("AdministrationPage", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
        }

        public IActionResult PermanentlyDeleteUser(String username)
        {
            //RemoveRegisteredVisitor(authToken, username);
            Response response = new Response();
            if (response.ErrorOccured)
            {
                return RedirectToAction("AdministrationPage", "Home", new { ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                return RedirectToAction("AdministrationPage", "Home", new { ErrorOccurred = false, Message = "User Deleted." });
            }
        }

        public IActionResult RemoveItemFromStock(String storeName, int itemID)
        {
            Response res;//=service.RemoveItemFromStock(itemID, storeName)
            res = new Response(new Exception("could'nt remove item: " + itemID + " from stock of store: " + storeName));//err
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = false, Message = "item removed successfully" });
            }
        }//

        public IActionResult UpdateItemQuantityInStock(String storeName, int itemID, int newQuantity)
        {
            Response res = new Response(new Exception("could'nt update item: " + itemID + " from store: " + storeName + " to quantity: " + newQuantity));//=service.removeItemFromStock(itemID, storeName, newQuantity)
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = false, Message = "item quantity updateed successfully" });
            }
        }
        public IActionResult AppointStoreOwner(String storeName, string ownerUsername)
        {
            Response res;//=service.AppointStoreOwner(token,itemID, storeName)
            res = new Response(new Exception("could'nt appoint user: " + ownerUsername + " to be owner of store: " + storeName));//err
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = false, Message = "user: " + ownerUsername + " is employed!" });

            }
        }//

        public IActionResult FireStoreOwner(String storeName, string ownerUsername)
        {
            //I_User_ServiceLayer SL = validateConnection();
            Response res;//=service.FireStoreOwner(token,itemID, storeName)
            res = new Response(new Exception("could'nt Fire user: " + ownerUsername + " from being owner of store: " + storeName));//err
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", 
                    new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home",
                    new { storeName = storeName, ErrorOccurred = false, Message = "user: " + ownerUsername + " got fired!" });

            }
        }
        public IActionResult AppointStoreManager(String storeName, string managerUsername)
        {
            Response res;//=service.AppointStoreManager(itemID, storeName)
            res = new Response(new Exception("could'nt appoint user: " + managerUsername + " to be Manager of store: " + storeName));//err
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home",
                    new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", 
                    new { storeName = storeName, ErrorOccurred = false, Message = "user: " + managerUsername + " is employed!"});

            }
        }//

        public IActionResult FireStoreManager(String storeName, string managerUsername)
        {
            Response res;//=service.FireStoreManager(itemID, storeName)
            res = new Response(new Exception("could'nt Fire user: " + managerUsername + " from being Manager of store: " + storeName));//err
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home",
                    new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", 
                    new { storeName = storeName, ErrorOccurred = false, Message ="user: " +managerUsername+" got fired!" });
                    
            }
        }
        public IActionResult ModifyStoreManagerPermission(String storeName, string managerUsername, bool ReceiveInfoAndReply, bool ReceiveStorePurchaseHistory)
        {
            string feedback = "";
            //hasPermmission(
            Response res_history= new Response();
            Response res_hasAcces_history = new Response();// service.HasPermission(storeName,managerUsername,"STORE_HISTORY_INFO")
            if((!res_hasAcces_history.ErrorOccured) != ReceiveStorePurchaseHistory)
            {
                if (ReceiveStorePurchaseHistory)
                {
                    res_history = new Response();//service.AddManagerPermmision(storeName,managerUsername,"STORE_HISTORY_INFO");
                    feedback += "add to store manager : " + managerUsername + " permission: Store Purchase History\n";
                }
                else
                {
                    res_history = new Response();//service.RemoveManagerPermmision(storeName,managerUsername,"STORE_HISTORY_INFO");
                    feedback += "remove from store manager : " + managerUsername + " permission: Store Purchase History\n";
                }
            }
            
            Response res_info = new Response();
            Response res_hasAcces_info = new Response();// service.HasPermission(storeName,managerUsername,"RECEIVE_AND_REPLY_STORE_MESSAGE")
            if ((!res_hasAcces_info.ErrorOccured) != ReceiveInfoAndReply)
            {
                if (ReceiveInfoAndReply)
                {
                    res_info = new Response();//service.AddManagerPermmision(storeName,managerUsername,"RECEIVE_AND_REPLY_STORE_MESSAGE");
                    feedback += "add to store manager : " + managerUsername + " permission: Recive and Reply to Msg\n";
                }
                else
                {
                    res_info = new Response();//service.RemoveManagerPermmision(storeName,managerUsername,"RECEIVE_AND_REPLY_STORE_MESSAGE");
                    feedback += "remove from store manager : " + managerUsername + " permission: Recive and Reply to Msg\n";
                }
            }
            //res = new Response(new Exception("could'nt Fire user: " + managerUsername + " from being Manager of store: " + storeName));//err
            if (res_info.ErrorOccured || res_history.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home",
                    new { storeName = storeName, ErrorOccurred = true, Message = "history permission: "+res_history.ErrorMessage+"\nMsg permission: "+res_info.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home",
                    new { storeName = storeName, ErrorOccurred = false, Message =feedback });

            }
        }
        public IActionResult UpdateItemName(string storename, string itemId, string newName)
        {
            Response res = new Response(new Exception("could'nt update item: " + itemId + " from store: " + storename + " to new name: " + newName));
            //=service.EditItemName(token,itemId, storemame, newName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("ItemPageEditable", "Home",
                    new { storeName= storename, itemId= itemId, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("ItemPageEditable", "Home",
                    new { storeName = storename, itemId = itemId });
            }
        }
        public IActionResult UpdateItemPrice(string storename, string itemId, string newPrice)
        {
            Response res = new Response(new Exception("could'nt update item: " + itemId + " from store: " + storename + " to new price: " + newPrice));
            //=service.EditItemPrice(token,itemId, storemame, newPrice)
            if (res.ErrorOccured)
            {
                return RedirectToAction("ItemPageEditable", "Home",
                    new { storeName = storename, itemId = itemId, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("ItemPageEditable", "Home",
                     new { storeName = storename, itemId = itemId });
            }
        }
        public IActionResult UpdateItemDescription(string storename, string itemId, string newDescription)
        {
            Response res = new Response(new Exception("could'nt update item: " + itemId + " from store: " + storename + " to new Description: " + newDescription));
            //=service.EditItemDescription(token,itemId, storemame, newDescription)
            if (res.ErrorOccured)
            {
                return RedirectToAction("ItemPageEditable", "Home",
                    new { storeName = storename, itemId = itemId, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("ItemPageEditable", "Home",
                    new { storeName = storename, itemId = itemId});
            }
        }
        public IActionResult AddItemToStoreStock(String storeName, int itemID, String name, double price, String description, String category, int quantity)
        {//II.4.1
            //I_User_ServiceLayer SL = validateConnection();
            Response res = new Response(new Exception("could'nt add item: " + itemID + " to store: " + storeName + " stock. "));//=service.AddItemToStoreStock(storeName, itemID, name, price, description, category, quantity)
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = false, Message = "item added successfully" });
            }
        }
        
        public IActionResult CloseStore(string storeName)
        {
            //I_User_ServiceLayer SL = validateConnection();
            Response res = new Response(new Exception("could'nt close store: " + storeName));//=service.CloseStore(storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", 
                    new { storeName = storeName, ErrorOccurred = false, Message = "store closed successfully" });
            }
        }
        public IActionResult ReopenStore(string storeName)
        {
            //I_User_ServiceLayer SL = validateConnection();
            Response res = new Response(new Exception("could'nt reopen store: " + storeName));//=service.ReopenStore(storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home",
                    new { storeName = storeName, ErrorOccurred = false, Message = "store reopend successfully" });
            }
        }
     
        public IActionResult CloseStorePermenantltz(string storeName)
        {
            //I_User_ServiceLayer SL = validateConnection();
            Response res = new Response(new Exception("could'nt close store: " + storeName));//=service.CloseStore(storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", 
                    new { storeName = storeName, ErrorOccurred = false, Message = "store closed successfully" });
            }
        }

        public ActionResult AddStoreReview(string storeName, int rating,string comment)
        {
            //I_User_ServiceLayer SL = validateConnection();
            if (comment == null)
                comment = "";
            Response res = new Response(new Exception("could'nt add review for store: " + storeName+"with rating: "+rating+" and comment: "+comment));//=service.CloseStore(storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storeName, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home", 
                    new { storeName = storeName, ErrorOccurred = false, Message = "review added successfully" });
            }
        }

        public ActionResult AddItemReview(string storeName, int itemId, int rating, string comment)
        {
            //I_User_ServiceLayer SL = validateConnection();
            if (comment == null)
                comment = "";
            Response res = new Response(new Exception("could'nt add review for item: " + itemId + "with rating: " + rating + " and comment: " + comment));//=service.CloseStore(storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("ItemPage", "Home", new { storeName=storeName,  itemId = itemId, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("ItemPage", "Home",
                    new { storeName = storeName, itemId = itemId, ErrorOccurred = false, Message = "review added successfully" });
            }
        }

        public ActionResult AddItemEditableReview(string storeName, int itemId, int rating, string comment)
        {
            //I_User_ServiceLayer SL = validateConnection();
            if (comment == null)
                comment = "";
            Response res = new Response(new Exception("could'nt add review for item: " + itemId + "with rating: " + rating + " and comment: " + comment));//=service.CloseStore(storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("ItemPageEditable", "Home", new { storeName = storeName, itemId = itemId, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("ItemPageEditable", "Home",
                    new { storeName = storeName, itemId = itemId, ErrorOccurred = false, Message = "review added successfully" });
            }
        }

        public IActionResult SendMsgToStore(string storename, string msg)
        {
            Response res = new Response(new Exception("could'nt send your msg: "+msg+" to store: "+storename));//=service.CloseStore(storeName)
            if (res.ErrorOccured)
            {
                return RedirectToAction("StorePage", "Home", new { storeName = storename, ErrorOccurred = true, Message = res.ErrorMessage });
            }
            else
            {
                return RedirectToAction("StorePage", "Home",
                    new { storeName = storename, ErrorOccurred = false, Message = "send msg successfully" });
            }
        }
    }
}