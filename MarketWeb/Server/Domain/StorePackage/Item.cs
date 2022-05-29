using MarketWeb.Server.Domain.PolicyPackage;
using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain
{
    public class Item
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Rating _rating;
        private ICollection<Discount> _discounts;
        private int _itemID;
        private String _name;
        public virtual double _price { get; set; }
        private String _description;
        private String _category;

        public Rating Rating => _rating;
        public ICollection<Discount> Discounts => _discounts;
        public int ItemID => _itemID;
        public String Name => _name;
        public String Description => _description;
        public String Category => _category;

        public Item(int id, String name, double price, String description, String category)
        {
            if (name == null)
            {
                String errorMessage = "Name is null";
                LogErrorMessage("Constructor", errorMessage);
                throw new Exception(errorMessage);
            }
            _rating = new Rating();
            _discounts = new List<Discount>();
            _itemID = id;
            _name = name;
            _price = price;
            _description = description;
            _category = category;
        }

        public void RateItem(String Username, int rating, String review)
        {
            String errorMessage;
            if (_rating.HasRating(Username)){
                errorMessage = "You can't rate the same item twice or more.";
                LogErrorMessage("RateItem", errorMessage);
                throw new Exception(errorMessage);
            }
            _rating.AddRating(Username, rating, review);
        }

        public void SetName(String name)
        {
            String errorMessage;
            if (name == null) 
            {
                errorMessage = "name";
                LogErrorMessage("SetName", errorMessage);
                throw new ArgumentNullException(errorMessage);
            }
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

        private void LogErrorMessage(String functionName, String message)
        {
            log.Error($"Exception thrown in Item.{functionName}. Cause: {message}.");
        }
    }
}
