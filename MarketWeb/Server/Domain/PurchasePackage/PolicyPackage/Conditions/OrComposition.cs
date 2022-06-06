using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class OrComposition : ComposedCondition
    {
        public OrComposition(bool negative) : base(negative)
        {
        }

        public OrComposition(bool negative, List<Condition> conditions) : base(negative, conditions)
        {
        }
        public override bool Check(ISearchablePriceable searchablePriceable)
        {
            bool result = _ConditionList.Count == 0; // returns true when the condition list is empty.
            foreach (Condition Condition in this._ConditionList)
            {
                if (Condition.Check(searchablePriceable))
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
            foreach (Condition con in _ConditionList)
                str += $"{pad2}{++index}. {con.GetConditionString(indent + 1)}";
            return str;
        }
    }
}
