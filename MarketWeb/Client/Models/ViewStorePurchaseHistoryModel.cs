using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ViewStorePurchaseHistoryModel
    {
        [Required]
        public string StoreName { get; set; }
    }
}
