using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class FireRoleModel
    {
        [Required]
        public string Username { get; set; }
    }
}
