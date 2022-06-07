using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class RateDAL
    {
        [Key]
        public int rid { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public int rate { get; set; }
        public string review { get; set; }

        public RateDAL(string username, int rate, string review)
        {
            this.username = username;
            this.rate = rate;
            this.review = review;
        }

        public RateDAL()
        {
            // ???
        }
    }
}
