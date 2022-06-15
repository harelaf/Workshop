using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchasePolicyDAL
    {
        [Key]
        public int id { get; set; }
        [Required]
        public List<ConditionDAL> conditions { get; set; }
        public PurchasePolicyDAL(List<ConditionDAL> conditions)
        {
            this.conditions = conditions;
        }

        public PurchasePolicyDAL()
        {
            conditions = new List<ConditionDAL>();

        }
    }
}
