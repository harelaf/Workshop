using System.Collections.Generic;

namespace MarketWeb.Server.DataLayer
{
    public class DiscountPolicyDAL
    {
        internal List<DiscountDAL> _discounts;

        internal DiscountPolicyDAL(List<DiscountDAL> discounts)
        {
            _discounts = discounts;
        }
    }
}
