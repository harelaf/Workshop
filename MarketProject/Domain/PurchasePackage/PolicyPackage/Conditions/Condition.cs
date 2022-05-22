using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public abstract class Condition
    {
        private bool _toNegative;
        public bool ToNegative
        {
            get { return _toNegative; }
            private set { _toNegative = value; }
        }

        protected Condition(bool negative)
        {
            _toNegative = negative;
        }
        protected String newLine(int indent)
        {
            String str = "\n";
            for (int i = 0; i < indent; i++)
                str += '\t';
            return str;
        }
        protected bool checkNegative(bool cond)
        {
            if (ToNegative)
                return !cond;
            else return cond;
        }

        public abstract bool Check(ISearchablePriceable searchablePriceable);
        public abstract string GetConditionString(int indent);
    }
}
