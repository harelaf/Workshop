using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class DiscountPolicyDAL
    {
        [Key]
        public int id { get; set; }
        public List<DiscountDAL> _discounts { get; set; }

        public DiscountPolicyDAL(List<DiscountDAL> discounts)
        {
            _discounts = discounts;
        }

        public DiscountPolicyDAL()
        {
            // ???
        }
    }
}
