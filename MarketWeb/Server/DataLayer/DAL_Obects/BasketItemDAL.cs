using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class BasketItemDAL
    {
        [Key]
        public int id { get; set; }
        [Required]
        public ItemDAL item { get; set; }
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
