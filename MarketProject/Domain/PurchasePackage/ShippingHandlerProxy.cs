using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class ShippingHandlerProxy : IShipping
    {
        private RealShippingSystem _realShippingSystem;

        public ShippingHandlerProxy(RealShippingSystem realShippingSystem)
        {
            _realShippingSystem = realShippingSystem;
        }
        // includes mock of shipp
        public virtual bool ShippingApproval(String address, String city, String country, String zip, String purchaserName)
        {
            if(_realShippingSystem == null)
                return true;
            return _realShippingSystem.ShippingAproval(address, city, country, zip, purchaserName);
        }
    }
}
