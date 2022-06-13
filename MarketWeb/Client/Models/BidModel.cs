using System.ComponentModel.DataAnnotations;
namespace MarketWeb.Client.Models
{
    public class BidModel
    {
        [Range(0, double.MaxValue)]
        public double CounterOffer { get; set; }
    }
}
