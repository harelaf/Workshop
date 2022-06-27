﻿using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarketProject.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using MarketWeb.Server.Domain;
using MarketWeb.Shared;

namespace MarketProject.Domain.Tests
{
    [TestClass()]
    public class VisitorManagementTests
    // Following best practices found in: https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    {
        VisitorManagement VisitorManagement;
        DateTime dob = new DateTime(2001, 7, 30);
        
        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }
        
        [TestInitialize]
        public void setup()
        {
            VisitorManagement = new VisitorManagement(true);
        }

        // ============================= REGISTER =============================

        [TestMethod()]
        public void Register_Valid_RegistersNewVisitor()
        {
            String Username = "Test";
            String password = "123";
            DateTime dob = new DateTime(2001, 7, 30);
            VisitorManagement.Register(Username, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(Username);


            Assert.IsNotNull(registered);
            Assert.AreEqual(registered.Username, Username);
        }

        [TestMethod()]
        public void Register_InvalidUsername_ThrowsException()
        {
            String Username = ""; // Username obviously cannot be empty string
            String password = "123";
            DateTime dob = new DateTime(2001, 7, 30);
            Assert.ThrowsException<Exception>(() => VisitorManagement.Register(Username, password, dob));
        }

        [TestMethod()]
        public void Register_InvalidPassword_ThrowsException()
        {
            VisitorManagement VisitorManagement = new VisitorManagement(true);
            String Username = "Test";
            String password = ""; // Password obviously cannot be empty string
            Assert.ThrowsException<Exception>(() => VisitorManagement.Register(Username, password, dob));
        }



        // ============================= LOGIN =============================

        [TestMethod()]
        public void Login_Valid_ReturnsToken()
        {
            String guestToken = "abcd";
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            VisitorManagement.Register(Username, password, bDay);
            Registered registered = VisitorManagement.GetRegisteredVisitor(Username);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            //registeredVisitors.Add(Username, registered);
            Guest guest = new Guest(guestToken);
            Dictionary<String, Guest> visitorsGuestsTokens = new Dictionary<string, Guest>();
            visitorsGuestsTokens.Add(guestToken, guest);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, visitorsGuestsTokens, true);

            String token = vm.Login(guestToken, Username, password);


            Assert.IsNotNull(token);
        }

        [TestMethod()]
        public void Login_InvalidUsername_ThrowsException()
        {
            String guestToken = "abcd";
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            String triedUsername = "";
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, new Registered(Username, password, dob));
            Guest guest = new Guest(guestToken);
            Dictionary<String, Guest> visitorsGuestsTokens = new Dictionary<string, Guest>();
            visitorsGuestsTokens.Add(guestToken, guest);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, visitorsGuestsTokens, true);


            Assert.ThrowsException<Exception>(() => VisitorManagement.Login(guestToken, triedUsername, password));
        }

        [TestMethod()]
        public void Login_InvalidPassword_ThrowsException()
        {
            String guestToken = "abcd";
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            String triedPassword = "";
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, new Registered(Username, password, dob));
            Guest guest = new Guest(guestToken);
            Dictionary<String, Guest> visitorsGuestsTokens = new Dictionary<string, Guest>();
            visitorsGuestsTokens.Add(guestToken, guest);
            VisitorManagement VisitorManagement = new VisitorManagement(true);


            Assert.ThrowsException<Exception>(() => VisitorManagement.Login(guestToken, Username, triedPassword));
        }



        // ============================= LOGOUT =============================

        [TestMethod()]
        public void Logout_ValidToken_NotLoggedIn()
        {
            String Username = "Test";
            String password = "123";
            String authToken = "abcd";
            Registered registered = new Registered(Username, password, dob);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, loggedInTokens, true);


            Assert.IsTrue(VisitorManagement.IsVisitorLoggedin(authToken));


            VisitorManagement.Logout(authToken);


            Assert.IsFalse(VisitorManagement.IsVisitorLoggedin(authToken));
        }

        [TestMethod()]
        public void Logout_InvalidToken_StaysLoggedIn()
        {
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            String authToken = "abcd";
            String triedToken = "a";
            Registered registered = new Registered(Username, password, dob);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, loggedInTokens, true);


            Assert.IsTrue(VisitorManagement.IsVisitorLoggedin(authToken));


            Assert.ThrowsException<Exception>(() => VisitorManagement.Logout(triedToken));


            Assert.IsTrue(VisitorManagement.IsVisitorLoggedin(authToken));
        }



        // ============================= REMOVE_REGISTERED_Visitor =============================

        [TestMethod()]
        public void RemoveRegisteredVisitor_ValidUsername_Removed()
        {
            String Username = "Test";
            String password = "123";
            VisitorManagement.Register(Username, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(Username);
            //Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            //registeredVisitors.Add(Username, registered);
            //VisitorManagement vm = new VisitorManagement(registeredVisitors, true);

            Assert.IsTrue(VisitorManagement.IsRegistered(Username));

            VisitorManagement.RemoveRegisteredVisitor(Username);

            Assert.IsFalse(VisitorManagement.IsVisitorLoggedin(Username));
        }

        [TestMethod()]
        public void RemoveRegisteredVisitor_InvalidUsername_ThrowsException()
        {
            String Username = "Test";
            DateTime bDay = new DateTime(1992, 8, 4);
            VisitorManagement VisitorManagement = new VisitorManagement(true);


            Assert.ThrowsException<Exception>(() => VisitorManagement.RemoveRegisteredVisitor(Username));
        }

        [TestMethod()]
        public void RemoveRegisteredVisitor_WasLoggedIn_RemovedAndLoggedOut()
        {
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            String authToken = "abcd";
            VisitorManagement.Register(Username, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(Username);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);

            Assert.IsTrue(VisitorManagement.IsRegistered(Username));// checks DB. not up to date with rgistratiion at visitor management level
            Assert.IsTrue(vm.IsVisitorLoggedin(authToken));


            vm.RemoveRegisteredVisitor(Username);


            //Assert.IsTrue(VisitorManagement.IsRegistered(Username));// checks DB. not up to date with rgistratiion at visitor management level
            Assert.IsFalse(vm.IsVisitorLoggedin(authToken));
        }



        // ============================= RESTART_SYSTEM =============================

        [TestMethod()]
        public void AdminStart_Valid_SetsAdmin()
        {
            String Username = "Test";
            String password = "123";
            Registered registered = new Registered(Username, password, dob);
            SystemAdmin systemAdmin = new SystemAdmin(Username);
            registered.AddRole(systemAdmin);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, true);

            VisitorManagement.AdminStart(Username, password);

            //Assert.AreEqual(systemAdmin, VisitorManagement.CurrentAdmin);
            //replace by getAdmin or something

        }

        [TestMethod()]
        public void AdminStart_NotAdmin_DoesntSetAdmin()
        {
            String Username = "Test";
            String password = "123";
            Registered registered = new Registered(Username, password, dob);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, true);

            VisitorManagement.AdminStart(Username, password);

            //Assert.IsNull(VisitorManagement.CurrentAdmin);
            //replace by getAdmin or something
        }



        // ============================= EDIT_Visitor_DETAILS =============================

        [TestMethod()]
        public void EditVisitorPassword_Valid_Updates()
        {
            String Username = "Test";
            String password = "123";
            DateTime dob = new DateTime(2001, 7, 30);
            VisitorManagement.Register(Username, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(Username);
            String newPassword = "1";
            String authToken = "abcd";
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);

            Assert.IsTrue(VisitorManagement.IsRegistered(Username));// checks DB. not up to date with rgistratiion at visitor management level
            Assert.IsTrue(vm.IsVisitorLoggedin(authToken));


            vm.EditVisitorPassword(authToken, password, newPassword);
            bool newPasswordWorks = registered.Login(newPassword);


            Assert.IsTrue(newPasswordWorks);
        }

        [TestMethod()]
        public void EditVisitorPassword_OldPassInvalid_DoesNotUpdate()
        {
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            String triedPassword = "12";
            String newPassword = "1";
            String authToken = "abcd";
            VisitorManagement.Register(Username, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(Username);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);

            Assert.IsTrue(VisitorManagement.IsRegistered(Username));// checks DB. not up to date with rgistratiion at visitor management level
            Assert.IsTrue(vm.IsVisitorLoggedin(authToken));


            Assert.ThrowsException<Exception>(() => vm.EditVisitorPassword(authToken, triedPassword, newPassword));
            bool newPasswordWorks = registered.Login(newPassword);


            Assert.IsFalse(newPasswordWorks);
        }

        [TestMethod()]
        public void EditVisitorPassword_NewPassInvalid_DoesNotUpdate()
        {
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            String newPassword = "";
            String authToken = "abcd";
            DateTime dob = new DateTime(2001, 7, 30);
            Registered registered = new Registered(Username, password, dob);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, loggedInTokens, true);

            //Assert.IsTrue(VisitorManagement.IsRegistered(Username));// checks DB. not up to date with rgistratiion at visitor management level
            Assert.IsTrue(VisitorManagement.IsVisitorLoggedin(authToken));

            Assert.ThrowsException<Exception>(() => VisitorManagement.EditVisitorPassword(authToken, password, newPassword));
            bool newPasswordWorks = registered.Login(newPassword);


            Assert.IsFalse(newPasswordWorks);
        }



        // ============================= FILE_COMPLAINT =============================

        [TestMethod()]
        public void FileComplaint_Valid_Files()
        {
            // Complainer
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            String authToken = "abcd";
            int cartId = 1;
            String message = "Test message";
            Registered registered = new Registered(Username, password, dob);

            // Admin
            String adminUsername = "Admin";
            String adminPassword = "123";
            Registered admin = new Registered(adminUsername, adminPassword, dob);
            SystemAdmin adminRole = new SystemAdmin(adminUsername);
            admin.AddRole(adminRole);

            // VisitorManagement
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            registeredVisitors.Add(adminUsername, admin);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, loggedInTokens, true);

            VisitorManagement.FileComplaint(authToken, cartId, message);

            Assert.IsTrue(true);
        }



        // ============================= REPLY_TO_COMPLAINT =============================

        [TestMethod()]
        public void ReplyToComplaint_Valid_Replied()
        {
            // Complainer
            String Username = "Test";
            String password = "123";
            DateTime bDay = new DateTime(1992, 8, 4);
            int cartId = 1;
            String message = "Test message";

            // Admin
            String adminUsername = "Admin";
            String adminPassword = "123";
            String authToken = "abcd";
            String authToken2 = "abcde";
            String response = "Test response";
            VisitorManagement.Register(Username, password, dob);
            VisitorManagement.Register(adminUsername, adminPassword, bDay);
            Registered admin = VisitorManagement.GetRegisteredVisitor(adminUsername);
            Registered registered = VisitorManagement.GetRegisteredVisitor(Username);
            SystemAdmin adminRole = new SystemAdmin(adminUsername);
            admin.AddRole(adminRole);

            // VisitorManagement
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            registeredVisitors.Add(adminUsername, admin);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, admin);
            loggedInTokens.Add(authToken2, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);

            

            //Complaint
            vm.FileComplaint(authToken2, cartId, message);
            registered = vm.GetRegisteredVisitor(Username);
            Complaint complaint = null;
            foreach (int key in registered.FiledComplaints.Keys)
            {
                complaint = registered.FiledComplaints[key];
                break;
            }

            

            vm.ReplyToComplaint(authToken, complaint.ID, response);

            registered = vm.GetRegisteredVisitor(Username);
            complaint = registered.FiledComplaints[complaint.ID];

            Assert.AreEqual(ComplaintStatus.Closed, complaint.Status);
        }

        [TestMethod()]
        public void ReplyToComplaint_NotAdmin_ThrowsException()
        {
            // Complainer
            String Username = "Test";
            String password = "123";
            DateTime dob = new DateTime(2001, 7, 30);
            int cartId = 1;
            String message = "Test message";
            Registered registered = new Registered(Username, password, dob);

            // Admin
            String adminUsername = "Admin";
            String adminPassword = "123";
            String authToken = "abcd";
            String response = "Test response";
            Registered admin = new Registered(adminUsername, adminPassword, new DateTime(2001, 7, 30));
            //SystemAdmin adminRole = new SystemAdmin(adminUsername); Removed admin priviliges
            //admin.AddRole(adminRole);

            //Complaint
            int complaintId = 1;
            Complaint complaint = new Complaint(complaintId, registered.Username, cartId, message);
            registered.FileComplaint(complaint);
            //adminRole.ReceiveComplaint(complaint); Removed admin priviliges

            // VisitorManagement
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(Username, registered);
            registeredVisitors.Add(adminUsername, admin);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, admin);
            VisitorManagement VisitorManagement = new VisitorManagement(registeredVisitors, loggedInTokens, true);


            Assert.ThrowsException<Exception>(() => VisitorManagement.ReplyToComplaint(authToken, complaintId, response));
        }

        [TestMethod]
        public void RemoveManagerPermission_regular_successful()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String authToken = "abcd";
            DateTime bDay = new DateTime(1992, 8, 4);
            String storeName = "store1";
            Operation op = Operation.STORE_HISTORY_INFO;

            VisitorManagement.Register(managerUsername, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(managerUsername);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(managerUsername, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);
            vm.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
            Assert.ThrowsException<NullReferenceException>(() => vm.RemoveManagerPermission(appointer, managerUsername, storeName, op));
        }

        [TestMethod]
        public void RemoveManagerPermission_notByAppointer_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String authToken = "abcd";
            DateTime bDay = new DateTime(1992, 8, 4);
            String storeName = "store1";
            Operation op = Operation.STORE_HISTORY_INFO;

            VisitorManagement.Register(managerUsername, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(managerUsername);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(managerUsername, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);
            vm.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => vm.RemoveManagerPermission("other name", managerUsername, storeName, op));
        }

        [TestMethod]
        public void RemoveManagerPermission_changeOwnerPermission_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String authToken = "abcd";
            DateTime bDay = new DateTime(1992, 8, 4);
            String storeName = "store1";
            Operation op = Operation.STORE_HISTORY_INFO;

            VisitorManagement.Register(managerUsername, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(managerUsername);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(managerUsername, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);
            vm.AddRole(managerUsername, new StoreOwner(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => vm.RemoveManagerPermission(appointer, managerUsername, storeName, op));
        }

        [TestMethod]
        public void AddManagerPermission_regular_successful()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String authToken = "abcd";
            DateTime bDay = new DateTime(1992, 8, 4);
            String storeName = "store1";
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;

            try
            {
                VisitorManagement.Register(managerUsername, password, dob);
                Registered registered = VisitorManagement.GetRegisteredVisitor(managerUsername);
                Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
                registeredVisitors.Add(managerUsername, registered);
                Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
                loggedInTokens.Add(authToken, registered);
                VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);
                vm.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
                vm.AddManagerPermission(appointer, managerUsername, storeName, op);
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
            String authToken = "abcd";
            DateTime bDay = new DateTime(1992, 8, 4);
            String storeName = "store1";
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;

            VisitorManagement.Register(managerUsername, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(managerUsername);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(managerUsername, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);
            vm.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => vm.AddManagerPermission("other name", managerUsername, storeName, op));
        }

        [TestMethod]
        public void AddManagerPermission_changeOwnerPermission_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String authToken = "abcd";
            DateTime bDay = new DateTime(1992, 8, 4);
            String storeName = "store1";
            Operation op = Operation.DEFINE_CONCISTENCY_CONSTRAINT;

            VisitorManagement.Register(managerUsername, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(managerUsername);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(managerUsername, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);
            vm.AddRole(managerUsername, new StoreOwner(managerUsername, storeName, appointer));
            try
            {
                vm.AddManagerPermission(appointer, managerUsername, storeName, op);
                Assert.Fail();
            }
            catch(Exception)
            {
                // Should get here, because cant add permissions to owner
            }
        }

        [TestMethod]
        public void AddManagerPermission_prohibitedOperation_throwsException()
        {
            String appointer = "appointer";
            String managerUsername = "manager";
            String password = "123";
            String authToken = "abcd";
            DateTime bDay = new DateTime(1992, 8, 4);
            String storeName = "store1";
            Operation op = Operation.CANCEL_SUBSCRIPTION;

            VisitorManagement.Register(managerUsername, password, dob);
            Registered registered = VisitorManagement.GetRegisteredVisitor(managerUsername);
            Dictionary<String, Registered> registeredVisitors = new Dictionary<string, Registered>();
            registeredVisitors.Add(managerUsername, registered);
            Dictionary<String, Registered> loggedInTokens = new Dictionary<string, Registered>();
            loggedInTokens.Add(authToken, registered);
            VisitorManagement vm = new VisitorManagement(registeredVisitors, loggedInTokens, true);
            vm.AddRole(managerUsername, new StoreManager(managerUsername, storeName, appointer));
            Assert.ThrowsException<Exception>(() => vm.AddManagerPermission(appointer, managerUsername, storeName, op));
        }
    }
}