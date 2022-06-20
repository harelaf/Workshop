using MarketWeb.Server.Domain.PurchasePackage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketWeb.Server.Domain
{
    public class ShippingHandlerProxy
    {
        private IDictionary<string, IShippingHandler> _shipmentHandlers;
        public static readonly string shippingMethode_mock_flase = "mock_false";
        public static readonly string shippingMethode_mock_true = "mock_true";

        public ShippingHandlerProxy()
        {
            _shipmentHandlers = new Dictionary<string, IShippingHandler>();
        }
        public void AddShipmentMethod(string name, IShippingHandler shippingHandler)
        {
            _shipmentHandlers.TryAdd(name, shippingHandler);
        }
        // includes mock of shipp
        public virtual async Task<int> ShippingApproval(String address, String city, String country, String zip, String purchaserName, string shipmentMethode)
        {
            if(shipmentMethode == shippingMethode_mock_flase)
                return -1;
            if(shipmentMethode == shippingMethode_mock_true)
                return 1;
            if(!_shipmentHandlers.ContainsKey(shipmentMethode))
                return -1;//default mock
            return await _shipmentHandlers[shipmentMethode].Supply(purchaserName, address, city, country, zip);
        }

        internal List<string> GetShipmentMethods()
        {
            return new List<string>(_shipmentHandlers.Keys);
        }
    }
}
