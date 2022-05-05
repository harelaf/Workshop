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
            StoreDTO store1 = new StoreDTO();
            StoreDTO store2 = new StoreDTO();
            List<StoreDTO> lst = new List<StoreDTO>();
            lst.Add(store1);
            lst.Add(store2);
            if (modelcs == null)
                modelcs = new MainModel(IsGuest, IsLoggedIn, IsAdmin);
            ViewResult view = View(modelcs);
            view.ViewData["activeStores"] = lst;
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

        public IActionResult RegistrationPage(MainModel modelcs)
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

        public ActionResult RemoveItemFromCart(String storeName, int itemID)
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
        public ActionResult UpdateItemQuantityInCart(String storeName, int itemID, int newQuantity)
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

        public ActionResult AddItemToCart(int amount, string storename, int itemid)
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
    }
}