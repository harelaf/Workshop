using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    internal abstract class AtomicDiscount : Discount
    {
        private DateTime _expiration;
        public override DateTime GetExpirationDate(ShoppingCart cart)
        {
            return _expiration;
        }
        protected String ExpirationToString()
        { 
            return "Expired on: " + _expiration.ToString();
        }
        protected AtomicDiscount(DiscountCondition condition, DateTime expiration) : base(condition)
        {
            _expiration = expiration;
        }

        protected AtomicDiscount(DateTime expiration) : base()
        {
            _expiration = expiration;
        }
    }
}
