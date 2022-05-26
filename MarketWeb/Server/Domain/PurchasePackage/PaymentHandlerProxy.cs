using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class PaymentHandlerProxy 
    {
        private IDictionary<string, int> _paymentServices_name_ip;
        public static readonly string paymentMethode_mock_flase = "mock_false";
        public static readonly string paymentMethode_mock_true = "mock_true";

        public PaymentHandlerProxy()
        {
            _paymentServices_name_ip = new Dictionary<string, int>();
        }
        // includes mock of pay
        public virtual bool Pay(double price, string paymentMethode)
        {
            if (paymentMethode==paymentMethode_mock_flase)
                return false;
            if (paymentMethode== paymentMethode_mock_true)
                return true;
            if(!_paymentServices_name_ip.ContainsKey(paymentMethode))
                return true;//default mock
            return realPay(price, _paymentServices_name_ip[paymentMethode]);
        }
        private bool realPay(double price, int ip)
        {
            throw new NotImplementedException();
        }
    }
}
