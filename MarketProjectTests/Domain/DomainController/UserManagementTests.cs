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

        // ==================== LOGOUT ====================

        [TestMethod()]
        public void Logout_ValidToken_NotLoggedIn()
        {
            String username = "Test";
            String password = "123";
            String authToken = "abcd";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);


            Assert.IsTrue(userManagement.IsUserLoggedin(authToken));


            userManagement.Logout(authToken);


            Assert.IsFalse(userManagement.IsUserLoggedin(authToken));
        }

        [TestMethod()]
        public void Logout_InvalidToken_StaysLoggedIn()
        {
            String username = "Test";
            String password = "123";
            String authToken = "abcd";
            String triedToken = "a";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);


            Assert.IsTrue(userManagement.IsUserLoggedin(authToken));


            Assert.ThrowsException<Exception>(() => userManagement.Logout(triedToken));


            Assert.IsTrue(userManagement.IsUserLoggedin(authToken));
        }


        // ==================== REMOVE_REGISTERED_USER ====================

        [TestMethod()]
        public void RemoveRegisteredUser_ValidUsername_Removed()
        {
            String username = "Test";
            String password = "123";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            UserManagement userManagement = new UserManagement(registeredUsers);

            Assert.IsTrue(userManagement.IsRegistered(username));

            userManagement.RemoveRegisteredUser(username);


            Assert.IsFalse(userManagement.IsRegistered(username));
        }

        [TestMethod()]
        public void RemoveRegisteredUser_InvalidUsername_ThrowsException()
        {
            String username = "Test";
            UserManagement userManagement = new UserManagement();


            Assert.ThrowsException<Exception>(() => userManagement.RemoveRegisteredUser(username));
        }

        [TestMethod()]
        public void RemoveRegisteredUser_WasLoggedIn_RemovedAndLoggedOut()
        {
            String username = "Test";
            String password = "123";
            String authToken = "abcd";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);

            Assert.IsTrue(userManagement.IsRegistered(username));
            Assert.IsTrue(userManagement.IsUserLoggedin(authToken));


            userManagement.RemoveRegisteredUser(username);


            Assert.IsFalse(userManagement.IsRegistered(username));
            Assert.IsFalse(userManagement.IsUserLoggedin(authToken));
        }

        // ==================== RESTART_SYSTEM ====================


        [TestMethod()]
        public void AdminStart_Valid_SetsAdmin()
        {
            String username = "Test";
            String password = "123";
            Registered registered = new Registered(username, password);
            SystemAdmin systemAdmin = new SystemAdmin(username);
            registered.AddRole(systemAdmin);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            UserManagement userManagement = new UserManagement(registeredUsers);

            Assert.IsNull(userManagement.CurrentAdmin);

            userManagement.AdminStart(username, password);

            Assert.AreEqual(systemAdmin, userManagement.CurrentAdmin);
        }

        [TestMethod()]
        public void AdminStart_NotAdmin_DoesntSetAdmin()
        {
            String username = "Test";
            String password = "123";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            UserManagement userManagement = new UserManagement(registeredUsers);

            Assert.IsNull(userManagement.CurrentAdmin);

            userManagement.AdminStart(username, password);

            Assert.IsNull(userManagement.CurrentAdmin);
        }
    }
}