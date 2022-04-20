using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class PaymentHandlerProxy : IPayment
    {
        private RealPaymentSystem _realPaymentSystem;
        public PaymentHandlerProxy(RealPaymentSystem realPaymentSystem)
        {
            _realPaymentSystem = realPaymentSystem;
        }
        // includes mock of pay
        public virtual bool Pay(double price)
        {
            if(_realPaymentSystem == null)
                return true;
            return _realPaymentSystem.pay(price);
        }
    }
}
