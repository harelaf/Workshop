using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketWeb.Server.DataLayer
{
    public class AdminMessageToRegisteredDAL
    {
        [Key]
        internal int mid { get; set; }
        [Required]
        internal String _receiverUsername { get; set; }
        [Required]
        internal String _senderUsername { get; set; }
        [Required]
        internal String _title { get; set; }
        [Required]
        internal String _message { get; set; }

        public AdminMessageToRegisteredDAL(int mid, string receiverUsername, string senderUsername, string title, string message)
        {
            this.mid = mid;
            _receiverUsername = receiverUsername;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }

        public AdminMessageToRegisteredDAL(string receiverUsername, string senderUsername, string title, string message)
        {
            _receiverUsername = receiverUsername;
            _senderUsername = senderUsername;
            _title = title;
            _message = message;
        }
    }
}
