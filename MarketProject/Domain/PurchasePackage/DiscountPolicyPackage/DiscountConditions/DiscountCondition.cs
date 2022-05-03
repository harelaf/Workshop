using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public abstract class DiscountCondition
    {
        private bool _toNegative;
        public bool ToNegative
        {
            get { return _toNegative; }
            private set { _toNegative = value; }
        }

        protected DiscountCondition(bool negative)
        {
            _toNegative = negative;
        }

        public abstract bool Check(ISearchablePriceable searchablePriceable);
    }
}
