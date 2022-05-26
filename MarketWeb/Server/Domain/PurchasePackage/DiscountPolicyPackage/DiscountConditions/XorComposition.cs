using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPackage
{
    public class XorComposition : ComposedDiscountCondition
    {
        public XorComposition(bool negative) : base(negative)
        {
        }

        public XorComposition(bool negative, List<DiscountCondition> conditions) : base(negative, conditions)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result;
            bool found = _discountConditionList.Count == 0;
            foreach (DiscountCondition discountCondition in this._discountConditionList)
            {
                if (discountCondition.Check(searchablePriceable))
                {
                    if(!found)
                        found = true;
                    else
                    {
                        found = false;
                        break;
                    }
                }
            }
            result = found;
            if (ToNegative)
            {
                return !result;
            }
            return result;
        }
        public override String GetConditionString(int indent)
        {
            String pad2 = newLine(indent + 1);
            String str = "Apply logic XOR on the following expressions:";
            int index = 0;
            foreach (DiscountCondition con in _discountConditionList)
                str += $"{pad2}{++index}. {con.GetConditionString(indent + 1)}";
            return str;
        }
    }
}
