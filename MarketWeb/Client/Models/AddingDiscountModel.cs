using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AddingDiscountModel
    {
        public string ConditionString { get; set; } = "";

        [Required]
        public string DiscountString { get; set; }
    }
}
