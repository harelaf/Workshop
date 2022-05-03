﻿using MarketWebProject.Models;
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

        public IActionResult RegistrationPage(MainModel modelcs)
        {
            if(modelcs == null)
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
        {
            Console.WriteLine("worksssssssssssssssssssssssssssssssssssss");
            Console.WriteLine("name: "+name);
            Console.WriteLine("pass: " + password);
            Console.WriteLine("dob: " + dob);
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "yourpass is not valid";
            return RedirectToAction("RegistrationPage","Home", new {ErrorMsg = err_msg });
        }
        public IActionResult LoginForm(string name, string password)
        {
            Console.WriteLine("worksssssssssssssssssssssssssssssssssssss");
            Console.WriteLine("name: " + name);
            Console.WriteLine("pass: " + password);
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "loggin faild: your username or password are wrong!";
            //for ourtest:
            if(name == "fail")
                return RedirectToAction("LoginPage", "Home", new { ErrorMsg = err_msg });
            else
            {
                return RedirectToAction("Index", "Home", new { IsGuest = false , IsLoggedIn=true, IsAdmin =false});;
            }
        }
        public IActionResult Logout()
        {
            //CALL THE CLIENT-> server-> market api
            //_CLIENT<- server <-market api
            String err_msg = "loggin faild: your username or password are wrong!";
            //for ourtest:
           
            return RedirectToAction("Index", "Home", new { IsGuest = true, IsLoggedIn = false, IsAdmin = false }); ;
            
        }

    }
}