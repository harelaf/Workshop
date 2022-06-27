using AcceptanceTest.DSL;
using AcceptanceTest.DSL.Drivers;
using MarketWeb.Server.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest.AcceptanceTests.Direct
{
    [TestClass()]
    public class RegisteredAcceptanceTest : DirectAcceptanceTest
    {

        DalController dc = DalController.GetInstance(true);
        [TestCleanup()]
        public void cleanup()
        {
            dc.Cleanup();
        }

        // ================================= Use Case 15 - Exit System =================================
        [TestMethod()]
        public void ExitSystem_Happy()
        {
            // Precondition: Visitor is logged in.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);


            _market.ExitSystem();


            _market.AssertNoError();
        }

        [TestMethod()]
        public void ExitSystem_Happy_CartSaved()
        {
            // Precondition: Visitor is logged in and cart is not empty.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            // _market.SearchForItem(DefaultValues.ItemName)                                TODO
            // _market.AddItemToCart(DefaultValues.ItemName, DefaultValues.BuyQuantity);    TODO


            _market.ExitSystem();


            // Log back in to check if cart was saved.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            //_market.AssertCartNotEmpty();                                                 TODO
        }



        // ================================= Use Case 16 - Logout =================================
        [TestMethod()]
        public void Logout_Happy()
        {   
            // Precondition: Visitor is logged in.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);


            _market.Logout();


            _market.AssertNoError();
        }

        [TestMethod()]
        public void Logout_Happy_CartSaved()
        {
            // Precondition: Visitor is logged in and cart is not empty.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            // _market.SearchForItem(DefaultValues.ItemName)                                TODO
            // _market.AddItemToCart(DefaultValues.ItemName, DefaultValues.BuyQuantity);    TODO


            _market.Logout();


            // Log back in to check if cart was saved.
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            //_market.AssertCartNotEmpty();                                                 TODO
        }

        // ================================= Use Case ? - Edit Visitor Details =================================
        [TestMethod()]
        public void EditVisitorDetails_Happy()
        {
            // Precondition: Visitor is logged in.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);


            _market.ChangePassword(DefaultValues.BuyerPassword, "newPassword");
            _market.AssertNoError();


            _market.Logout();
            _market.Login(DefaultValues.BuyerUsername, "newPassword"); // Visitor should be able to login with the new password
            _market.AssertNoError();
        }

        [TestMethod()]
        public void EditVisitorDetails_Sad_OldPasswordWrong()
        {
            // Precondition: Visitor is logged in.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);


            _market.ChangePassword("wrong", "newPassword");
            _market.AssertErrorMessageRecieved();


            _market.Logout();
            _market.Login(DefaultValues.BuyerUsername, "newPassword"); // Visitor should NOT be able to login with the new password
            _market.AssertErrorMessageRecieved();
        }
    }
}
