using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWeb.Server.DataLayer
{
    public class PurchaseDetailsDAL
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [ForeignKey("ItemDAL")]
        public int _itemID { get; set; }
        [Required]
        public int amount { get; set; }
        [Required]
        public List<AtomicDiscountDAL> discountList { get; set; }

        public PurchaseDetailsDAL(int ItemID, int amount, List<AtomicDiscountDAL> discountList)
        {
            _itemID = ItemID;
            this.amount = amount;
            this.discountList = discountList;
        }

        public PurchaseDetailsDAL()
        {
            discountList = new List<AtomicDiscountDAL>();
        }
    }
}