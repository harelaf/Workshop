using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchaseDetailsDAL
    {
        [Key]
        internal int ItemID;
        [Required]
        internal int amount;
        [Required]
        internal List<DiscountDAL> discountList;

        internal PurchaseDetailsDAL(int ItemID, int amount, List<DiscountDAL> discountList)
        {
            this.amount = amount;
            this.discountList = discountList;
        }
    }
}