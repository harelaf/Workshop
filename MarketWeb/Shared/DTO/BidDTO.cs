using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class BidDTO
    {
        private String _bidder;
        private int _itemId;
        private int _amount;
        private double _biddedPrice;
        private double _counterOffer;
        private ISet<string> _acceptors;
        private double _originalPrice;
        public String Bidder => _bidder;
        public int ItemID => _itemId;
        public int Amount => _amount;
        private bool _acceptedByAll;

        // the bidding price is per item and for specific amount (enforced in GetAcceptedBidPrice(...) in Store class).
        public double BiddedPrice => _biddedPrice;
        public double CounterOffer
        {
            get => _counterOffer;
            set => _counterOffer = value;
        }
        public ISet<string> Acceptors => _acceptors;
        public double OriginalPrice => _originalPrice;
        public bool AcceptedByAll => _acceptedByAll;

        public BidDTO(String bidder, int itemId, int amount, double biddedPrice, double counterOffer, ISet<String> acceptors, double originalPrice, bool acceptedByAll)
        {
            _bidder = bidder;
            _itemId = itemId;
            _amount = amount;
            _biddedPrice = biddedPrice;
            _counterOffer = counterOffer;
            _acceptors = acceptors;
            _originalPrice = originalPrice;
            _acceptedByAll = acceptedByAll;
        }
    }
}
