using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public abstract class AtomicDiscount : Discount
    {
        private DateTime _expiration;
        public override DateTime GetExpirationDate(ISearchablePriceable searchablePriceable)
        {
            return _expiration;
        }
        protected String ExpirationToString(int indent)
        { 
            return newLine(indent) + "Expired on: " + _expiration.ToString();
        }
        protected AtomicDiscount(Condition condition, DateTime expiration) : base(condition)
        {
            _expiration = expiration;
        }
        protected AtomicDiscount(DateTime expiration) : base()
        {
            _expiration = expiration;
        }
        public abstract double calcPriceFromCurrPrice(double currPrice);
    }
}
