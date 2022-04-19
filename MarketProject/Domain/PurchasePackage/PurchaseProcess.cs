using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public sealed class PurchaseProcess
    {
        private PaymentHandlerProxy _paymentHandlerProxy;
        private ShippingHandlerProxy _shippingHandlerProxy;
        //singelton:
        private static PurchaseProcess _instance;
        private PurchaseProcess()
        {
            _paymentHandlerProxy = new PaymentHandlerProxy(null);
            _shippingHandlerProxy = new ShippingHandlerProxy(null);
        }
        public static PurchaseProcess GetInstance()
        {
            if (_instance == null)
                _instance = new PurchaseProcess();
            return _instance;
        }

        public void Purchase(string adr, ShoppingCart cartToPurchase)
        {
            string errorMessage="";
            //first: should check that shippingSystem willig to provide cart:
            if(_shippingHandlerProxy.ShippingApproval(adr))// ------AFIK: send cart?????------
            {
                //second: the actual payment:
                double price = CalculatePrice(cartToPurchase);
                if (_paymentHandlerProxy.Pay(price))// payment succseded
                    return;
                errorMessage= "Purchase failed: paymentSystem refuses."
            }
            else
            {
                errorMessage = "Purchase failed: Shipping services refuse to provide your cart."
            } 
             //relaseCart:
             cartToPurchase.RelaseItemsOfCart();
            throw new Exception(errorMessage);
        }
        private double CalculatePrice(ShoppingCart shoppingCart)
        {
            double price = 0;
            foreach( ShoppingBasket shoppingBasket in shoppingCart.ShoppingBaskets)
            {
                foreach(Item item in shoppingBasket.GetItems())
                {
                    int itemQuantity = shoppingBasket.GetAmountOfItem(item);
                    price += itemQuantity* item.Price;
                }
            }
            return price;
        }

    }
}
