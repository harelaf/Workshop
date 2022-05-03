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

        public ActionResult RemoveItemFormCart(int itemId, String storeName)
        {
            //I_User_ServiceLayer SL = validateConnection();
            try
            {
                Response res = null;//service.removeItemFromCart(itemId, store)
                if (!res.ErrorOccured)
                {
                    return RedirectToAction("MyStores", new { storename = storeName });
                }
            }
            catch
            {

            }
            ViewData["alertRemoveProduct"] = true;
            return RedirectToAction("Item", new { storename = storeName, productname = itemId });
        }
    }
}