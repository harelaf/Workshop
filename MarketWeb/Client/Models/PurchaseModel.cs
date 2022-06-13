using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class PurchaseModel
    {
        [Required]
        public string address { get; set; }

        [Required]
        public string city { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string zip { get; set; }
        [Required]
        public string purchaserName { get; set; }
        [Required]
        public string paymentMethod { get; set; }
        [Required]
        public string shipmentMethod { get; set; }

    }
}
