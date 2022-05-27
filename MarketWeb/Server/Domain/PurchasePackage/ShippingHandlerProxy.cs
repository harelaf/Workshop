using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain
{
    public class ShippingHandlerProxy
    {
        private IDictionary<string, int> _shipmentServices_name_ip;
        public static readonly string shippingMethode_mock_flase = "mock_false";
        public static readonly string shippingMethode_mock_true = "mock_true";

        public ShippingHandlerProxy()
        {
            _shipmentServices_name_ip = new Dictionary<string, int>();
        }
        // includes mock of shipp
        public virtual bool ShippingApproval(String address, String city, String country, String zip, String purchaserName, string shipmentMethode)
        {
            if(shipmentMethode == shippingMethode_mock_flase)
                return false;
            if(shipmentMethode == shippingMethode_mock_true)
                return true;
            if(!_shipmentServices_name_ip.ContainsKey(shipmentMethode))
                return true;//default mock
            return realShippment(address, city, country, zip, purchaserName, _shipmentServices_name_ip[shipmentMethode]);
        }
     
        private bool realShippment(String address, String city, String country, String zip, String purchaserName, int ip)
        {
            throw new NotImplementedException();
        }
    }
}
