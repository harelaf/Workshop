using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class RatingDAL
    {
        [Key]
        public int id { get; set; }
        public ICollection<RateDAL> _ratings { get; set; } //ratings =: rating:int, review:String

        public RatingDAL(ICollection<RateDAL> ratings)
        {
            _ratings = ratings;
        }

        public RatingDAL()
        {
            // ???
        }
    }
}
