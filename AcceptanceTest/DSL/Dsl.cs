using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTest.DSL
{
    /// <summary>
    /// Contains the MarketDsl. Acceptance test classes inherit this to be able to 
    /// interact with the market.
    /// </summary>
    public abstract class Dsl
    {
        protected MarketDsl _market;
    }
}
