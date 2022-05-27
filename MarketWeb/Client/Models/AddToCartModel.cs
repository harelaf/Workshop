using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AddToCartModel
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be at least 1.")]
        public int Amount { get; set; }
    }
}
