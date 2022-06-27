using MarketWeb.Shared;
using System;

namespace MarketWeb.Server.Domain
{
    public class PopulationStatistics
    {
        public PopulationSection _section { get; set; }
        public DateTime _visitDay { get; set; }

        public int _count { get; set; }

        public PopulationStatistics(PopulationSection section, DateTime visitDay, int count)
        {
            _section = section;
            _visitDay = visitDay;
            _count = count;
        }
    }
}
