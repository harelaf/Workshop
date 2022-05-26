﻿using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Client.Models
{
    public class AddItemModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }

    }
}
