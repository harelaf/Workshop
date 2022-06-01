using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain.PurchasePackage.PolicyPackage
{
    public class PurchasePolicy
    {
        private AndComposition _conditions;
        public AndComposition Conditions => _conditions;
        public PurchasePolicy()
        {
            _conditions = new AndComposition(false, new List<Condition>());
        }
        public PurchasePolicy(PurchasePolicy policy)
        {
            _conditions = policy._conditions;
        }
        public void AddCondition(Condition condition)
        {
            _conditions.AddConditionToComposition(condition);
        }
        public virtual bool checkPolicyConditions(ISearchablePriceable searchablePriceable)
        {
            return _conditions.Check(searchablePriceable);
        }
    }
}
