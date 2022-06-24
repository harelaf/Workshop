using MarketWeb.Server.Domain.PurchasePackage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarketWeb.Server.Domain
{
    public class PurchaseProcess
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual PaymentHandlerProxy _paymentHandlerProxy { get; set; }
        public virtual ShippingHandlerProxy _shippingHandlerProxy{ get; set; }
        //singelton:
        private static PurchaseProcess _instance;
        private PurchaseProcess()
        {
            _paymentHandlerProxy = new PaymentHandlerProxy();
            _shippingHandlerProxy = new ShippingHandlerProxy();
        }
        private PurchaseProcess(PaymentHandlerProxy paymentHandlerProxy, ShippingHandlerProxy shippingHandlerProxy)
        {
            _paymentHandlerProxy = paymentHandlerProxy;
            _shippingHandlerProxy = shippingHandlerProxy;
        }
        public static PurchaseProcess GetInstance()
        {
            if (_instance == null)
                _instance = new PurchaseProcess();
            return _instance;
        }
        public void  SetInstance(PaymentHandlerProxy paymentHandlerProxy, ShippingHandlerProxy shippingHandlerProxy)
        {
            _instance._shippingHandlerProxy = shippingHandlerProxy;
            _instance._paymentHandlerProxy = paymentHandlerProxy;
        }
        public void AddPaymentMethod(string name, IPaymentHandler paymentHandler)
        {
            _paymentHandlerProxy.AddPaymentMethod(name, paymentHandler);
        }
        public void AddShipmentMethod(string name, IShippingHandler shippingHandler)
        {
            _shippingHandlerProxy.AddShipmentMethod(name, shippingHandler);
        }
        public async Task Purchase(String address, String city, String country, String zip, String purchaserName, ShoppingCart cartToPurchase, string paymentMethode, string shipmentMethode,  string cardNumber = null, string month = null, string year = null, string holder = null, string ccv = null, string id = null)
        {
            string errorMessage = "";
            foreach (ShoppingBasket basket in cartToPurchase._shoppingBaskets)
            {
                if (!basket.checkPurchasePolicy())
                {
                    errorMessage = $"Purchase failed: this shop is not approved by '{basket.Store().StoreName}' store purchase policy.";
                }
            }
            //first: should check that shippingSystem willig to provide cart:
            if(errorMessage == "" && await _shippingHandlerProxy.ShippingApproval(address, city, country, zip, purchaserName, shipmentMethode) != -1)
            {
                //second: the actual payment:
                double price = CalculatePrice(cartToPurchase);
                if (-1 != await _paymentHandlerProxy.Pay(price, paymentMethode, cardNumber, month, year, holder, ccv, id))// payment succseded
                    return;
                errorMessage = "Purchase failed: paymentSystem refuses.";
            }
            else if(errorMessage == "")
            {
                errorMessage = "Purchase failed: Shipping services refuse to provide your cart.";
            }
            //relaseCart:
            //if(errorMessage != "")
            //{
            //    cartToPurchase.RelaseItemsOfCart();
            //}
            LogErrorMessage("Purchase", errorMessage);
            throw new Exception(errorMessage);
        }
        private double CalculatePrice(ShoppingCart shoppingCart)
        {
            //double price = 0;
            //foreach (ShoppingBasket shoppingBasket in shoppingCart._shoppingBaskets)
            //{
            //    foreach (Item item in shoppingBasket.GetItems())
            //    {
            //        lock (item)
            //        {
            //            int itemQuantity = shoppingBasket.GetAmountOfItem(item);
            //            price += itemQuantity * item._price;
            //        }
            //    }
            //}
            //return price;
            return shoppingCart.getActualPrice();
        }

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in PurchaseProcess.{functionName}. Cause: {message}.");
        }
    }
}
