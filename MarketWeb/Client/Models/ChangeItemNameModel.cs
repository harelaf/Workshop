using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ChangeItemNameModel
    {
        [Required]
        public string NewName { get; set; }
    }
}
