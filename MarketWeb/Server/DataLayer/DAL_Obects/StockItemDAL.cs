using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class StockItemDAL
    {
        [Key]
        public int id { get; set; }
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
