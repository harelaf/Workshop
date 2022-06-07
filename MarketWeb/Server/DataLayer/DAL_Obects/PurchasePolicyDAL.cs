using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchasePolicyDAL
    {
        [Key]
        int id;
        internal List<ConditionDAL> conditions;
        internal PurchasePolicyDAL(List<ConditionDAL> conditions)
        {
            this.conditions = conditions;
        }
    }
}
