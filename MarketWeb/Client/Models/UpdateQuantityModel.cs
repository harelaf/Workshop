using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class UpdateQuantityModel
    {
        [Required]
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
    }
}
