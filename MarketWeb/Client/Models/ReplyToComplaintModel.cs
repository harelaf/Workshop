using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ReplyToComplaintModel
    {
        [Required]
        public string Reply { get; set; }
    }
}
