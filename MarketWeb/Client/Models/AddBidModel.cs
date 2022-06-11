using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AddBidModel
    {
        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be at least 1.")]
        public int Amount { get; set; }
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public double PricePerUnit { get; set; }
    }
}
