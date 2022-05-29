using AcceptanceTest.DSL;
using AcceptanceTest.DSL.Drivers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTest.AcceptanceTests.Direct
{
    [TestClass()]
    public class DirectAcceptanceTest : Dsl
    {
        [TestInitialize]
        public void Setup()
        {
            _market = new MarketDsl(new DirectMarketDriver());
        }
    }
}
