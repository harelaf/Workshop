using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AddReviewModel
    {
        [Required]
        public int Rating { get; set; } = 5;

        public string Review { get; set; }
    }
}
