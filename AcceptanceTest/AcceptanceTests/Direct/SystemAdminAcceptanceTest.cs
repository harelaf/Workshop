using AcceptanceTest.DSL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTest.AcceptanceTests.Direct
{
    [TestClass()]

    public class SystemAdminAcceptanceTest : DirectAcceptanceTest
    {
        // ================================= Use Case ? - Remove Registered Visitor =================================
        [TestMethod()]
        public void RemoveRegisteredVisitor_Happy()
        {
            // Precondition: Admin is logged in, Visitor to remove exists.
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            _market.AssertNoError();// Make sure Visitor exists
            _market.ExitSystem();
            _market.EnterSystem();
            _market.Login(DefaultValues.AdminUsername, DefaultValues.AdminPassword);


            _market.RemoveRegisteredVisitor(DefaultValues.BuyerUsername);


            _market.AssertNoError();
            _market.ExitSystem();
            _market.EnterSystem();
            _market.Login(DefaultValues.BuyerUsername, DefaultValues.BuyerPassword);
            _market.AssertErrorMessageRecieved(); // Visitor shouldn't be able to login since they were removed.
        }
    }
}
