using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AppointRoleModel
    {
        [Required]
        public string Username { get; set; }
    }
}
