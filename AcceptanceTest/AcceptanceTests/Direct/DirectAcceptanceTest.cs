﻿using AcceptanceTest.DSL;
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
    public class DirectAcceptanceTest : Dsl
    {
        [TestInitialize]
        public void Setup()
        {
            _market = new MarketDsl(new DirectMarketDriver());
        }
    }
}