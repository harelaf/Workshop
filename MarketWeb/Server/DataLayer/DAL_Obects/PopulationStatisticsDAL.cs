using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using MarketWeb.Shared;

namespace MarketWeb.Server.DataLayer
{
    public class PopulationStatisticsDAL
    {

        [Key]
        public int id { get; set; }

        public string _userNane { get; set; } // for guest-> null. for registered-> username

        [Required]
        public DateTime _visitDay { get; set; }
    }
}
