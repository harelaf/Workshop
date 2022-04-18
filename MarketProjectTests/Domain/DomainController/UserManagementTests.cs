using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class UserManagementTests
    // Following best practices found in: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    {
        // ==================== REGISTER ====================
        [TestMethod()]
        public void Register_Valid_RegistersNewUser()
        {
            UserManagement userManagement = new UserManagement();
            String username = "Test";
            String password = "123";

            bool response = userManagement.Register(username, password);
            Registered registered = userManagement.GetRegisteredUser(username);


            Assert.IsTrue(response);
            Assert.IsNotNull(registered);
        }

        [TestMethod()]
        public void Register_InvalidUsername_DoesNotRegister()
        {
            UserManagement userManagement = new UserManagement();
            String username = ""; // Username obviously cannot be empty string
            String password = "123";

            bool response = userManagement.Register(username, password);
            Registered registered = userManagement.GetRegisteredUser(username);


            Assert.IsFalse(response);
            Assert.IsNull(registered);
        }

        [TestMethod()]
        public void Register_InvalidPassword_DoesNotRegister()
        {
            UserManagement userManagement = new UserManagement();
            String username = "Test";
            String password = ""; // Password obviously cannot be empty string

            bool response = userManagement.Register(username, password);
            Registered registered = userManagement.GetRegisteredUser(username);


            Assert.IsFalse(response);
            Assert.IsNull(registered);
        }

        // ==================== LOGIN ====================

        [TestMethod()]
        public void Login_Valid_ReturnsToken()
        {
            
            String username = "Test";
            String password = "123";
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, new Registered(username, password));
            UserManagement userManagement = new UserManagement(registeredUsers);


            String token = userManagement.Login(username, password);


            Assert.IsNotNull(token);
        }

        [TestMethod()]
        public void Login_InvalidUsername_ReturnsNull()
        {

            String username = "Test";
            String password = "123";
            String triedUsername = "";
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, new Registered(username, password));
            UserManagement userManagement = new UserManagement(registeredUsers);


            String token = userManagement.Login(triedUsername, password);


            Assert.IsNull(token);
        }

        [TestMethod()]
        public void Login_InvalidPassword_ReturnsNull()
        {
            String username = "Test";
            String password = "123";
            String triedPassword = "";
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, new Registered(username, password));
            UserManagement userManagement = new UserManagement(registeredUsers);


            String token = userManagement.Login(username, triedPassword);


            Assert.IsNull(token);
        }
    }
}