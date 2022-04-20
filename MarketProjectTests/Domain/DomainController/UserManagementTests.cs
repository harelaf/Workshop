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
        [TestMethod()]
        public void Register_Valid_RegistersNewUser()
        {
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
            String username = "Test";
            String password = ""; // Password obviously cannot be empty string

            bool response = userManagement.Register(username, password);
            Registered registered = userManagement.GetRegisteredUser(username);


            Assert.IsFalse(response);
            Assert.IsNull(registered);
        }

        [TestMethod()]
        public void AddRole_addTwoRolesToStore_throwsException()
        {
            string validUsername = "Test";
            string validPassword = "123";
            string validStorename = "someStoreName";
            SystemRole role1 = new StoreManager(validUsername, validStorename, "big brother");
            SystemRole role2 = new StoreOwner(validUsername, validStorename, "big brother");
            userManagement.Register(validUsername, validPassword);
            userManagement.AddRole(validUsername, role1);
            Assert.ThrowsException<Exception>(() => userManagement.AddRole(validUsername, role2));
            Assert.IsTrue(userManagement.GetRegisteredUser(validUsername).hasRoleInStore(validStorename));
        }

        [TestMethod]
        public void AddRole_addStoreManager_returnsTrue()
        {
            string validUsername = "Test";
            string validPassword = "123";
            string validStorename = "someStoreName";
            userManagement.Register(validUsername, validPassword);
            SystemRole role1 = new StoreManager(validUsername, validStorename, "big brother");
            userManagement.AddRole(validUsername, role1);
            Assert.IsTrue(userManagement.GetRegisteredUser(validUsername).hasRoleInStore(validStorename));
        }

        [TestMethod()]
        public void RemoveRole_addAndRemoveSystemAdmin_returnsTrue()
        {
            string validUsername = "Test";
            string validPassword = "123";
            string validStorename = "someStoreName";
            userManagement.Register(validUsername, validPassword);
            SystemRole role1 = new SystemAdmin(validUsername);
            userManagement.AddRole(validUsername, role1);
            userManagement.RemoveRole(validUsername, validStorename);
            Assert.IsFalse(userManagement.GetRegisteredUser(validUsername).hasRoleInStore(validStorename));
        }

        [TestMethod()]
        public void RemoveRole_RemoveNonExistingSystemAdmin_returnsFalse()
        {
            string validUsername = "Test";
            string validPassword = "123";
            string validStorename = "someStoreName";
            userManagement.Register(validUsername, validPassword);
            SystemRole role1 = new StoreManager(validUsername, validStorename, "big brother");
            userManagement.AddRole(validUsername, role1);
            bool removed = userManagement.RemoveRole(validUsername, validStorename);
            Assert.IsTrue(removed);
            Assert.IsFalse(userManagement.GetRegisteredUser(validUsername).hasRoleInStore(validStorename));
        }
    }
}