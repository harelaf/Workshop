using AcceptanceTest.DSL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AcceptanceTest.AcceptanceTests.Direct
{
    [TestClass()]
    public class GuestAcceptanceTest : DirectAcceptanceTest
    {
        // ================================= Use Case 1 - Enter System =================================
        [TestMethod()]
        public void EnterSystem_Happy()
        {
            _market.EnterSystem();


            _market.AssertNoError();
        }



        // ================================= Use Case 2 - Exit System =================================
        [TestMethod()]
        public void ExitSystem_Happy()
        {
            _market.EnterSystem();
            _market.ExitSystem();


            _market.AssertNoError();
        }



        // ================================= Use Case 3 - Register =================================
        [TestMethod()]
        public void Register_Happy()
        {
            _market.EnterSystem();
            _market.Register("Username", "password", new DateTime(2000, 1, 1));


            _market.AssertNoError();
        }

        [TestMethod()]
        public void Register_Sad_UsernameTaken()
        {
            // Precondition: Previous visitor registered with "Username"
            _market.EnterSystem();
            _market.Register("Username", "password", new DateTime(2000, 1, 1));
            _market.AssertNoError();
            _market.ExitSystem();


            _market.EnterSystem();
            _market.Register("Username", "password", new DateTime(2000, 1, 1));


            _market.AssertErrorMessageRecieved();
        }



        // ================================= Use Case 4 - Login =================================
        [TestMethod()]
        public void Login_Happy()
        {
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);


            _market.AssertNoError();
        }

        [TestMethod()]
        public void Login_Sad_WrongUsername()
        {   
            _market.EnterSystem();
            _market.Login("Username", "password");


            _market.AssertErrorMessageRecieved();
        }

        [TestMethod()]
        public void Login_Sad_WrongPassword()
        {   
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, "wrong");


            _market.AssertErrorMessageRecieved();
        }
    }
}
