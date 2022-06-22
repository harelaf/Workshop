using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
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
        [JsonConstructor]
        public ComposedDiscount(List<Discount> discountList, Condition condition) : base(condition)
        {
            lock(DiscountList){
                _discountList = discountList;
            }
        }
        public ComposedDiscount(List<Discount> discounts) : base()
        {
            lock (DiscountList)
            {
                _discountList = discounts;
            }
        }
    }
}
