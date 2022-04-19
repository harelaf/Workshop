using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class PaymentHandlerProxy : IPayment
    {
        private RealPaymentSystem _realPaymentSystem;
        public PaymentHandlerProxy(RealPaymentSystem realPaymentSystem)
        {
            _realPaymentSystem = realPaymentSystem;
        }
        // includes mock of pay
        public bool Pay(double price)
        {
            if(RealPaymentSystem == null)
                return true;
            return _realPaymentSystem.pay(price);
        }
    }
}
