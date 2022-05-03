using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class OrComposition : ComposedDiscountCondition
    {
        public OrComposition(bool negative) : base(negative)
        {
        }

        public OrComposition(bool negative, List<DiscountCondition> conditions) : base(negative, conditions)
        {
        }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result = _discountConditionList.Count == 0; // returns true when the condition list is empty.
            foreach (DiscountCondition discountCondition in this._discountConditionList)
            {
                if (discountCondition.Check(searchablePriceable))
                {
                    result = true;
                    break;
                }
            }
            if (ToNegative)
            {
                return !result;
            }
            return result;
        }
    }
}
