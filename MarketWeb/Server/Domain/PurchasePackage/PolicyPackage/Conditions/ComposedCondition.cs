using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public abstract class ComposedCondition : Condition
    {
        protected List<Condition> _ConditionList;
        public List<Condition> ConditionList => _ConditionList;

        protected ComposedCondition(bool negative) : base(negative)
        {
            _ConditionList = new List<Condition>();
        }

        protected ComposedCondition(bool negative, List<Condition> conditions) : base(negative)
        {
            _ConditionList = conditions;
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
