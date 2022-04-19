using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class ShippingHandlerProxy : IShipping
    {
        private RealShippingSystem _realShippingSystem;

        public ShippingHandlerProxy(RealShippingSystem realShippingSystem)
        {
            _realShippingSystem = realShippingSystem;
        }
        // includes mock of shipp
        public bool ShippingApproval(String adress)
        {
            if(_realShippingSystem == null)
                return true;
            return _realShippingSystem.ShippingAproval(adress);
        }
    }
}
