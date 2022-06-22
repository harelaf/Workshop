using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public abstract class ComposedCondition : Condition
    {
        protected List<Condition> _ConditionList;
        public List<Condition> ConditionList => _ConditionList;

        protected ComposedCondition(bool toNegative) : base(toNegative)
        {
            _ConditionList = new List<Condition>();
        }
        [JsonConstructor]
        protected ComposedCondition(bool toNegative, List<Condition> conditionList) : base(toNegative)
        {
            _ConditionList = conditionList;
        }

        public void AddConditionToComposition(Condition condition)
        {
            lock (ConditionList)
            {
                if (condition != null)
                    _ConditionList.Add(condition);
            }
        }
    }
}
