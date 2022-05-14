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
        public string paymentMethode { get; set; }
        [Required]
        public string shipmentMethode { get; set; }

    }
}
