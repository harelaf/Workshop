using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class PurchaseDetailsDAL
    {
        [Key]
        internal int _itemID;
        [Required]
        internal int amount;
        [Required]
        internal List<AtomicDiscountDAL> discountList;

        internal PurchaseDetailsDAL(int ItemID, int amount, List<AtomicDiscountDAL> discountList)
        {
            _itemID = ItemID;
            this.amount = amount;
            this.discountList = discountList;
        }
    }
}