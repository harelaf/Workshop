﻿using System.Collections.Generic;

namespace MarketWeb.Server.Domain.PolicyPackage
{
    public abstract class ComposedCondition : Condition
    {
        protected List<Condition> _ConditionList;

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
            if (condition != null)
                _ConditionList.Add(condition);
        }
    }
}