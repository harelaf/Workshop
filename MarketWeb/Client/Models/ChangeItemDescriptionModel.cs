using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ChangeItemDescriptionModel
    {
        [Required]
        public string NewDescription { get; set; }
    }
}
