﻿

using System;

namespace MarketWeb.Shared.DTO
{
    public class ItemDTO
    {
        private RatingDTO _rating;
        private int _itemID;
        private String _name;
        private double _price;
        private String _description;
        private String _category;
        private String _storeName;
        public RatingDTO Rating => _rating;
        public int ItemID => _itemID;
        public String Name => _name;
        public double Price => _price;
        public String Description => _description;
        public String Category => _category;
        public String StoreName => _storeName;


        public ItemDTO(int id, String name, double price, String description, String category, RatingDTO rating, String storeName)
        {
            _rating = rating;
            _itemID = id;
            _name = name;
            _price = price;
            _description = description;
            _category = category;
            _storeName = null;
            _storeName = storeName;
        }
    }
}
