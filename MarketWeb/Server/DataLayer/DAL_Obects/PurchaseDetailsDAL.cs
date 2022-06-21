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
        public string discountListJSON { get; set; }

        public PurchaseDetailsDAL(int ItemID, int amount, string discountListJSON)
        {
            _itemID = ItemID;
            this.amount = amount;
            this.discountListJSON = discountListJSON;
        }

        public PurchaseDetailsDAL()
        {
            discountListJSON = "";
        }
    }
}