using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class AndComposition : ComposedDiscountCondition
    {
        public AndComposition(bool negative) : base(negative)
        {
        }

        public AndComposition(bool negative, List<DiscountCondition> conditions) : base(negative, conditions)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result = true;
            foreach (DiscountCondition discountCondition in _discountConditionList)
            {
                if (!discountCondition.Check(searchablePriceable))
                {
                    result = false;
                    break;
                }
            }
            if (ToNegative)
            {
                return !result;
            }
            return result;
        }

        public override string GetConditionString(int indent)
        {
            String pad2 = newLine(indent + 1);
            String str = "Apply logic AND on the following expressions:";
            int index = 0;
            foreach (DiscountCondition con in _discountConditionList)
                str += $"{pad2}{++index}. {con.GetConditionString(indent + 1)}";
            return str;
        }
    }
}
