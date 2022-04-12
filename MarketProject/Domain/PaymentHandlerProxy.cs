using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    class PaymentHandlerProxy : IPayment
    {
        private RealPaymentSystem _realPaymentSystem;
    }
}
