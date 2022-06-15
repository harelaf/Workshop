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
        public ItemDAL item { get; set; }
        [Required]
        public PurchaseDetailsDAL purchaseDetails { get; set; }

        public BasketItemDAL()
        {
            // ???
        }

        public BasketItemDAL(ItemDAL item, PurchaseDetailsDAL purchaseDetails)
        {
            this.item = item;
            this.purchaseDetails = purchaseDetails;
        }

    }
}
