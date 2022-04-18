using System;
using System.Collections.Generic;
using System.Text;

namespace MarketProject.Domain
{
    public class Item
    {
        private ICollection<Rating> _ratings;
        private ICollection<IDiscount> _discounts;
        private int _itemID;
        private String _name;
        private float _price;
        private String _description;


        public int ItemID => _itemID;
        public String Name => _name;
        public float Price => _price;
        public String Description => _description;

        public Item(int id, String name, float price, String description)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            _ratings = new List<Rating>();
            _discounts = new List<IDiscount>();
            _itemID = id;
            _name = Name;
            _price = price;
            _description = description;
        }

        public void SetName(String name)
        {
            if (name == null) { throw new ArgumentNullException("name"); }
            _name = name;
        }

        public void SetPrice(float price)
        {
            this._price = price;
        }

        public void SetDescription(String description)
        {
            _description = description;
        }

    }
}
