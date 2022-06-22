using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public class AndComposition : ComposedCondition
    {
        public AndComposition(bool toNegative) : base(toNegative)
        {
        }
        [JsonConstructor]
        public AndComposition(bool toNegative, List<Condition> conditionList) : base(toNegative, conditionList)
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
            return checkNegative(result);
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
