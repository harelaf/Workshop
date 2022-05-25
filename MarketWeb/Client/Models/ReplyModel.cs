using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ReplyModel
    {
        [Required]
        public string Reply { get; set; }
    }
}
