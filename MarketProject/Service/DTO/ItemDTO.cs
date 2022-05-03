using MarketProject.Domain;
using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Service.DTO
{
    public class ItemDTO
    {
        private RatingDTO _rating;
        private ICollection<Discount> _discounts;
        private int _itemID;
        private String _name;
        private double _price;
        private String _description;
        private String _category;

        public RatingDTO Rating => _rating;
        public ICollection<Discount> Discounts => _discounts;
        public int ItemID => _itemID;
        public String Name => _name;
        public double Price => _price;
        public String Description => _description;
        public String Category => _category;

        public ItemDTO(Item item)
        {
            _rating = new RatingDTO(item.Rating);
            _discounts = item.Discounts;
            _itemID = item.ItemID;
            _name = item.Name;
            _price = item._price;
            _description = item.Description;
            _category = item.Category;
        }
    }
}
