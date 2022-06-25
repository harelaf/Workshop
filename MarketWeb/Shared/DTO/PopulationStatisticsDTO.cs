using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class PopulationStatisticsDTO
    {
        public PopulationSection _section { get; set; }
        public DateTime _visitDay { get; set; }

        public int _count { get; set; }

        public PopulationStatisticsDTO(PopulationSection section, DateTime visitDay, int count)
        {
            _section = section;
            _visitDay = visitDay;
            _count = count;
        }
    }
}
