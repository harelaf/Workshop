using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class XorComposition : ComposedCondition
    {
        public XorComposition(bool toNegative) : base(toNegative)
        {
        }
        [JsonConstructor]
        public XorComposition(bool toNegative, List<Condition> conditionList) : base(toNegative, conditionList)
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
            return checkNegative(result);
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
