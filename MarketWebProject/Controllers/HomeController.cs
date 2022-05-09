using MarketWebProject.DTO;
using MarketWebProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MarketWebProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private Service service
        private bool IsGuest = true;
        private bool IsLoggedIn = false;
        private bool IsAdmin = false;

        private void TurnGuest()
        {
            IsGuest = true; IsLoggedIn = false; IsAdmin = false;
        }

        private void TurnLoggedIn()
        {
            IsGuest = false; IsLoggedIn = true; IsAdmin = false;
        }

        private void TurnAdmin()
        {
            IsGuest = false; IsLoggedIn = false; IsAdmin = true;
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            StoreDTO store1 = new StoreDTO();
            StoreDTO store2 = new StoreDTO();
            List<StoreDTO> lst = new List<StoreDTO>();
            lst.Add(store1);
            lst.Add(store2);
            Response<List<StoreDTO>> response = new Response<List<StoreDTO>>(lst);
            ViewResult view;
            if (response.ErrorOccured)
            {
                view = View(modelcs);
                view.ViewData["activeStores"] = new List<StoreDTO>();
                //TODO: SOMEHOW PASS ERROR MESSAGE?
            }
            else
            {
                view = View(modelcs);
                view.ViewData["activeStores"] = response.Value;
            }
            return view;
        }

        public IActionResult Privacy(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            return View(modelcs);
        }

        public IActionResult CartPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            //viewMyCart
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            Response<ShoppingCartDTO> cart = new Response<ShoppingCartDTO>(new ShoppingCartDTO());
            if (cart.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = cart.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["cart"] = cart.Value;
                return viewResult;
            }

        }

        public IActionResult ItemSearchPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            List<ItemDTO> items = new List<ItemDTO>();
            items.Add(new ItemDTO("item1", 10.5, "store1"));
            items.Add(new ItemDTO("item2", 9.5, "store1"));
            items.Add(new ItemDTO("item3", 15.5, "store2"));
            Response<List<ItemDTO>> response = new Response<List<ItemDTO>>(items);
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["searchedItems"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult ItemPage(string storeName, int itemId)
        {

            MainModel modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            //Response<ItemDTO> response=  getItem(storeName, itemId)
            Response<ItemDTO> response = new Response<ItemDTO>(new ItemDTO("banana", 20.5, "store1"));
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
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
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            //Response<RegisteredDTO> response = GetUserInformation(authToken);
            Response<RegisteredDTO> response = new Response<RegisteredDTO>(new RegisteredDTO());
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["visitor"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult MyPurchaseHistory(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            //Response<ICollection<PurchasedCartDTO>> response1 = GetMyPurchasesHistory(authToken);
            //Response<RegisteredDTO> response2 = GetUserInformation(authToken);
            List<PurchasedCartDTO> history = new List<PurchasedCartDTO>();
            history.Add(new PurchasedCartDTO());
            history.Add(new PurchasedCartDTO());
            Response<ICollection<PurchasedCartDTO>> response1 = new Response<ICollection<PurchasedCartDTO>>(history);
            Response<RegisteredDTO> response2 = new Response<RegisteredDTO>(new RegisteredDTO());
            if (response1.ErrorOccured || response2.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response1.ErrorMessage });
            }
            else if (response2.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response2.ErrorMessage });
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
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            //Response<RegisteredDTO> response = GetUserInformation(authToken);
            Response<RegisteredDTO> response = new Response<RegisteredDTO>(new RegisteredDTO());
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(modelcs);
                viewResult.ViewData["visitor"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult ChangeUsername(MainModel modelcs, string newUsername)
        {
            //Response response = EditVisitorUsername(authToken, newUsername);
            Response response = new Response();
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = false, Message = "Operation Successful!" });
            }
        }

        public IActionResult ChangePassword(MainModel modelcs, string newPassword, string oldPassword)
        {
            //Response response = EditPassword(authToken, newPassword, oldPassword);
            Response response = new Response();
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = false, Message = "Operation Successful!" });
            }
        }

        public IActionResult StorePage(MainModel modelcs, string storename)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            string viewName = "StorePageGuest";
            Response isManager = new Response();// service.isManager(token, storeName)
            Response isOwner = new Response();// service.isOwner(token, storeName)
            Response isFounder = new Response();// service.isFounder(token, storeName)
            if (!isManager.ErrorOccured)
            {
                viewName = "StorePageManager";
            }
            //GetStore
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            Response<StoreDTO> response = new Response<StoreDTO>(new StoreDTO());// get store frome service
            if (response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
            }
            else
            {
                ViewResult viewResult = View(viewName, modelcs);
                viewResult.ViewData["store"] = response.Value;
                return viewResult;
            }
        }

        public IActionResult RegistrationPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            return View(modelcs);
        }
        public IActionResult PurchasePage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            return View(modelcs);
        }

        public IActionResult LoginPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            return View(modelcs);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RegisterForm(string name, string password, DateTime dob)
        {// register
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "register faild: your pass\\username are not valid";
            String SuccessMessage = "Successfully registered! You can now log in.";
            Response response = new Response(new Exception(err_msg));//call register(name, password, dob)
            return RedirectToAction("RegistrationPage", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = response.ErrorOccured, Message = response.ErrorMessage });
        }

        public IActionResult LoginForm(string name, string password)
        {//login
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "login faild: your username or password are wrong!";
            Response response = new Response();//call login(name, password)
            if (response.ErrorOccured)
            {
                TurnGuest();
                return RedirectToAction("LoginPage", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = response.ErrorOccured, Message = err_msg });
            }
            else
            {
                //TODO: CHECK IF IS ADMIN THEN TurnAdmin();
                TurnLoggedIn();
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin });
            }
        }

        public IActionResult Logout()
        {//logout
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "Logout Failed";
            Response response = new Response(new Exception(err_msg));//call login(name, password)
            if (response.ErrorOccured)
            {
                TurnGuest();
                return RedirectToAction("LoginPage", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = response.ErrorOccured, Message = err_msg });
            }
            else
            {
                TurnLoggedIn();
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin });
            }
        }

        public IActionResult RemoveItemFromCart(String storeName, int itemID)
        {
            //I_User_ServiceLayer SL = validateConnection();
            Response res = new Response(new Exception("could'nt remove item: " + itemID + " from store: " + storeName));//=service.removeItemFromCart(token,itemID, storeName)
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
            Response res = new Response(new Exception("could'nt update item: "+itemID+" from store: "+storeName+" to quantity: "+newQuantity));//=service.removeItemFromCart(token,itemID, storeName, newQuantity)
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
            Console.WriteLine(amount + storename + itemid);
            Response res = new Response();//service.AddItemToCart(itemId, store, amount)
            if (!res.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", ModelState);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = res.ErrorMessage }); ;
            }
        }

        public IActionResult PurchaseUserCart(String address, String city, String country, String zip, String purchaserName, string paymentMethode, string shipmentMethode)
        {
            String purchaseDet = "address: "+address+", city: "+ city+", country: "+country+", zip: "+zip+", purchaserName: "+purchaserName;
            Response response = new Response();// service.PurchaseMyCart(address, city, country, zip, purchaserName, paymentMethode, shipmentMethode)
            if (!response.ErrorOccured)
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = false, Message = "congratulations on your purchase!!\nyour purchase details: \n"+purchaseDet });
            }
            else
            {
                return RedirectToAction("Index", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage }); ;
            }
        }

        public IActionResult StorePurchaseHistoryPage(MainModel modelcs, string storeName)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            List<Tuple<DateTime, ShoppingBasketDTO>> tuples = new List<Tuple<DateTime, ShoppingBasketDTO>>();
            tuples.Add(new Tuple<DateTime, ShoppingBasketDTO>(new DateTime(2022, 5, 30, 14, 30, 0), new ShoppingBasketDTO()));
            tuples.Add(new Tuple<DateTime, ShoppingBasketDTO>(new DateTime(2022, 5, 18, 10, 14, 0), new ShoppingBasketDTO()));
            tuples.Add(new Tuple<DateTime, ShoppingBasketDTO>(new DateTime(2022, 5, 1, 22, 55, 0), new ShoppingBasketDTO()));
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
                return RedirectToAction("StorePage", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage }); ;
            }
        }

        public IActionResult StoreMessagesPage(MainModel modelcs, string storeName)
        {
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            Queue<MessageToStoreDTO> messages = new Queue<MessageToStoreDTO>();
            messages.Enqueue(new MessageToStoreDTO(storeName, "afik", "mmm", "shit store"));
            messages.Enqueue(new MessageToStoreDTO(storeName, "harel", "mmm", "nice store"));
            messages.Enqueue(new MessageToStoreDTO(storeName, "moshe", "mmm", "nice store, shit people"));
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
                return RedirectToAction("StorePage", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage }); ;
            }
        }

        public IActionResult ReplyMessage(string storename, string userName, string title, string replyMessage)
        {
            Response response = new Response();// service.AnswerStoreMesseage(token, storename, userName, title,replyMessage );
            if (!response.ErrorOccured)
            {
                return RedirectToAction("StoreMessagesPage", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = false, Message = "reply sent" });
            }
            else
            {
                return RedirectToAction("StoreMessagesPage", "Home", new { IsGuest = IsGuest, IsLoggedIn = IsLoggedIn, IsAdmin = IsAdmin, ErrorOccurred = true, Message = response.ErrorMessage });
            }
        }

    }
}