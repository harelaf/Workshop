using AcceptanceTest.DSL;
using AcceptanceTest.DSL.Drivers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
