using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class ModifyStoreManagerPermissionsModel
    {
        [Required]
        public string Username { get; set; }

        public bool ReceiveInfoAndReply { get; set; }

        public bool ReceiveStorePurchaseHistory { get; set; }
    }
}
