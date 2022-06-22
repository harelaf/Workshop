using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class BidDAL
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string _bidder { get; set; }
        [Required]
        public int _itemId { get; set; }
        [Required]
        public int _amount { get; set; }
        [Required]
        public double _biddedPrice { get; set; }
        [Required]
        public double _counterOffer { get; set; }
        //[Required]
        public ICollection<StringData> _acceptors { get; set; }
        public bool _acceptedByAll { get; set; }

        public BidDAL(
            string bidder, 
            int itemId, 
            int amount, 
            double biddedPrice, 
            double counterOffer,
            ICollection<StringData> acceptors,
            bool acceptedByAll)
        {
            _bidder = bidder;
            _itemId = itemId;
            _amount = amount;
            _biddedPrice = biddedPrice;
            _counterOffer = counterOffer;
            _acceptors = acceptors;
            _acceptedByAll = acceptedByAll;
        }
        public BidDAL(
            string bidder,
            int itemId,
            int amount,
            double biddedPrice)
        {
            _bidder = bidder;
            _itemId = itemId;
            _amount = amount;
            _biddedPrice = biddedPrice;
            _counterOffer = -1;
            _acceptors = new List<StringData>();
            _acceptedByAll = false;
        }
        public BidDAL()
        {
        }
    }
}