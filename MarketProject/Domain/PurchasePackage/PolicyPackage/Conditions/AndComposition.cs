using System;
using System.Collections.Generic;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public class AndComposition : ComposedCondition
    {
        public AndComposition(bool negative) : base(negative)
        {
        }

        public AndComposition(bool negative, List<Condition> conditions) : base(negative, conditions)
        {
        }

        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result = true;
            foreach (Condition Condition in _ConditionList)
            {
                if (!Condition.Check(searchablePriceable))
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
            foreach (Condition con in _ConditionList)
                str += $"{pad2}{++index}. {con.GetConditionString(indent + 1)}";
            return str;
        }
    }
}
