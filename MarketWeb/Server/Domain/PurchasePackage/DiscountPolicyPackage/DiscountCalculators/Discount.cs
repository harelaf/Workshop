using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPackage
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

        protected bool CheckCondition(ISearchablePriceable searchablePriceable)
        {
            return _condition == null || _condition.Check(searchablePriceable);
        }

        protected String ConditionToString(int indent)
        {
            return _condition == null ? "" : newLine(indent) + "Condition(s): " + _condition.GetConditionString(indent);
        }

        public abstract double GetTotalDiscount(ISearchablePriceable searchablePriceable);
        public abstract String GetDiscountString(int indent);
        public abstract String GetActualDiscountString(ISearchablePriceable searchablePriceable, int indent);
        public abstract DateTime GetExpirationDate(ISearchablePriceable searchablePriceable);
        protected String newLine(int numOfTabs)
        {
            String str = "\n";
            for (int i = 0; i < numOfTabs; i++)
                str += '\t';
            return str;
        }
    }
}
