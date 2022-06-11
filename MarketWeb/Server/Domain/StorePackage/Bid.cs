using System;
using System.Collections.Generic;

namespace MarketWeb.Server.Domain
{
    internal class Bid
    {
        private int _itemId;
        private int _amount;
        private double _biddedPrice;
        private double _counterOffer;
        private List<string> _acceptors;
        public int ItemId => _itemId;
        public int Amount => _amount;
        public double BiddedPrice => _biddedPrice;
        public double CounterOffer => _counterOffer;
        public List<string> Acceptors => _acceptors;

        public Bid(int itemId, double biddedPrice)
        {
            _itemId = itemId;
            _biddedPrice = biddedPrice;
            _counterOffer = -1;
            _acceptors = new List<string>();
        }
        public void AcceptBid(string userName)
        {
            _acceptors.Add(userName);
        }
        public void CounterOfferBid(string userName, double newPrice)
        {
            _acceptors.Add(userName);
            _counterOffer = newPrice;
        }
    }
}