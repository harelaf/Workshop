using MarketWeb.Client.Helpers;
using MarketWeb.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using static System.Net.WebRequestMethods;

namespace MarketWeb.Client.Connect
{
    public interface IMarketAPIClient 
    {

        public void EnterSystem();

        public Response ExitSystem(string authToken);
        public  void Login(string username, string password);

        public Response<string> Logout(string authToken);

        public Response Register(string authToken, string Username, string password);
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

        public async void EnterSystem()
        {
            string token = await _httpService.Get<string>("api/users/enter");
            if (token != null)
            {
                _httpService.Token = token;
            }
        }

        public Response ExitSystem(string authToken)
        {
            throw new NotImplementedException();
        }
        public async void Login(string username, string password)
        {
            string token = await _httpService.Post<string>("api/users/login", new { username = username, password = password });
            if (token != null)
            {
                _httpService.Token = token;
                LoggedIn = true;
            }
        }

        public Response<string> Logout(string authToken)
        {
            throw new NotImplementedException();
        }

        public Response Register(string authToken, string Username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
