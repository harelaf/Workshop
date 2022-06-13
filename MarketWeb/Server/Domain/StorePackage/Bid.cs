using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain
{
    public class Bid
    {
        private String _bidder;
        private int _itemId;
        private int _amount;
        private double _biddedPrice;
        private double _counterOffer;
        private ISet<string> _acceptors;
        public String Bidder => _bidder;
        public int ItemID => _itemId;
        public int Amount => _amount;

        // the bidding price is per item and for specific amount (enforced in GetAcceptedBidPrice(...) in Store class).
        public double BiddedPrice => _biddedPrice;
        public double CounterOffer => _counterOffer;
        public ISet<string> Acceptors => _acceptors;
        public bool AccepttedByAll = false;

        public Bid(String bidder, int itemId, int amount, double biddedPrice)
        {
            _bidder = bidder;
            _itemId = itemId;
            _amount = amount;
            _biddedPrice = biddedPrice;
            _counterOffer = -1;
            _acceptors = new HashSet<string>();
        }
        public void AcceptBid(string userName)
        {
            _acceptors.Add(userName);
        }
        public void CounterOfferBid(string userName, double newPrice)
        {
            _acceptors.Add(userName);
            if(CounterOffer < newPrice)
                _counterOffer = newPrice;
        }

        internal double GetFinalPrice()
        {
            return CounterOffer == -1 ? BiddedPrice : CounterOffer;
        }
    }
}