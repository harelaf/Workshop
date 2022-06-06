using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public abstract class ComposedDiscount : Discount
    {
        private List<Discount> _discountList = new List<Discount>();
        public List<Discount> DiscountList
        {
            get { return _discountList; }
            private set
            {
                //if (value == null)
                //    throw new ArgumentNullException("discount list must contain at least one discount.");
                _discountList = value;
            }
        }
        public ComposedDiscount(List<Discount> discounts, Condition condition) : base(condition)
        {
            lock(_discountList){
                _discountList = discounts;
            }
        }
        public ComposedDiscount(List<Discount> discounts) : base()
        {
            lock (_discountList)
            {
                _discountList = discounts;
            }
        }
    }
}
