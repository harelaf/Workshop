using MarketProject.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest.DSL.Drivers
{
    /// <summary>
    /// <para>DirectMarketDriver interacts with the API in the service layer directly 
    /// (unlike through, for example, a GUI).</para>
    /// Developed for Iteration 1.
    /// </summary>
    public class DirectMarketDriver : IMarketDriver
    {
        private MarketAPI _marketAPI = new MarketAPI();
        private String? _errorMessage = null;
        private String? _currentGuestToken = null;
        private String? _loggedInUserToken = null;

        public DirectMarketDriver()
        {
            // Register Admin
            EnterSystem();
            Register(DefaultValues.AdminUsername, DefaultValues.AdminPassword);
            ExitSystem();

            // Register Buyer
            EnterSystem();
            Register(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            ExitSystem();
        }

        public void EnterSystem()
        {
            Response<String> response = _marketAPI.EnterSystem();
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
            _currentGuestToken = response.Value;
        }
        public void ExitSystem()
        {
            if(_loggedInUserToken != null)
            {
                Response response = _marketAPI.ExitSystem(_loggedInUserToken);
                if (response.ErrorOccured) 
                {
                    _errorMessage = response.ErrorMessage;
                    return;
                } 
                _loggedInUserToken = null;
            }
            else
            {
                if (_currentGuestToken != null)
                {
                    Response response = _marketAPI.ExitSystem(_currentGuestToken);
                    if (response.ErrorOccured)
                    {
                        _errorMessage = response.ErrorMessage;
                        return;
                    }
                    _currentGuestToken = null;
                }
            }
        }

        public void Register(string username, string password)
        {
            Response response = _marketAPI.Register(_currentGuestToken, username, password);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
        }

        public void Login(string username, string password)
        {
            Response<String> response = _marketAPI.Login(_currentGuestToken, username, password);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
            _loggedInUserToken = response.Value;
            _currentGuestToken = null;
        }

        public void Logout()
        {
            Response<String> response = _marketAPI.Logout(_loggedInUserToken);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
            _currentGuestToken = response.Value;
            _loggedInUserToken = null;
        }

        public void AssertNoError()
        {
            Assert.IsNull(_errorMessage);
        }

        public void AssertErrorMessageRecieved()
        {
            Assert.IsNotNull(_errorMessage);
        }
    }
}
