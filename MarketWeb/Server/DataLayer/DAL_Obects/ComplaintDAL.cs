using MarketWeb.Server.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class ComplaintDAL
    {
        [Key]
        public int _id { get; set; }
        [Required]
        [ForeignKey("RegisteredDAL")]
        public string _complainer { get; set; }
        [Required]
        public int _cartID { get; set; }
        [Required]
        public String _message { get; set; }
        public String _response { get; set; }
        [NotMapped]
        public ComplaintStatus Status { get { return _response == null ? ComplaintStatus.Open : ComplaintStatus.Closed; } }

        public ComplaintDAL()
        {
        }

        public ComplaintDAL(int id, string complainer, int cartID, string message, string response)
        {
            _id = id;
            _complainer = complainer;
            _cartID = cartID;
            _message = message;
            _response = response;
        }

        public ComplaintDAL(string complainer, int cartID, string message)
        {
            _complainer = complainer;
            _cartID = cartID;
            _message = message;
        }
    }

}
