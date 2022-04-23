using AcceptanceTest.DSL;
using AcceptanceTest.DSL.Drivers;
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
        // ================================= Use Case 15 - Exit System =================================
        [TestMethod()]
        public void ExitSystem_Happy()
        {
            // Precondition: User is logged in.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);


            _market.ExitSystem();


            _market.AssertNoError();
        }

        [TestMethod()]
        public void ExitSystem_Happy_CartSaved()
        {
            // Precondition: User is logged in and cart is not empty.
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
            // Precondition: User is logged in.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);


            _market.Logout();


            _market.AssertNoError();
        }

        [TestMethod()]
        public void Logout_Happy_CartSaved()
        {
            // Precondition: User is logged in and cart is not empty.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            // _market.SearchForItem(DefaultValues.ItemName)                                TODO
            // _market.AddItemToCart(DefaultValues.ItemName, DefaultValues.BuyQuantity);    TODO


            _market.Logout();


            // Log back in to check if cart was saved.
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            //_market.AssertCartNotEmpty();                                                 TODO
        }
    }
}
