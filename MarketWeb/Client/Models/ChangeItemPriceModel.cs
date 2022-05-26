using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ChangeItemPriceModel
    {
        [Required]
        public float NewPrice { get; set; }
    }
}
