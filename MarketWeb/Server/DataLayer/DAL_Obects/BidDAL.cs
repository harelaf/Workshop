using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class BidDAL
    {
        [Key]
        public string _bidder;
        [Key]
        public int _itemId;
        [Required]
        public int _amount;
        [Required]
        public double _biddedPrice;
        [Required]
        public double _counterOffer;
        public ISet<string> _acceptors;

        public BidDAL(
            string bidder, 
            int itemId, 
            int amount, 
            double biddedPrice, 
            double counterOffer, 
            ISet<string> acceptors)
        {
            _bidder = bidder;
            _itemId = itemId;
            _amount = amount;
            _biddedPrice = biddedPrice;
            _counterOffer = counterOffer;
            _acceptors = acceptors;
        }
    }
}