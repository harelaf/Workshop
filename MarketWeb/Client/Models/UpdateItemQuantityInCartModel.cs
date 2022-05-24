using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class UpdateItemQuantityInCartModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "New quantity has to be at least 1.")]
        public int NewQuantity { get; set; }
    }
}
