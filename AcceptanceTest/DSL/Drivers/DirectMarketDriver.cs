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
        private String? _loggedInVisitorToken = null;

        public DirectMarketDriver()
        {
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
            if(_loggedInVisitorToken != null)
            {
                Response response = _marketAPI.ExitSystem(_loggedInVisitorToken);
                if (response.ErrorOccured) 
                {
                    _errorMessage = response.ErrorMessage;
                    return;
                } 
                _loggedInVisitorToken = null;
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
            _errorMessage = null;
        }

        public void Register(string Username, string password)
        {
            Response response = _marketAPI.Register(_currentGuestToken, Username, password);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
        }

        public void Login(string Username, string password)
        {
            Response<String> response = _marketAPI.Login(_currentGuestToken, Username, password);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
            _loggedInVisitorToken = response.Value;
            _currentGuestToken = null;
        }

        public void Logout()
        {
            Response<String> response = _marketAPI.Logout(_loggedInVisitorToken);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
            _currentGuestToken = response.Value;
            _loggedInVisitorToken = null;
            _errorMessage = null;
        }

        public void AssertNoError()
        {
            Assert.IsNull(_errorMessage);
        }

        public void AssertErrorMessageRecieved()
        {
            Assert.IsNotNull(_errorMessage);
        }

        public void RemoveRegisteredVisitor(string Username)
        {
            Response response = _marketAPI.RemoveRegisteredVisitor(_loggedInVisitorToken, Username);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
        }

        public void ChangePassword(string oldPassword, string newPassword)
        {
            Response response = _marketAPI.EditVisitorPassword(_loggedInVisitorToken, oldPassword, newPassword);
            if (response.ErrorOccured)
            {
                _errorMessage = response.ErrorMessage;
                return;
            }
        }
    }
}
