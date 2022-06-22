using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class OwnerAcceptors
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string _newOwner { get; set; }
        [Required]
        public string _appointer { get; set; }
        //[Required]
        public ICollection<StringData> _acceptors { get; set; }
        public OwnerAcceptors(string newOwner, ICollection<StringData> acceptors, string appointer)
        {
            _newOwner = newOwner;
            _appointer = appointer;
            _acceptors = acceptors;
            
        }
        public OwnerAcceptors()
        {
        }
    }
}