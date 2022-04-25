using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Item
    {
        private Rating _rating;
        private ICollection<IDiscount> _discounts;
        private int _itemID;
        private String _name;
        public virtual double _price { get; set; }
        private String _description;
        private String _category;

        public Rating Rating => _rating;
        public ICollection<IDiscount> Discounts => _discounts;
        public int ItemID => _itemID;
        public String Name => _name;
        public String Description => _description;
        public String Category => _category;

        public Item(int id, String name, double price, String description, String category)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            _rating = new Rating();
            _discounts = new List<IDiscount>();
            _itemID = id;
            _name = name;
            _price = price;
            _description = description;
            _category = category;
        }

        public void RateItem(String Username, int rating, String review)
        {
            if (_rating.HasRating(Username)){
                throw new Exception("You can't rate the same item twice or more.");
            }
            _rating.AddRating(Username, rating, review);
        }

        public void SetName(String name)
        {
            if (name == null) { throw new ArgumentNullException("name"); }
            _name = name;
        }

        public void SetPrice(double price)
        {
            _price = price;
        }

        public void SetDescription(String description)
        {
            _description = description;
        }

        
    }
}
