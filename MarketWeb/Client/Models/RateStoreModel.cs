using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class RateStoreModel
    {
        [Required]
        [Range(0, 10)]
        public int rating { get; set; } = 5;

        public string comment { get; set; }
    }
}
