using System;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models.Account
{
    public class RegisterModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Password and Confirm Password must match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
    }
}