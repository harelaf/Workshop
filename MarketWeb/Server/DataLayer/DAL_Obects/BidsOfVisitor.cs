using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketWeb.Server.DataLayer
{
    public class BidsOfVisitor
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string _bidder { get; set; }
        [Required]
        public ICollection<BidDAL> _bids { get; set; }
        public BidsOfVisitor(string bidder, ICollection<BidDAL> bids)
        {
            _bidder = bidder;
            _bids = bids;
        }
        public BidsOfVisitor()
        {
        }
    }
}
