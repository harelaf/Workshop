using MarketWeb.Server.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ComplaintDAL
    {
        [Key]
        internal int _id { get; set; }
        [Required]
        internal RegisteredDAL _complainer { get; set; }
        [Required]
        internal int _cartID { get; set; }
        [Required]
        internal String _message { get; set; }
        internal String _response { get; set; }
        public ComplaintStatus Status { get { return _response == null ? ComplaintStatus.Open : ComplaintStatus.Closed; } }

        public ComplaintDAL(RegisteredDAL complainer, int cartID, string message)
        {
            _complainer = complainer;
            _cartID = cartID;
            _message = message;
        }
    }

}
