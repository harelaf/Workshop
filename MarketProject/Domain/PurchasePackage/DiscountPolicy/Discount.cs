using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPolicy
{
    public abstract class Discount
    {
        private DiscountCondition _condition; 

        protected Discount(DiscountCondition condition)
        {
            _condition = condition;
        }

        protected Discount()
        {
            _condition = null;
        }

        protected bool CheckCondition()
        {
            return _condition == null || _condition.Check();
        }

        protected String ConditionToString()
        {
            return _condition == null ? "" : "Condition(s): \n" + _condition.ToString();
        }

        public abstract double GetTotalDiscount(ShoppingCart cart);
        public abstract String GetDiscountString(ShoppingCart cart);
        public abstract DateTime GetExpirationDate(ShoppingCart);
    }
}
