using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models.Account
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}