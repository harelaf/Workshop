using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class PurchaseProcess
    {
        public virtual PaymentHandlerProxy _paymentHandlerProxy { get; set; }
        public virtual ShippingHandlerProxy _shippingHandlerProxy{ get; set; }
        //singelton:
        private static PurchaseProcess _instance;
        private PurchaseProcess()
        {
            _paymentHandlerProxy = new PaymentHandlerProxy(null);
            _shippingHandlerProxy = new ShippingHandlerProxy(null);
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
        public void Purchase(String address, String city, String country, String zip, String purchaserName, ShoppingCart cartToPurchase)
        {
            string errorMessage="";
            //first: should check that shippingSystem willig to provide cart:
            if(_shippingHandlerProxy.ShippingApproval(address, city, country, zip, purchaserName))
            {
                //second: the actual payment:
                double price = CalculatePrice(cartToPurchase);
                if (_paymentHandlerProxy.Pay(price))// payment succseded
                    return;
                errorMessage = "Purchase failed: paymentSystem refuses.";
            }
            else
            {
                errorMessage = "Purchase failed: Shipping services refuse to provide your cart.";
            } 
             //relaseCart:
             cartToPurchase.RelaseItemsOfCart();
            throw new Exception(errorMessage);
        }
        private double CalculatePrice(ShoppingCart shoppingCart)
        {
            double price = 0;
            foreach( ShoppingBasket shoppingBasket in shoppingCart._shoppingBaskets)
            {
                foreach(Item item in shoppingBasket.GetItems())
                {
                    lock (item)
                    {
                        int itemQuantity = shoppingBasket.GetAmountOfItem(item);
                        price += itemQuantity* item._price;
                    }
                    
                }
            }
            return price;
        }

    }
}
