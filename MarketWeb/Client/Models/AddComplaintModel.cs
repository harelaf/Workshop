using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AddComplaintModel
    {
        [Required]
        public string Message { get; set; } = "";
    }
}
