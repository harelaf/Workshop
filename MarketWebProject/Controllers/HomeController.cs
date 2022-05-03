using MarketWebProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MarketWebProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult RegistrationPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RegisterForm(string name, string password, DateTime dob)
        {
            Console.WriteLine("worksssssssssssssssssssssssssssssssssssss");
            Console.WriteLine("name: "+name);
            Console.WriteLine("pass: " + password);
            Console.WriteLine("dob: " + dob);
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            return RedirectToAction("Index");
        }
    }
}