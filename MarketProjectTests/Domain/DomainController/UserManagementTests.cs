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
    }
}