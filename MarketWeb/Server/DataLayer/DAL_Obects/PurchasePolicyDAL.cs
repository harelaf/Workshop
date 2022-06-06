using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchasePolicyDAL
    {
        [Key]
        internal string storeName;
        internal List<ConditionDAL> conditions;
        internal PurchasePolicyDAL(string storeName, List<ConditionDAL> conditions)
        {
            this.storeName = storeName;
            this.conditions = conditions;
        }
    }
}
