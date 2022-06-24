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
        [Required]
        public PopulationSection _section { get; set; }

        [Required]
        public DateTime _visitDay { get; set; }

        [Required]
        public int _count { get; set; } 
    }
}
