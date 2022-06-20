using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWeb.Server.DataLayer
{
    public class StockItemDAL
    {
        [Key]
        public int id { get; set; }
        [Required]
        [ForeignKey("ItemDAL")]
        public int itemID { get; set; }
        public int amount { get; set; }
        public StockItemDAL(int itemID, int amount)
        {
            this.itemID = itemID;
            this.amount = amount;
        }

        public StockItemDAL()
        {
            // ???
        }
    }
}
