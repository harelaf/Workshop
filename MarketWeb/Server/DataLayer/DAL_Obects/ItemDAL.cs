﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ItemDAL
    {
        [Key]
        public int _itemID { get; set; }
        [Required]
        [ForeignKey("RatingDAL")]
        public ICollection<RateDAL> _rating { get; set; }
        [Required]
        public String _name { get; set; }
        [Required]
        public virtual double _price { get; set; }
        [Required]
        public String _description { get; set; }
        [Required]
        public String _category { get; set; }

        public ItemDAL(ICollection<RateDAL> rating, string name, double price, string description, string category)
        {
            _rating = rating;
            _name = name;
            _price = price;
            _description = description;
            _category = category;
        }

        public ItemDAL(int itemID, ICollection<RateDAL> rating, string name, double price, string description, string category)
        {
            _itemID = itemID;
            _rating = rating;
            _name = name;
            _price = price;
            _description = description;
            _category = category;
        }

        //private ICollection<DiscountDAL> _discounts;

        public ItemDAL()
        {
            _rating = new List<RateDAL>(); 
        }
    }
}
