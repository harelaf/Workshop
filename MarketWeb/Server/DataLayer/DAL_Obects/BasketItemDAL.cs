using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class BasketItemDAL
    {
        [Key]
        public int id { get; set; }
        [Required]
        [ForeignKey("ItemDAL")]
        public int itemID { get; set; }
        [Required]
        public PurchaseDetailsDAL purchaseDetails { get; set; }

        public BasketItemDAL()
        {
        }

        public BasketItemDAL(int itemID, PurchaseDetailsDAL purchaseDetails)
        {
            this.itemID = itemID;
            this.purchaseDetails = purchaseDetails;
        }

    }
}
