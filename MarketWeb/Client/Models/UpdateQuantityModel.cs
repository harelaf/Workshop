using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class UpdateQuantityModel
    {
        [Required]
        public int ItemID { get; set; }
        [Required]
        [Range(0,1000000)]
        public int Quantity { get; set; }
    }
}
