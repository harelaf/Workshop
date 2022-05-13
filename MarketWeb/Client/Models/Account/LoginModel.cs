using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models.Account
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}