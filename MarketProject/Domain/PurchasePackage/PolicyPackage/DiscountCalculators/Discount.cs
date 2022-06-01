using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public abstract class Discount
    {
        private Condition _condition; 

        protected Discount(Condition condition)
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

        public abstract void applyDiscount(ISearchablePriceable searchablePriceable);
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
