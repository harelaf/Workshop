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
        UserManagement userManagement;

        [TestInitialize]
        public void setup()
        {
            userManagement = new UserManagement();
        }



        // ============================= REGISTER =============================

        [TestMethod()]
        public void Register_Valid_RegistersNewUser()
        {
            String username = "Test";
            String password = "123";

            userManagement.Register(username, password);
            Registered registered = userManagement.GetRegisteredUser(username);


            Assert.IsNotNull(registered);
            Assert.AreEqual(registered, userManagement.GetRegisteredUser(username));
        }

        [TestMethod()]
        public void Register_InvalidUsername_ThrowsException()
        {
            String username = ""; // Username obviously cannot be empty string
            String password = "123";

            Assert.ThrowsException<Exception>(() => userManagement.Register(username, password));
        }

        [TestMethod()]
        public void Register_InvalidPassword_ThrowsException()
        {
            UserManagement userManagement = new UserManagement();
            String username = "Test";
            String password = ""; // Password obviously cannot be empty string

            Assert.ThrowsException<Exception>(() => userManagement.Register(username, password));
        }



        // ============================= LOGIN =============================

        [TestMethod()]
        public void Login_Valid_ReturnsToken()
        {
            String guestToken = "abcd";
            String username = "Test";
            String password = "123";
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, new Registered(username, password));
            Guest guest = new Guest(guestToken);
            Dictionary<String, Guest> visitorsGuestsTokens = new Dictionary<string, Guest>();
            visitorsGuestsTokens.Add(guestToken, guest);
            UserManagement userManagement = new UserManagement(registeredUsers, visitorsGuestsTokens);


            String token = userManagement.Login(guestToken, username, password);


            Assert.IsNotNull(token);
        }

        [TestMethod()]
        public void Login_InvalidUsername_ThrowsException()
        {
            String guestToken = "abcd";
            String username = "Test";
            String password = "123";
            String triedUsername = "";
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, new Registered(username, password));
            Guest guest = new Guest(guestToken);
            Dictionary<String, Guest> visitorsGuestsTokens = new Dictionary<string, Guest>();
            visitorsGuestsTokens.Add(guestToken, guest);
            UserManagement userManagement = new UserManagement(registeredUsers, visitorsGuestsTokens);


            Assert.ThrowsException<Exception>(() => userManagement.Login(guestToken, triedUsername, password));
        }

        [TestMethod()]
        public void Login_InvalidPassword_ThrowsException()
        {
            String guestToken = "abcd";
            String username = "Test";
            String password = "123";
            String triedPassword = "";
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, new Registered(username, password));
            Guest guest = new Guest(guestToken);
            Dictionary<String, Guest> visitorsGuestsTokens = new Dictionary<string, Guest>();
            visitorsGuestsTokens.Add(guestToken, guest);
            UserManagement userManagement = new UserManagement(registeredUsers, visitorsGuestsTokens);


            Assert.ThrowsException<Exception>(() => userManagement.Login(guestToken, username, triedPassword));
        }



        // ============================= LOGOUT =============================

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



        // ============================= REMOVE_REGISTERED_USER =============================

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



        // ============================= RESTART_SYSTEM =============================

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

            userManagement.AdminStart(username, password);

            Assert.IsNull(userManagement.CurrentAdmin);
        }



        // ============================= EDIT_USER_DETAILS =============================

        [TestMethod()]
        public void EditUserPassword_Valid_Updates()
        {
            String username = "Test";
            String password = "123";
            String newPassword = "1";
            String authToken = "abcd";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);

            Assert.IsTrue(userManagement.IsRegistered(username));
            Assert.IsTrue(userManagement.IsUserLoggedin(authToken));


            userManagement.EditUserPassword(authToken, password, newPassword);
            bool newPasswordWorks = registered.Login(newPassword);


            Assert.IsTrue(newPasswordWorks);
        }

        [TestMethod()]
        public void EditUserPassword_OldPassInvalid_DoesNotUpdate()
        {
            String username = "Test";
            String password = "123";
            String triedPassword = "12";
            String newPassword = "1";
            String authToken = "abcd";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);

            Assert.IsTrue(userManagement.IsRegistered(username));
            Assert.IsTrue(userManagement.IsUserLoggedin(authToken));


            Assert.ThrowsException<Exception>(() => userManagement.EditUserPassword(authToken, triedPassword, newPassword));
            bool newPasswordWorks = registered.Login(newPassword);


            Assert.IsFalse(newPasswordWorks);
        }

        [TestMethod()]
        public void EditUserPassword_NewPassInvalid_DoesNotUpdate()
        {
            String username = "Test";
            String password = "123";
            String newPassword = "";
            String authToken = "abcd";
            Registered registered = new Registered(username, password);
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);

            Assert.IsTrue(userManagement.IsRegistered(username));
            Assert.IsTrue(userManagement.IsUserLoggedin(authToken));


            Assert.ThrowsException<Exception>(() => userManagement.EditUserPassword(authToken, password, newPassword));
            bool newPasswordWorks = registered.Login(newPassword);


            Assert.IsFalse(newPasswordWorks);
        }



        // ============================= FILE_COMPLAINT =============================

        [TestMethod()]
        public void FileComplaint_Valid_Files()
        {
            // Complainer
            String username = "Test";
            String password = "123";
            String authToken = "abcd";
            int cartId = 1;
            String message = "Test message";
            Registered registered = new Registered(username, password);

            // Admin
            String adminUsername = "Admin";
            String adminPassword = "123";
            Registered admin = new Registered(adminUsername, adminPassword);
            SystemAdmin adminRole = new SystemAdmin(adminUsername);
            admin.AddRole(adminRole);

            // UserManagement
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            registeredUsers.Add(adminUsername, admin);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);
            userManagement.CurrentAdmin = adminRole;

            userManagement.FileComplaint(authToken, cartId, message);

            Assert.IsTrue(true);
        }



        // ============================= REPLY_TO_COMPLAINT =============================

        [TestMethod()]
        public void ReplyToComplaint_Valid_Replied()
        {
            // Complainer
            String username = "Test";
            String password = "123";
            int cartId = 1;
            String message = "Test message";
            Registered registered = new Registered(username, password);

            // Admin
            String adminUsername = "Admin";
            String adminPassword = "123";
            String authToken = "abcd";
            String response = "Test response";
            Registered admin = new Registered(adminUsername, adminPassword);
            SystemAdmin adminRole = new SystemAdmin(adminUsername);
            admin.AddRole(adminRole);

            //Complaint
            int complaintId = 1;
            Complaint complaint = new Complaint(complaintId, registered, cartId, message);
            registered.FileComplaint(complaint);
            adminRole.ReceiveComplaint(complaint);

            // UserManagement
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            registeredUsers.Add(adminUsername, admin);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, admin);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);


            userManagement.ReplyToComplaint(authToken, complaintId, response);


            Assert.AreEqual(ComplaintStatus.Closed, complaint.Status);
        }

        [TestMethod()]
        public void ReplyToComplaint_NotAdmin_ThrowsException()
        {
            // Complainer
            String username = "Test";
            String password = "123";
            int cartId = 1;
            String message = "Test message";
            Registered registered = new Registered(username, password);

            // Admin
            String adminUsername = "Admin";
            String adminPassword = "123";
            String authToken = "abcd";
            String response = "Test response";
            Registered admin = new Registered(adminUsername, adminPassword);
            //SystemAdmin adminRole = new SystemAdmin(adminUsername); Removed admin priviliges
            //admin.AddRole(adminRole);

            //Complaint
            int complaintId = 1;
            Complaint complaint = new Complaint(complaintId, registered, cartId, message);
            registered.FileComplaint(complaint);
            //adminRole.ReceiveComplaint(complaint); Removed admin priviliges

            // UserManagement
            Dictionary<String, Registered> registeredUsers = new Dictionary<string, Registered>();
            registeredUsers.Add(username, registered);
            registeredUsers.Add(adminUsername, admin);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, admin);
            UserManagement userManagement = new UserManagement(registeredUsers, loggedInTokens);


            Assert.ThrowsException<Exception>(() => userManagement.ReplyToComplaint(authToken, complaintId, response));
        }

        [TestMethod]
        public void RemoveManagerPermission_regular_successful()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String storeName = "store1";
            Operation op = Operation.STORE_HISTORY_INFO;

            try
            {
                userManagement.Register(managerUsername, password);
                userManagement.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
                userManagement.RemoveManagerPermission(appointer, managerUsername, storeName, op);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RemoveManagerPermission_notByAppointer_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String storeName = "store1";
            Operation op = Operation.STORE_HISTORY_INFO;

            userManagement.Register(managerUsername, password);
            userManagement.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => userManagement.RemoveManagerPermission("other name", managerUsername, storeName, op));
        }

        [TestMethod]
        public void RemoveManagerPermission_changeOwnerPermission_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String storeName = "store1";
            Operation op = Operation.STORE_HISTORY_INFO;

            userManagement.Register(managerUsername, password);
            userManagement.AddRole(managerUsername, new StoreOwner(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => userManagement.RemoveManagerPermission(appointer, managerUsername, storeName, op));
        }

        [TestMethod]
        public void AddManagerPermission_regular_successful()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String storeName = "store1";
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;

            try
            {
                userManagement.Register(managerUsername, password);
                userManagement.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
                userManagement.AddManagerPermission(appointer, managerUsername, storeName, op);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void AddManagerPermission_notByAppointer_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String storeName = "store1";
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;

            userManagement.Register(managerUsername, password);
            userManagement.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => userManagement.AddManagerPermission("other name", managerUsername, storeName, op));
        }

        [TestMethod]
        public void AddManagerPermission_changeOwnerPermission_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String storeName = "store1";
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;

            userManagement.Register(managerUsername, password);
            userManagement.AddRole(managerUsername, new StoreOwner(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => userManagement.AddManagerPermission(appointer, managerUsername, storeName, op));
        }

        [TestMethod]
        public void AddManagerPermission_prohibitedOperation_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String storeName = "store1";
            Operation op = Operation.CANCEL_SUBSCRIPTION;

            userManagement.Register(managerUsername, password);
            userManagement.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => userManagement.AddManagerPermission(appointer, managerUsername, storeName, op));
        }
    }
}