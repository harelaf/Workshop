using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class ShippingHandlerProxy : IShipping
    {
        private RealShippingSystem _realShippingSystem;
    }
}
