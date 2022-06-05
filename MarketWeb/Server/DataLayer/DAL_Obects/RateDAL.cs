using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class RateDAL
    {
        [Key]
        internal int rid { get; set; }
        [Required]
        internal string username { get; set; }
        [Required]
        internal int rate { get; set; }
        internal string review { get; set; }

        public RateDAL(string username, int rate, string review)
        {
            this.username = username;
            this.rate = rate;
            this.review = review;
        }
    }
}
