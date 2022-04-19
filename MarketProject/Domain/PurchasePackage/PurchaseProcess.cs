using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class PurchaseProcess
    {
        private PaymentHandlerProxy _paymentHandlerProxy;
        private ShippingHandlerProxy _shippingHandlerProxy;

        private static PurchaseProcess instance = new PurchaseProcess();
        private PurchaseProcess()
        {

        }
        public static PurchaseProcess GetInstance()
        {
            return instance;
        }

    }
}
