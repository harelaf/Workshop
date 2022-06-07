using MarketProject.Domain.PurchasePackage.PolicyPackage;
using System;
using System.Collections.Generic;

namespace MarketProject.Domain
{
    public class DiscountDetails
    {
        private int _amount;
        public int Amount => _amount;
        private List<AtomicDiscount> _discountList;
        public List<AtomicDiscount> DiscountList
        {
            get { return _discountList; }
            private set { _discountList = value; }
        }
        public DiscountDetails(int amount)
        {
            _amount = amount;
            DiscountList = new List<AtomicDiscount>();
        }
        public void AddDiscount(AtomicDiscount discount)
        {
            DiscountList.Add(discount);
        }
        public double calcPriceFromCurrPrice(double currPrice)
        {
            double price = currPrice;
            foreach(AtomicDiscount discount in DiscountList)
                price = discount.calcPriceFromCurrPrice(price);
            return price;
        }
        public void AddAmount(int amount)
        {
            _amount += amount;
        }
        public void reduceAmount(int amount)
        {
            _amount -= amount;
        }

        internal void UpdateAmount(int newQuantity)
        {
            _amount = newQuantity;
        }
    }
}