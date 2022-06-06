using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AddingPurchasePolicyModel
    {
        [Required]
        public string ConditionString { get; set; }
    }
}
