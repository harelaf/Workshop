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
        public ItemDAL item { get; set; }
        public int amount { get; set; }

        public StockItemDAL(ItemDAL item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        public StockItemDAL()
        {
            // ???
        }
    }
}
