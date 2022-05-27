using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class SendAdminMessageModel
    {
        [Required]
        public string ReceiverUsername { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
