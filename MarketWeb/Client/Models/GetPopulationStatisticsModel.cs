using System;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class GetPopulationStatisticsModel
    {
        [Required]
        public DateTime date { get; set; }
    }
}
