using System.ComponentModel.DataAnnotations;
namespace MarketWeb.Client.Models
{
    public class OpenNewStoreModel
    {
        [Required]
        public string storeName { get; set; }

    }
}
