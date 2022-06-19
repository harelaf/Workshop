using MarketWeb.Server.Domain.PurchasePackage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketWeb.Server.Domain
{
    public class PaymentHandlerProxy 
    {
        private IDictionary<string, IPaymentHandler> _paymentHandlers                                              ;
        public static readonly string paymentMethode_mock_flase = "mock_false";
        public static readonly string paymentMethode_mock_true = "mock_true";

        public PaymentHandlerProxy()
        {
            _paymentHandlers = new Dictionary<string, IPaymentHandler>();
        }

        public void AddPaymentMethod(string name, IPaymentHandler paymentHandler)
        {
            _paymentHandlers.TryAdd(name, paymentHandler);
        }
        // includes mock of pay
        public virtual async Task<int> Pay(double price, string paymentMethode, string cardNumber = null, string month = null, string year = null, string holder = null, string ccv = null, string id = null)
        {
            if (paymentMethode == paymentMethode_mock_flase)
                return -1;
            if (paymentMethode == paymentMethode_mock_true)
                return 1;
            if(!_paymentHandlers.ContainsKey(paymentMethode))
                return -1;//default mock
            return await _paymentHandlers[paymentMethode].Pay(cardNumber, month, year, holder, ccv, id);
        }

        public List<String> GetPaymentMethods()
        {
            return new List<string>(_paymentHandlers.Keys);
        }
    }
}
