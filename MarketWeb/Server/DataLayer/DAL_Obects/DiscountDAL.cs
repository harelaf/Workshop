using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    internal class DiscountDAL
    {
        [Key]
        public string storeName { get; set; }

    }
}