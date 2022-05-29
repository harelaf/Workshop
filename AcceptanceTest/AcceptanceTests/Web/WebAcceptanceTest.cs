using AcceptanceTest.DSL;
using AcceptanceTest.DSL.Drivers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AcceptanceTest.AcceptanceTests.Web
{
    [TestClass()]
    public class WebAcceptanceTest : Dsl
    {
        [TestInitialize]
        public void Setup()
        {
            _market = new MarketDsl(new WebMarketDriver());
        }
    }
}
