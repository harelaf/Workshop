using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ModifyStoreManagerPermissionsModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Permission { get; set; }
    }
}
