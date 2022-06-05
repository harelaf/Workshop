using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ItemDAL
    {
        [Key]
        internal int _itemID { get; set; }
        internal RatingDAL _rating { get; set; }
        [Required]
        internal String _name { get; set; }
        [Required]
        internal virtual double _price { get; set; }
        [Required]
        internal String _description { get; set; }
        [Required]
        internal String _category { get; set; }

        public ItemDAL(RatingDAL rating, string name, double price, string description, string category)
        {
            _rating = rating;
            _name = name;
            _price = price;
            _description = description;
            _category = category;
        }

        //private ICollection<DiscountDAL> _discounts;
    }
}
