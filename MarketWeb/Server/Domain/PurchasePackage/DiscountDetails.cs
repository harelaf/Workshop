using MarketWeb.Server.Domain.PolicyPackage;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain
{
    public class DiscountDetails
    {
        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (_amount >= 0)
                    _amount = value;
                else throw new ArgumentException("Amount cannot be negative.");
            }
        }
        private ISet<AtomicDiscount> _discountList;
        public ISet<AtomicDiscount> DiscountList
        {
            get { return _discountList; }
            private set { _discountList = value; }
        }
        public DiscountDetails(int amount)
        {
            _amount = amount;
            DiscountList = new HashSet<AtomicDiscount>();
        }

        public DiscountDetails(int amount, ISet<AtomicDiscount> disList)
        {
            Amount = amount;
            DiscountList = disList;
        }

        public void AddDiscount(AtomicDiscount discount)
        {
            DiscountList.Add(discount);
        }
        public double calcPriceFromCurrPrice(double currPrice)
        {
            double price = currPrice * Amount;
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

        public void resetDiscounts()
        {
            DiscountList.Clear();
        }
    }
}