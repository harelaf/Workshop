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
        public override String GetConditionString(int indent)
        {
            String pad2 = newLine(indent + 1);
            String str = "Apply logic OR on the following expressions:";
            int index = 0;
            foreach (DiscountCondition con in _discountConditionList)
                str += $"{pad2}{++index}. {con.GetConditionString(indent + 1)}";
            return str;
        }
    }
}
