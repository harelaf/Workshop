using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.DiscountPackage
{
    public class XorComposition : ComposedCondition
    {
        public XorComposition(bool negative) : base(negative)
        {
        }

        public XorComposition(bool negative, List<Condition> conditions) : base(negative, conditions)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result;
            bool found = _ConditionList.Count == 0;
            foreach (Condition Condition in this._ConditionList)
            {
                if (Condition.Check(searchablePriceable))
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
            foreach (Condition con in _ConditionList)
                str += $"{pad2}{++index}. {con.GetConditionString(indent + 1)}";
            return str;
        }
    }
}
