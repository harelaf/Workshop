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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            return View(modelcs);
        }

        public IActionResult Privacy(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            return View(modelcs);
        }
        public IActionResult CartPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            //viewMyCart
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            Response<ShoppingCartDTO> cart = new Response<ShoppingCartDTO>(new ShoppingCartDTO());
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

        public IActionResult ItemSearchPage(MainModel modelcs)
        {
            if (modelcs == null)
                modelcs = new MainModel();
            List<ItemDTO> items = new List<ItemDTO>();
            items.Add(new ItemDTO("item1", 10.5, "store1"));
            items.Add(new ItemDTO("item2", 9.5, "store1"));
            items.Add(new ItemDTO("item3", 15.5, "store2"));
            Response<List<ItemDTO>> response = new Response<List<ItemDTO>>(items);
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

        public IActionResult ItemPage(string storeName, int itemId)
        {
            
            MainModel modelcs = new MainModel();
            //Response<ItemDTO> response=  getItem(storeName, itemId)
            Response<ItemDTO> response = new Response<ItemDTO>(new ItemDTO("banana", 20.5, "store1"));
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

        public IActionResult RegistrationPage(MainModel modelcs)
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
            Console.WriteLine("worksssssssssssssssssssssssssssssssssssss");
            Console.WriteLine("name: " + name);
            Console.WriteLine("pass: " + password);
            Console.WriteLine("dob: " + dob);
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "register faild: your pass\\username are not valid";
            // >>>>>> USE SuccessMessage IF RESPONSE DIDNT FAIL <<<<<< IMPORTANT
            String SuccessMessage = "Successfully registered! You can now log in.";
            bool ErrorOccurred = false;
            return RedirectToAction("RegistrationPage", "Home", new { ErrorOccurred = ErrorOccurred, Message = SuccessMessage });
        }
        public IActionResult LoginForm(string name, string password)
        {//login
            Console.WriteLine("worksssssssssssssssssssssssssssssssssssss");
            Console.WriteLine("name: " + name);
            Console.WriteLine("pass: " + password);
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "loggin faild: your username or password are wrong!";
            bool ErrorOccurred = true;
            //for ourtest:
            if (name == "fail")
                return RedirectToAction("LoginPage", "Home", new { ErrorOccurred = ErrorOccurred, Message = err_msg });
            else
            {
                return RedirectToAction("Index", "Home", new { IsGuest = false, IsLoggedIn = true, IsAdmin = false }); ;
            }
        }
        public IActionResult Logout()
        {//logout
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "loggin faild: your username or password are wrong!";
            //for ourtest:

            return RedirectToAction("Index", "Home", new { IsGuest = true, IsLoggedIn = false, IsAdmin = false }); ;

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


    }
}