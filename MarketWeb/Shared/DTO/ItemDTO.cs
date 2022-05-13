﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class ItemDTO
    {
        private RatingDTO _rating;
        //private ICollection<IDiscount> _discounts;
        private int _itemID;
        private String _name;
        private double _price;
        private String _description;
        private String _category;
        private String _storeName;

        public ItemDTO(string name, double price)
        {
            _name = name;
            _price = price;
            _itemID = name[name.Length - 1];
            _description = "blaaaaaaaaa";
            _category = "fruts";
        }
        public ItemDTO(string name, double price, string storeName)
        {
            _name = name;
            _price = price;
            _storeName = storeName;
            _rating = new RatingDTO();
            _itemID = name[name.Length - 1];
            _description = "blaaaaaaaaa";
            _category = "fruts";
        }

        public RatingDTO Rating => _rating;
        //public ICollection<IDiscount> Discounts => _discounts;
        public int ItemID => _itemID;
        public String Name => _name;
        public double Price => _price;
        public String Description => _description;
        public String Category => _category;

        public String StoreName => _storeName;


    }
}
