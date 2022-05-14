using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class DeleteUserModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
